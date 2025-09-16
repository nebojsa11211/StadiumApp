using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Models;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Service for protecting against brute force attacks
/// </summary>
public class BruteForceProtectionService : IBruteForceProtectionService
{
    private readonly ApplicationDbContext _context;
    private readonly RateLimitingConfig _config;
    private readonly ICentralizedLoggingClient _loggingClient;
    private readonly ILogger<BruteForceProtectionService> _logger;

    public BruteForceProtectionService(
        ApplicationDbContext context,
        IOptions<RateLimitingConfig> config,
        ICentralizedLoggingClient? loggingClient,
        ILogger<BruteForceProtectionService> logger)
    {
        _context = context;
        _config = config.Value;
        _loggingClient = loggingClient ?? new NullCentralizedLoggingClient();
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task RecordFailedAttemptAsync(string ipAddress, string? email, string attemptType, string? userAgent, string? context = null)
    {
        try
        {
            // Record the failed attempt
            var failedAttempt = new FailedAttempt
            {
                IPAddress = ipAddress,
                Email = email,
                AttemptType = attemptType,
                UserAgent = userAgent,
                Context = context,
                AttemptTime = DateTime.UtcNow
            };

            _context.FailedAttempts.Add(failedAttempt);
            await _context.SaveChangesAsync();

            // Log the failed attempt
            await _loggingClient.LogUserActionAsync(
                action: $"FailedAuth_{attemptType}",
                category: "Security",
                details: $"Failed {attemptType} attempt from IP: {ipAddress}, Email: {email ?? "N/A"}");

            // Check for account lockout (if email provided)
            if (!string.IsNullOrEmpty(email))
            {
                await CheckAndApplyAccountLockoutAsync(email);
            }

            // Check for IP ban
            await CheckAndApplyIPBanAsync(ipAddress);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording failed attempt for IP {IPAddress}, Email {Email}", ipAddress, email);
            await _loggingClient.LogErrorAsync(ex, "RecordFailedAttempt", "SystemError");
        }
    }

    /// <inheritdoc />
    public async Task<bool> IsIPBannedAsync(string ipAddress)
    {
        try
        {
            var now = DateTime.UtcNow;
            var activeBan = await _context.IPBans
                .Where(b => b.IPAddress == ipAddress && b.IsActive && b.BanEnd > now)
                .FirstOrDefaultAsync();

            return activeBan != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking IP ban for {IPAddress}", ipAddress);
            return false; // Fail open to avoid blocking legitimate users
        }
    }

    /// <inheritdoc />
    public async Task<AccountLockout?> GetAccountLockoutAsync(string email)
    {
        try
        {
            var now = DateTime.UtcNow;
            return await _context.AccountLockouts
                .Where(l => l.Email == email && l.IsActive && l.LockoutEnd > now)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking account lockout for {Email}", email);
            return null; // Fail open
        }
    }

    /// <inheritdoc />
    public async Task<int> GetProgressiveDelayAsync(string ipAddress, string? email = null)
    {
        if (!_config.BruteForce.ProgressiveDelay.Enabled)
            return 0;

        try
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-30); // Look at last 30 minutes
            var query = _context.FailedAttempts
                .Where(f => f.AttemptTime > cutoffTime);

            // Count failures by IP
            var ipFailures = await query
                .Where(f => f.IPAddress == ipAddress)
                .CountAsync();

            // Count failures by email (if provided)
            var emailFailures = 0;
            if (!string.IsNullOrEmpty(email))
            {
                emailFailures = await query
                    .Where(f => f.Email == email)
                    .CountAsync();
            }

            // Use the higher count for delay calculation
            var failureCount = Math.Max(ipFailures, emailFailures);

            if (failureCount <= 1)
                return 0;

            // Calculate progressive delay: baseDelay * multiplier^(failures-1)
            var delay = _config.BruteForce.ProgressiveDelay.BaseDelayMs *
                       Math.Pow(_config.BruteForce.ProgressiveDelay.Multiplier, failureCount - 1);

            // Cap at maximum delay
            return (int)Math.Min(delay, _config.BruteForce.ProgressiveDelay.MaxDelayMs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating progressive delay for IP {IPAddress}, Email {Email}", ipAddress, email);
            return 0; // Fail open
        }
    }

