using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.TaskDtos
{
    public class UpdateTaskItemDto : IMap
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        public TaskStatusEnum Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string Category { get; set; } = default!;

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<UpdateTaskItemDto, TaskItem>();
        }
    }
}
