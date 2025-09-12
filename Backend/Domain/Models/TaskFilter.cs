using Domain.Enums;

namespace Domain.Models
{
    public class TaskFilter
    {
        public string? Title { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public string? Category { get; set; }
        public string SortByDueDate { get; set; } = "asc";
    }
}
