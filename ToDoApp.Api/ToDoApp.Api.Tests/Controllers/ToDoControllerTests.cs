using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.Api.Controllers;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.ToDoServices;
using Xunit;

namespace ToDoApp.Api.ToDoApp.Api.Tests.Controllers
{
    public class ToDoControllerTests
    {
        private readonly Mock<IToDoService> _mockService;
        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _mockService = new Mock<IToDoService>();
            _controller = new ToDoController(_mockService.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedResult_WhenValidToDo()
        {
            // Arrange
            var toDoItem = new ToDoItem { UserId = "1", Subject = "New Task", Description = "Task Description" };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<ToDoItem>())).ReturnsAsync(toDoItem);

            // Act
            var result = await _controller.Create(toDoItem);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(toDoItem.UserId, createdResult.RouteValues["userId"]);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WhenValidUserId()
        {
            // Arrange
            var userId = "1";
            var toDoItems = new List<ToDoItem>
            {
                new ToDoItem { UserId = userId, Subject = "Task 1", IsActive = false },
                new ToDoItem { UserId = userId, Subject = "Task 2", IsActive = false }
            };

            _mockService.Setup(s => s.GetAllAsync(userId)).ReturnsAsync(toDoItems);

            // Act
            var result = await _controller.GetAll(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IEnumerable<ToDoItem>>(okResult.Value);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenToDoItemExists()
        {
            // Arrange
            var userId = "1";
            var toDoItem = new ToDoItem { Subject = "Updated Task" };
            _mockService.Setup(s => s.UpdateAsync(userId, toDoItem)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(userId, toDoItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Mark_ShouldReturnNoContent_WhenToDoItemExists()
        {
            // Arrange
            var userId = "1";
            var status = 1; // Status yang ingin ditandai
            _mockService.Setup(s => s.MarkAsync(userId, status)).ReturnsAsync(true);

            // Act
            var result = await _controller.Mark(userId, status);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenToDoItemExists()
        {
            // Arrange
            var userId = "1";
            _mockService.Setup(s => s.DeleteAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
