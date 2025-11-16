using Entities;

namespace Services
{
    public interface IUserService
    {
        User? addUser(User user);
        User getUserById(int id);
        User? UpdateUser(User userToUpdate);
    }
}