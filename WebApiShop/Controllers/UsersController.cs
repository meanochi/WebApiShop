using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> Get(int id)
        {
            UserReadDTO user = await _userService.getUserById(id);
            if (user == null)
                return NoContent();
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost("user")]
        public async Task<ActionResult<UserReadDTO>> Post([FromBody] UserCreateDTO user)
        {
            UserReadDTO newUser = await _userService.addUser(user);
            if (newUser == null)
                return BadRequest("Password is too weak");
            return CreatedAtAction(nameof(Get), new { newUser.Id }, newUser);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDTO>> Put([FromBody] UserUpdateDTO userToUpdate, int id)
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
    }
}
    }
}
