using Application.DTOs;
using Application.DTOs.TaskDtos;
using Domain.Models;

public interface ITaskService
{
    Task<IEnumerable<TaskItemDto>> GetAllAsync();
    Task<IEnumerable<TaskItemDto>> GetFilteredAsync(TaskFilter filter);
    Task<TaskItemDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateTaskItemDto dto);
    Task<bool> UpdateAsync(UpdateTaskItemDto dto);
    Task<bool> DeleteAsync(Guid id);
}

