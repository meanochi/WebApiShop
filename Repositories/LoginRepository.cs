using Entities;
using System.Text.Json;


namespace Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "file.txt");

        public User Login(LoginUser user)
        {
            using (StreamReader reader = System.IO.File.OpenText(_filePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User fullUser = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (fullUser.UserName == user.UserName && fullUser.Password == user.Password)
                        return fullUser;
                }
            }
            return null;
        }

    }
}
