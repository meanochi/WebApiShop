using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrders()
        {
            List<OrderDTO> orders = await _service.getAllOrders();
            if (orders == null || orders.Count == 0)
                return NoContent();
            return Ok(orders);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            OrderDTO order = await _service.getOrderById(id);
            return Ok(order);
        }
        [HttpGet("userId")]
        public async Task<ActionResult<List<OrderDTO>>> GetForUser(int id)
        {
            List<OrderDTO> orders = await _service.getOrdersForUser(id);
            if (orders == null || orders.Count == 0)
                return NoContent();
            return Ok(orders);
        }
        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderCreateDTO orderDTO)
        {
            
            OrderDTO newOrderDTO = await _service.addOrder(orderDTO);
            if (newOrderDTO == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { newOrderDTO.Id }, orderDTO);

        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> Put(int id, [FromBody] OrderUpdateDTO orderToUpdat)
        {
            OrderDTO order = await _service.updateOrder(orderToUpdat,id);
            if(order == null)
                return BadRequest();
            return Ok(order);
        }

        //// DELETE api/<OrderController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
