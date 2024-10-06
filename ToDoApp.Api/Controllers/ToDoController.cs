using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.ToDoServices;
using ToDoApp.Api.Services.UserServices;
using System;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoItem toDo)
        {
            try
            {
                var createdToDo = await _toDoService.CreateAsync(toDo);
                return CreatedAtAction(nameof(Create), new { userId = createdToDo.UserId }, createdToDo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            try
            {
                var toDoItems = await _toDoService.GetAllAsync(userId);
                return Ok(toDoItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("edit/{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] ToDoItem toDo)
        {
            try
            {
                var success = await _toDoService.UpdateAsync(userId, toDo);
                if (!success) return NotFound("To-Do item not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("mark/{userId}")]
        public async Task<IActionResult> Mark(string userId, [FromBody] int status)
        {
            try
            {
                var success = await _toDoService.MarkAsync(userId, status);
                if (!success) return NotFound("To-Do item not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var success = await _toDoService.DeleteAsync(userId);
                if (!success) return NotFound("To-Do item not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
