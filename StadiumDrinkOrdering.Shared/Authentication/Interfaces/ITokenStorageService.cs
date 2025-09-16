using StadiumDrinkOrdering.Shared.Authentication.Models;

namespace StadiumDrinkOrdering.Shared.Authentication.Interfaces;

/// <summary>
/// Unified interface for secure token storage and management
/// </summary>
public interface ITokenStorageService
{
    /// <summary>
    /// Gets or sets the current authentication token
    /// </summary>
    string? Token { get; set; }

    /// <summary>
    /// Gets token information including expiration
    /// </summary>
    TokenInfo? TokenInfo { get; }

    /// <summary>
    /// Gets the application context for storage key generation
    /// </summary>
    string ApplicationContext { get; }

    /// <summary>
    /// Stores a token with its metadata
    /// </summary>
    Task StoreTokenAsync(string token, DateTime? expiresAt = null, string? userEmail = null);

    /// <summary>
    /// Retrieves the stored token
    /// </summary>
    Task<string?> GetTokenAsync();

    /// <summary>
    /// Retrieves complete token information
    /// </summary>
    Task<TokenInfo?> GetTokenInfoAsync();

    /// <summary>
    /// Clears the stored token and related data
    /// </summary>
    Task ClearTokenAsync();

    /// <summary>
    /// Checks if the current token is valid and not expired
    /// </summary>
    Task<bool> IsTokenValidAsync();

    /// <summary>
    /// Gets the remaining time until token expiration
    /// </summary>
    TimeSpan? GetTokenTimeToExpiry();

    /// <summary>
    /// Event fired when token expires or becomes invalid
    /// </summary>
    event Action? OnTokenExpired;

    /// <summary>
    /// Stores additional user data associated with the token
    /// </summary>
    Task StoreUserDataAsync(string key, string value);

    /// <summary>
    /// Retrieves stored user data
    /// </summary>
    Task<string?> GetUserDataAsync(string key);

    /// <summary>
    /// Clears all stored user data
    /// </summary>
    Task ClearUserDataAsync();

    /// <summary>
    /// Gets or sets the current refresh token
    /// </summary>
    string? RefreshToken { get; set; }

    /// <summary>
    /// Stores a refresh token with its expiration
    /// </summary>
    Task StoreRefreshTokenAsync(string refreshToken, DateTime? expiresAt = null);

    /// <summary>
    /// Retrieves the stored refresh token
    /// </summary>
    Task<string?> GetRefreshTokenAsync();

    /// <summary>
    /// Clears the stored refresh token
    /// </summary>
    Task ClearRefreshTokenAsync();

    /// <summary>
    /// Checks if the current refresh token is valid and not expired
    /// </summary>
    Task<bool> IsRefreshTokenValidAsync();

    /// <summary>
    /// Gets the remaining time until refresh token expiration
    /// </summary>
    TimeSpan? GetRefreshTokenTimeToExpiry();

    /// <summary>
    /// Stores both access and refresh tokens in one operation
    /// </summary>
    Task StoreTokenPairAsync(string accessToken, string refreshToken,
        DateTime? accessTokenExpiresAt = null, DateTime? refreshTokenExpiresAt = null,
        string? userEmail = null);

    /// <summary>
    /// Event fired when refresh token expires or becomes invalid
    /// </summary>
    event Action? OnRefreshTokenExpired;
}