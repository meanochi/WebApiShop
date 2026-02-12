//using Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Repositories
//{
//    public class ProductRepository : IProductRepository
//    {
//        ShowsCenterContext _context;
//        public ProductRepository(ShowsCenterContext ShowsCenterContext)
//        {
//            _context = ShowsCenterContext;
//        }

//        public async Task<List<Product>> getAllProducts(int? position, int? skip, int? maxPrice, int? minPrice, string? order)
//        {
//            var products = await _context.Products.OrderBy(p => p.Price)
//                .ToListAsync();
//            return products; 
//        }
//    }
//}
