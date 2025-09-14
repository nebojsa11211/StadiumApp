using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("search")]
    public async Task<ActionResult<UserListDto>> GetUsers([FromBody] UserFilterDto filter)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.GetUsersAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _authService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.CreateUserAsync(createUserDto);
        if (result == null)
        {
            return BadRequest("User with this email or username already exists");
        }

        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.UpdateUserAsync(id, updateUserDto);
        if (result == null)
        {
            return BadRequest("User not found or email/username already taken");
        }

        return Ok(result);
    }

    [HttpPut("{id}/password")]
    public async Task<ActionResult> ChangeUserPassword(int id, [FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.ChangeUserPasswordAsync(id, changePasswordDto);
        if (!result)
        {
            return BadRequest("User not found");
        }

        return Ok(new { message = "Password changed successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await _authService.DeleteUserAsync(id);
        if (!result)
        {
            return BadRequest("User not found or cannot delete the last admin");
        }

        return Ok(new { message = "User deleted successfully" });
    }

    [HttpGet("online-statistics")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> GetOnlineUserStatistics()
    {
        try
        {
            // For now, return mock data. In a real implementation, this would track active sessions
            var statistics = new
            {
                TotalOnline = 87,
                StaffOnline = 12,
                CustomersOnline = 75
            };

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error getting online user statistics" });
        }
    }
}