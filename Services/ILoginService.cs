using Entities;

namespace Services
{
    public interface ILoginService
    {
        Task<User> Login(LoginUser user);
    }
}