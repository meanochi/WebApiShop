using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<UserReadDTO> getUserById(int id);
        Task<UserReadDTO> UpdateUser(UserUpdateDTO userToUpdate, int id);
        Task<(UserReadDTO user, string token)> Login(UserLoginDTO user);
        Task<(UserReadDTO user, string token)> addUser(UserCreateDTO user);
    }
}