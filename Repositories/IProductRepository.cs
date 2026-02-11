using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> getAllProducts(int? position, int? skip, int? maxPrice, int? minPrice, string? order);
    }
}