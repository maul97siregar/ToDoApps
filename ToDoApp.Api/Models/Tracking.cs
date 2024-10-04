namespace ToDoApp.Api.Models
{
    public class Tracking
    {
        public DateTime? CreatedDate { get; set; }
        //public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
    public class LoginHistory : Tracking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string Token { get; set; }
    }

}
