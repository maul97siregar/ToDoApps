using Microsoft.AspNetCore.Components;
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
        [Inject]
        private ApiEndpoints ApiEndpoints { get; set; }
        public ToDoService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            ApiEndpoints = apiEndpoints;
        }

        public async Task<List<ToDoItemDto>> GetToDoListAsync(string userId)
        {

            return await _httpClient.GetFromJsonAsync<List<ToDoItemDto>>($"{ApiEndpoints.ToDo}/{userId}");
        }

        public async Task CreateToDoAsync(ToDoItemDto toDo)
        {
            await _httpClient.PostAsJsonAsync(ApiEndpoints.ToDo, toDo);
        }

        public async Task EditToDoAsync(string userId, ToDoItemDto toDo)
        {
            await _httpClient.PutAsJsonAsync($"{ApiEndpoints.ToDo}/edit/{userId}", toDo);
        }

        public async Task MarkToDoAsync(string userId, string activityId)
        {
            await _httpClient.PutAsync($"{ApiEndpoints.ToDo}/mark/{userId}", new StringContent(activityId));
        }

        public async Task DeleteToDoAsync(string userId, string activityId)
        {
            await _httpClient.DeleteAsync($"{ApiEndpoints.ToDo}/delete/{userId}");
        }
    }

}
