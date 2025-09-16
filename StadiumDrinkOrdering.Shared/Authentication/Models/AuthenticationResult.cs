using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Shared.Authentication.Models;

/// <summary>
/// Result of authentication operations (login, register, token refresh)
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Indicates if the authentication was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// The authentication token (if successful)
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// When the token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserDto? User { get; set; }

    /// <summary>
    /// Error message (if unsuccessful)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Detailed error information
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Error code for programmatic handling
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Additional metadata about the authentication
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Indicates if this is a token refresh result
    /// </summary>
    public bool IsTokenRefresh { get; set; }

    /// <summary>
    /// Indicates if the user needs to change their password
    /// </summary>
    public bool RequirePasswordChange { get; set; }

    /// <summary>
    /// Indicates if two-factor authentication is required
    /// </summary>
    public bool RequiresTwoFactor { get; set; }

    /// <summary>
    /// Creates a successful authentication result
    /// </summary>
    public static AuthenticationResult Success(string token, DateTime expiresAt, UserDto user, bool isTokenRefresh = false)
    {
        return new AuthenticationResult
        {
            IsSuccess = true,
            Token = token,
            ExpiresAt = expiresAt,
            User = user,
            IsTokenRefresh = isTokenRefresh
        };
    }

    /// <summary>
    /// Creates a successful authentication result from LoginResponseDto
    /// </summary>
    public static AuthenticationResult Success(LoginResponseDto response)
    {
        return new AuthenticationResult
        {
            IsSuccess = true,
            Token = response.Token,
            ExpiresAt = response.ExpiresAt,
            User = response.User
        };
    }

    /// <summary>
    /// Creates a failed authentication result
    /// </summary>
    public static AuthenticationResult Failure(string errorMessage, string? errorCode = null)
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Errors = new List<string> { errorMessage }
        };
    }

    /// <summary>
    /// Creates a failed authentication result with multiple errors
    /// </summary>
    public static AuthenticationResult Failure(IEnumerable<string> errors, string? errorCode = null)
    {
        var errorList = errors.ToList();
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = errorList.FirstOrDefault(),
            ErrorCode = errorCode,
            Errors = errorList
        };
    }

    /// <summary>
    /// Creates a result indicating password change is required
    /// </summary>
    public static AuthenticationResult CreatePasswordChangeRequired(string message = "Password change required")
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = message,
            ErrorCode = "PASSWORD_CHANGE_REQUIRED",
            RequirePasswordChange = true
        };
    }

    /// <summary>
    /// Creates a result indicating two-factor authentication is required
    /// </summary>
    public static AuthenticationResult CreateTwoFactorRequired(string message = "Two-factor authentication required")
    {
        return new AuthenticationResult
        {
            IsSuccess = false,
            ErrorMessage = message,
            ErrorCode = "TWO_FACTOR_REQUIRED",
            RequiresTwoFactor = true
        };
    }

    /// <summary>
    /// Adds an error to the result
    /// </summary>
    public void AddError(string error)
    {
        Errors.Add(error);
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = error;
        }
    }

    /// <summary>
    /// Adds metadata to the result
    /// </summary>
    public void AddMetadata(string key, object value)
    {
        Metadata[key] = value;
    }

    /// <summary>
    /// Gets metadata value by key
    /// </summary>
    public T? GetMetadata<T>(string key)
    {
        if (Metadata.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    /// <summary>
    /// Gets the first error message or a default message
    /// </summary>
    public string GetErrorMessage(string defaultMessage = "Authentication failed")
    {
        return ErrorMessage ?? Errors.FirstOrDefault() ?? defaultMessage;
    }

    /// <summary>
    /// Converts to TokenInfo if successful
    /// </summary>
    public TokenInfo? ToTokenInfo()
    {
        if (!IsSuccess || string.IsNullOrEmpty(Token))
            return null;

        return TokenInfo.Create(Token, ExpiresAt, User?.Email, User?.Role.ToString());
    }
}