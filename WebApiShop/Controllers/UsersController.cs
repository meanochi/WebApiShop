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
        UserService userService = new UserService();
        // GET: api/<UsersController>
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user = userService.getUserById(id);
            if(user == null)
                return NoContent();
            return Ok(user);
        }
        
        // POST api/<UsersController>
        [HttpPost]
        public ActionResult <User> POST([FromBody] User user)
        {
            user = userService.addUser(user);
            return CreatedAtAction(nameof(Get), new {user.Id }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id,[FromBody] User userToUpdate)
        {
            userService.UpdateUser(userToUpdate);

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
