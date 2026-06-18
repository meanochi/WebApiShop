using AutoMapper;
using DTOs;
using Entities;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories;
using System.Text.Json;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {
            IPasswordService _passService;
            IUserRepository _repository;
            IMapper _mapper;
            IAuth _auth;
            ICacheService _cache;
            IConfiguration _configuration;

            public UserService(IPasswordService passService, IUserRepository repository,
                               IMapper mapper, IAuth auth,
                               ICacheService cache, IConfiguration configuration)
            {
                _passService = passService;
                _repository = repository;
                _mapper = mapper;
                _auth = auth;
                _cache = cache;
                _configuration = configuration;
            }

            public async Task<UserReadDTO> getUserById(int id)
            {
                string cacheKey = $"user:{id}";

                // נסה לשלוף מה-cache
                var cached = await _cache.GetAsync<UserReadDTO>(cacheKey);
                if (cached != null)
                {
                    return cached; // Cache HIT ✅
                }

                // Cache MISS — שלוף מה-DB
                User user = await _repository.getUserById(id);
                UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(user);

                // שמור ב-cache עם TTL מה-configuration
                int ttlMinutes = _configuration.GetValue<int>("Redis:TTLMinutes");
                await _cache.SetAsync(cacheKey, userDTO, TimeSpan.FromMinutes(ttlMinutes));

                return userDTO;
            }

        public async Task<(UserReadDTO user, string token)> addUser(UserCreateDTO user)
        {
            if (_passService.getStrengthByPassword(user.Password).Strength < 2)
                return (null, null);
            string securedPassword = _passService.HashPassword(user.Password);
            User newUser = _mapper.Map<UserCreateDTO, User>(user);
            newUser.Password = securedPassword;
            newUser = await _repository.addUser(newUser);
            UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(newUser);
            bool isManager = await _auth.IsManager(newUser.Id);
            string token = _auth.GenerateToken(newUser.Id, newUser.EmailAddress, isManager);
            return (userDTO, token);
        }

        public async Task<UserReadDTO> UpdateUser(UserUpdateDTO userToUpdate, int id)
        {
            if (userToUpdate.Password != "")
                if (_passService.getStrengthByPassword(userToUpdate.Password).Strength < 2)
                    return null;

            User user = _mapper.Map<UserUpdateDTO, User>(userToUpdate);
            user.Id = id;
            user = await _repository.UpdateUser(user);

            // Cache Invalidation — מחק את הרשומה הישנה
            await _cache.RemoveAsync($"user:{id}");

            UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(user);
            return userDTO;
        }
        public async Task<(UserReadDTO user, string token)> Login(UserLoginDTO user)
        {
            User loginUser = _mapper.Map<UserLoginDTO, User>(user);
            loginUser = await _auth.Login(loginUser.EmailAddress, loginUser.Password);
            if (loginUser == null) return (null, null);
            UserReadDTO logged = _mapper.Map<User, UserReadDTO>(loginUser);
            bool isManager = await _auth.IsManager(loginUser.Id);
            string token = _auth.GenerateToken(loginUser.Id, loginUser.EmailAddress, isManager);
            return (logged, token);
        }

    }
}
