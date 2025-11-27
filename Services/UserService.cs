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

        public User getUserById(int id)
        {
            return _repository.getUserById(id);
        }

        public User? addUser(User user)
        {
            return _repository.addUser(user);
        }

        public User? UpdateUser(User userToUpdate)
        {
            _repository.UpdateUser(userToUpdate);
            return userToUpdate;
        }

    }
}
