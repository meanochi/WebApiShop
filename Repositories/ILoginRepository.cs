using Entities;

namespace Repositories
{
    public interface ILoginRepository
    {
        Task<User> Login(LoginUser user);
    }
}