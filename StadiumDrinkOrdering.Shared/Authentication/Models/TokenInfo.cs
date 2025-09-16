using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace StadiumDrinkOrdering.Shared.Authentication.Models;

/// <summary>
/// Contains detailed information about an authentication token
/// </summary>
public class TokenInfo
{
    /// <summary>
    /// The raw JWT token string
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// When the token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// When the token was issued
    /// </summary>
    public DateTime IssuedAt { get; set; }

    /// <summary>
    /// The token issuer
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// The token audience
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// The subject (usually user ID)
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// User email from token
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// User role from token
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// All claims from the token
    /// </summary>
    public Dictionary<string, string> Claims { get; set; } = new();

    /// <summary>
    /// Indicates if the token is currently valid (not expired)
    /// </summary>
    public bool IsValid => !IsExpired && !string.IsNullOrEmpty(Token);

    /// <summary>
    /// Indicates if the token is expired
    /// </summary>
    public bool IsExpired => ExpiresAt <= DateTime.UtcNow;

    /// <summary>
    /// Time remaining until expiration
    /// </summary>
    public TimeSpan TimeToExpiry => ExpiresAt - DateTime.UtcNow;

    /// <summary>
    /// Indicates if the token will expire soon (within 5 minutes)
    /// </summary>
    public bool IsExpiringSoon => TimeToExpiry.TotalMinutes <= 5;

    /// <summary>
    /// Creates TokenInfo from a JWT token string
    /// </summary>
    public static TokenInfo FromJwtToken(string token)
    {
        var tokenInfo = new TokenInfo { Token = token };

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            tokenInfo.ExpiresAt = jsonToken.ValidTo;
            tokenInfo.IssuedAt = jsonToken.ValidFrom;
            tokenInfo.Issuer = jsonToken.Issuer;
            tokenInfo.Audience = jsonToken.Audiences?.FirstOrDefault();
            tokenInfo.Subject = jsonToken.Subject;

            // Extract common claims
            tokenInfo.Email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;
            tokenInfo.Role = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

            // Extract all claims
            foreach (var claim in jsonToken.Claims)
            {
                if (!tokenInfo.Claims.ContainsKey(claim.Type))
                {
                    tokenInfo.Claims[claim.Type] = claim.Value;
                }
            }
        }
        catch (Exception)
        {
            // Invalid token format - set minimal info
            tokenInfo.ExpiresAt = DateTime.UtcNow.AddMinutes(-1); // Mark as expired
        }

        return tokenInfo;
    }

    /// <summary>
    /// Creates TokenInfo with custom expiration
    /// </summary>
    public static TokenInfo Create(string token, DateTime expiresAt, string? email = null, string? role = null)
    {
        return new TokenInfo
        {
            Token = token,
            ExpiresAt = expiresAt,
            IssuedAt = DateTime.UtcNow,
            Email = email,
            Role = role
        };
    }

    /// <summary>
    /// Gets a specific claim value
    /// </summary>
    public string? GetClaim(string claimType)
    {
        return Claims.TryGetValue(claimType, out var value) ? value : null;
    }

    /// <summary>
    /// Checks if the token contains a specific claim
    /// </summary>
    public bool HasClaim(string claimType)
    {
        return Claims.ContainsKey(claimType);
    }

    /// <summary>
    /// Checks if the token has the specified role
    /// </summary>
    public bool HasRole(string role)
    {
        return string.Equals(Role, role, StringComparison.OrdinalIgnoreCase) ||
               Claims.Any(c => (c.Key == ClaimTypes.Role || c.Key == "role") &&
                              string.Equals(c.Value, role, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the user ID from the token
    /// </summary>
    public string? GetUserId()
    {
        return Subject ?? GetClaim(ClaimTypes.NameIdentifier) ?? GetClaim("sub") ?? GetClaim("user_id");
    }

    /// <summary>
    /// Gets the username from the token
    /// </summary>
    public string? GetUserName()
    {
        return GetClaim(ClaimTypes.Name) ?? GetClaim("name") ?? GetClaim("username");
    }

    /// <summary>
    /// Creates a ClaimsPrincipal from the token information
    /// </summary>
    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var claims = new List<Claim>();

        // Add standard claims
        if (!string.IsNullOrEmpty(Subject))
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Subject));

        if (!string.IsNullOrEmpty(Email))
            claims.Add(new Claim(ClaimTypes.Email, Email));

        if (!string.IsNullOrEmpty(Role))
            claims.Add(new Claim(ClaimTypes.Role, Role));

        // Add all other claims
        foreach (var kvp in Claims)
        {
            if (!claims.Any(c => c.Type == kvp.Key))
            {
                claims.Add(new Claim(kvp.Key, kvp.Value));
            }
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        return new ClaimsPrincipal(identity);
    }
}