using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.Models
{
    public class User : Tracking
    {
        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public ICollection<ToDoItem> ToDoItems { get; set; }
    }

    public class LoginModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

}
