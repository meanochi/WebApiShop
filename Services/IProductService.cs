using DTOs;
using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<ProductsDTO>> getAllProducts();
    }
}