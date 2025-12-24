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
    public class ProductService : IProductService
    {
        IProductRepository _repository;
        IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ProductsDTO>> getAllProducts()
        {
            List<Product> products = await _repository.getAllProducts();
            List<ProductsDTO> productsDTO = _mapper.Map<List<Product>, List<ProductsDTO>>(products);
            return productsDTO;
        }
    }
}
