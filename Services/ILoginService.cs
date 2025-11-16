using Entities;

namespace Services
{
    public interface ILoginService
    {
        User Login(LoginUser user);
    }
}