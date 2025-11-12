using Entities;
using Repositories;
using System.Text.Json;
using Zxcvbn;
namespace Services
{
    public class UserService
    {
        PasswordService passService = new PasswordService();
        UserRepository repository = new UserRepository();
        public User getUserById(int id)
        {
            return repository.getUserById(id);
        }

        public User? addUser(User user)
        {
            if (passService.getStrengthByPassword(user.Password).Strength < 2)
                return null;
            return repository.addUser(user);
        }

        public void UpdateUser(User userToUpdate)
        {
            repository.UpdateUser(userToUpdate);
        }

    }
}
