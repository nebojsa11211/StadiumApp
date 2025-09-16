using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StadiumDrinkOrdering.API.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<EnhancedLoginResponseDto?> LoginWithRefreshTokenAsync(LoginDto loginDto, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null);
    Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null);
    Task<bool> RevokeAllUserRefreshTokensAsync(int userId, string? reason = null);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> GetUserByIdAsync(int userId);

    // User management methods for Admin
    Task<UserListDto> GetUsersAsync(UserFilterDto filter);
    Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    Task<bool> ChangeUserPasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<bool> DeleteUserAsync(int userId);

    // Refresh Token management methods
    Task<List<RefreshTokenInfoDto>> GetUserRefreshTokensAsync(int userId);
    Task CleanupExpiredRefreshTokensAsync();
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        // Use AsNoTracking for read-only operations to improve performance
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return null;
        }

        // Verify password - this is working correctly
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        // Update last login with optimized approach - use ExecuteUpdateAsync for better performance
        try
        {
            await _context.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(u => u.SetProperty(p => p.LastLoginAt, DateTime.UtcNow));
        }
        catch
        {
            // Don't fail login if last login update fails
        }

        var token = GenerateJwtToken(user);
        var userDto = MapToUserDto(user);

        return new LoginResponseDto
        {
            Token = token,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15) // Shorter access token lifetime for security
        };
    }

    public async Task<EnhancedLoginResponseDto?> LoginWithRefreshTokenAsync(LoginDto loginDto, string? deviceInfo = null, string? ipAddress = null, string? userAgent = null)
    {
        // Use existing login logic but add refresh token
        var loginResult = await LoginAsync(loginDto);
        if (loginResult == null)
        {
            return null;
        }

        // Generate refresh token
        var jwtId = ExtractJwtId(loginResult.Token) ?? Guid.NewGuid().ToString();
        var refreshToken = RefreshToken.Create(
            loginResult.User.Id,
            jwtId,
            TimeSpan.FromDays(7), // 7 days for refresh token
            deviceInfo,
            ipAddress,
            userAgent);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return EnhancedLoginResponseDto.FromLoginResponse(
            loginResult,
            refreshToken.Token,
            refreshToken.ExpiresAt);
    }

    public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null)
    {
        // Validate access token format (even if expired)
        if (!IsValidTokenFormat(request.AccessToken))
        {
            return null;
        }

        // Extract user ID and JWT ID from access token
        var userId = ExtractUserIdFromToken(request.AccessToken);
        var jwtId = ExtractJwtId(request.AccessToken);

        if (userId == null || jwtId == null)
        {
            return null;
        }

        // Find and validate refresh token
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken
                                      && rt.JwtId == jwtId
                                      && rt.UserId == userId.Value
                                      && !rt.IsUsed
                                      && !rt.IsRevoked
                                      && rt.ExpiresAt > DateTime.UtcNow);

        if (refreshToken == null)
        {
            return null;
        }

        // Get user for new token generation
        var user = await _context.Users.FindAsync(userId.Value);
        if (user == null)
        {
            return null;
        }

        // Mark old refresh token as used
        refreshToken.MarkAsUsed("Token refreshed");

        // Generate new tokens
        var newAccessToken = GenerateJwtToken(user);
        var newJwtId = ExtractJwtId(newAccessToken) ?? Guid.NewGuid().ToString();
        var newRefreshToken = RefreshToken.Create(
            user.Id,
            newJwtId,
            TimeSpan.FromDays(7),
            request.DeviceInfo ?? refreshToken.DeviceInfo,
            ipAddress ?? refreshToken.IpAddress,
            userAgent ?? refreshToken.UserAgent);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpiresAt = newRefreshToken.ExpiresAt,
            User = MapToUserDto(user)
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

        if (token == null)
        {
            return false;
        }

        token.MarkAsRevoked(reason ?? "Token revoked");
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeAllUserRefreshTokensAsync(int userId, string? reason = null)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.MarkAsRevoked(reason ?? "All tokens revoked");
        }

        await _context.SaveChangesAsync();
        return tokens.Any();
    }

    public async Task<List<RefreshTokenInfoDto>> GetUserRefreshTokensAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync();

        return tokens.Select(rt => new RefreshTokenInfoDto
        {
            Id = rt.Id,
            TokenPreview = rt.Token.Length > 8 ? rt.Token[..8] + "..." : rt.Token,
            CreatedAt = rt.CreatedAt,
            ExpiresAt = rt.ExpiresAt,
            DeviceInfo = rt.DeviceInfo,
            IpAddress = rt.IpAddress,
            IsValid = rt.IsValid,
            InvalidatedAt = rt.InvalidatedAt,
            InvalidationReason = rt.InvalidationReason
        }).ToList();
    }

    public async Task CleanupExpiredRefreshTokensAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(rt => rt.ExpiresAt <= DateTime.UtcNow || rt.IsUsed || rt.IsRevoked)
            .Where(rt => rt.CreatedAt <= DateTime.UtcNow.AddDays(-30)) // Keep recent tokens for audit
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }

    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username))
        {
            return null;
        }

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToUserDto(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user != null ? MapToUserDto(user) : null;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        // Get JWT secret from environment variable first, then configuration
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                        ?? jwtSettings["SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException(
                "JWT Secret Key is not configured. Set the JWT_SECRET_KEY environment variable or JwtSettings:SecretKey in configuration.");
        }

        if (secretKey.Length < 32)
        {
            throw new InvalidOperationException(
                "JWT Secret Key must be at least 32 characters long for security.");
        }

        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                     ?? jwtSettings["Issuer"]
                     ?? "StadiumDrinkOrdering";

        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                       ?? jwtSettings["Audience"]
                       ?? "StadiumDrinkOrdering";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Generate unique JWT ID for refresh token tracking
        var jwtId = Guid.NewGuid().ToString();

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Short-lived access tokens
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #region JWT Helper Methods

    private static bool IsValidTokenFormat(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            return jsonToken != null;
        }
        catch
        {
            return false;
        }
    }

    private static int? ExtractUserIdFromToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
        }
        catch
        {
            // Token parsing failed
        }

        return null;
    }

    private static string? ExtractJwtId(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            return jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    public async Task<UserListDto> GetUsersAsync(UserFilterDto filter)
    {
        var query = _context.Users.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(u => u.Username.ToLower().Contains(searchTerm) || 
                                    u.Email.ToLower().Contains(searchTerm));
        }

        if (filter.Role.HasValue)
        {
            query = query.Where(u => u.Role == filter.Role.Value);
        }

        if (filter.CreatedAfter.HasValue)
        {
            query = query.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);
        }

        if (filter.CreatedBefore.HasValue)
        {
            query = query.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);
        }

        var totalCount = await query.CountAsync();

        // Apply pagination
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(u => MapToUserDto(u))
            .ToListAsync();

        return new UserListDto
        {
            Users = users,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email || u.Username == createUserDto.Username))
        {
            return null;
        }

        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            Role = createUserDto.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return MapToUserDto(user);
    }

    public async Task<UserDto?> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        // Check if email/username is taken by another user
        if (await _context.Users.AnyAsync(u => u.Id != userId && 
            (u.Email == updateUserDto.Email || u.Username == updateUserDto.Username)))
        {
            return null;
        }

        user.Username = updateUserDto.Username;
        user.Email = updateUserDto.Email;
        if (updateUserDto.Role.HasValue)
        {
            user.Role = updateUserDto.Role.Value;
        }

        await _context.SaveChangesAsync();
        return MapToUserDto(user);
    }

    public async Task<bool> ChangeUserPasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Prevent deletion if this is the last admin user
        if (user.Role == UserRole.Admin)
        {
            var adminCount = await _context.Users.CountAsync(u => u.Role == UserRole.Admin);
            if (adminCount <= 1)
            {
                return false; // Cannot delete the last admin
            }
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
