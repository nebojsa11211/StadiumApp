using System.Security.Claims;

namespace StadiumDrinkOrdering.Shared.Authentication.Models;

/// <summary>
/// Represents the current authentication state of a user
/// </summary>
public class AuthenticationState
{
    /// <summary>
    /// Indicates if the user is authenticated
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// The user's email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The user's unique identifier
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The user's display name
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The user's role
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Additional user roles
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// The authentication token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime? TokenExpiresAt { get; set; }

    /// <summary>
    /// When the user was authenticated
    /// </summary>
    public DateTime AuthenticatedAt { get; set; }

    /// <summary>
    /// Additional user claims
    /// </summary>
    public Dictionary<string, string> Claims { get; set; } = new();

    /// <summary>
    /// Application context (Admin, Customer, Staff)
    /// </summary>
    public string? ApplicationContext { get; set; }

    /// <summary>
    /// Creates an unauthenticated state
    /// </summary>
    public static AuthenticationState Unauthenticated(string? applicationContext = null) => new()
    {
        IsAuthenticated = false,
        ApplicationContext = applicationContext
    };

    /// <summary>
    /// Creates an authenticated state from an authentication result
    /// </summary>
    public static AuthenticationState FromAuthenticationResult(AuthenticationResult result, string? applicationContext = null) => new()
    {
        IsAuthenticated = true,
        Email = result.User?.Email,
        UserId = result.User?.Id.ToString(),
        UserName = result.User?.Username,
        Role = result.User?.Role.ToString(),
        Roles = result.User?.Role != null ? new List<string> { result.User.Role.ToString() } : new(),
        Token = result.Token,
        TokenExpiresAt = result.ExpiresAt,
        AuthenticatedAt = DateTime.UtcNow,
        ApplicationContext = applicationContext
    };

    /// <summary>
    /// Creates an authenticated state from a ClaimsPrincipal
    /// </summary>
    public static AuthenticationState FromClaimsPrincipal(ClaimsPrincipal principal, string? token = null, string? applicationContext = null)
    {
        var state = new AuthenticationState
        {
            IsAuthenticated = principal.Identity?.IsAuthenticated ?? false,
            ApplicationContext = applicationContext
        };

        if (state.IsAuthenticated)
        {
            state.Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? principal.FindFirst("email")?.Value;
            state.UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? principal.FindFirst("sub")?.Value;
            state.UserName = principal.FindFirst(ClaimTypes.Name)?.Value ?? principal.FindFirst("name")?.Value;
            state.Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? principal.FindFirst("role")?.Value;
            state.Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            state.Token = token;
            state.AuthenticatedAt = DateTime.UtcNow;

            // Extract all claims
            foreach (var claim in principal.Claims)
            {
                if (!state.Claims.ContainsKey(claim.Type))
                {
                    state.Claims[claim.Type] = claim.Value;
                }
            }

            // Parse token expiration if available
            var expClaim = principal.FindFirst("exp")?.Value;
            if (long.TryParse(expClaim, out var exp))
            {
                state.TokenExpiresAt = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;
            }
        }

        return state;
    }

    /// <summary>
    /// Checks if the token is expired
    /// </summary>
    public bool IsTokenExpired => TokenExpiresAt.HasValue && TokenExpiresAt.Value <= DateTime.UtcNow;

    /// <summary>
    /// Gets the time remaining until token expiration
    /// </summary>
    public TimeSpan? TimeToExpiry => TokenExpiresAt.HasValue ? TokenExpiresAt.Value - DateTime.UtcNow : null;

    /// <summary>
    /// Checks if the user has the specified role
    /// </summary>
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase) ||
                                       string.Equals(Role, role, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the user has any of the specified roles
    /// </summary>
    public bool HasAnyRole(params string[] roles) => roles.Any(HasRole);

    /// <summary>
    /// Gets a claim value by type
    /// </summary>
    public string? GetClaim(string claimType) => Claims.TryGetValue(claimType, out var value) ? value : null;

    /// <summary>
    /// Creates a copy of the current authentication state
    /// </summary>
    public AuthenticationState Clone() => new()
    {
        IsAuthenticated = IsAuthenticated,
        Email = Email,
        UserId = UserId,
        UserName = UserName,
        Role = Role,
        Roles = new List<string>(Roles),
        Token = Token,
        TokenExpiresAt = TokenExpiresAt,
        AuthenticatedAt = AuthenticatedAt,
        Claims = new Dictionary<string, string>(Claims),
        ApplicationContext = ApplicationContext
    };
}