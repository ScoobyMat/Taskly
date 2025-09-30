using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Entities;
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
    public async Task Register_Succeeds_Returns_UserDto_And_HashesPassword()
    {
        var dto = new RegisterDto
        {
            FirstName = "Ana",
            LastName = "Nowak",
            UserName = "anowak",
            Email = "ana@example.com",
            Password = "Secret#123"
        };

        _userRepoMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _authService.Register(dto);

        result.Should().NotBeNull();
        result.Email.Should().Be(dto.Email);
        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == dto.Email &&
            u.UserName == dto.UserName &&
            u.Password != dto.Password
        )), Times.Once);
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


    [Fact]
    public async Task Login_ValidCredentials_Returns_Token_And_UserData()
    {
        var login = new LoginDto
        {
            Email = "ana@example.com",
            Password = "Secret#123"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Ana",
            LastName = "Nowak",
            UserName = "anowak",
            Email = login.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(login.Password)
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(login.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.CreateToken(user)).Returns("dummy.jwt.token");

        var auth = await _authService.Login(login);

        auth.Should().NotBeNull();
        auth.Token.Should().Be("dummy.jwt.token");
        auth.FirstName.Should().Be("Ana");
        auth.UserName.Should().Be("anowak");

        _tokenServiceMock.Verify(t => t.CreateToken(user), Times.Once);
    }

    [Theory]
    [InlineData("missing@example.com", "whatever", false)]
    [InlineData("test@example.com", "wrongPassword", true)]
    public async Task Login_InvalidCredentials_ThrowsUnauthorized(string email, string password, bool userExists)
    {
        User? user = null;
        if (userExists)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = email,
                UserName = "TestUser",
                Password = BCrypt.Net.BCrypt.HashPassword("correct")
            };
        }

        _userRepoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

        var loginDto = new LoginDto { Email = email, Password = password };

        var act = async () => await _authService.Login(loginDto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
                 .WithMessage("Invalid email or password");
    }


}
