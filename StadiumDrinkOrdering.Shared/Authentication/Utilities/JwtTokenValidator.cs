using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StadiumDrinkOrdering.Shared.Authentication.Models;

namespace StadiumDrinkOrdering.Shared.Authentication.Utilities;

/// <summary>
/// Utility class for JWT token validation and parsing
/// </summary>
public static class JwtTokenValidator
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    /// <summary>
    /// Validates a JWT token with the provided secret key
    /// </summary>
    public static bool ValidateToken(string token, string secretKey, string? issuer = null, string? audience = null)
    {
        try
        {
            var validationParameters = CreateValidationParameters(secretKey, issuer, audience);
            TokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates a JWT token and returns the claims principal
    /// </summary>
    public static (bool IsValid, ClaimsPrincipal? Principal) ValidateTokenWithClaims(string token, string secretKey, string? issuer = null, string? audience = null)
    {
        try
        {
            var validationParameters = CreateValidationParameters(secretKey, issuer, audience);
            var principal = TokenHandler.ValidateToken(token, validationParameters, out _);
            return (true, principal);
        }
        catch
        {
            return (false, null);
        }
    }

    /// <summary>
    /// Parses JWT token without validation (useful for reading claims from trusted tokens)
    /// </summary>
    public static TokenInfo? ParseTokenUnsafe(string token)
    {
        try
        {
            return TokenInfo.FromJwtToken(token);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Checks if a token is expired without full validation
    /// </summary>
    public static bool IsTokenExpired(string token)
    {
        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            return jsonToken.ValidTo <= DateTime.UtcNow;
        }
        catch
        {
            return true; // Treat invalid tokens as expired
        }
    }

    /// <summary>
    /// Gets the expiration time from a token without full validation
    /// </summary>
    public static DateTime? GetTokenExpiration(string token)
    {
        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            return jsonToken.ValidTo;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the time remaining until token expiration
    /// </summary>
    public static TimeSpan? GetTimeToExpiry(string token)
    {
        var expiration = GetTokenExpiration(token);
        if (expiration.HasValue)
        {
            var timeToExpiry = expiration.Value - DateTime.UtcNow;
            return timeToExpiry.TotalSeconds > 0 ? timeToExpiry : TimeSpan.Zero;
        }
        return null;
    }

    /// <summary>
    /// Checks if token will expire soon (within specified buffer time)
    /// </summary>
    public static bool IsTokenExpiringSoon(string token, int bufferMinutes = 5)
    {
        var timeToExpiry = GetTimeToExpiry(token);
        return timeToExpiry.HasValue && timeToExpiry.Value.TotalMinutes <= bufferMinutes;
    }

    /// <summary>
    /// Extracts claims from token without validation
    /// </summary>
    public static Dictionary<string, string> GetClaims(string token)
    {
        var claims = new Dictionary<string, string>();

        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            foreach (var claim in jsonToken.Claims)
            {
                if (!claims.ContainsKey(claim.Type))
                {
                    claims[claim.Type] = claim.Value;
                }
            }
        }
        catch
        {
            // Return empty dictionary for invalid tokens
        }

        return claims;
    }

    /// <summary>
    /// Gets a specific claim value from token
    /// </summary>
    public static string? GetClaimValue(string token, string claimType)
    {
        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            return jsonToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets user ID from token
    /// </summary>
    public static string? GetUserId(string token)
    {
        return GetClaimValue(token, ClaimTypes.NameIdentifier) ??
               GetClaimValue(token, "sub") ??
               GetClaimValue(token, "user_id");
    }

    /// <summary>
    /// Gets user email from token
    /// </summary>
    public static string? GetUserEmail(string token)
    {
        return GetClaimValue(token, ClaimTypes.Email) ??
               GetClaimValue(token, "email");
    }

    /// <summary>
    /// Gets user role from token
    /// </summary>
    public static string? GetUserRole(string token)
    {
        return GetClaimValue(token, ClaimTypes.Role) ??
               GetClaimValue(token, "role");
    }

    /// <summary>
    /// Gets user roles from token
    /// </summary>
    public static List<string> GetUserRoles(string token)
    {
        var roles = new List<string>();

        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            roles.AddRange(jsonToken.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                                          .Select(c => c.Value));
        }
        catch
        {
            // Return empty list for invalid tokens
        }

        return roles.Distinct().ToList();
    }

    /// <summary>
    /// Checks if token contains specific role
    /// </summary>
    public static bool HasRole(string token, string role)
    {
        var userRoles = GetUserRoles(token);
        return userRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Validates token format without cryptographic validation
    /// </summary>
    public static bool IsValidTokenFormat(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            TokenHandler.ReadJwtToken(token);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets token metadata (issuer, audience, etc.)
    /// </summary>
    public static (string? Issuer, string? Audience, DateTime? IssuedAt, string? JwtId) GetTokenMetadata(string token)
    {
        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            return (
                jsonToken.Issuer,
                jsonToken.Audiences?.FirstOrDefault(),
                jsonToken.ValidFrom,
                jsonToken.Id
            );
        }
        catch
        {
            return (null, null, null, null);
        }
    }

    /// <summary>
    /// Creates token validation parameters
    /// </summary>
    private static TokenValidationParameters CreateValidationParameters(string secretKey, string? issuer, string? audience)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = !string.IsNullOrEmpty(issuer),
            ValidIssuer = issuer,
            ValidateAudience = !string.IsNullOrEmpty(audience),
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1), // Allow 1 minute clock skew
            RequireExpirationTime = true
        };
    }

    /// <summary>
    /// Compares two tokens for equality
    /// </summary>
    public static bool TokensEqual(string? token1, string? token2)
    {
        if (string.IsNullOrEmpty(token1) && string.IsNullOrEmpty(token2))
            return true;

        if (string.IsNullOrEmpty(token1) || string.IsNullOrEmpty(token2))
            return false;

        return string.Equals(token1, token2, StringComparison.Ordinal);
    }

    /// <summary>
    /// Gets the remaining lifetime percentage of a token (0.0 - 1.0)
    /// </summary>
    public static double GetTokenLifetimePercentage(string token)
    {
        try
        {
            var jsonToken = TokenHandler.ReadJwtToken(token);
            var totalLifetime = jsonToken.ValidTo - jsonToken.ValidFrom;
            var remainingLifetime = jsonToken.ValidTo - DateTime.UtcNow;

            if (totalLifetime.TotalSeconds <= 0)
                return 0.0;

            var percentage = remainingLifetime.TotalSeconds / totalLifetime.TotalSeconds;
            return Math.Max(0.0, Math.Min(1.0, percentage));
        }
        catch
        {
            return 0.0;
        }
    }

    /// <summary>
    /// Checks if a token is about to expire and needs refresh
    /// </summary>
    public static bool ShouldRefreshToken(string token, double refreshThreshold = 0.2)
    {
        var lifetimePercentage = GetTokenLifetimePercentage(token);
        return lifetimePercentage <= refreshThreshold;
    }
}