using System.ComponentModel.DataAnnotations;

namespace ToDoApp.UI.DTOs
{
    public class UserDto
    {
        [Required]
        [StringLength(16, ErrorMessage = "Identifier too long (16 character limit).")]
        public string? UserId { get; set; }

        public string? Password { get; set; }
    }

    public class UserRegistrationDto
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public List<ToDoItemDto> ToDoItems { get; set; } = new List<ToDoItemDto>();
    }

    public class UserLoginDto
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public UserLoginData Data { get; set; }
    }

    public class UserLoginData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class Tracking
    {
        public DateTime? CreatedDate { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

}
