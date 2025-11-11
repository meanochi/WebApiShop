using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LoginService
    {
        LoginRepository repository = new LoginRepository();
        public User Login(LoginUser user)
        {
            return repository.Login(user);
        }

    }
}
