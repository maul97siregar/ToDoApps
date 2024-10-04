using ToDoApp.Api.Models;

namespace ToDoApp.Api.Services.UserServices
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<LoginHistory> LoginAsync(LoginModel login);
        Task LogoutAsync(string userId);
    }
}
