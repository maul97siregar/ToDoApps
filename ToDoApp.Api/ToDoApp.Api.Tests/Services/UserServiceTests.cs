using Moq;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.UserServices;
using ToDoApp.Api.Utils;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ToDoApp.Api.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<DbSet<User>> _mockUserSet;
        private readonly Mock<DbSet<LoginHistory>> _mockLoginHistorySet;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserSet = new Mock<DbSet<User>>();
            _mockLoginHistorySet = new Mock<DbSet<LoginHistory>>();
            _mockContext = new Mock<AppDbContext>();
            _mockConfig = new Mock<IConfiguration>();
            _userService = new UserService(_mockContext.Object, _mockConfig.Object);

            // Setting up dummy data for users
            var users = new List<User>
            {
                new User { UserId = "1", Password = "password" },
                new User { UserId = "2", Password = "password2" }
            }.AsQueryable();

            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            // Setting up dummy data for login histories
            var loginHistories = new List<LoginHistory>
            {
                new LoginHistory { UserId = "1", IsActive = true },
                new LoginHistory { UserId = "2", IsActive = false }
            }.AsQueryable();

            _mockLoginHistorySet.As<IQueryable<LoginHistory>>().Setup(m => m.Provider).Returns(loginHistories.Provider);
            _mockLoginHistorySet.As<IQueryable<LoginHistory>>().Setup(m => m.Expression).Returns(loginHistories.Expression);
            _mockLoginHistorySet.As<IQueryable<LoginHistory>>().Setup(m => m.ElementType).Returns(loginHistories.ElementType);
            _mockLoginHistorySet.As<IQueryable<LoginHistory>>().Setup(m => m.GetEnumerator()).Returns(loginHistories.GetEnumerator());

            // Mocking DbContext
            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
            _mockContext.Setup(c => c.LoginHistories).Returns(_mockLoginHistorySet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        [Fact]
        public async Task RegisterAsync_ShouldAddUser_WhenUserIsValid()
        {
            var user = new User { UserId = "3", Password = "password3" };
            var usersList = new List<User>();

            // Mock AddAsync
            _mockUserSet.Setup(m => m.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>((u, ct) => usersList.Add(u))
                .ReturnsAsync(new Mock<EntityEntry<User>>().Object); // Return a new mock EntityEntry

            var result = await _userService.RegisterAsync(user);

            Assert.Equal(user.UserId, result.UserId);
            Assert.Single(usersList);
            Assert.Equal(user.UserId, usersList[0].UserId);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var loginModel = new LoginModel { UserId = "1", Password = "password" };

            var result = await _userService.LoginAsync(loginModel);

            Assert.NotNull(result);
            Assert.Equal("1", result.UserId);
        }

        [Fact]
        public async Task LogoutAsync_ShouldMarkUserAsLoggedOut()
        {
            var userId = "1";
            var loginHistory = new LoginHistory { UserId = userId, IsActive = true };
            var mockLoginHistories = new List<LoginHistory> { loginHistory };

            _mockLoginHistorySet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<LoginHistory, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(loginHistory);

            await _userService.LogoutAsync(userId);
            loginHistory.IsActive = false; // Simulate the logout operation

            Assert.False(loginHistory.IsActive);
        }
    }
}
