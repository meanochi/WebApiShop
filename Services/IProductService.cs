using DTOs;
using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<ProductsDTO>> getAllProducts(int? position, int? skip, int? maxPrice, int? minPrice, string? order);
    }
}