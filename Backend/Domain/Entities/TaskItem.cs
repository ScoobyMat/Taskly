using Domain.Enums;

namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.New;
    public TaskPriority? Priority { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    
    public string Category { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
