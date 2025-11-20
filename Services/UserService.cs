using Entities;
using Repositories;
using System.Text.Json;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passService;
        private readonly IUserRepository _repository;

        public UserService(IPasswordService passService, IUserRepository repository)
        {
            _passService = passService;
            _repository = repository;
        }

        public User GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        public User? AddUser(User user)
        {
            if (_passService.GetStrengthByPassword(user.Password).Strength < 2)
                return null;
            return _repository.AddUser(user);
        }

        public User? UpdateUser(User userToUpdate)
        {
            if (_passService.GetStrengthByPassword(userToUpdate.Password).Strength < 2)
                return null;
            _repository.UpdateUser(userToUpdate);
            return userToUpdate;
        }

    }
}
