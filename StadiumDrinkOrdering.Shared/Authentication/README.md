# Shared Authentication System

## Overview

The Stadium Drink Ordering shared authentication system provides a comprehensive, standardized foundation for authentication across all applications (Admin, Customer, Staff). It includes interfaces, models, utilities, and extensions that ensure consistent authentication patterns and security practices throughout the entire ecosystem.

## Architecture

```
Authentication/
├── Constants/
│   └── AuthenticationConstants.cs     # Central constants and configuration
├── Extensions/
│   ├── ClaimsPrincipalExtensions.cs   # Claims extraction utilities
│   ├── HttpClientExtensions.cs        # HTTP client authentication helpers
│   └── ServiceCollectionExtensions.cs # DI container setup
├── Interfaces/
│   ├── IAuthenticationStateService.cs # Core authentication state management
│   ├── ISecureApiService.cs           # Authenticated API service interface
│   └── ITokenStorageService.cs        # Token storage and management
├── Models/
│   ├── AuthenticationConfiguration.cs # Configuration settings
│   ├── AuthenticationResult.cs        # Authentication operation results
│   ├── AuthenticationState.cs         # Current user authentication state
│   └── TokenInfo.cs                   # JWT token information and validation
└── Utilities/
    ├── JwtTokenValidator.cs           # JWT validation and parsing
    ├── PasswordValidator.cs           # Password strength validation
    ├── SecurityUtilities.cs           # Cryptographic operations
    └── ValidationAttributes.cs        # Data annotation validators
```

## Key Components

### 1. Authentication Interfaces

#### IAuthenticationStateService
Central interface for managing authentication state across applications.

```csharp
public interface IAuthenticationStateService
{
    AuthenticationState State { get; }
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    string? UserRole { get; }

    event Action<AuthenticationState>? OnAuthenticationStateChanged;

    Task InitializeAsync();
    Task<bool> LoginAsync(AuthenticationResult authResult);
    Task LogoutAsync();
    Task<bool> RefreshTokenAsync();
    bool HasRole(string role);
}
```

#### ITokenStorageService
Unified interface for secure token storage and management.

```csharp
public interface ITokenStorageService
{
    string? Token { get; set; }
    TokenInfo? TokenInfo { get; }
    string ApplicationContext { get; }

    Task StoreTokenAsync(string token, DateTime? expiresAt = null, string? userEmail = null);
    Task<bool> IsTokenValidAsync();
    Task ClearTokenAsync();
    event Action? OnTokenExpired;
}
```

#### ISecureApiService
Interface for making authenticated API calls with automatic token handling.

```csharp
public interface ISecureApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
    Task<HttpResponseMessage> UploadFileAsync(string endpoint, Stream fileStream, string fileName);
    bool HasValidToken();
}
```

### 2. Authentication Models

#### AuthenticationState
Represents the current authentication state with comprehensive user information.

```csharp
public class AuthenticationState
{
    public bool IsAuthenticated { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public string? Role { get; set; }
    public List<string> Roles { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiresAt { get; set; }
    public Dictionary<string, string> Claims { get; set; }

    public bool HasRole(string role);
    public static AuthenticationState FromAuthenticationResult(AuthenticationResult result);
}
```

#### TokenInfo
Detailed JWT token information with validation capabilities.

```csharp
public class TokenInfo
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsValid { get; }
    public bool IsExpired { get; }
    public TimeSpan TimeToExpiry { get; }

    public static TokenInfo FromJwtToken(string token);
    public ClaimsPrincipal ToClaimsPrincipal();
}
```

#### AuthenticationResult
Result of authentication operations with comprehensive error handling.

```csharp
public class AuthenticationResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; }

    public static AuthenticationResult Success(string token, DateTime expiresAt, UserDto user);
    public static AuthenticationResult Failure(string errorMessage, string? errorCode = null);
}
```

### 3. Extension Methods

#### ClaimsPrincipal Extensions
Standardized methods for extracting user information from claims.

```csharp
public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal);
    public static string? GetUserEmail(this ClaimsPrincipal principal);
    public static string? GetUserRole(this ClaimsPrincipal principal);
    public static bool HasRole(this ClaimsPrincipal principal, string role);
    public static bool IsTokenExpired(this ClaimsPrincipal principal);
    public static object GetUserInfo(this ClaimsPrincipal principal);
}
```

