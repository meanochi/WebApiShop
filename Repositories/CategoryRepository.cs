using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        WebApiShop_329084941Context _context;
        public CategoryRepository(WebApiShop_329084941Context webApiShop_329084941Context)
        {
            _context = webApiShop_329084941Context;
        }
        public async Task<List<Category>> getAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> getCategorieById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task<Category> addCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
