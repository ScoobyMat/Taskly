using Application.DTOs;
using Application.DTOs.TaskDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

public class TaskService : ITaskService
{
    private readonly ITaskItemRepository _repo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _current;

    public TaskService(ITaskItemRepository repo, IMapper mapper, ICurrentUserService current)
    {
        _repo = repo;
        _mapper = mapper;
        _current = current;
    }

    private Guid RequireUser()
    {
        if (!_current.IsAuthenticated || _current.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("Brak użytkownika w tokenie.");
        return _current.UserId;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllAsync()
    {
        var userId = RequireUser();
        var tasks = await _repo.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TaskItemDto>>(tasks);
    }

    public async Task<IEnumerable<TaskItemDto>> GetFilteredAsync(TaskFilter filter)
    {
        var userId = RequireUser();
        var tasks = await _repo.GetFilteredAsync(userId, filter);
        return _mapper.Map<IEnumerable<TaskItemDto>>(tasks);
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        var userId = RequireUser();
        var task = await _repo.GetByIdForUserAsync(id, userId);
        return task is null ? null : _mapper.Map<TaskItemDto>(task);
    }

    public async Task<Guid> CreateAsync(CreateTaskItemDto dto)
    {
        var userId = RequireUser();
        var task = _mapper.Map<TaskItem>(dto);
        task.UserId = userId;
        task.Status = TaskStatusEnum.New;
        task.CreatedAt = DateTime.UtcNow;

        await _repo.AddAsync(task);
        return task.Id;
    }

    public async Task<bool> UpdateAsync(UpdateTaskItemDto dto)
    {
        var userId = RequireUser();
        var entity = await _repo.GetByIdForUserAsync(dto.Id, userId);
        if (entity is null) return false;

        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var userId = RequireUser();
        var entity = await _repo.GetByIdForUserAsync(id, userId);
        if (entity is null) return false;

        await _repo.DeleteAsync(id, userId);
        return true;
    }
}
