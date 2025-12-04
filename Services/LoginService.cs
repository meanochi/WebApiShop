using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    public class LoginService : ILoginService
    {
        ILoginRepository _repository;

        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Login(LoginUser user)
        {
            return await _repository.Login(user);
        }

    }
}
