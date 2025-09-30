using Application.DTOs;
using Application.DTOs.TaskDtos;
using Application.Interfaces;
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
    private readonly Mock<ITaskItemRepository> _repoMock = new();
    private readonly Mock<ICurrentUserService> _currentMock = new();
    private readonly IMapper _mapper;
    private readonly TaskService _service;
    private readonly Guid _userId = Guid.NewGuid();

    public TaskServiceTests()
    {
        var cfg = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = cfg.CreateMapper();

        _currentMock.SetupGet(c => c.UserId).Returns(_userId);
        _currentMock.SetupGet(c => c.IsAuthenticated).Returns(true);

        _service = new TaskService(_repoMock.Object, _mapper, _currentMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTasks_ForCurrentUser()
    {
        var list = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Test A", Description = "Test A", Status = TaskStatusEnum.New, Category = "Test", UserId = _userId },
            new TaskItem { Id = Guid.NewGuid(), Title = "Test B", Description = "Test B", Status = TaskStatusEnum.New, Category = "Test", UserId = _userId },
        };

        _repoMock.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(list);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Select(x => x.Title).Should().Contain(new[] { "Test A", "Test B" });
    }

    [Fact]
    public async Task GetFilteredAsync_PassesFilter_ToRepository()
    {
        var filter = new TaskFilter { Title = "Test", Priority = TaskPriority.High, SortByDueDate = "desc" };
        var list = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Test", Description = "x", Status = TaskStatusEnum.InProgress, Priority = TaskPriority.High, Category = "Test", UserId = _userId }
        };

        _repoMock.Setup(r => r.GetFilteredAsync(_userId, filter)).ReturnsAsync(list);

        var result = await _service.GetFilteredAsync(filter);

        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Test");
        _repoMock.Verify(r => r.GetFilteredAsync(_userId, filter), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var entity = new TaskItem { Id = id, Title = "T", Description = "D", Status = TaskStatusEnum.New, Category = "c", UserId = _userId };

        _repoMock.Setup(r => r.GetByIdForUserAsync(id, _userId)).ReturnsAsync(entity);

        var dto = await _service.GetByIdAsync(id);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdForUserAsync(id, _userId)).ReturnsAsync((TaskItem?)null);

        var dto = await _service.GetByIdAsync(id);

        dto.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_MapsAndPersists_ForCurrentUser_ReturnsNewId()
    {
        var dto = new CreateTaskItemDto
        {
            Title = "New",
            Description = "Desc",
            Priority = TaskPriority.Medium,
            Category = "work"
        };

        TaskItem? captured = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                 .Callback<TaskItem>(t => captured = t)
                 .Returns(Task.CompletedTask);

        var newId = await _service.CreateAsync(dto);

        newId.Should().NotBe(Guid.Empty);
        captured.Should().NotBeNull();
        captured!.Title.Should().Be("New");
        captured.UserId.Should().Be(_userId);
    }

    [Fact]
    public async Task UpdateAsync_WhenFound_Updates_And_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var existing = new TaskItem
        {
            Id = id,
            Title = "Old",
            Description = "Old D",
            Status = TaskStatusEnum.New,
            Priority = TaskPriority.Low,
            Category = "c",
            UserId = _userId
        };

        var update = new UpdateTaskItemDto
        {
            Id = id,
            Title = "New title",
            Description = "New desc",
            Status = TaskStatusEnum.Done,
            Priority = TaskPriority.High,
            Category = "x"
        };

        _repoMock.Setup(r => r.GetByIdForUserAsync(id, _userId)).ReturnsAsync(existing);

        TaskItem? captured = null;
        _repoMock
            .Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(t => captured = t)
            .Returns(Task.CompletedTask);

        var ok = await _service.UpdateAsync(update);

        ok.Should().BeTrue();

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);

        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);
        captured.Title.Should().Be("New title");
        captured.Description.Should().Be("New desc");
        captured.Status.Should().Be(TaskStatusEnum.Done);
        captured.Priority.Should().Be(TaskPriority.High);
        captured.Category.Should().Be("x");
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ReturnsFalse()
    {
        var update = new UpdateTaskItemDto
        {
            Id = Guid.NewGuid(),
            Title = "X",
            Description = "Y",
            Status = TaskStatusEnum.New,
            Category = "c"
        };

        _repoMock.Setup(r => r.GetByIdForUserAsync(update.Id, _userId)).ReturnsAsync((TaskItem?)null);

        var ok = await _service.UpdateAsync(update);

        ok.Should().BeFalse();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ReturnsTrue_And_CallsDelete()
    {
        var id = Guid.NewGuid();
        var entity = new TaskItem { Id = id, Title = "Test", Description = "Test", Category = "Test", UserId = _userId };

        _repoMock.Setup(r => r.GetByIdForUserAsync(id, _userId)).ReturnsAsync(entity);
        _repoMock.Setup(r => r.DeleteAsync(id, _userId)).Returns(Task.CompletedTask);

        var ok = await _service.DeleteAsync(id);

        ok.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(id, _userId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ReturnsFalse()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdForUserAsync(id, _userId)).ReturnsAsync((TaskItem?)null);

        var ok = await _service.DeleteAsync(id);

        ok.Should().BeFalse();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }
}
