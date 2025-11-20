using Entities;

namespace Services
{
    public interface IPasswordService
    {
        PasswordEntity GetStrengthByPassword(string password);
    }
}