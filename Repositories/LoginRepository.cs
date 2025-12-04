using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Repositories
{
    public class LoginRepository : ILoginRepository
    {
        WebApiShop_329084941Context _context;
        public LoginRepository(WebApiShop_329084941Context webApiShop_329084941Context)
        {
            _context = webApiShop_329084941Context;
        }
        public async Task<User> Login(LoginUser user)
        {
            // Corrected the syntax for querying the database
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
        }
    }
}
