using AutoMapper;
using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IMapper _mapper;
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        //// GET: api/<UsersController>
        //[HttpGet]
        //public string Get()
        //{
        //    return "value";
        //}

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            UserDTO user = await _userService.getUserById(id);
            if(user == null)
                return NoContent();
            return Ok(user);
        }
        
        // POST api/<UsersController>
        [HttpPost]
        public async  Task<ActionResult<User>> POST([FromBody] User user)
        {
            user = await _userService.addUser(user);
            if (user == null)
                return BadRequest("Password is too weak");
            return CreatedAtAction(nameof(Get), new {user.Id }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PUT([FromBody] User userToUpdate,int id)
        {
            userToUpdate.Id = id;
            userToUpdate = await _userService.UpdateUser(userToUpdate);
            if (userToUpdate == null)
                return BadRequest("Password is too weak");
            else
                return Ok(userToUpdate);


        }

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
