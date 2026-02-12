using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        ShowsCenterContext _context;
        public UserRepository(ShowsCenterContext ShowsCenterContext)
        {
            _context = ShowsCenterContext;
        }
        public async Task<User> getUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> addUser(User user)
        {
           await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            if (getUserById(user.Id) != null)
                return user;
            else
                return null;

        }

        public async Task<User> UpdateUser(User userToUpdate)
        {
            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync();
            return userToUpdate;

        }
        public async Task<User> Login(User user)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == user.EmailAddress && u.Password == user.Password);
        }
    }
}
