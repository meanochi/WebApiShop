using DTOs;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<UserReadDTO> addUser(UserCreateDTO user);
        Task<UserReadDTO> getUserById(int id);
        Task<UserReadDTO> UpdateUser(UserUpdateDTO userToUpdate);
        Task<UserReadDTO> Login(UserLoginDTO user);
    }
}