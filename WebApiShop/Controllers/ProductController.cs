using AutoMapper;
using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        
        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<List<ProductsDTO>>> Get()
        {
            List<ProductsDTO> products = await _service.getAllProducts();
            return Ok(products);
        }
    }
}
