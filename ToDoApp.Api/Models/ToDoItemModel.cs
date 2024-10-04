﻿using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.Models
{
    public class ToDoItem : Tracking
    {
        [Key]
        public string ActivitiesNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
    }


    public enum ToDoStatus
    {
        Unmarked = 1,
        Done = 2,
        Canceled = 3
    }

}
