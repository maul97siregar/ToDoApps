using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApp.UI.DTOs;


namespace ToDoApp.UI.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        public ApiEndpoints ApiEndpoints { get; set; }

        public UserService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            ApiEndpoints = apiEndpoints;
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserRegistrationDto user)
        {
            Console.WriteLine(JsonSerializer.Serialize(user));
            Console.WriteLine(JsonSerializer.Serialize(ApiEndpoints));
            return await _httpClient.PostAsJsonAsync($"{ApiEndpoints.User}/register", user);
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginDto user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiEndpoints.User}/login", user);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return null;
            var loginResponse = await response.Content.ReadFromJsonAsync<UserLoginResponse>();

            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync($"{ApiEndpoints.User}/logout", null);
        }
    }

}
