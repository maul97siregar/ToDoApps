using Moq;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.ToDoServices;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Api.Tests.Services
{
    public class ToDoServiceTests
    {
        private readonly ToDoService _toDoService;
        private readonly List<ToDoItem> _toDoItems;

        public ToDoServiceTests()
        {
            // Initialize dummy data
            _toDoItems = new List<ToDoItem>
            {
                new ToDoItem { UserId = "1", Subject = "Test 1", Description = "Description 1", Status = 0, IsActive = false },
                new ToDoItem { UserId = "1", Subject = "Test 2", Description = "Description 2", Status = 0, IsActive = false }
            };

        }

        [Fact]
        public async Task CreateAsync_ShouldAddToDoItem_WhenValid()
        {
            var toDo = new ToDoItem { UserId = "1", Subject = "New Task", Description = "New Description", Status = 0 };

            var result = await _toDoService.CreateAsync(toDo);

            Assert.NotNull(result);
            Assert.Equal(toDo.Subject, result.Subject);
            Assert.Equal(3, _toDoItems.Count); // Verify that the new item was added
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnToDoItems_WhenValidUserId()
        {
            var userId = "1";
            var result = await _toDoService.GetAllAsync(userId);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenToDoItemExists()
        {
            var userId = "1";
            var toDoUpdate = new ToDoItem { Subject = "Updated Subject", Description = "Updated Description", Status = 1 };

            var result = await _toDoService.UpdateAsync(userId, toDoUpdate);

            Assert.True(result);
            var updatedItem = _toDoItems.FirstOrDefault(item => item.UserId == userId && item.Subject == "Updated Subject");
            Assert.NotNull(updatedItem);
        }

        [Fact]
        public async Task MarkAsync_ShouldReturnTrue_WhenToDoItemExists()
        {
            var userId = "1";
            var status = 1; // Status to mark
            var existingToDo = _toDoItems.First();

            var result = await _toDoService.MarkAsync(userId, status);

            Assert.True(result);
            Assert.Equal(status, existingToDo.Status);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenToDoItemExists()
        {
            var userId = "1";
            var existingToDo = _toDoItems.First();

            var result = await _toDoService.DeleteAsync(userId);

            Assert.True(result);
            Assert.True(existingToDo.IsActive); // Verify that the item is marked as deleted
        }
    }
}
