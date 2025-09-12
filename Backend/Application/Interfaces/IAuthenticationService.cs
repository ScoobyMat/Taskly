using Application.DTOs.Auth;
using Application.DTOs.UserDtos;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<UserDto> Register(RegisterDto registerDto);
    Task<AuthResponseDto> Login(LoginDto loginDto);
}
