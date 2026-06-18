using AutoMapper;
using DTOs;
using Entities;
using Microsoft.Extensions.Configuration;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAuth _auth;
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly TimeSpan _cacheTtl;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IAuth auth, ICacheService cache, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _auth = auth;
            _cache = cache;

            // קריאת ה-TTL מההגדרות (ברירת מחדל 5 דקות אם לא נמצא)
            var ttlMinutes = config.GetValue<int>("Redis:TTLMinutes", 5);
            _cacheTtl = TimeSpan.FromMinutes(ttlMinutes);
        }

        public async Task<List<CategoryDTO>> getAllCategories()
        {
            string cacheKey = "categories_all";

            // שלב 1: ניסיון שליפה מה-Cache
            var cachedCategories = await _cache.GetAsync<List<CategoryDTO>>(cacheKey);
            if (cachedCategories != null)
            {
                return cachedCategories; // החזרה מהירה מהמטמון
            }

            // שלב 2: אם אין במטמון, שולפים מהמסד
            List<Category> categories = await _repository.getAllCategories();
            List<CategoryDTO> categoriesDTO = _mapper.Map<List<Category>, List<CategoryDTO>>(categories);

            // שלב 3: שמירה במטמון לפעם הבאה עם ה-TTL המוגדר
            await _cache.SetAsync(cacheKey, categoriesDTO, _cacheTtl);

            return categoriesDTO;
        }

        public async Task<CategoryDTO> getCategoryById(int id)
        {
            string cacheKey = $"category_{id}";

            var cachedCategory = await _cache.GetAsync<CategoryDTO>(cacheKey);
            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            Category category = await _repository.getCategoryById(id);
            if (category == null) return null;

            CategoryDTO categoryDTO = _mapper.Map<Category, CategoryDTO>(category);

            await _cache.SetAsync(cacheKey, categoryDTO, _cacheTtl);
            return categoryDTO;
        }

        public async Task<Category?> addCategory(CategoryDTO category, int userId)
        {
            if (!await _auth.IsManager(userId))
            {
                return null;
            }
            Category newCategory = _mapper.Map<CategoryDTO, Category>(category);
            newCategory = await _repository.addCategory(newCategory);

            // Cache Invalidation - מחיקת הרשימה הכללית כדי שהפריט החדש יופיע בשליפה הבאה
            await _cache.RemoveAsync("categories_all");

            return newCategory;
        }

        public async Task<int?> Delete(int id, int userId)
        {
            if (!await _auth.IsManager(userId))
            {
                return null;
            }
            var result = await _repository.Delete(id);

            // Cache Invalidation - מחיקת הרשימה הכללית והפריט הספציפי
            await _cache.RemoveAsync("categories_all");
            await _cache.RemoveAsync($"category_{id}");

            return result;
        }
    }
}