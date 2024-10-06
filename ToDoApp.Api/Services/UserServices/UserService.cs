using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Models;
using ToDoApp.Api.Utils;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ToDoApp.Api.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        private async Task<LoginHistory> TrackUserLogin(User user)
        {
            var timeoutMinutes = int.Parse(_config["Jwt:ExpiresInMinutes"]);
            var timeout = TimeSpan.FromMinutes(timeoutMinutes);

            var lastLogin = await _context.LoginHistories
                .Where(u => u.UserId == user.UserId)
                .OrderByDescending(u => u.LoginTime)
                .FirstOrDefaultAsync();

            //if (lastLogin != null && (DateTime.Now - lastLogin.LoginTime) < timeout && lastLogin.IsActive) throw new Exception("User is already logged in. Please log out before logging in again.");

            var token = GenerateToken(user);
            var loginHistory = new LoginHistory
            {
                UserId = user.UserId,
                LoginTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                IsActive = true,
                Token = token
            };

            _context.LoginHistories.Add(loginHistory);
            await _context.SaveChangesAsync();

            return loginHistory;
        }


        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<User> RegisterAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while registering the user. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred. Please try again later.", ex);
            }
        }

        public async Task<LoginHistory> LoginAsync(LoginModel login)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == login.UserId && u.Password == login.Password);
                if (user == null) throw new Exception("Invalid username or password.");

                // Track user login
                var response = await TrackUserLogin(user);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task LogoutAsync(string userId)
        {
            try
            {
                // Get the last login history for the user
                var lastLogin = await _context.LoginHistories
                    .Where(u => u.UserId == userId && u.IsActive)
                    .OrderByDescending(u => u.LoginTime)
                    .FirstOrDefaultAsync();
                if (lastLogin == null) throw new Exception("Data Not Found");


                if (lastLogin != null)
                {
                    lastLogin.IsActive = false;
                    lastLogin.ModifiedDate = DateTime.Now;
                    _context.LoginHistories.Update(lastLogin);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { }
        }
    }
}
