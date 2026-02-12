//using Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Text.Json;


//namespace Repositories
//{
//    public class LoginRepository : ILoginRepository
//    {
//        ShowsCenterContext _context;
//        public LoginRepository(ShowsCenterContext ShowsCenterContext)
//        {
//            _context = ShowsCenterContext;
//        }
//        public async Task<User> Login(LoginUser user)
//        {
//            // Corrected the syntax for querying the database
//            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
//        }
//    }
//}
