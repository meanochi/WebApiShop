using AutoMapper;
using DTOs;
using Entities;
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
        public UserService(IPasswordService passService, IUserRepository repository, IMapper mapper)
        {
            _passService = passService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDTO> getUserById(int id)
        {
            User user = await _repository.getUserById(id);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
            return userDTO;
        }

        public async Task<User?> addUser(User user)
        {
            if (_passService.getStrengthByPassword(user.Password).Strength < 2)
                return null;
            return await _repository.addUser(user);
        }

        public async Task<User?> UpdateUser(User userToUpdate)
        {
            if (_passService.getStrengthByPassword(userToUpdate.Password).Strength < 2)
                return null;
            await _repository.UpdateUser(userToUpdate);
            return userToUpdate;
        }

    }
}
