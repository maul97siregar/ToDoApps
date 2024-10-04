using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Models;
using ToDoApp.Api.Services.UserServices;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.RegisterAsync(user);
                return CreatedAtAction(nameof(Register), new { userId = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var token = await _userService.LoginAsync(login);
                if (token == null) return Unauthorized("Invalid credentials.");

                return Ok(new { data = token});
            }
            catch (Exception ex)
            {
                return Unauthorized($"{ex.Message}");
            }
        }

        //[Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string userId)
        {
            try
            {
                await _userService.LogoutAsync(userId); // Call the LogoutAsync method
                return Ok("Successfully logged out.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
