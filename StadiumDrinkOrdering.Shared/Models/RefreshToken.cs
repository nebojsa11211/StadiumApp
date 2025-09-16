using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Represents a refresh token used for JWT token renewal
/// </summary>
[Table("RefreshTokens")]
public class RefreshToken
{
    /// <summary>
    /// Unique identifier for the refresh token
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The refresh token value
    /// </summary>
    [Required]
    [StringLength(512)]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// JWT token identifier that this refresh token corresponds to
    /// </summary>
    [Required]
    [StringLength(256)]
    public string JwtId { get; set; } = string.Empty;

    /// <summary>
    /// User ID who owns this refresh token
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// When the refresh token was created
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the refresh token expires
    /// </summary>
    [Required]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Whether the refresh token has been used/invalidated
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Whether the refresh token has been revoked
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// When the refresh token was invalidated (used or revoked)
    /// </summary>
    public DateTime? InvalidatedAt { get; set; }

    /// <summary>
    /// Reason for invalidation (optional)
    /// </summary>
    [StringLength(200)]
    public string? InvalidationReason { get; set; }

    /// <summary>
    /// Device information for this refresh token
    /// </summary>
    [StringLength(500)]
    public string? DeviceInfo { get; set; }

    /// <summary>
    /// IP address where the refresh token was created
    /// </summary>
    [StringLength(45)] // IPv6 max length
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent when the refresh token was created
    /// </summary>
    [StringLength(1000)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Navigation property to the user
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Checks if the refresh token is valid (not expired, used, or revoked)
    /// </summary>
    public bool IsValid => !IsUsed && !IsRevoked && ExpiresAt > DateTime.UtcNow;

    /// <summary>
    /// Marks the refresh token as used
    /// </summary>
    public void MarkAsUsed(string? reason = null)
    {
        IsUsed = true;
        InvalidatedAt = DateTime.UtcNow;
        InvalidationReason = reason ?? "Token used for refresh";
    }

    /// <summary>
    /// Marks the refresh token as revoked
    /// </summary>
    public void MarkAsRevoked(string? reason = null)
    {
        IsRevoked = true;
        InvalidatedAt = DateTime.UtcNow;
        InvalidationReason = reason ?? "Token revoked";
    }

    /// <summary>
    /// Creates a new refresh token
    /// </summary>
    public static RefreshToken Create(
        int userId,
        string jwtId,
        TimeSpan? lifetime = null,
        string? deviceInfo = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        var token = GenerateSecureToken();
        var expiresAt = DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromDays(7));

        return new RefreshToken
        {
            Token = token,
            JwtId = jwtId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }

    /// <summary>
    /// Generates a cryptographically secure random token
    /// </summary>
    private static string GenerateSecureToken()
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[64]; // 512 bits
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}