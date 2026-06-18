
using Entities;

namespace Services
{
    public interface IAuth
    {
        Task<bool> IsManager(int id);
        string GenerateToken(int userId, string email, bool isManager);
        Task<User?> Login(string email, string password);
    }
}