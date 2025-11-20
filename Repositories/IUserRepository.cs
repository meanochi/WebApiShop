using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User GetUserById(int id);
        User UpdateUser(User userToUpdate);
    }
}