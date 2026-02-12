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
    public class OrderItemController : ControllerBase
    {
        IMapper _mapper;
        public OrderItemController(IMapper mapper)
        {
            _mapper = mapper;
        }

        //// GET: api/<OrderItemController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<OrderItemController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> Get(int id)
        {
            OrderItemDTO value = new OrderItemDTO(5);
            OrderItem order = _mapper.Map<OrderItemDTO, OrderItem>(value);
            if (order == null)
                return NoContent();
            return Ok(order);
        }

        //// POST api/<OrderItemController>
        //[HttpPost]
        //public Task<Order> Post([FromBody] OrderItemDTO value)
        //{

        //}

        //// PUT api/<OrderItemController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<OrderItemController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
