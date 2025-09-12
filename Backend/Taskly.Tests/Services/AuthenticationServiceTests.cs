using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Taskly.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly IMapper _mapper;
    private readonly AuthenticationService _authService;

    public AuthenticationServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
        _authService = new AuthenticationService(_userRepoMock.Object, _tokenServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var password = "password123";
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = hashed,
            UserName = "TestUser"
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.CreateToken(user)).Returns("valid.jwt.token");

        var loginDto = new LoginDto { Email = user.Email, Password = password };

        var result = await _authService.Login(loginDto);

        result.Should().NotBeNull();
        result.Token.Should().Be("valid.jwt.token");
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ThrowsUnauthorizedAccessException()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("wrong@example.com")).ReturnsAsync((User?)null);

        var loginDto = new LoginDto { Email = "wrong@example.com", Password = "anything" };

        var act = async () => await _authService.Login(loginDto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password");
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ThrowsUnauthorizedAccessException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("correct"),
            UserName = "TestUser"
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

        var loginDto = new LoginDto { Email = user.Email, Password = "wrongPassword" };

        var act = async () => await _authService.Login(loginDto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password");
    }

    [Fact]
    public async Task Register_WithNewEmail_CreatesUser()
    {
        var registerDto = new RegisterDto
        {
            Email = "new@example.com",
            Password = "Secure123!",
            UserName = "newuser",
            FirstName = "John",
            LastName = "Doe"
        };

        _userRepoMock.Setup(r => r.ExistsByEmailAsync(registerDto.Email))
            .ReturnsAsync(false);

        User? savedUser = null;

        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u)
            .Returns(Task.CompletedTask);

        var result = await _authService.Register(registerDto);

        //result.Email.Should().Be(registerDto.Email);
        result.UserName.Should().Be(registerDto.UserName);

        savedUser.Should().NotBeNull();
        savedUser!.FirstName.Should().NotBeNullOrEmpty();
        savedUser.LastName.Should().NotBeNullOrEmpty();  
        savedUser.Email.Should().Be(registerDto.Email);
        savedUser.UserName.Should().Be(registerDto.UserName);
        savedUser.Password.Should().NotBeNullOrWhiteSpace();
        BCrypt.Net.BCrypt.Verify(registerDto.Password, savedUser.Password).Should().BeTrue();

        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Register_WithExistingEmail_ThrowsArgumentException()
    {
        var registerDto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "somepassword",
            UserName = "existing"
        };

        _userRepoMock.Setup(r => r.ExistsByEmailAsync(registerDto.Email)).ReturnsAsync(true);

        var act = async () => await _authService.Register(registerDto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("A user with this email address already exists");
    }

}
