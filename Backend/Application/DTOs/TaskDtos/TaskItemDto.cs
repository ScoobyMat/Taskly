using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class TaskItemDto : IMap
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TaskStatusEnum Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public string Category { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TaskItem, TaskItemDto>();
    }
}
