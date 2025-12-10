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
        IMapper _mapper;

        public OrderController(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //// GET: api/<OrderController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {
            Order order = await _service.getOrderById(id);
            OrdersDTO orederDTO = _mapper.Map<Order, OrdersDTO>(order);
            return Ok(orederDTO);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] OrdersDTO orderDTO)
        {
            Order order = _mapper.Map<OrdersDTO, Order>(orderDTO);
            order = await _service.addOrder(order);
            //OrdersDTO orederDTO = _mapper.Map<Order, OrdersDTO>(order);
            if (order == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { order.OrderId }, order);

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
