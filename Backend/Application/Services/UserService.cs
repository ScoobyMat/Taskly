using Application.DTOs.UserDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found");

        await _userRepository.DeleteAsync(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User not found.");

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }


    public async Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found.");

        _mapper.Map(userUpdateDto, user);
        if (!string.IsNullOrWhiteSpace(userUpdateDto.Password))
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdateDto.Password);
    }

    await _userRepository.UpdateAsync(user);
    return _mapper.Map<UserDto>(user);
    }
}
