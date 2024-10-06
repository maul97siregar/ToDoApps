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
            // Menggunakan URL relatif, dengan asumsi BaseAddress sudah disetel
            return await _httpClient.GetFromJsonAsync<List<ToDoItemDto>>($"api/ToDo/{userId}");
        }

        public async Task CreateToDoAsync(ToDoItemDto toDo)
        {
            // URL relatif
            await _httpClient.PostAsJsonAsync("api/ToDo", toDo);
        }

        public async Task EditToDoAsync(string userId, ToDoItemDto toDo)
        {
            // URL relatif
            await _httpClient.PutAsJsonAsync($"api/ToDo/edit/{userId}", toDo);
        }

        public async Task MarkToDoAsync(string userId, string activityId)
        {
            // Menggunakan StringContent jika API memerlukan body, namun untuk pengiriman ID saja bisa dikirim melalui query atau path
            await _httpClient.PutAsync($"api/ToDo/mark/{userId}", new StringContent(activityId));
        }

        public async Task DeleteToDoAsync(string userId, string activityId)
        {
            // URL relatif
            await _httpClient.DeleteAsync($"api/ToDo/delete/{userId}");
        }
    }
}
