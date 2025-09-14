using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class DebugAuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DebugAuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-password")]
    public async Task<IActionResult> TestPassword([FromQuery] string email, [FromQuery] string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            return Ok(new { 
                UserFound = false, 
                Message = "User not found with email: " + email 
            });
        }

        var verificationResult = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        var hashLength = user.PasswordHash != null ? user.PasswordHash.Length : 0;
        var hashPrefix = user.PasswordHash != null && user.PasswordHash.Length >= 10 
            ? user.PasswordHash.Substring(0, 10) 
            : (user.PasswordHash ?? "");
        
        return Ok(new
        {
            UserFound = true,
            UserEmail = user.Email,
            UserUsername = user.Username,
            PasswordHash = user.PasswordHash,
            ProvidedPassword = password,
            VerificationResult = verificationResult,
            HashLength = hashLength,
            HashPrefix = hashPrefix
        });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        
        var result = users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            u.Role,
            PasswordHashPreview = u.PasswordHash != null 
                ? u.PasswordHash.Substring(0, Math.Min(20, u.PasswordHash.Length)) + "..." 
                : "",
            HashLength = u.PasswordHash != null ? u.PasswordHash.Length : 0
        });
        
        return Ok(result);
    }
}
