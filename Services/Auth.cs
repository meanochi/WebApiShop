using Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class Auth : IAuth
    {
        IUserRepository _repository;
        IConfiguration _configuration;
        List<string> _managerEmails = ["r0583285891@gmail.com", "michal.icecream@gmail.com"];
        private readonly IPasswordService _passService;

        public Auth(IUserRepository repository, IConfiguration configuration, IPasswordService passService)
        {
            _repository = repository;
            _configuration = configuration;
            _passService = passService;
        }

        public async Task<bool> IsManager(int id)
        {
            User user = await _repository.getUserById(id);
            if (user == null) return false;
            return _managerEmails.Contains(user.EmailAddress.Trim());
        }

        public string GenerateToken(int userId, string email, bool isManager)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, isManager ? "Manager" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> Login(string email, string password)
        {
            // 1. שליפת המשתמש מהרפוזיטורי לפי אימייל
            User user = await _repository.GetUserByEmail(email);

            if (user == null)
            {
                return null; // המשתמש לא קיים
            }

            // 2. אימות הסיסמה באמצעות ה-PasswordService
            bool isPasswordValid = _passService.VerifyPassword(password, user.Password);

            if (!isPasswordValid)
            {
                return null; // הסיסמה שגויה
            }

            // 3. אם הכל תקין - ממשיכים לייצר Token או להחזיר את המשתמש
            return user;
        }
    }
}