using DataAccess.Models;


namespace DataAccess.DTOs
{
    public class TaskFilterDTO
    {
        public Models.TaskStatus? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority? Priority { get; set; }
        public string SortBy { get; set; } = "None";// "DueDate" or "Priority"
    }
}
