using Entities;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        WebApiShop_329084941Context _context;
        public UserRepository(WebApiShop_329084941Context webApiShop_329084941Context)
        {
            _context = webApiShop_329084941Context;
        }
        public async Task<User> getUserById(int id)
        {
            return await _context.Users.FindAsync(id);

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
    }
}
