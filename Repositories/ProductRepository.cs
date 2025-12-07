using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        WebApiShop_329084941Context _context;
        public ProductRepository(WebApiShop_329084941Context webApiShop_329084941Context)
        {
            _context = webApiShop_329084941Context;
        }

        public async Task<List<Product>> getAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
