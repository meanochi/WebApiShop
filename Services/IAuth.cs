
namespace Services
{
    public interface IAuth
    {
        Task<bool> IsManager(int id);
        string GenerateToken(int userId, string email, bool isManager);

    }
}