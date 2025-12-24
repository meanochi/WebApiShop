using Entities;
using Repositories;
using System.Linq;
using System.Threading.Tasks;
using TestProject;
using Xunit;
using Assert = Xunit.Assert;

namespace Repositories.Tests
{
    public class UserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _userRepository = new UserRepository(_fixture.Context);
        }

        [Fact]
        public async Task GetUserById_ReturnsUser_WhenUserExists()
        {
            var result = await _userRepository.getUserById(1);

            Assert.NotNull(result);
            Assert.Equal("JohnDoe", result.UserName);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task AddUser_AddsUserToDatabase()
        {
            var newUser = new User
            {
                UserName = "JaneDoe",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "password123"
            };

            var result = await _userRepository.addUser(newUser);

            Assert.NotNull(result);
            Assert.Equal("JaneDoe", result.UserName);
            Assert.Contains(result, _fixture.Context.Users);
        }

        [Fact]
        public async Task UpdateUser_UpdatesExistingUser()
        {
            var user = await _userRepository.getUserById(1);
            user.UserName = "Updated";

            var updatedUser = await _userRepository.UpdateUser(user);

            Assert.NotNull(updatedUser);
            Assert.Equal("Updated", updatedUser.UserName);
        }
    }
}
