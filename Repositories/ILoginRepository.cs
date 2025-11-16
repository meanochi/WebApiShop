using Entities;

namespace Repositories
{
    public interface ILoginRepository
    {
        User Login(LoginUser user);
    }
}