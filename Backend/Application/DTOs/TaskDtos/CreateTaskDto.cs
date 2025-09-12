using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateTaskItemDto : IMap
{
    [Required]
    public string Title { get; set; } = default!;
    [Required]
    public string Description { get; set; } = default!;
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    [Required]
    public string Category { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateTaskItemDto, TaskItem>();
    }
}
