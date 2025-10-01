using Application.DTOs.UserDtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Taskly.Tests.Services;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new Application.Mappings.MappingProfile());
        });

        _mapper = config.CreateMapper();

        _userService = new UserService(_userRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnsUserDto()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Test",
            LastName = "Test",
            UserName = "Test",
            Email = "test@example.com",
            Password = "password"
        };
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(userId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.UserName.Should().Be("Test");
        _userRepoMock.Verify(r => r.GetByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var act = async () => await _userService.GetUserByIdAsync(userId);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found.");
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesAndReturnsUserDto()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Old",
            LastName = "Name",
            UserName = "jdoe",
            Email = "john@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("oldpass")
        };
        var updateDto = new UserUpdateDto
        {
            FirstName = "New",
            LastName = "Name",
            UserName = "johnny",
            Email = "johnny@example.com",
            Password = "newpass"
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _userService.UpdateUserAsync(userId, updateDto);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.UserName.Should().Be("johnny");

        BCrypt.Net.BCrypt.Verify("newpass", user.Password).Should().BeTrue();
        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
    }


    [Fact]
    public async Task UpdateUserAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        var updateDto = new UserUpdateDto { FirstName = "X" };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var act = async () => await _userService.UpdateUserAsync(userId, updateDto);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found.");
    }

    [Fact]
    public async Task DeleteUserAsync_CallsRepositoryDelete()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Test",
            LastName = "Test",
            UserName = "TestUser",
            Email = "testuser@example.com",
            Password = "password"
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.DeleteAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        await _userService.DeleteUserAsync(userId);

        _userRepoMock.Verify(r => r.DeleteAsync(It.Is<User>(u => u.Id == userId)), Times.Once);
    }
}