#### Service Collection Extensions
Simplified authentication service registration.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedAuthentication<TAuthStateService, TTokenStorageService, TSecureApiService>(
        this IServiceCollection services,
        string applicationContext,
        string apiBaseUrl);

    public static IServiceCollection AddAuthenticationBackgroundServices(this IServiceCollection services);
}
```

#### HTTP Client Extensions
Automatic token injection and authentication handling.

```csharp
public static class HttpClientExtensions
{
    public static HttpClient WithAuthToken(this HttpClient client, string? token);
    public static Task<HttpResponseMessage> GetWithAuthAsync(this HttpClient client, string requestUri, ITokenStorageService tokenStorage);
    public static StringContent ToJsonContent<T>(this T obj, JsonSerializerOptions? options = null);
}
```

### 4. Security Utilities

#### JWT Token Validator
Comprehensive JWT validation and parsing utilities.

```csharp
public static class JwtTokenValidator
{
    public static bool ValidateToken(string token, string secretKey, string? issuer = null, string? audience = null);
    public static TokenInfo? ParseTokenUnsafe(string token);
    public static bool IsTokenExpired(string token);
    public static string? GetUserId(string token);
    public static bool HasRole(string token, string role);
}
```

#### Password Validator
Advanced password strength validation with customizable policies.

```csharp
public static class PasswordValidator
{
    public static PasswordValidationResult ValidatePassword(string password, PasswordPolicy policy);
    public static int CalculatePasswordStrength(string password);
    public static string GenerateSecurePassword(int length = 12, bool includeSymbols = true);
}
```

#### Security Utilities
Cryptographic operations and secure random generation.

```csharp
public static class SecurityUtilities
{
    public static string GenerateSecureRandomString(int length, string? allowedCharacters = null);
    public static string GenerateAuthToken(int length = 32);
    public static (string Hash, string Salt) HashPassword(string password);
    public static bool VerifyPassword(string password, string hash, string salt);
    public static string GenerateTotpCode(string secret, DateTime? timestamp = null);
}
```

## Usage Examples

### 1. Basic Setup in Program.cs

```csharp
// Admin Application
builder.Services.AddSharedAuthentication<AuthStateService, TokenStorageService, AdminApiService>(
    "Admin",
    "https://localhost:7010"
);

// Customer Application
builder.Services.AddSharedAuthentication<AuthStateService, TokenStorageService, CustomerApiService>(
    "Customer",
    "https://localhost:7010"
);
```

### 2. Using Authentication State Service

```csharp
@inject IAuthenticationStateService AuthService

@if (AuthService.IsAuthenticated)
{
    <p>Welcome, @AuthService.UserEmail!</p>
    @if (AuthService.HasRole("Admin"))
    {
        <a href="/admin">Admin Panel</a>
    }
}
else
{
    <a href="/login">Login</a>
}

@code {
    protected override async Task OnInitializedAsync()
    {
        AuthService.OnAuthenticationStateChanged += StateHasChanged;
        await AuthService.InitializeAsync();
    }
}
```

### 3. Making Authenticated API Calls

```csharp
@inject ISecureApiService ApiService

