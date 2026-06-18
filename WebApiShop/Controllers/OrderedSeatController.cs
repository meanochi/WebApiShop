using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [EnableRateLimiting("standard")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderedSeatController : ControllerBase
    {
        IOrderService _service;

        public OrderedSeatController(IOrderService service)
        {
            _service = service;
        }

        // GET: api/<OrderedSeatController>
        [HttpGet("showId/{showId}")]
        public async Task<ActionResult<List<OrderedSeatReadDTO>>> GetForShow(int showId)
        {
            var result = await _service.GetOrderedSeatsForShow(showId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("userId/{userId}")]
        public async Task<ActionResult<List<OrderedSeatReadDTO>>> GetForUser(int userId)
        {
            var result = await _service.GetOrderedSeatsForUser(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
