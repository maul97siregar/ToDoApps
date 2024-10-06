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
            return await _httpClient.PostAsJsonAsync("https://localhost:44351/api/User/register", user);
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44351/api/User/login", user);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            var loginResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("https://localhost:44351/api/User/logout", null);
        }
    }

}
