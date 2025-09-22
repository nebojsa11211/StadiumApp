using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Controllers;

/// <summary>
/// Controller to handle authentication session synchronization
/// between client-side localStorage and server-side session state
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthSessionController : ControllerBase
{
    private readonly ILogger<AuthSessionController> _logger;

    // Session keys for server-side storage
    private const string SESSION_TOKEN_KEY = "auth_token";
    private const string SESSION_TOKEN_EXPIRATION_KEY = "auth_token_expiration";
    private const string SESSION_REFRESH_TOKEN_KEY = "auth_refresh_token";
    private const string SESSION_REFRESH_TOKEN_EXPIRATION_KEY = "auth_refresh_token_expiration";
    private const string SESSION_USER_EMAIL_KEY = "auth_user_email";

    public AuthSessionController(ILogger<AuthSessionController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Synchronizes authentication tokens from client localStorage to server session
    /// </summary>
    [HttpPost("sync-session")]
    public IActionResult SyncSession([FromBody] TokenSyncRequest request)
    {
        try
        {
            var session = HttpContext.Session;

            // Store token data in session
            if (!string.IsNullOrEmpty(request.Token))
            {
                session.SetString(SESSION_TOKEN_KEY, request.Token);
                _logger.LogTrace("Token synced to session");
            }

            if (!string.IsNullOrEmpty(request.TokenExpiration))
            {
                session.SetString(SESSION_TOKEN_EXPIRATION_KEY, request.TokenExpiration);
                _logger.LogTrace("Token expiration synced to session");
            }

            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                session.SetString(SESSION_REFRESH_TOKEN_KEY, request.RefreshToken);
                _logger.LogTrace("Refresh token synced to session");
            }

            if (!string.IsNullOrEmpty(request.RefreshTokenExpiration))
            {
                session.SetString(SESSION_REFRESH_TOKEN_EXPIRATION_KEY, request.RefreshTokenExpiration);
                _logger.LogTrace("Refresh token expiration synced to session");
            }

            if (!string.IsNullOrEmpty(request.UserEmail))
            {
                session.SetString(SESSION_USER_EMAIL_KEY, request.UserEmail);
                _logger.LogTrace("User email synced to session");
            }

            _logger.LogDebug("Authentication tokens successfully synced to session");
            return Ok(new { success = true, message = "Tokens synced to session" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync tokens to session");
            return StatusCode(500, new { success = false, message = "Failed to sync tokens to session" });
        }
    }

    /// <summary>
    /// Clears authentication tokens from server session
    /// </summary>
    [HttpPost("clear-session")]
    public IActionResult ClearSession()
    {
        try
        {
            var session = HttpContext.Session;

            // Remove all authentication-related session data
            session.Remove(SESSION_TOKEN_KEY);
            session.Remove(SESSION_TOKEN_EXPIRATION_KEY);
            session.Remove(SESSION_REFRESH_TOKEN_KEY);
            session.Remove(SESSION_REFRESH_TOKEN_EXPIRATION_KEY);
            session.Remove(SESSION_USER_EMAIL_KEY);

            // Also remove any user data keys
            var keysToRemove = new List<string>();
            foreach (var key in session.Keys)
            {
                if (key.StartsWith("user_data_"))
                {
                    keysToRemove.Add(key);
                }
            }
            foreach (var key in keysToRemove)
            {
                session.Remove(key);
            }

            _logger.LogDebug("Authentication session cleared successfully");
            return Ok(new { success = true, message = "Session cleared" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear session");
            return StatusCode(500, new { success = false, message = "Failed to clear session" });
        }
    }

    /// <summary>
    /// Gets the current authentication status from session
    /// </summary>
    [HttpGet("session-status")]
    public IActionResult GetSessionStatus()
    {
        try
        {
            var session = HttpContext.Session;

            var status = new
            {
                hasToken = !string.IsNullOrEmpty(session.GetString(SESSION_TOKEN_KEY)),
                hasRefreshToken = !string.IsNullOrEmpty(session.GetString(SESSION_REFRESH_TOKEN_KEY)),
                userEmail = session.GetString(SESSION_USER_EMAIL_KEY),
                tokenExpiration = session.GetString(SESSION_TOKEN_EXPIRATION_KEY),
                sessionId = session.Id
            };

            _logger.LogTrace("Session status retrieved: hasToken={HasToken}, hasRefreshToken={HasRefreshToken}",
                status.hasToken, status.hasRefreshToken);

            return Ok(status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get session status");
            return StatusCode(500, new { success = false, message = "Failed to get session status" });
        }
    }
}

/// <summary>
/// Request model for token synchronization
/// </summary>
public class TokenSyncRequest
{
    public string? Token { get; set; }
    public string? TokenExpiration { get; set; }
    public string? RefreshToken { get; set; }
    public string? RefreshTokenExpiration { get; set; }
    public string? UserEmail { get; set; }
}