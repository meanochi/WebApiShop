using AutoMapper;
using DTOs;
using Entities;
using Microsoft.AspNetCore.Http.Metadata;
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
        IAuth _auth;
        public UsersController(IUserService userService, IMapper mapper, IAuth auth)
        {
            _userService = userService;
            _mapper = mapper;
            _auth = auth;
        }


        //// GET: api/<UsersController>
        //[HttpGet]
        //public string Get()
        //{
        //    return "value";
        //}

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> Get(int id)
        {
            UserReadDTO user = await _userService.getUserById(id);
            if(user == null)
                return NoContent();
            return Ok(user);
        }
        
        // POST api/<UsersController>
        [HttpPost("user")]
        public async  Task<ActionResult<UserReadDTO>> POST([FromBody] UserCreateDTO user)
        {
           UserReadDTO newUser = await _userService.addUser(user);
            if (newUser == null)
                return BadRequest("Password is too weak");
            return CreatedAtAction(nameof(Get), new { newUser.Id }, newUser);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDTO>> PUT([FromBody] UserUpdateDTO userToUpdate,int id)
        {
            UserReadDTO user = await _userService.UpdateUser(userToUpdate);
            if (user == null)
                return BadRequest("Password is too weak");
            else
                return Ok(user);


        }
        [HttpPost("loginUser")]
        public async Task<ActionResult<UserReadDTO>> GetLogin([FromBody] UserLoginDTO loginUser)
        {
            UserReadDTO user = await _userService.Login(loginUser);
            if (user == null)
                return NoContent();
            return Ok(user);
        }

        [HttpGet("isManger")]
        public async Task<Boolean> IsManager(int id)
        {
            return await _auth.IsManager(id);
        }

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
