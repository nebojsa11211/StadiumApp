using StadiumDrinkOrdering.Shared.Authentication.Models;

namespace StadiumDrinkOrdering.Shared.Authentication.Interfaces;

/// <summary>
/// Unified interface for managing authentication state across all applications
/// </summary>
public interface IAuthenticationStateService
{
    /// <summary>
    /// Gets the current authentication state
    /// </summary>
    AuthenticationState State { get; }

    /// <summary>
    /// Indicates if the user is currently authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the current user's email address
    /// </summary>
    string? UserEmail { get; }

    /// <summary>
    /// Gets the current user's role
    /// </summary>
    string? UserRole { get; }

    /// <summary>
    /// Gets the current user's ID
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Event fired when authentication state changes
    /// </summary>
    event Action<AuthenticationState>? OnAuthenticationStateChanged;

    /// <summary>
    /// Initializes the authentication state from storage
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Logs in a user with the provided authentication result
    /// </summary>
    Task<bool> LoginAsync(AuthenticationResult authResult);

    /// <summary>
    /// Logs out the current user and clears authentication state
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Refreshes the authentication token if possible
    /// </summary>
    Task<bool> RefreshTokenAsync();

    /// <summary>
    /// Validates the current authentication state
    /// </summary>
    Task<bool> ValidateAuthenticationAsync();

    /// <summary>
    /// Checks if the current user has the specified role
    /// </summary>
    bool HasRole(string role);

    /// <summary>
    /// Checks if the current user has any of the specified roles
    /// </summary>
    bool HasAnyRole(params string[] roles);
}