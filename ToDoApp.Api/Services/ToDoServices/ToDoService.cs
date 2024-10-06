using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Models;
using ToDoApp.Api.Utils;
using Microsoft.Extensions.Logging;
using System;

namespace ToDoApp.Api.Services.ToDoServices
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(AppDbContext context, ILogger<ToDoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> Sequencer(ToDoItem toDo)
        {
            try
            {
                _logger.LogInformation("Sequencer started for User {UserId} at {Time}", toDo.UserId, DateTime.Now);
                var currentDate = DateTime.Now.ToString("yyyyMMdd");

                var countForToday = await _context.ToDoItems
                    .Where(t => t.UserId == toDo.UserId && t.CreatedDate == DateTime.Now.Date)
                    .CountAsync();

                int nextSequenceNo = countForToday + 1;

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();

                string randomAlphanumeric = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                var activitiesNo = $"AC-{randomAlphanumeric}{nextSequenceNo.ToString().ToUpper().PadLeft(4, '0')}";

                _logger.LogInformation("Sequencer finished for User {UserId} with sequence {Sequence} at {Time}", toDo.UserId, activitiesNo, DateTime.Now);
                return activitiesNo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Sequencer for User {UserId} at {Time}", toDo.UserId, DateTime.Now);
                throw;
            }
        }

        public async Task<ToDoItem> CreateAsync(ToDoItem toDo)
        {
            try
            {
                _logger.LogInformation("Creating ToDoItem for User {UserId} at {Time}", toDo.UserId, DateTime.Now);

                var todoItem = new ToDoItem
                {
                    UserId = toDo.UserId,
                    Subject = toDo.Subject,
                    Description = toDo.Description,
                    Status = toDo.Status,
                    ActivitiesNo = await Sequencer(toDo),
                    CreatedDate = DateTime.Now,
                };

                _context.ToDoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("ToDoItem created for User {UserId} at {Time}", toDo.UserId, DateTime.Now);
                return todoItem;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while creating ToDoItem for User {UserId} at {Time}", toDo.UserId, DateTime.Now);
                throw new Exception("An error occurred while creating the To-Do item. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving all ToDoItems for User {UserId} at {Time}", userId, DateTime.Now);
                return await _context.ToDoItems.Where(t => t.UserId == userId && !t.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all ToDoItems for User {UserId} at {Time}", userId, DateTime.Now);
                throw new Exception("An error occurred while retrieving the To-Do items.", ex);
            }
        }

        public async Task<ToDoItem> GetData(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving ToDoItem for User {UserId} at {Time}", userId, DateTime.Now);
                return await _context.ToDoItems.Where(t => t.UserId == userId && !t.IsActive).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ToDoItem for User {UserId} at {Time}", userId, DateTime.Now);
                throw new Exception("An error occurred while retrieving the To-Do item.", ex);
            }
        }

        public async Task<bool> UpdateAsync(string userId, ToDoItem toDo)
        {
            try
            {
                _logger.LogInformation("Updating ToDoItem {ActivitiesNo} for User {UserId} at {Time}", toDo.ActivitiesNo, userId, DateTime.Now);

                var existingToDo = await _context.ToDoItems
                    .Where(t => t.UserId == userId && t.ActivitiesNo == toDo.ActivitiesNo && !t.IsActive)
                    .FirstOrDefaultAsync();

                if (existingToDo == null)
                {
                    _logger.LogWarning("ToDoItem {ActivitiesNo} not found for User {UserId} at {Time}", toDo.ActivitiesNo, userId, DateTime.Now);
                    return false;
                }

                existingToDo.Subject = toDo.Subject;
                existingToDo.Description = toDo.Description;
                existingToDo.Status = toDo.Status;
                existingToDo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                _logger.LogInformation("ToDoItem {ActivitiesNo} updated for User {UserId} at {Time}", toDo.ActivitiesNo, userId, DateTime.Now);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while updating ToDoItem {ActivitiesNo} for User {UserId} at {Time}", toDo.ActivitiesNo, userId, DateTime.Now);
                throw new Exception("An error occurred while updating the To-Do item.", ex);
            }
        }

        public async Task<bool> MarkAsync(string userId, int status)
        {
            try
            {
                _logger.LogInformation("Marking ToDoItem for User {UserId} with status {Status} at {Time}", userId, status, DateTime.Now);

                var todo = await GetData(userId);
                if (todo == null)
                {
                    _logger.LogWarning("No ToDoItem found for User {UserId} at {Time}", userId, DateTime.Now);
                    return false;
                }

                todo.Status = status;
                todo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                _logger.LogInformation("ToDoItem marked for User {UserId} with status {Status} at {Time}", userId, status, DateTime.Now);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while marking ToDoItem for User {UserId} with status {Status} at {Time}", userId, status, DateTime.Now);
                throw new Exception("An error occurred while marking the To-Do item.", ex);
            }
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Deleting ToDoItem for User {UserId} at {Time}", userId, DateTime.Now);

                var todo = await GetData(userId);
                if (todo == null)
                {
                    _logger.LogWarning("No ToDoItem found for User {UserId} at {Time}", userId, DateTime.Now);
                    return false;
                }

                todo.IsActive = true;
                todo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                _logger.LogInformation("ToDoItem deleted for User {UserId} at {Time}", userId, DateTime.Now);
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting ToDoItem for User {UserId} at {Time}", userId, DateTime.Now);
                throw new Exception("An error occurred while deleting the To-Do item.", ex);
            }
        }
    }
}
