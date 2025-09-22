using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Controller for security monitoring and management
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SecurityController : ControllerBase
{
    private readonly IBruteForceProtectionService _bruteForceService;
    private readonly ICentralizedLoggingClient _loggingClient;
    private readonly ILogger<SecurityController> _logger;

    public SecurityController(
        IBruteForceProtectionService bruteForceService,
        ICentralizedLoggingClient loggingClient,
        ILogger<SecurityController> logger)
    {
        _bruteForceService = bruteForceService;
        _loggingClient = loggingClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets rate limiting and security statistics
    /// </summary>
    /// <returns>Security statistics</returns>
    [HttpGet("stats")]
    public async Task<ActionResult<RateLimitingStats>> GetSecurityStats()
    {
        try
        {
            var stats = await _bruteForceService.GetStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving security statistics");
            return StatusCode(500, new { error = "Failed to retrieve security statistics" });
        }
    }

    /// <summary>
    /// Manually removes an IP ban
    /// </summary>
    /// <param name="ipAddress">IP address to unban</param>
    /// <returns>Result of unban operation</returns>
    [HttpDelete("ip-bans/{ipAddress}")]
    public async Task<ActionResult> RemoveIPBan(string ipAddress)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return BadRequest(new { error = "IP address is required" });
            }

            await _bruteForceService.RemoveIPBanAsync(ipAddress);

            await _loggingClient.LogUserActionAsync(
                action: "AdminRemoveIPBan",
                category: "Security",
                userId: User.Identity?.Name,
                details: $"Admin manually removed IP ban for: {ipAddress}");

            return Ok(new { message = $"IP ban removed for {ipAddress}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing IP ban for {IPAddress}", ipAddress);
            return StatusCode(500, new { error = "Failed to remove IP ban" });
        }
    }

    /// <summary>
    /// Manually removes an account lockout
    /// </summary>
    /// <param name="email">Email address to unlock</param>
    /// <returns>Result of unlock operation</returns>
    [HttpDelete("account-lockouts/{email}")]
    public async Task<ActionResult> RemoveAccountLockout(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { error = "Email address is required" });
            }

            await _bruteForceService.RemoveAccountLockoutAsync(email);

            await _loggingClient.LogUserActionAsync(
                action: "AdminRemoveAccountLockout",
                category: "Security",
                userId: User.Identity?.Name,
                details: $"Admin manually removed account lockout for: {email}");

            return Ok(new { message = $"Account lockout removed for {email}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing account lockout for {Email}", email);
            return StatusCode(500, new { error = "Failed to remove account lockout" });
        }
    }

    /// <summary>
    /// Manually triggers cleanup of expired restrictions
    /// </summary>
    /// <returns>Result of cleanup operation</returns>
    [HttpPost("cleanup")]
    public async Task<ActionResult> TriggerCleanup()
    {
        try
        {
            await _bruteForceService.CleanupExpiredRestrictionsAsync();

            await _loggingClient.LogUserActionAsync(
                action: "AdminTriggerSecurityCleanup",
                category: "Security",
                userId: User.Identity?.Name,
                details: "Admin manually triggered security restrictions cleanup");

            return Ok(new { message = "Security cleanup completed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during manual security cleanup");
            return StatusCode(500, new { error = "Failed to perform security cleanup" });
        }
    }

    /// <summary>
    /// Gets rate limiting configuration status
    /// </summary>
    /// <returns>Configuration status</returns>
    [HttpGet("config")]
    public ActionResult<object> GetSecurityConfig()
    {
        try
        {
            var config = new
            {
                Message = "Rate limiting and brute force protection is active",
                Features = new[]
                {
                    "IP-based rate limiting",
                    "Progressive delay for failed attempts",
                    "Account lockout protection",
                    "IP banning for repeated violations",
                    "Automatic cleanup of expired restrictions",
                    "Comprehensive security logging"
                },
                Endpoints = new
                {
                    Statistics = "/security/stats",
                    RemoveIPBan = "/security/ip-bans/{ipAddress}",
                    RemoveAccountLockout = "/security/account-lockouts/{email}",
                    TriggerCleanup = "/security/cleanup"
                }
            };

            return Ok(config);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving security configuration");
            return StatusCode(500, new { error = "Failed to retrieve security configuration" });
        }
    }
}