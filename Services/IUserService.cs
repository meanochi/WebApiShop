using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<User> addUser(User user);
        Task<UserDTO> getUserById(int id);
        Task<User?> UpdateUser(User userToUpdate);
    }
}