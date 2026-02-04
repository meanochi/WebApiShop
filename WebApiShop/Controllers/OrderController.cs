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
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

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
            if (orderDTO == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { orderDTO.OrderId }, orderDTO);
        }
    }
}
