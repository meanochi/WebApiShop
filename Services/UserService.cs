using AutoMapper;
using DTOs;
using Entities;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IPasswordService passwordService, IUserRepository repository, IMapper mapper)
        {
            _passwordService = passwordService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserReadDTO> getUserById(int id)
        {
            User user = await _repository.getUserById(id);
            UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(user);
            return userDTO;
        }

        public async Task<UserReadDTO> addUser(UserCreateDTO user)
        {
            if (_passwordService.getStrengthByPassword(user.Password).Strength < 2)
                return null;
            User newUser = _mapper.Map<UserCreateDTO, User>(user);
            newUser = await _repository.addUser(newUser);
            UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(newUser);
            return userDTO;
        }

        public async Task<UserReadDTO> UpdateUser(UserUpdateDTO userToUpdate)
        {
            if (_passwordService.getStrengthByPassword(userToUpdate.Password).Strength < 2)
                return null;
            User user = _mapper.Map<UserUpdateDTO, User>(userToUpdate);
            user = await _repository.UpdateUser(user);
            UserReadDTO userDTO = _mapper.Map<User, UserReadDTO>(user);
            return userDTO;
        }

        public async Task<UserReadDTO> Login(UserLoginDTO user)
        {
            User loginUser = _mapper.Map<UserLoginDTO, User>(user);
            loginUser = await _repository.Login(loginUser);
            UserReadDTO logged = _mapper.Map<User, UserReadDTO>(loginUser);
            return logged;
        }
    }
}
