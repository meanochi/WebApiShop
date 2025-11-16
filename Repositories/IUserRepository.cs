using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        User addUser(User user);
        User getUserById(int id);
        User UpdateUser(User userToUpdate);
    }
}