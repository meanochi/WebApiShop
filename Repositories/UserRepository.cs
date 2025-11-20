using Entities;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "file.txt");
        
        public User GetUserById(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Id == id)
                        return user;
                }
                return null;
            }
        }

        public User AddUser(User user)
        {
            int numberOfUsers = System.IO.File.ReadLines(_filePath).Count();
            user.Id = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(_filePath, userJson + Environment.NewLine);
            return user;
        }

        public User UpdateUser(User userToUpdate)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {

                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.Id == userToUpdate.Id)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(_filePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText(_filePath, text);
            }
            return userToUpdate;
        }
    }
}
