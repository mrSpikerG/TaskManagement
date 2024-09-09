using DataAccess.Models;

namespace DataAccess.DTOs
{
    public class TaskDTO
    {
        public string Title { get; set; } 

        public string Description { get; set; } 

        public DateTime? DueDate { get; set; }

        public Models.TaskStatus Status { get; set; } 

        public TaskPriority Priority { get; set; } 
    }
}
