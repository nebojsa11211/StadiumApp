namespace StadiumDrinkOrdering.Shared.Authentication.Models;

/// <summary>
/// Configuration settings for authentication across applications
/// </summary>
public class AuthenticationConfiguration
{
    /// <summary>
    /// Application context (Admin, Customer, Staff)
    /// </summary>
    public string ApplicationContext { get; set; } = string.Empty;

    /// <summary>
    /// Base API URL for authentication endpoints
    /// </summary>
    public string ApiBaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Login endpoint path
    /// </summary>
    public string LoginEndpoint { get; set; } = "/auth/login";

    /// <summary>
    /// Logout endpoint path
    /// </summary>
    public string LogoutEndpoint { get; set; } = "/auth/logout";

    /// <summary>
    /// Token refresh endpoint path
    /// </summary>
    public string RefreshEndpoint { get; set; } = "/auth/refresh";

    /// <summary>
    /// Register endpoint path
    /// </summary>
    public string RegisterEndpoint { get; set; } = "/auth/register";

    /// <summary>
    /// Token validation endpoint path
    /// </summary>
    public string ValidateEndpoint { get; set; } = "/auth/validate";

    /// <summary>
    /// Local storage key prefix for this application
    /// </summary>
    public string StorageKeyPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration buffer time in minutes (refresh token when this close to expiry)
    /// </summary>
    public int TokenRefreshBufferMinutes { get; set; } = 5;

    /// <summary>
    /// Automatic token refresh interval in minutes
    /// </summary>
    public int AutoRefreshIntervalMinutes { get; set; } = 30;

    /// <summary>
    /// Maximum token lifetime in hours
    /// </summary>
    public int MaxTokenLifetimeHours { get; set; } = 24;

    /// <summary>
    /// Enable automatic token refresh
    /// </summary>
    public bool EnableAutoRefresh { get; set; } = true;

    /// <summary>
    /// Enable persistent login (remember me)
    /// </summary>
    public bool EnablePersistentLogin { get; set; } = true;

    /// <summary>
    /// Enable secure storage (HTTPS only cookies)
    /// </summary>
    public bool EnableSecureStorage { get; set; } = true;

    /// <summary>
    /// Redirect URL after successful login
    /// </summary>
    public string? LoginRedirectUrl { get; set; }

    /// <summary>
    /// Redirect URL after logout
    /// </summary>
    public string? LogoutRedirectUrl { get; set; }

    /// <summary>
    /// Login page URL
    /// </summary>
    public string LoginPageUrl { get; set; } = "/login";

    /// <summary>
    /// Unauthorized access redirect URL
    /// </summary>
    public string UnauthorizedRedirectUrl { get; set; } = "/login";

    /// <summary>
    /// Additional HTTP headers to include in authentication requests
    /// </summary>
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();

    /// <summary>
    /// Allowed roles for this application
    /// </summary>
    public List<string> AllowedRoles { get; set; } = new();

    /// <summary>
    /// Creates default configuration for Admin application
    /// </summary>
    public static AuthenticationConfiguration ForAdmin(string apiBaseUrl) => new()
    {
        ApplicationContext = "Admin",
        ApiBaseUrl = apiBaseUrl,
        StorageKeyPrefix = "admin_auth",
        AllowedRoles = new() { "Admin" },
        LoginRedirectUrl = "/dashboard",
        LogoutRedirectUrl = "/login"
    };

    /// <summary>
    /// Creates default configuration for Customer application
    /// </summary>
    public static AuthenticationConfiguration ForCustomer(string apiBaseUrl) => new()
    {
        ApplicationContext = "Customer",
        ApiBaseUrl = apiBaseUrl,
        StorageKeyPrefix = "customer_auth",
        AllowedRoles = new() { "Customer" },
        LoginRedirectUrl = "/",
        LogoutRedirectUrl = "/"
    };

    /// <summary>
    /// Creates default configuration for Staff application
    /// </summary>
    public static AuthenticationConfiguration ForStaff(string apiBaseUrl) => new()
    {
        ApplicationContext = "Staff",
        ApiBaseUrl = apiBaseUrl,
        StorageKeyPrefix = "staff_auth",
        AllowedRoles = new() { "Staff", "Admin" },
        LoginRedirectUrl = "/orders",
        LogoutRedirectUrl = "/login"
    };

    /// <summary>
    /// Gets the storage key for a specific data type
    /// </summary>
    public string GetStorageKey(string dataType) => $"{StorageKeyPrefix}_{dataType}";

    /// <summary>
    /// Gets the token storage key
    /// </summary>
    public string TokenStorageKey => GetStorageKey("token");

    /// <summary>
    /// Gets the email storage key
    /// </summary>
    public string EmailStorageKey => GetStorageKey("email");

    /// <summary>
    /// Gets the user data storage key
    /// </summary>
    public string UserDataStorageKey => GetStorageKey("user_data");

    /// <summary>
    /// Gets the token expiration storage key
    /// </summary>
    public string TokenExpirationStorageKey => GetStorageKey("token_expiration");

    /// <summary>
    /// Validates the configuration
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ApplicationContext) &&
               !string.IsNullOrEmpty(ApiBaseUrl) &&
               !string.IsNullOrEmpty(StorageKeyPrefix) &&
               TokenRefreshBufferMinutes > 0 &&
               AutoRefreshIntervalMinutes > 0 &&
               MaxTokenLifetimeHours > 0;
    }

    /// <summary>
    /// Gets validation errors
    /// </summary>
    public List<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(ApplicationContext))
            errors.Add("ApplicationContext is required");

        if (string.IsNullOrEmpty(ApiBaseUrl))
            errors.Add("ApiBaseUrl is required");

        if (string.IsNullOrEmpty(StorageKeyPrefix))
            errors.Add("StorageKeyPrefix is required");

        if (TokenRefreshBufferMinutes <= 0)
            errors.Add("TokenRefreshBufferMinutes must be greater than 0");

        if (AutoRefreshIntervalMinutes <= 0)
            errors.Add("AutoRefreshIntervalMinutes must be greater than 0");

        if (MaxTokenLifetimeHours <= 0)
            errors.Add("MaxTokenLifetimeHours must be greater than 0");

        return errors;
    }
}