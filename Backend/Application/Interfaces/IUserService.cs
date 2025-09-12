using Application.DTOs.UserDtos;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(Guid id);
}
