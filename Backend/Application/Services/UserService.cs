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
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        await _userRepository.DeleteAsync(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        if (users == null)
        {
            users = new List<User>();
        }

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return userDtos;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
       
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserAsync(UserUpdateDto userUpdateDto)
{
    var user = await _userRepository.GetByIdAsync(userUpdateDto.Id);

    if (user == null)
    {
        throw new KeyNotFoundException("User not found.");
    }

    _mapper.Map(userUpdateDto, user);

    await _userRepository.UpdateAsync(user);

    var userDto = _mapper.Map<UserDto>(user);

    return userDto;
}
}
