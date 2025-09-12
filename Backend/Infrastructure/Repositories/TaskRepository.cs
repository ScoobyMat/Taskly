using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class TaskRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(TaskItem taskItem)
    {
        await _context.TaskItems.AddAsync(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TaskItem>> GetFilteredAsync(Guid userId, TaskFilter filter)
    {
        IQueryable<TaskItem> query = _context.TaskItems
            .AsNoTracking()
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        if (filter.Priority.HasValue)
            query = query.Where(t => t.Priority == filter.Priority.Value);

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(t => t.Category == filter.Category);

        query = filter.SortByDueDate?.ToLower() switch
        {
            "desc" => query.OrderByDescending(t => t.DueDate),
            _ => query.OrderBy(t => t.DueDate)
        };

        return await query.ToListAsync();
    }

    public Task<TaskItem?> GetByIdForUserAsync(Guid id, Guid userId)
    {
        return _context.TaskItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task != null)
        {
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
