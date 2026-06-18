using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [EnableRateLimiting("strict")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        IPasswordService _service;

        public PasswordController(IPasswordService service)
        {
            _service = service;
        }

        // POST api/<PasswordController>
        [HttpPost]
        public ActionResult<PasswordEntity> POST([FromBody] string pass)
        {
            PasswordEntity password = _service.getStrengthByPassword(pass);
            if (password == null)
                return NoContent();
            return Ok(password);
        }
    }
}
