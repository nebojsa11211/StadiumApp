using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using System.Text;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IBruteForceProtectionService _bruteForceService;
    private readonly ICentralizedLoggingClient _loggingClient;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IBruteForceProtectionService bruteForceService,
        ICentralizedLoggingClient loggingClient,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _bruteForceService = bruteForceService;
        _loggingClient = loggingClient;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<ActionResult<EnhancedLoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var ipAddress = GetClientIPAddress();
        var userAgent = Request.Headers.UserAgent.ToString();
        var deviceInfo = GetDeviceInfo();

        try
        {
            if (loginDto == null)
            {
                return BadRequest(new { error = "Invalid request body" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if IP is banned
            if (await _bruteForceService.IsIPBannedAsync(ipAddress))
            {
                await _loggingClient.LogUserActionAsync(
                    action: "LoginAttemptFromBannedIP",
                    category: "Security",
                    details: $"Login attempt from banned IP: {ipAddress}");

                return StatusCode(429, new { error = "IP address is temporarily banned", retryAfter = "3600" });
            }

            // Check if account is locked - TEMPORARILY DISABLED DUE TO DATABASE CONNECTION ISSUES
            Console.WriteLine("=== DEBUG: SKIPPING account lockout check due to database connection issues ===");
            /*
            if (!string.IsNullOrEmpty(loginDto.Email))
            {
                Console.WriteLine($"=== DEBUG: Checking lockout for email: {loginDto.Email} ===");
                var lockout = await _bruteForceService.GetAccountLockoutAsync(loginDto.Email);
                Console.WriteLine($"=== DEBUG: Account lockout result: {lockout} ===");
                if (lockout != null)
                {
                    var remainingTime = (lockout.LockoutEnd - DateTime.UtcNow).TotalMinutes;
                    await _loggingClient.LogUserActionAsync(
                        action: "LoginAttemptOnLockedAccount",
                        category: "Security",
                        details: $"Login attempt on locked account: {loginDto.Email}");

                    return StatusCode(423, new
                    {
                        error = "Account is temporarily locked",
                        lockoutEnd = lockout.LockoutEnd,
                        remainingMinutes = Math.Ceiling(remainingTime)
                    });
                }
            }
            */
            Console.WriteLine("=== DEBUG: Account lockout check skipped ===");

            // Apply progressive delay - TEMPORARILY DISABLED DUE TO DATABASE CONNECTION ISSUES
            Console.WriteLine("=== DEBUG: SKIPPING progressive delay check due to database connection issues ===");
            /*
            var delay = await _bruteForceService.GetProgressiveDelayAsync(ipAddress, loginDto.Email);
            Console.WriteLine($"=== DEBUG: Progressive delay result: {delay}ms ===");
            if (delay > 0)
            {
                Console.WriteLine($"=== DEBUG: Applying delay of {delay}ms ===");
                await Task.Delay(delay);
            }
            */

            // Attempt authentication with basic login (temporary fix for hanging issue)
            Console.WriteLine("=== DEBUG: Starting authentication with AuthService.LoginAsync ===");
            var basicResult = await _authService.LoginAsync(loginDto);
            Console.WriteLine($"=== DEBUG: Authentication result: {basicResult?.Token?.Length ?? 0} chars ===");
            var result = basicResult == null ? null : new EnhancedLoginResponseDto
            {
                Token = basicResult.Token,
                RefreshToken = "temp_refresh_token", // Temporary placeholder
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                User = basicResult.User,
                ExpiresAt = basicResult.ExpiresAt
            };

            if (result == null)
            {
                // Record failed attempt - TEMPORARILY DISABLED DUE TO DATABASE CONNECTION ISSUES
                Console.WriteLine("=== DEBUG: SKIPPING RecordFailedAttemptAsync due to database connection issues ===");
                /*
                await _bruteForceService.RecordFailedAttemptAsync(
                    ipAddress,
                    loginDto.Email,
                    "Login",
                    userAgent,
                    "Invalid credentials");
                */

                return Unauthorized(new { error = "Invalid email or password" });
            }

            // Successful login - clear failed attempts - TEMPORARILY DISABLED DUE TO DATABASE CONNECTION ISSUES
            Console.WriteLine("=== DEBUG: SKIPPING ClearFailedAttemptsAsync due to database connection issues ===");
            /*
            await _bruteForceService.ClearFailedAttemptsAsync(loginDto.Email);
            */

            await _loggingClient.LogUserActionAsync(
                action: "LoginSuccessful",
                category: "UserAction",
                details: $"Successful login for user: {loginDto.Email}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email} from IP {IPAddress}", loginDto.Email, ipAddress);

            // Record as failed attempt on system error - TEMPORARILY DISABLED DUE TO DATABASE CONNECTION ISSUES
            Console.WriteLine("=== DEBUG: SKIPPING RecordFailedAttemptAsync (system error) due to database connection issues ===");
            /*
            await _bruteForceService.RecordFailedAttemptAsync(
                ipAddress,
                loginDto.Email,
                "Login",
                userAgent,
                "System error");
            */

            return StatusCode(500, new { error = "Authentication service error" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var ipAddress = GetClientIPAddress();
        var userAgent = Request.Headers.UserAgent.ToString();

        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if IP is banned
            if (await _bruteForceService.IsIPBannedAsync(ipAddress))
            {
                await _loggingClient.LogUserActionAsync(
                    action: "RefreshAttemptFromBannedIP",
                    category: "Security",
                    details: $"Token refresh attempt from banned IP: {ipAddress}");

                return StatusCode(429, new { error = "IP address is temporarily banned", retryAfter = "3600" });
            }

            var result = await _authService.RefreshTokenAsync(request, ipAddress, userAgent);

            if (result == null)
            {
                await _loggingClient.LogUserActionAsync(
                    action: "RefreshTokenFailed",
                    category: "Security",
                    details: $"Failed token refresh from IP: {ipAddress}");

                return Unauthorized(new { error = "Invalid refresh token" });
            }

            await _loggingClient.LogUserActionAsync(
                action: "RefreshTokenSuccess",
                category: "UserAction",
                details: $"Token successfully refreshed for user: {result.User?.Email}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh from IP {IPAddress}", ipAddress);
            return StatusCode(500, new { error = "Token refresh service error" });
        }
    }

    [HttpPost("revoke-token")]
    public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenRequestDto request)
    {
        var ipAddress = GetClientIPAddress();

        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RevokeRefreshTokenAsync(request.RefreshToken, request.Reason);

            if (!result)
            {
                await _loggingClient.LogUserActionAsync(
                    action: "RevokeTokenFailed",
                    category: "Security",
                    details: $"Failed to revoke token from IP: {ipAddress}");

                return BadRequest(new { error = "Invalid refresh token" });
            }

            await _loggingClient.LogUserActionAsync(
                action: "RevokeTokenSuccess",
                category: "UserAction",
                details: $"Token successfully revoked from IP: {ipAddress}");

            return Ok(new { message = "Token revoked successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token revocation from IP {IPAddress}", ipAddress);
            return StatusCode(500, new { error = "Token revocation service error" });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
    {
        var ipAddress = GetClientIPAddress();
        var userAgent = Request.Headers.UserAgent.ToString();

        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if IP is banned
            if (await _bruteForceService.IsIPBannedAsync(ipAddress))
            {
                await _loggingClient.LogUserActionAsync(
                    action: "RegisterAttemptFromBannedIP",
                    category: "Security",
                    details: $"Registration attempt from banned IP: {ipAddress}");

                return StatusCode(429, new { error = "IP address is temporarily banned", retryAfter = "3600" });
            }

            // Apply progressive delay for registration attempts
            var delay = await _bruteForceService.GetProgressiveDelayAsync(ipAddress, registerDto.Email);
            if (delay > 0)
            {
                await Task.Delay(delay);
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (result == null)
            {
                // Record failed registration attempt
                await _bruteForceService.RecordFailedAttemptAsync(
                    ipAddress,
                    registerDto.Email,
                    "Register",
                    userAgent,
                    "User already exists");

                return BadRequest(new { error = "User with this email or username already exists" });
            }

            await _loggingClient.LogUserActionAsync(
                action: "RegistrationSuccessful",
                category: "UserAction",
                details: $"Successful registration for user: {registerDto.Email}");

            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email} from IP {IPAddress}", registerDto.Email, ipAddress);

            // Record as failed attempt on system error
            await _bruteForceService.RecordFailedAttemptAsync(
                ipAddress,
                registerDto.Email,
                "Register",
                userAgent,
                "System error");

            return StatusCode(500, new { error = "Registration service error" });
        }
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _authService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    /// <summary>
    /// Gets the client IP address from the request
    /// </summary>
    /// <returns>Client IP address</returns>
    private string GetClientIPAddress()
    {
        try
        {
            // Check for forwarded IP first (for load balancers/proxies)
            var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // X-Forwarded-For can contain multiple IPs, take the first one
                var firstIP = forwardedFor.Split(',').FirstOrDefault()?.Trim();
                if (!string.IsNullOrEmpty(firstIP))
                    return firstIP;
            }

            // Check for real IP header
            var realIP = Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIP))
                return realIP;

            // Fall back to connection remote IP
            var connectionIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(connectionIP))
            {
                // Handle IPv6 loopback
                if (connectionIP == "::1")
                    return "127.0.0.1";

                return connectionIP;
            }

            // Final fallback
            return "127.0.0.1";
        }
        catch
        {
            return "127.0.0.1";
        }
    }

    /// <summary>
    /// Extracts device information from request headers
    /// </summary>
    /// <returns>Device information string</returns>
    private string GetDeviceInfo()
    {
        try
        {
            var userAgent = Request.Headers.UserAgent.ToString();
            var acceptLanguage = Request.Headers.AcceptLanguage.ToString();

            var deviceInfo = $"UserAgent: {userAgent}";
            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                deviceInfo += $"; Language: {acceptLanguage}";
            }

            return deviceInfo;
        }
        catch
        {
            return "Unknown Device";
        }
    }
}


