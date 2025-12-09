using Entities;
using Repositories;
using System.Text.Json;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {
        private IPasswordService _passwordService;
        private IUserRepository _userRepository;

        public UserService(IPasswordService passwordService, IUserRepository userRepository)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<User?> AddUser(User user)
        {
            if (_passwordService.GetStrengthByPassword(user.Password).Strength < 2)
                return null;
            return await _userRepository.AddUser(user);
        }

        public async Task<User?> UpdateUser(User userToUpdate)
        {
            if (_passwordService.GetStrengthByPassword(userToUpdate.Password).Strength < 2)
                return null;
            await _userRepository.UpdateUser(userToUpdate);
            return userToUpdate;
        }

    }
}
