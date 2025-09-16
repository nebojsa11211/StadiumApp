using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Request DTO for token refresh operations
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// The current access token
    /// </summary>
    [Required(ErrorMessage = "Access token is required")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// The refresh token to use for obtaining a new access token
    /// </summary>
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Device information (optional)
    /// </summary>
    [StringLength(500)]
    public string? DeviceInfo { get; set; }
}

/// <summary>
/// Response DTO for token refresh operations
/// </summary>
public class RefreshTokenResponseDto
{
    /// <summary>
    /// The new access token
    /// </summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// The new refresh token
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// When the new access token expires
    /// </summary>
    [Required]
    public DateTime AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// When the new refresh token expires
    /// </summary>
    [Required]
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// User information (included for consistency)
    /// </summary>
    public UserDto? User { get; set; }

    /// <summary>
    /// Token type (typically "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";
}

/// <summary>
/// Enhanced login response that includes refresh token
/// </summary>
public class EnhancedLoginResponseDto : LoginResponseDto
{
    /// <summary>
    /// The refresh token for token renewal
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// When the refresh token expires
    /// </summary>
    [Required]
    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Token type (typically "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Converts from standard LoginResponseDto
    /// </summary>
    public static EnhancedLoginResponseDto FromLoginResponse(
        LoginResponseDto loginResponse,
        string refreshToken,
        DateTime refreshTokenExpiresAt)
    {
        return new EnhancedLoginResponseDto
        {
            Token = loginResponse.Token,
            User = loginResponse.User,
            ExpiresAt = loginResponse.ExpiresAt,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }
}

/// <summary>
/// Request DTO for revoking refresh tokens
/// </summary>
public class RevokeTokenRequestDto
{
    /// <summary>
    /// The refresh token to revoke
    /// </summary>
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Optional reason for revocation
    /// </summary>
    [StringLength(200)]
    public string? Reason { get; set; }
}

/// <summary>
/// DTO for listing active refresh tokens (admin/user management)
/// </summary>
public class RefreshTokenInfoDto
{
    /// <summary>
    /// Token ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// First 8 characters of the token (for identification)
    /// </summary>
    public string TokenPreview { get; set; } = string.Empty;

    /// <summary>
    /// When the token was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Device information
    /// </summary>
    public string? DeviceInfo { get; set; }

    /// <summary>
    /// IP address where created
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Whether the token is still valid
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// When the token was invalidated (if applicable)
    /// </summary>
    public DateTime? InvalidatedAt { get; set; }

    /// <summary>
    /// Reason for invalidation
    /// </summary>
    public string? InvalidationReason { get; set; }
}