using Moq;
using ToDoApp.Api.Controllers;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.UserServices;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ToDoApp.Api.ToDoApp.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnCreated_WhenUserIsRegistered()
        {
            // Arrange
            var user = new User { UserId = "1", Password = "password" };
            _mockUserService.Setup(s => s.RegisterAsync(user)).ReturnsAsync(user);

            // Act
            var result = await _userController.Register(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(user, createdResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsArexInValid()
        {
            // Arrange
            var loginModel = new LoginModel { UserId = "1", Password = "password" };
            var loginHistory = new LoginHistory { UserId = "1", Token = "token" };
            _mockUserService.Setup(s => s.LoginAsync(loginModel)).ReturnsAsync(loginHistory);

            // Act
            var result = await _userController.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("token", ((dynamic)okResult.Value).data);
        }

        [Fact]
        public async Task Logout_ShouldReturnOk_WhenUserIsLoggedOut()
        {
            // Arrange
            string userId = "1";
            _mockUserService.Setup(s => s.LogoutAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _userController.Logout(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully logged out.", okResult.Value);
        }
    }
}
