using Application.DTOs.UserDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public sealed class UsersController : BaseApiController
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserService _userService;

    public UsersController(ICurrentUserService currentUser, IUserService userService)
    {
        _currentUser = currentUser;
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = _currentUser.UserId;
        var dto = await _userService.GetUserByIdAsync(userId);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UserUpdateDto dto)
    {
        var userId = _currentUser.UserId;
        await _userService.UpdateUserAsync(userId, dto);
        return NoContent();
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var userId = _currentUser.UserId;
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}