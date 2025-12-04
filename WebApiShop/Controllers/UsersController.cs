using Entities;
using Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService, IPasswordService passwordService)
        {
            _userService = userService;
            _passwordService = passwordService;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await _userService.getUserById(id);
            if(user == null)
                return NotFound();
            return Ok(user);
        }
        
        // POST api/<UsersController>
        [HttpPost]
        public async  Task<ActionResult<User>> POST([FromBody] User user)
        {
            user = await _userService.addUser(user);
            if (user == null)
                return BadRequest("Failed to create user");
            return CreatedAtAction(nameof(Get), new {user.Id }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PUT([FromBody] User userToUpdate,int id)
        {
            int passwordScore = _passwordService.getStrengthByPassword(userToUpdate.Password).Strength;
            if (passwordScore < 2)
                return BadRequest($"Password too weak (score: {passwordScore}/4). Minimum required: 2");

            userToUpdate.Id = id;
            userToUpdate = await _userService.UpdateUser(userToUpdate);
            if (userToUpdate == null)
                return BadRequest("Failed to update user");
            else
                return Ok(userToUpdate);


        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
