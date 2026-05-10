using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        IUserService _userService;
        IMapper _mapper;
        IAuth _auth;
        private readonly IForgotPasswordService _forgotPassword;
        private readonly IOrderConfirmationEmailService _orderConfirmationEmail;
        public UsersController(IUserService userService, IMapper mapper, IAuth auth, IForgotPasswordService forgotPassword, IOrderConfirmationEmailService orderConfirmationEmail, ILogger<UsersController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _auth = auth;
            _forgotPassword = forgotPassword;
            _orderConfirmationEmail = orderConfirmationEmail;
            _logger = logger;
        }

        // GET api/<UsersController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> Get(int id)
        {
            UserReadDTO user = await _userService.getUserById(id);
            if (user == null)
                return NoContent();
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("loginUser")]
        public async Task<ActionResult<UserReadDTO>> GetLogin([FromBody] UserLoginDTO loginUser)
        {
            _logger.LogInformation($"Login attempted with email: {loginUser.EmailAddress}");
            var (user, token) = await _userService.Login(loginUser);
            if (user == null) return NoContent();

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,         // HTTPS בלבד
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("user")]
        public async Task<ActionResult<UserReadDTO>> POST([FromBody] UserCreateDTO user)
        {
            var (newUser, token) = await _userService.addUser(user);
            if (newUser == null) return BadRequest("Password is too weak");

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });

            return CreatedAtAction(nameof(Get), new { newUser.Id }, newUser);
        }

        // PUT api/<UsersController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserReadDTO>> PUT([FromBody] UserUpdateDTO userToUpdate, int id)
        {
            UserReadDTO user = await _userService.UpdateUser(userToUpdate, id);
            if (user == null)
                return BadRequest("Password is too weak");
            else
                return Ok(user);


        }

        [ManagerOnly]
        [HttpGet("isManger")]
        public async Task<Boolean> IsManager(int id)
        {
            return await _auth.IsManager(id);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
        {
            var result = await _forgotPassword.RequestCodeAsync(request.Email ?? "", ct);
            return Ok(new ForgotPasswordResponse { Sent = result.Sent, Message = result.Message });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
        {
            var result = await _forgotPassword.ResetPasswordAsync(
                request.Email ?? "",
                request.Code ?? "",
                request.NewPassword ?? "",
                ct);
            return Ok(new ResetPasswordResponse { Success = result.Success, Message = result.Message });
        }

        [HttpPost("send-order-confirmation")]
        public async Task<ActionResult<SendOrderConfirmationResponse>> SendOrderConfirmation(
            [FromBody] SendOrderConfirmationRequest request,
             CancellationToken ct)
        {
            var result = await _orderConfirmationEmail.SendAsync(request, ct);
            return Ok(new SendOrderConfirmationResponse
            {
                Sent = result.Sent,
                Message = result.Message
            });
        }

    }
}
