using Entities;
using Repositories;
using System.Text.Json;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {
        IPasswordService _passService;
        IUserRepository _repository;

        public UserService(IPasswordService passService, IUserRepository repository)
        {
            _passService = passService;
            _repository = repository;
        }

        public async Task<User> getUserById(int id)
        {
            return await _repository.getUserById(id);
        }

        public async Task<User?> addUser(User user)
        {
            if (_passService.getStrengthByPassword(user.Password).Strength < 2)
                return null;
            return await _repository.addUser(user);
        }

        public async Task<User?> UpdateUser(User userToUpdate)
        {
            if (_passService.getStrengthByPassword(userToUpdate.Password).Strength < 2)
                return null;
            await _repository.UpdateUser(userToUpdate);
            return userToUpdate;
        }

    }
}
