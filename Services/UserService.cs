using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UserService
    {
        UserRepository repository = new UserRepository();
        public User getUserById(int id)
        {
            return repository.getUserById(id);
        }

        public User addUser(User user)
        {
            return repository.addUser(user);
        }

        public void UpdateUser(User userToUpdate)
        {
            repository.UpdateUser(userToUpdate);
        }

    }
}
