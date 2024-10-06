using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoApp.UI.DTOs;

namespace ToDoApp.UI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserRegistrationDto user)
        {
            // URL relatif
            return await _httpClient.PostAsJsonAsync("api/User/register", user);
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginDto user)
        {
            // URL relatif
            var response = await _httpClient.PostAsJsonAsync("api/User/login", user);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;

            var loginResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            // URL relatif
            await _httpClient.PostAsync("api/User/logout", null);
        }
    }
}
