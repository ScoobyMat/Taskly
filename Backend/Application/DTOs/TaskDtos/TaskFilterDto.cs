using Domain.Enums;

namespace Application.DTOs.TaskDtos
{
    public class TaskFilterDto
    {
        public string? Title { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public bool? SortByDueDateAsc { get; set; }
    }

}
