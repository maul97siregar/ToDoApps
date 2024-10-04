using ToDoApp.Api.Models;

namespace ToDoApp.Api.Services.ToDoServices
{
    public interface IToDoService
    {
        Task<ToDoItem> CreateAsync(ToDoItem toDo);
        Task<IEnumerable<ToDoItem>> GetAllAsync(string userId);
        Task<bool> UpdateAsync(string userId, ToDoItem toDo);
        Task<bool> MarkAsync(string userId, int status);
        Task<bool> DeleteAsync(string userId);
    }
}
