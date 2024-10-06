using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Models;
using ToDoApp.Api.Utils;
using System;

namespace ToDoApp.Api.Services.ToDoServices
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;

        public ToDoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> Sequencer(ToDoItem toDo)
        {
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

            return activitiesNo;
        }

        public async Task<ToDoItem> CreateAsync(ToDoItem toDo)
        {
            try
            {
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
                return todoItem;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the To-Do item. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(string userId)
        {
            try
            {
                return await _context.ToDoItems.Where(t => t.UserId == userId && !t.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the To-Do items.", ex);
            }
        }

        public async Task<ToDoItem> GetData(string userId)
        {
            try
            {
                return await _context.ToDoItems.Where(t => t.UserId == userId && !t.IsActive).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the To-Do item.", ex);
            }
        }

        public async Task<bool> UpdateAsync(string userId, ToDoItem toDo)
        {
            try
            {
                var existingToDo = await _context.ToDoItems.Where(t => t.UserId == userId && t.ActivitiesNo == toDo.ActivitiesNo && !t.IsActive).FirstOrDefaultAsync();
                if (existingToDo == null) return false;

                existingToDo.Subject = toDo.Subject;
                existingToDo.Description = toDo.Description;
                existingToDo.Status = toDo.Status;
                existingToDo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the To-Do item.", ex);
            }
        }

        public async Task<bool> MarkAsync(string userId, int status)
        {
            try
            {
                var todo = await GetData(userId);
                if (todo == null) return false;

                todo.Status = status;
                todo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while marking the To-Do item.", ex);
            }
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            try
            {
                var todo = await GetData(userId);
                if (todo == null) return false;

                todo.IsActive = true;
                todo.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the To-Do item.", ex);
            }
        }
    }
}
