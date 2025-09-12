using Application.DTOs;
using Application.DTOs.TaskDtos;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Taskly.Tests.Services;
public class TaskServiceTests
{
/*    private readonly Mock<ITaskItemRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _repoMock = new Mock<ITaskItemRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        _mapper = config.CreateMapper();

        _service = new TaskService(_repoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_AdminRole_ReturnsAllTasks()
    {
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description for task 1", UserId = Guid.NewGuid() },
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description for task 2", UserId = Guid.NewGuid() }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        var result = await _service.GetAllAsync(Guid.NewGuid(), UserRole.Admin);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_UserRole_ReturnsOnlyUserTasks()
    {
        var userId = Guid.NewGuid();

        var allTasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Own", Description = "Description", UserId = userId },
            new() { Id = Guid.NewGuid(), Title = "NotVisible", Description = "Description", UserId = Guid.NewGuid() }
        };

        var expectedTasks = allTasks.Where(t => t.UserId == userId).ToList();

        _repoMock.Setup(r => r.GetByUserId(userId)).ReturnsAsync(expectedTasks);

        var result = await _service.GetAllAsync(userId, UserRole.User);

        result.Should().HaveCount(expectedTasks.Count);
        result.Should().OnlyContain(t => t.UserId == userId);
    }

    [Fact]
    public async Task GetByIdAsync_AdminRole_CanAccessAnyTask()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Admin sees all",
            Description = "Description",
            UserId = Guid.NewGuid()
        };

        _repoMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);

        var result = await _service.GetByIdAsync(task.Id, Guid.NewGuid(), UserRole.Admin);

        result.Should().NotBeNull();
        result!.Id.Should().Be(task.Id);
    }

    [Fact]
    public async Task GetByIdAsync_UserRole_SeesOnlyOwnTasks()
    {
        var userId = Guid.NewGuid();
        var ownTask = new TaskItem { Id = Guid.NewGuid(), Title = "Own", Description = "Description", UserId = userId };
        var foreignTask = new TaskItem { Id = Guid.NewGuid(), Title = "Forbidden", Description = "Description", UserId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(ownTask.Id)).ReturnsAsync(ownTask);
        _repoMock.Setup(r => r.GetByIdAsync(foreignTask.Id)).ReturnsAsync(foreignTask);

        var res1 = await _service.GetByIdAsync(ownTask.Id, userId, UserRole.User);
        var res2 = await _service.GetByIdAsync(foreignTask.Id, userId, UserRole.User);

        res1.Should().NotBeNull();
        res2.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_Admin_CanAssign()
    {
        var dto = new CreateTaskItemDto
        {
            Title = "AdminTask",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        var id = await _service.CreateAsync(dto, Guid.NewGuid(), UserRole.Admin);

        id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateAsync_User_AssignedUserId_IsIgnored()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateTaskItemDto
        {
            Title = "UserTask",
            Description = "Description",
            CategoryId = Guid.NewGuid(),
        };

        TaskItem? savedTask = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(task => savedTask = task)
            .Returns(Task.CompletedTask);

        var id = await _service.CreateAsync(dto, userId, UserRole.User);

        savedTask.Should().NotBeNull();
        savedTask!.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task UpdateAsync_Admin_CanUpdateAny()
    {
        var taskId = Guid.NewGuid();
        var dto = new UpdateTaskItemDto { Id = taskId, Title = "Updated", Description = "Changed", CategoryId = Guid.NewGuid() };
        var existing = new TaskItem { Id = taskId, Title = "Old", Description = "Old", UserId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(dto, Guid.NewGuid(), UserRole.Admin);

        existing.Title.Should().Be("Updated");
        existing.Description.Should().Be("Changed");
        existing.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_User_CannotUpdateOthersTask()
    {
        var userId = Guid.NewGuid();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "x", Description = "x", UserId = Guid.NewGuid() };
        var dto = new UpdateTaskItemDto { Id = task.Id, Title = "new", Description = "new", CategoryId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);

        var act = async () => await _service.UpdateAsync(dto, userId, UserRole.User);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdateAsync_TaskNotFound_ThrowsException()
    {
        var dto = new UpdateTaskItemDto { Id = Guid.NewGuid(), Title = "x", Description = "x", CategoryId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((TaskItem?)null);

        var act = async () => await _service.UpdateAsync(dto, Guid.NewGuid(), UserRole.Admin);

        await act.Should().ThrowAsync<Exception>().WithMessage("Task not found");
    }

    [Fact]
    public async Task DeleteAsync_User_CannotDeleteOthersTask()
    {
        var userId = Guid.NewGuid();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "ToDelete", Description = "x", UserId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);

        var act = async () => await _service.DeleteAsync(task.Id, userId, UserRole.User);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_Admin_CanDelete()
    {
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "AdminDelete", Description = "x", UserId = Guid.NewGuid() };

        _repoMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);
        _repoMock.Setup(r => r.DeleteAsync(task.Id)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(task.Id, Guid.NewGuid(), UserRole.Admin);

        _repoMock.Verify(r => r.DeleteAsync(task.Id), Times.Once);
    }

    [Fact]
    public async Task GetFilteredAsync_FiltersByTitle()
    {
        var userId = Guid.NewGuid();
        var filter = new TaskFilter { Title = "Test" };

        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Test task", Description = "Description", UserId = userId },
            new TaskItem { Id = Guid.NewGuid(), Title = "Other", Description = "Description", UserId = userId }
        };

        var expected = tasks.Where(t => t.Title.Contains("Test")).ToList();

        _repoMock.Setup(r => r.GetFilteredAsync(userId, UserRole.User, filter))
                 .ReturnsAsync(expected);

        var result = await _service.GetFilteredAsync(userId, UserRole.User, filter);

        result.Should().HaveCount(expected.Count);
        result.Should().OnlyContain(t => t.Title.Contains("Test"));
    }*/
}
