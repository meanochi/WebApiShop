using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<ActionResult<User>> GetLogin([FromBody] LoginUser loginUser)
        {
            User user = await _loginService.Login(loginUser);
            if (user == null)
                return NoContent();
            _logger.LogInformation($"Login attempted with Email {user.UserName} and password {user.Password}");
            return Ok(user);
        }
    }
}
