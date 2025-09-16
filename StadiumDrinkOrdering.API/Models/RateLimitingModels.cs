using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Models;

/// <summary>
/// Configuration model for rate limiting settings
/// </summary>
public class RateLimitingConfig
{
    /// <summary>
    /// Enable/disable rate limiting globally
    /// </summary>
    public bool EnableRateLimiting { get; set; } = true;

    /// <summary>
    /// Authentication endpoint specific settings
    /// </summary>
    public AuthRateLimitConfig Authentication { get; set; } = new();

    /// <summary>
    /// General API endpoint settings
    /// </summary>
    public GeneralRateLimitConfig General { get; set; } = new();

    /// <summary>
    /// Brute force protection settings
    /// </summary>
    public BruteForceConfig BruteForce { get; set; } = new();
}

/// <summary>
/// Rate limiting configuration for authentication endpoints
/// </summary>
public class AuthRateLimitConfig
{
    /// <summary>
    /// Maximum login attempts per IP per minute
    /// </summary>
    public int LoginAttemptsPerMinute { get; set; } = 5;

    /// <summary>
    /// Maximum registration attempts per IP per hour
    /// </summary>
    public int RegisterAttemptsPerHour { get; set; } = 3;

    /// <summary>
    /// Time window for rate limiting (in seconds)
    /// </summary>
    public int TimeWindowSeconds { get; set; } = 60;

    /// <summary>
    /// Retry delay after rate limit exceeded (in seconds)
    /// </summary>
    public int RetryAfterSeconds { get; set; } = 60;
}

/// <summary>
/// General API rate limiting configuration
/// </summary>
public class GeneralRateLimitConfig
{
    /// <summary>
    /// Maximum requests per IP per minute for general endpoints
    /// </summary>
    public int RequestsPerMinute { get; set; } = 100;

    /// <summary>
    /// Maximum requests per authenticated user per minute
    /// </summary>
    public int AuthenticatedRequestsPerMinute { get; set; } = 200;
}

/// <summary>
/// Brute force protection configuration
/// </summary>
public class BruteForceConfig
{
    /// <summary>
    /// Maximum failed login attempts before account lockout
    /// </summary>
    public int MaxFailedAttempts { get; set; } = 5;

    /// <summary>
    /// Account lockout duration in minutes
    /// </summary>
    public int LockoutDurationMinutes { get; set; } = 15;

    /// <summary>
    /// Maximum failed attempts from single IP before IP ban
    /// </summary>
    public int MaxFailedAttemptsPerIP { get; set; } = 20;

    /// <summary>
    /// IP ban duration in minutes
    /// </summary>
    public int IPBanDurationMinutes { get; set; } = 60;

    /// <summary>
    /// Progressive delay configuration
    /// </summary>
    public ProgressiveDelayConfig ProgressiveDelay { get; set; } = new();
}

/// <summary>
/// Progressive delay configuration for failed authentication attempts
/// </summary>
public class ProgressiveDelayConfig
{
    /// <summary>
    /// Enable progressive delay
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Base delay in milliseconds
    /// </summary>
    public int BaseDelayMs { get; set; } = 1000;

    /// <summary>
    /// Maximum delay in milliseconds
    /// </summary>
    public int MaxDelayMs { get; set; } = 30000;

    /// <summary>
    /// Multiplier for each failed attempt
    /// </summary>
    public double Multiplier { get; set; } = 2.0;
}

/// <summary>
/// Model for tracking failed authentication attempts
/// </summary>
public class FailedAttempt
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// IP address of the attempt
    /// </summary>
    [Required]
    [StringLength(45)] // Support IPv6
    public string IPAddress { get; set; } = string.Empty;

    /// <summary>
    /// Email address attempted (if provided)
    /// </summary>
    [StringLength(255)]
    public string? Email { get; set; }

    /// <summary>
    /// Timestamp of the attempt
    /// </summary>
    public DateTime AttemptTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Type of failed attempt (Login, Register, etc.)
    /// </summary>
    [Required]
    [StringLength(50)]
    public string AttemptType { get; set; } = string.Empty;

    /// <summary>
    /// User agent string
    /// </summary>
    [StringLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Additional context about the failure
    /// </summary>
    [StringLength(1000)]
    public string? Context { get; set; }
}

/// <summary>
/// Model for tracking account lockouts
/// </summary>
public class AccountLockout
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Email address of the locked account
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// When the lockout started
    /// </summary>
    public DateTime LockoutStart { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the lockout expires
    /// </summary>
    public DateTime LockoutEnd { get; set; }

    /// <summary>
    /// Number of failed attempts that triggered the lockout
    /// </summary>
    public int FailedAttemptCount { get; set; }

    /// <summary>
    /// Whether the lockout is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Reason for the lockout
    /// </summary>
    [StringLength(500)]
    public string? Reason { get; set; }
}

/// <summary>
/// Model for tracking IP bans
/// </summary>
public class IPBan
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Banned IP address
    /// </summary>
    [Required]
    [StringLength(45)] // Support IPv6
    public string IPAddress { get; set; } = string.Empty;

    /// <summary>
    /// When the ban started
    /// </summary>
    public DateTime BanStart { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the ban expires
    /// </summary>
    public DateTime BanEnd { get; set; }

    /// <summary>
    /// Whether the ban is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Reason for the ban
    /// </summary>
    [StringLength(500)]
    public string? Reason { get; set; }

    /// <summary>
    /// Number of violations that triggered the ban
    /// </summary>
    public int ViolationCount { get; set; }
}