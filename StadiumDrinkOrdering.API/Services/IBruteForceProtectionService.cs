using StadiumDrinkOrdering.API.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Interface for brute force protection service
/// </summary>
public interface IBruteForceProtectionService
{
    /// <summary>
    /// Records a failed authentication attempt
    /// </summary>
    /// <param name="ipAddress">IP address of the attempt</param>
    /// <param name="email">Email address attempted (if provided)</param>
    /// <param name="attemptType">Type of attempt (Login, Register)</param>
    /// <param name="userAgent">User agent string</param>
    /// <param name="context">Additional context</param>
    /// <returns>Task</returns>
    Task RecordFailedAttemptAsync(string ipAddress, string? email, string attemptType, string? userAgent, string? context = null);

    /// <summary>
    /// Checks if an IP address is currently banned
    /// </summary>
    /// <param name="ipAddress">IP address to check</param>
    /// <returns>True if IP is banned, false otherwise</returns>
    Task<bool> IsIPBannedAsync(string ipAddress);

    /// <summary>
    /// Checks if an account is currently locked out
    /// </summary>
    /// <param name="email">Email address to check</param>
    /// <returns>AccountLockout if account is locked, null otherwise</returns>
    Task<AccountLockout?> GetAccountLockoutAsync(string email);

    /// <summary>
    /// Calculates progressive delay for failed attempts
    /// </summary>
    /// <param name="ipAddress">IP address</param>
    /// <param name="email">Email address (optional)</param>
    /// <returns>Delay in milliseconds</returns>
    Task<int> GetProgressiveDelayAsync(string ipAddress, string? email = null);

    /// <summary>
    /// Clears failed attempts for successful authentication
    /// </summary>
    /// <param name="email">Email address that successfully authenticated</param>
    /// <returns>Task</returns>
    Task ClearFailedAttemptsAsync(string email);

    /// <summary>
    /// Gets rate limiting statistics for monitoring
    /// </summary>
    /// <returns>Rate limiting statistics</returns>
    Task<RateLimitingStats> GetStatsAsync();

    /// <summary>
    /// Manually removes an IP ban (admin function)
    /// </summary>
    /// <param name="ipAddress">IP address to unban</param>
    /// <returns>Task</returns>
    Task RemoveIPBanAsync(string ipAddress);

    /// <summary>
    /// Manually removes an account lockout (admin function)
    /// </summary>
    /// <param name="email">Email address to unlock</param>
    /// <returns>Task</returns>
    Task RemoveAccountLockoutAsync(string email);

    /// <summary>
    /// Cleanup expired bans and lockouts
    /// </summary>
    /// <returns>Task</returns>
    Task CleanupExpiredRestrictionsAsync();
}

/// <summary>
/// Statistics for rate limiting monitoring
/// </summary>
public class RateLimitingStats
{
    public int ActiveIPBans { get; set; }
    public int ActiveAccountLockouts { get; set; }
    public int FailedAttemptsLast24Hours { get; set; }
    public int FailedAttemptsLastHour { get; set; }
    public Dictionary<string, int> AttemptsByType { get; set; } = new();
    public List<string> TopOffendingIPs { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}