@code {
    private async Task LoadUserData()
    {
        try
        {
            var userData = await ApiService.GetAsync<UserDto>("/users/profile");
            if (userData != null)
            {
                // Use user data
            }
        }
        catch (Exception ex)
        {
            // Handle authentication errors
        }
    }
}
```

### 4. Custom Password Validation

```csharp
public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StrongPassword(minLength: 8, requireUppercase: true)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [ConfirmPassword(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}
```

### 5. Token Validation

```csharp
var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

// Basic validation
if (JwtTokenValidator.IsTokenExpired(token))
{
    // Token expired, need to refresh
}

// Get token information
var tokenInfo = JwtTokenValidator.ParseTokenUnsafe(token);
if (tokenInfo != null)
{
    var userId = tokenInfo.GetUserId();
    var email = tokenInfo.Email;
    var roles = tokenInfo.GetUserRoles();
}
```

## Configuration

### Application Settings

```json
{
  "Authentication": {
    "TokenRefreshBufferMinutes": 5,
    "AutoRefreshIntervalMinutes": 30,
    "EnableAutoRefresh": true,
    "EnablePersistentLogin": true
  },
  "ApiSettings": {
    "BaseUrl": "https://localhost:7010"
  }
}
```

### Environment-Specific Configuration

```csharp
// Development
services.AddSharedAuthentication<AuthStateService, TokenStorageService, ApiService>(
    AuthenticationConfiguration.ForAdmin("https://localhost:7010")
);

// Production
services.AddSharedAuthentication<AuthStateService, TokenStorageService, ApiService>(
    AuthenticationConfiguration.ForAdmin("https://api.stadium.com")
);
```

## Constants and Standards

### Storage Keys
- **Admin**: `admin_auth_token`, `admin_user_email`, `admin_user_data`
- **Customer**: `customer_auth_token`, `customer_user_email`, `customer_user_data`
- **Staff**: `staff_auth_token`, `staff_user_email`, `staff_user_data`

### Roles
- **Admin**: Administrative access
- **Staff**: Staff application access
- **Customer**: Customer application access
- **Manager**: Enhanced staff privileges

### Error Codes
- `INVALID_CREDENTIALS`: Login failed
- `TOKEN_EXPIRED`: Token needs refresh
- `INSUFFICIENT_PERMISSIONS`: Role-based access denied
- `USER_NOT_FOUND`: User account not found

## Security Features

### Password Requirements
- Minimum 6 characters (configurable)
- Requires lowercase letters
- Requires digits
- Optional uppercase and special characters
- Prevents common patterns and sequential characters

### Token Security
- JWT-based authentication
- Automatic token refresh
- Secure token storage
- Token expiration validation
- Role-based access control

### Cryptographic Security
- PBKDF2 password hashing with salt
- Cryptographically secure random generation
- Constant-time string comparison
- TOTP support for two-factor authentication

## Error Handling

The system includes comprehensive error handling with:
- Detailed error messages for development
- Sanitized error responses for production
- Automatic token refresh on authentication failures
- Graceful degradation for offline scenarios

## Testing

### Unit Testing Examples

```csharp
[Test]
public void PasswordValidator_ValidatesStrongPassword()
{
    var result = PasswordValidator.ValidatePassword("MyStr0ngP@ssw0rd!");
    Assert.IsTrue(result.IsValid);
    Assert.IsTrue(result.Strength > 80);
}

[Test]
public void JwtTokenValidator_DetectsExpiredToken()
{
    var expiredToken = "eyJ..."; // Token with past expiration
    Assert.IsTrue(JwtTokenValidator.IsTokenExpired(expiredToken));
}
```

### Integration Testing

```csharp
[Test]
public async Task AuthenticationStateService_MaintainsStateAcrossRequests()
{
    var authService = serviceProvider.GetRequiredService<IAuthenticationStateService>();
    await authService.InitializeAsync();

    var loginResult = AuthenticationResult.Success("token", DateTime.UtcNow.AddHours(1), new UserDto());
    await authService.LoginAsync(loginResult);

    Assert.IsTrue(authService.IsAuthenticated);
    Assert.IsNotNull(authService.UserEmail);
}
```

## Migration Guide

### From Existing Authentication

1. **Install Dependencies**: Update project references to include shared authentication
2. **Update Service Registration**: Replace custom auth services with shared interfaces
3. **Update Components**: Use new authentication state service instead of custom implementations
4. **Update API Calls**: Use secure API service for authenticated requests
5. **Update Validation**: Replace custom password validation with shared validators

### Breaking Changes

- Authentication state is now centralized in `IAuthenticationStateService`
- Token storage uses unified `ITokenStorageService` interface
- API calls should use `ISecureApiService` for automatic token handling
- Custom password validation should use `PasswordValidator` utility

## Best Practices

1. **Always use shared interfaces** instead of concrete implementations
2. **Register services using extension methods** for consistency
3. **Use validation attributes** for data validation
4. **Handle authentication events** for UI updates
5. **Implement proper error handling** for authentication failures
6. **Use secure utilities** for password hashing and token generation
7. **Follow role-based access patterns** using `HasRole()` methods
8. **Implement token refresh** to maintain user sessions

## Support and Maintenance

This shared authentication system is designed to be:
- **Extensible**: Easy to add new authentication methods
- **Maintainable**: Centralized logic for easier updates
- **Testable**: Interfaces allow for easy mocking and testing
- **Secure**: Built-in security best practices and validation
- **Consistent**: Standardized patterns across all applications

For issues or feature requests, update the shared library and all consuming applications will benefit from the improvements.