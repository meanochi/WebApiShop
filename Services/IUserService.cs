using Entities;

namespace Services
{
    public interface IUserService
    {
        User? AddUser(User user);
        User GetUserById(int id);
        User? UpdateUser(User userToUpdate);
    }
}