    /// <inheritdoc />
    public async Task ClearFailedAttemptsAsync(string email)
    {
        try
        {
            // Clear failed attempts for this email
            var cutoffTime = DateTime.UtcNow.AddHours(-24); // Clear last 24 hours
            var attemptsToRemove = await _context.FailedAttempts
                .Where(f => f.Email == email && f.AttemptTime > cutoffTime)
                .ToListAsync();

            _context.FailedAttempts.RemoveRange(attemptsToRemove);

            // Deactivate any active lockouts
            var activeLockout = await _context.AccountLockouts
                .Where(l => l.Email == email && l.IsActive)
                .FirstOrDefaultAsync();

            if (activeLockout != null)
            {
                activeLockout.IsActive = false;
                _context.AccountLockouts.Update(activeLockout);
            }

            await _context.SaveChangesAsync();

            await _loggingClient.LogUserActionAsync(
                action: "ClearFailedAttempts",
                category: "Security",
                details: $"Cleared failed attempts for email: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing failed attempts for {Email}", email);
        }
    }

    /// <inheritdoc />
    public async Task<RateLimitingStats> GetStatsAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var last24Hours = now.AddHours(-24);
            var lastHour = now.AddHours(-1);

            var stats = new RateLimitingStats
            {
                ActiveIPBans = await _context.IPBans.CountAsync(b => b.IsActive && b.BanEnd > now),
                ActiveAccountLockouts = await _context.AccountLockouts.CountAsync(l => l.IsActive && l.LockoutEnd > now),
                FailedAttemptsLast24Hours = await _context.FailedAttempts.CountAsync(f => f.AttemptTime > last24Hours),
                FailedAttemptsLastHour = await _context.FailedAttempts.CountAsync(f => f.AttemptTime > lastHour)
            };

