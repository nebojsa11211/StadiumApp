namespace StadiumDrinkOrdering.Shared.Authentication.Constants;

/// <summary>
/// Constants used throughout the authentication system
/// </summary>
public static class AuthenticationConstants
{
    /// <summary>
    /// Application context identifiers for different applications
    /// </summary>
    public static class ApplicationContexts
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Staff = "Staff";
    }

    /// <summary>
    /// Storage keys for different applications and token types
    /// </summary>
    public static class StorageKeys
    {
        /// <summary>
        /// Admin application storage keys
        /// </summary>
        public static class Admin
        {
            public const string Token = "stadium_admin_token";
            public const string RefreshToken = "stadium_admin_refresh_token";
            public const string TokenExpiration = "stadium_admin_token_expiration";
            public const string RefreshTokenExpiration = "stadium_admin_refresh_token_expiration";
            public const string Email = "stadium_admin_email";
            public const string UserData = "stadium_admin_userdata";
        }

        /// <summary>
        /// Customer application storage keys
        /// </summary>
        public static class Customer
        {
            public const string Token = "stadium_customer_token";
            public const string RefreshToken = "stadium_customer_refresh_token";
            public const string TokenExpiration = "stadium_customer_token_expiration";
            public const string RefreshTokenExpiration = "stadium_customer_refresh_token_expiration";
            public const string Email = "stadium_customer_email";
            public const string UserData = "stadium_customer_userdata";
        }

        /// <summary>
        /// Staff application storage keys
        /// </summary>
        public static class Staff
        {
            public const string Token = "stadium_staff_token";
            public const string RefreshToken = "stadium_staff_refresh_token";
            public const string TokenExpiration = "stadium_staff_token_expiration";
            public const string RefreshTokenExpiration = "stadium_staff_refresh_token_expiration";
            public const string Email = "stadium_staff_email";
            public const string UserData = "stadium_staff_userdata";
        }
    }

    /// <summary>
    /// JWT claim types and standard claims
    /// </summary>
    public static class ClaimTypes
    {
        public const string UserId = "user_id";
        public const string Email = "email";
        public const string Role = "role";
        public const string Username = "username";
        public const string SessionId = "session_id";
        public const string DeviceInfo = "device_info";
    }

    /// <summary>
    /// Token refresh configuration
    /// </summary>
    public static class TokenRefresh
    {
        /// <summary>
        /// Time before expiration to trigger proactive refresh
        /// </summary>
        public static readonly TimeSpan RefreshWindow = TimeSpan.FromMinutes(2);

        /// <summary>
        /// Maximum time to wait for refresh operation
        /// </summary>
        public static readonly TimeSpan RefreshTimeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Default refresh token lifetime
        /// </summary>
        public static readonly TimeSpan DefaultRefreshTokenLifetime = TimeSpan.FromDays(7);

        /// <summary>
        /// Default access token lifetime
        /// </summary>
        public static readonly TimeSpan DefaultAccessTokenLifetime = TimeSpan.FromMinutes(15);
    }

    /// <summary>
    /// Security configuration constants
    /// </summary>
    public static class Security
    {
        /// <summary>
        /// Minimum required JWT secret key length
        /// </summary>
        public const int MinSecretKeyLength = 32;

        /// <summary>
        /// Allowed JWT algorithms
        /// </summary>
        public static readonly string[] AllowedAlgorithms = { "HS256", "HS384", "HS512" };

        /// <summary>
        /// Default JWT algorithm
        /// </summary>
        public const string DefaultAlgorithm = "HS256";

        /// <summary>
        /// Maximum number of active refresh tokens per user
        /// </summary>
        public const int MaxRefreshTokensPerUser = 5;
    }

    /// <summary>
    /// HTTP headers related to authentication
    /// </summary>
    public static class Headers
    {
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer";
        public const string DeviceInfo = "X-Device-Info";
        public const string ClientVersion = "X-Client-Version";
        public const string SessionId = "X-Session-Id";
    }

    /// <summary>
    /// API endpoint paths for authentication
    /// </summary>
    public static class Endpoints
    {
        public const string Login = "/auth/login";
        public const string Register = "/auth/register";
        public const string RefreshToken = "/auth/refresh-token";
        public const string RevokeToken = "/auth/revoke-token";
        public const string Profile = "/auth/profile";
        public const string ChangePassword = "/auth/change-password";
    }

    /// <summary>
    /// Password requirements and validation rules
    /// </summary>
    public static class PasswordRequirements
    {
        /// <summary>
        /// Minimum password length
        /// </summary>
        public const int MinLength = 8;

        /// <summary>
        /// Maximum password length
        /// </summary>
        public const int MaxLength = 128;

        /// <summary>
        /// Require at least one digit
        /// </summary>
        public const bool RequireDigit = true;

        /// <summary>
        /// Require at least one lowercase letter
        /// </summary>
        public const bool RequireLowercase = true;

        /// <summary>
        /// Require at least one uppercase letter
        /// </summary>
        public const bool RequireUppercase = true;

        /// <summary>
        /// Require at least one non-alphanumeric character
        /// </summary>
        public const bool RequireNonAlphanumeric = true;

        /// <summary>
        /// Minimum number of unique characters
        /// </summary>
        public const int UniqueChars = 4;
    }

    /// <summary>
    /// Standard JWT claim types used in the application
    /// </summary>
    public static class StandardClaims
    {
        public const string UserId = "user_id";
        public const string Email = "email";
        public const string Role = "role";
        public const string Username = "username";
        public const string FullName = "full_name";
        public const string Subject = "sub";
    }

    /// <summary>
    /// Application roles
    /// </summary>
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string Customer = "Customer";
    }
}