using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoApp.UI.DTOs;

namespace ToDoApp.UI.Services
{
    public class ToDoService
    {
        private readonly HttpClient _httpClient;

        public ToDoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ToDoItemDto>> GetToDoListAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<ToDoItemDto>>($"https://localhost:44351/api/ToDo/testUser");
        }

        public async Task CreateToDoAsync(ToDoItemDto toDo)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:44351/api/ToDO", toDo);
        }

        public async Task EditToDoAsync(string userId, ToDoItemDto toDo)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:44351/api/ToDo/edit/{userId}", toDo);
        }

        public async Task MarkToDoAsync(string userId, string activityId)
        {
            await _httpClient.PutAsync($"https://localhost:44351/api/ToDO/mark/{userId}", new StringContent(activityId));
        }

        public async Task DeleteToDoAsync(string userId, string activityId)
        {
            await _httpClient.DeleteAsync($"https://localhost:44351/api/ToDO/delete/{userId}");
        }
    }

}
