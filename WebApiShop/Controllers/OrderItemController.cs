using Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        // POST api/<OrderItemController>
        [HttpPost]
        public IActionResult Post([FromBody] OrderItem value)
        {
            if (value == null)
                return BadRequest();
            return Ok("OrderItem received");
        }
    }
}
