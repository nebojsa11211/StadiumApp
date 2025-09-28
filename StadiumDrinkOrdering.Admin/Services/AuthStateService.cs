using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using UserRoleEnum = StadiumDrinkOrdering.Shared.Models.UserRole;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Admin-specific implementation of the unified authentication state service
/// </summary>
public class AuthStateService : IAuthenticationStateService, IAuthStateService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly IJSRuntime _jsRuntime;
    private readonly SynchronizationContext? _synchronizationContext;
    private bool _initialized = false;
    private AuthenticationState _state = new();

    public AuthenticationState State => _state;
    public bool IsAuthenticated => _state.IsAuthenticated;
    public string? UserEmail => _state.Email;
    public string? UserRole => _state.Role;
    public string? UserId => _state.UserId;

    public event Action<AuthenticationState>? OnAuthenticationStateChanged;

    // Legacy event for backward compatibility
    event Action? IAuthStateService.OnAuthenticationStateChanged
    {
        add => _legacyAuthChanged += value;
        remove => _legacyAuthChanged -= value;
    }
    private event Action? _legacyAuthChanged;

    public AuthStateService(ITokenStorageService tokenStorage, IJSRuntime jsRuntime)
    {
        _tokenStorage = tokenStorage;
        _jsRuntime = jsRuntime;
        _synchronizationContext = SynchronizationContext.Current;

        // Subscribe to token expiration events
        _tokenStorage.OnTokenExpired += HandleTokenExpired;
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            Console.WriteLine($"AuthStateService already initialized - IsAuthenticated: {_state.IsAuthenticated}");
            // Even if already initialized, ensure we have the current state
            // This handles cases where a new component instance is checking authentication
            return;
        }

        Console.WriteLine("AuthStateService: Starting initialization...");

        try
        {
            // Add a small delay to ensure JSRuntime is ready
            await Task.Delay(100);

            var tokenInfo = await _tokenStorage.GetTokenInfoAsync();
            Console.WriteLine($"AuthStateService: Retrieved token info - Token present: {tokenInfo?.Token != null}");

            if (tokenInfo?.Token != null)
            {
                var isValid = await _tokenStorage.IsTokenValidAsync();
                Console.WriteLine($"AuthStateService: Token valid: {isValid}");

                if (isValid)
                {
                    await UpdateAuthenticationStateFromToken(tokenInfo.Token);
                    Console.WriteLine($"AuthStateService: Authentication state updated - IsAuthenticated: {_state.IsAuthenticated}, Email: {_state.Email}");
                }
                else
                {
                    Console.WriteLine("AuthStateService: Token invalid - logging out");
                    await LogoutAsync();
                }
            }
            else
            {
                Console.WriteLine("AuthStateService: No token found - setting unauthenticated state");
                _state = new AuthenticationState
                {
                    IsAuthenticated = false,
                    ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AuthStateService: Initialization error - {ex.Message}");
            Console.WriteLine($"AuthStateService: Stack trace - {ex.StackTrace}");
            // Handle initialization errors by setting unauthenticated state
            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
            };
        }

        _initialized = true;
        Console.WriteLine($"AuthStateService: Initialization complete - IsAuthenticated: {_state.IsAuthenticated}");

        // Only trigger events if there are listeners
        await InvokeStateChangeAsync();
    }

    public async Task<bool> LoginAsync(AuthenticationResult authResult)
    {
        if (authResult == null || string.IsNullOrEmpty(authResult.Token))
            return false;

        try
        {
            // Store the token
            await _tokenStorage.StoreTokenAsync(
                authResult.Token,
                authResult.ExpiresAt,
                authResult.User?.Email);

            // Store user data if available
            if (authResult.User != null)
            {
                var userDataJson = JsonSerializer.Serialize(authResult.User, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await _tokenStorage.StoreUserDataAsync("profile", userDataJson);
            }

            // Update authentication state
            await UpdateAuthenticationStateFromToken(authResult.Token);

            await InvokeStateChangeAsync();
            return true;
        }
        catch (Exception)
        {
            await LogoutAsync();
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _tokenStorage.ClearTokenAsync();

            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
            };

            await InvokeStateChangeAsync();
        }
        catch (Exception)
        {
            // Even if localStorage operations fail, clear the in-memory state
            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
            };
            await InvokeStateChangeAsync();
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        // For this implementation, we don't have refresh token functionality
        // Just validate the current token
        if (!await _tokenStorage.IsTokenValidAsync())
        {
            await LogoutAsync();
            return false;
        }

        return true;
    }

    public async Task<bool> ValidateAuthenticationAsync()
    {
        var isValid = await _tokenStorage.IsTokenValidAsync();

        if (!isValid && _state.IsAuthenticated)
        {
            await LogoutAsync();
        }

        return isValid;
    }

    public bool HasRole(string role)
    {
        if (string.IsNullOrEmpty(role) || !IsAuthenticated)
            return false;

        return string.Equals(_state.Role, role, StringComparison.OrdinalIgnoreCase);
    }

    public bool HasAnyRole(params string[] roles)
    {
        if (roles == null || roles.Length == 0 || !IsAuthenticated)
            return false;

        return roles.Any(role => HasRole(role));
    }

    private async Task UpdateAuthenticationStateFromToken(string token)
    {
        try
        {
            var claims = JwtTokenValidator.GetClaims(token);

            _state = new AuthenticationState
            {
                IsAuthenticated = true,
                UserId = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.UserId) ??
                        claims.GetValueOrDefault(AuthenticationConstants.StandardClaims.Subject),
                Email = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Email) ??
                        claims.GetValueOrDefault("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"),
                Role = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Role),
                UserName = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Username),
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin,
                AuthenticatedAt = DateTime.UtcNow
            };

            // Store additional claims
            foreach (var claim in claims)
            {
                if (!_state.Claims.ContainsKey(claim.Key))
                {
                    _state.Claims[claim.Key] = claim.Value;
                }
            }
        }
        catch (Exception)
        {
            // If token parsing fails, logout
            await LogoutAsync();
        }
    }

    private async void HandleTokenExpired()
    {
        await LogoutAsync();
    }

    private async Task InvokeStateChangeAsync()
    {
        if (_synchronizationContext != null)
        {
            // Use the captured synchronization context to marshal to the UI thread
            await Task.Run(() =>
            {
                _synchronizationContext.Post(_ =>
                {
                    try
                    {
                        OnAuthenticationStateChanged?.Invoke(_state);
                        _legacyAuthChanged?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"AuthStateService: Error invoking state change events - {ex.Message}");
                    }
                }, null);
            });
        }
        else
        {
            // Fallback: invoke directly (may cause threading issues but better than failing)
            try
            {
                OnAuthenticationStateChanged?.Invoke(_state);
                _legacyAuthChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AuthStateService: Error invoking state change events - {ex.Message}");
            }
        }
    }

    // Legacy methods for backward compatibility
    public async Task LoginAsync(string token, string email)
    {
        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            Token = token,
            User = new UserDto
            {
                Email = email,
                Id = GetUserIdFromToken(token),
                Role = GetUserRoleFromToken(token)
            }
        };

        await LoginAsync(authResult);
    }

    private static int GetUserIdFromToken(string token)
    {
        var userIdStr = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.UserId) ??
                       JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.StandardClaims.Subject);
        return int.TryParse(userIdStr, out var userId) ? userId : 0;
    }

    private static UserRoleEnum GetUserRoleFromToken(string token)
    {
        var roleStr = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.Role);
        return Enum.TryParse<UserRoleEnum>(roleStr, out var role) ? role : UserRoleEnum.Admin;
    }
}

/// <summary>
/// Legacy interface for backward compatibility
/// </summary>
public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    event Action? OnAuthenticationStateChanged;
    Task InitializeAsync();
    Task LoginAsync(string token, string email);
    Task LogoutAsync();
}