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
    public class OrderController : ControllerBase
    {
        IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        //// GET: api/<OrderController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersDTO>> Get(int id)
        {
            OrdersDTO order = await _service.getOrderById(id);
            return Ok(order);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<OrdersDTO>> Post([FromBody] OrdersDTO orderDTO)
        {
            
            orderDTO = await _service.addOrder(orderDTO);
            //OrdersDTO orederDTO = _mapper.Map<Order, OrdersDTO>(order);
            if (orderDTO == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { orderDTO.OrderId }, orderDTO);

        }

        //// PUT api/<OrderController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<OrderController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
