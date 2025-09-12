using Application.DTOs.Auth;
using Application.DTOs.UserDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthenticationService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = _tokenService.CreateToken(user);

        return response;
    }

    public async Task<UserDto> Register(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
        {
            throw new ArgumentException("A user with this email address already exists");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = _mapper.Map<User>(registerDto);
        user.Password = hashedPassword;

        await _userRepository.AddAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

}
