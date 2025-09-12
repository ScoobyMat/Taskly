using Domain.Entities;
using Domain.Models;

public interface ITaskItemRepository
{
    Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<TaskItem>> GetFilteredAsync(Guid userId, TaskFilter filter);
    Task<TaskItem?> GetByIdForUserAsync(Guid id, Guid userId);
    Task AddAsync(TaskItem taskItem);
    Task UpdateAsync(TaskItem taskItem);
    Task DeleteAsync(Guid id, Guid userId);
}


