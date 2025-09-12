using Application.DTOs.UserDtos;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto> UpdateUserAsync(UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(Guid id);
}
