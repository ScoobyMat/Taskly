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
            // Zakładam, że masz MappingProfile z mapowaniami User <-> UserDto etc.
            cfg.AddProfile(new Application.Mappings.MappingProfile());
        });

        _mapper = config.CreateMapper();

        _userService = new UserService(_userRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnsUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "hashed_password" // required
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.UserName.Should().Be("testuser");
    }

    [Fact]
    public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        Func<Task> act = async () => await _userService.GetUserByIdAsync(userId);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found.");
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsListOfUserDtos()
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "user1",
                Email = "u1@example.com",
                FirstName = "Alice",
                LastName = "Smith",
                Password = "hashed_password1"
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "user2",
                Email = "u2@example.com",
                FirstName = "Bob",
                LastName = "Brown",
                Password = "hashed_password2"
            }
        };

        _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _userService.GetAllUsersAsync();

        result.Should().HaveCount(2);
        result.Should().ContainSingle(u => u.UserName == "user1");
        result.Should().ContainSingle(u => u.UserName == "user2");
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsers_ReturnsEmptyList()
    {
        _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        var result = await _userService.GetAllUsersAsync();

        result.Should().BeEmpty();
    }


    [Fact]
    public async Task DeleteUserAsync_UserExists_DeletesUser()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            UserName = "todelete",
            Email = "delete@example.com",
            FirstName = "Del",
            LastName = "User",
            Password = "hashed_password"
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.DeleteAsync(user)).Returns(Task.CompletedTask);

        await _userService.DeleteUserAsync(userId);

        _userRepoMock.Verify(r => r.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        Func<Task> act = async () => await _userService.DeleteUserAsync(userId);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found");
    }

    [Fact]
    public async Task UpdateUserAsync_UserExists_UpdatesUser()
    {
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "Old",
            LastName = "Name",
            UserName = "olduser",
            Email = "old@example.com",
            Password = "old_hashed_password"
        };

        var updateDto = new UserUpdateDto
        {
            Id = userId,
            FirstName = "New",
            LastName = "Name",
            UserName = "newuser",
            Email = "new@example.com"
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

        var result = await _userService.UpdateUserAsync(updateDto);

        result.Should().NotBeNull();
        result.UserName.Should().Be("newuser");
        //result.Email.Should().Be("new@example.com");

        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
    {
        var updateDto = new UserUpdateDto { Id = Guid.NewGuid() };

        _userRepoMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((User?)null);

        Func<Task> act = async () => await _userService.UpdateUserAsync(updateDto);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("User not found.");
    }
}
