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
        ShowsCenterContext _context;
        public CategoryRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }
        public async Task<List<Category>> getAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> getCategoryById(int id)
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
