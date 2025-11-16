using Entities;
using System.Text.Json;


namespace Repositories
{
    public class LoginRepository : ILoginRepository
    {
        string filePath = "..\\file.txt";

        public User Login(LoginUser user)
        {
            using (StreamReader reader = System.IO.File.OpenText(filePath))
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
