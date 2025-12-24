using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _repository;
        IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDTO>> getAllCategories()
        {
            List<Category> categories = await _repository.getAllCategories();
            List<CategoryDTO> categoriesDTO = _mapper.Map<List<Category>, List<CategoryDTO>>(categories);
            return categoriesDTO;
        }
    }
}