            // Get attempts by type
            stats.AttemptsByType = await _context.FailedAttempts
                .Where(f => f.AttemptTime > last24Hours)
                .GroupBy(f => f.AttemptType)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            // Get top offending IPs
            stats.TopOffendingIPs = await _context.FailedAttempts
                .Where(f => f.AttemptTime > last24Hours)
                .GroupBy(f => f.IPAddress)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(g => $"{g.Key} ({g.Count()} attempts)")
                .ToListAsync();

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rate limiting stats");
            return new RateLimitingStats(); // Return empty stats on error
        }
    }

    /// <inheritdoc />
    public async Task RemoveIPBanAsync(string ipAddress)
    {
        try
        {
            var activeBans = await _context.IPBans
                .Where(b => b.IPAddress == ipAddress && b.IsActive)
                .ToListAsync();

            foreach (var ban in activeBans)
            {
                ban.IsActive = false;
            }

            await _context.SaveChangesAsync();

            await _loggingClient.LogUserActionAsync(
                action: "RemoveIPBan",
                category: "Security",
                details: $"Manually removed IP ban for: {ipAddress}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing IP ban for {IPAddress}", ipAddress);
        }
    }

    /// <inheritdoc />
    public async Task RemoveAccountLockoutAsync(string email)
    {
        try
        {
            var activeLockouts = await _context.AccountLockouts
                .Where(l => l.Email == email && l.IsActive)
                .ToListAsync();

            foreach (var lockout in activeLockouts)
            {
                lockout.IsActive = false;
            }

            await _context.SaveChangesAsync();

            await _loggingClient.LogUserActionAsync(
                action: "RemoveAccountLockout",
                category: "Security",
                details: $"Manually removed account lockout for: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing account lockout for {Email}", email);
        }
    }

    /// <inheritdoc />
    public async Task CleanupExpiredRestrictionsAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var cleanupCutoff = now.AddDays(-30); // Keep records for 30 days

            // Deactivate expired IP bans
            var expiredBans = await _context.IPBans
                .Where(b => b.IsActive && b.BanEnd <= now)
                .ToListAsync();

            foreach (var ban in expiredBans)
            {
                ban.IsActive = false;
            }

            // Deactivate expired account lockouts
            var expiredLockouts = await _context.AccountLockouts
                .Where(l => l.IsActive && l.LockoutEnd <= now)
                .ToListAsync();

            foreach (var lockout in expiredLockouts)
            {
                lockout.IsActive = false;
            }

            // Remove old failed attempts (older than 30 days)
            var oldAttempts = await _context.FailedAttempts
                .Where(f => f.AttemptTime < cleanupCutoff)
                .ToListAsync();

            _context.FailedAttempts.RemoveRange(oldAttempts);

            await _context.SaveChangesAsync();

            if (expiredBans.Count > 0 || expiredLockouts.Count > 0 || oldAttempts.Count > 0)
            {
                await _loggingClient.LogUserActionAsync(
                    action: "CleanupSecurityRestrictions",
                    category: "Security",
                    details: $"Cleaned up {expiredBans.Count} IP bans, {expiredLockouts.Count} account lockouts, {oldAttempts.Count} old attempts");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during security restrictions cleanup");
        }
    }

    private async Task CheckAndApplyAccountLockoutAsync(string email)
    {
        try
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-1); // Look at last hour
            var recentFailures = await _context.FailedAttempts
                .Where(f => f.Email == email && f.AttemptTime > cutoffTime)
                .CountAsync();

            if (recentFailures >= _config.BruteForce.MaxFailedAttempts)
            {
                // Check if account is already locked
                var existingLockout = await GetAccountLockoutAsync(email);
                if (existingLockout != null)
                    return; // Already locked

                // Create new lockout
                var lockout = new AccountLockout
                {
                    Email = email,
                    LockoutStart = DateTime.UtcNow,
                    LockoutEnd = DateTime.UtcNow.AddMinutes(_config.BruteForce.LockoutDurationMinutes),
                    FailedAttemptCount = recentFailures,
                    IsActive = true,
                    Reason = $"Too many failed login attempts ({recentFailures})"
                };

                _context.AccountLockouts.Add(lockout);
                await _context.SaveChangesAsync();

                await _loggingClient.LogUserActionAsync(
                    action: "AccountLockout",
                    category: "Security",
                    details: $"Account locked due to {recentFailures} failed attempts: {email}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking account lockout for {Email}", email);
        }
    }

    private async Task CheckAndApplyIPBanAsync(string ipAddress)
    {
        try
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-1); // Look at last hour
            var recentFailures = await _context.FailedAttempts
                .Where(f => f.IPAddress == ipAddress && f.AttemptTime > cutoffTime)
                .CountAsync();

            if (recentFailures >= _config.BruteForce.MaxFailedAttemptsPerIP)
            {
                // Check if IP is already banned
                var isBanned = await IsIPBannedAsync(ipAddress);
                if (isBanned)
                    return; // Already banned

                // Create new IP ban
                var ban = new IPBan
                {
                    IPAddress = ipAddress,
                    BanStart = DateTime.UtcNow,
                    BanEnd = DateTime.UtcNow.AddMinutes(_config.BruteForce.IPBanDurationMinutes),
                    IsActive = true,
                    ViolationCount = recentFailures,
                    Reason = $"Too many failed attempts from IP ({recentFailures})"
                };

                _context.IPBans.Add(ban);
                await _context.SaveChangesAsync();

                await _loggingClient.LogUserActionAsync(
                    action: "IPBan",
                    category: "Security",
                    details: $"IP banned due to {recentFailures} failed attempts: {ipAddress}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking IP ban for {IPAddress}", ipAddress);
        }
    }
}