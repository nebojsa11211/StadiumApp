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

    /// <summary>Updates the signed-in fan's own profile (name, phone, OIB). Returns the updated
    /// profile, or null if the user no longer exists.</summary>
    Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto dto);

    /// <summary>Validates an account-activation token and reports who it belongs to (for the set-password
    /// page). Never throws.</summary>
    Task<ActivationInfoDto> GetActivationInfoAsync(string token);

    /// <summary>Claims a shell account by setting its first password from an activation token, then returns
    /// a login response so the fan is signed in. Null if the token is missing/expired/used or the account
    /// is already active.</summary>
    Task<LoginResponseDto?> ActivateAccountAsync(ActivateAccountDto dto);
    Task<StaffMemberStatsDto?> GetStaffMemberStatsAsync(int userId);

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
        Console.WriteLine("=== DEBUG: AuthService.LoginAsync starting ===");

        // Use AsNoTracking for read-only operations to improve performance
        Console.WriteLine("=== DEBUG: Looking up user by email ===");

        // User lookup - database is fast, no timeout needed
        User? user = null;
        try
        {
            // Use FirstOrDefaultAsync for better performance
            user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == loginDto.Email)
                .FirstOrDefaultAsync();

            Console.WriteLine($"=== DEBUG: User lookup completed successfully, found: {user != null} ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=== DEBUG: User lookup failed with exception: {ex.Message} ===");
            return null;
        }

        if (user == null)
        {
            Console.WriteLine("=== DEBUG: User not found, returning null ===");
            return null;
        }

        // Verify password - this is working correctly
        Console.WriteLine("=== DEBUG: Starting password verification ===");
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        Console.WriteLine($"=== DEBUG: Password verification completed, valid: {isPasswordValid} ===");
        if (!isPasswordValid)
        {
            Console.WriteLine("=== DEBUG: Invalid password, returning null ===");
            return null;
        }

        // Update last login with optimized approach - TEMPORARILY DISABLED DUE TO DATABASE TIMEOUT ISSUES
        Console.WriteLine("=== DEBUG: SKIPPING last login update due to database timeout issues ===");
        /*
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
        */

        // Attach any season tickets bought under this email so wallet eligibility is up to date.
        await LinkSeasonTicketsByEmailAsync(user);

        var token = GenerateJwtToken(user);
        var userDto = MapToUserDto(user);

        return new LoginResponseDto
        {
            Token = token,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddHours(24) // 24-hour access token for better user experience
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
            AccessTokenExpiresAt = DateTime.UtcNow.AddHours(24),
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
        // A shell account may already exist for this email (auto-provisioned from a ticket). Registering
        // with that email *claims* it — sets the first password — rather than being rejected as "taken".
        var existingByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
        if (existingByEmail != null)
        {
            if (!existingByEmail.IsShellAccount)
                return null; // a real (already-claimed) account owns this email

            // Claim the shell: keep the requested username only if it's free for another account.
            var usernameTaken = await _context.Users
                .AnyAsync(u => u.Id != existingByEmail.Id && u.Username == registerDto.Username);
            if (!usernameTaken && !string.IsNullOrWhiteSpace(registerDto.Username))
                existingByEmail.Username = registerDto.Username;

            existingByEmail.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            if (!string.IsNullOrWhiteSpace(registerDto.FirstName)) existingByEmail.FirstName = registerDto.FirstName.Trim();
            if (!string.IsNullOrWhiteSpace(registerDto.LastName)) existingByEmail.LastName = registerDto.LastName.Trim();
            if (!string.IsNullOrWhiteSpace(registerDto.Oib)) existingByEmail.Oib = registerDto.Oib.Trim();
            existingByEmail.IsShellAccount = false;

            await ConsumeActivationTokensAsync(existingByEmail.Id);
            await _context.SaveChangesAsync();
            await LinkSeasonTicketsByEmailAsync(existingByEmail);
            return MapToUserDto(existingByEmail);
        }

        // No account owns this email, but the fan supplied an OIB (Croatian personal id, unique per person).
        // A shell may already exist for that OIB — provisioned from an OIB-bearing ticket under a different
        // (or placeholder) email. Registering *claims* that shell and adopts the real email the fan is
        // registering with, rather than creating a duplicate account for the same person. If a *real*
        // (already-claimed) account carries this OIB, the person already has an account under another email
        // and should log in instead — so we reject.
        if (!string.IsNullOrWhiteSpace(registerDto.Oib))
        {
            var oib = registerDto.Oib.Trim();
            var accountsByOib = await _context.Users.Where(u => u.Oib == oib).ToListAsync();
            if (accountsByOib.Count > 0)
            {
                var shell = accountsByOib.FirstOrDefault(u => u.IsShellAccount);
                if (shell == null)
                    return null; // a claimed account already exists for this OIB

                // The email is free (no email match above), so the shell can safely adopt it as its real,
                // claimable address — future email logins/claims and season-ticket linking now key on it.
                var usernameTaken = await _context.Users
                    .AnyAsync(u => u.Id != shell.Id && u.Username == registerDto.Username);
                if (!usernameTaken && !string.IsNullOrWhiteSpace(registerDto.Username))
                    shell.Username = registerDto.Username;

                shell.Email = registerDto.Email;
                shell.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                if (!string.IsNullOrWhiteSpace(registerDto.FirstName)) shell.FirstName = registerDto.FirstName.Trim();
                if (!string.IsNullOrWhiteSpace(registerDto.LastName)) shell.LastName = registerDto.LastName.Trim();
                shell.IsShellAccount = false;

                await ConsumeActivationTokensAsync(shell.Id);
                await _context.SaveChangesAsync();
                await LinkSeasonTicketsByEmailAsync(shell);
                return MapToUserDto(shell);
            }
        }

        // Brand-new account: the username must be free.
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            return null;

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            FirstName = string.IsNullOrWhiteSpace(registerDto.FirstName) ? null : registerDto.FirstName.Trim(),
            LastName = string.IsNullOrWhiteSpace(registerDto.LastName) ? null : registerDto.LastName.Trim(),
            Oib = string.IsNullOrWhiteSpace(registerDto.Oib) ? null : registerDto.Oib.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Attach any season tickets already sold under this email (e.g. externally ingested) so the
        // fan is immediately wallet-eligible.
        await LinkSeasonTicketsByEmailAsync(user);

        return MapToUserDto(user);
    }

    public async Task<ActivationInfoDto> GetActivationInfoAsync(string token)
    {
        var row = await _context.AccountActivationTokens.AsNoTracking()
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == token);

        if (row == null)
            return new ActivationInfoDto { Valid = false, Reason = "NotFound" };
        if (row.IsUsed)
            return new ActivationInfoDto { Valid = false, Reason = "AlreadyUsed" };
        if (row.ExpiresAt <= DateTime.UtcNow)
            return new ActivationInfoDto { Valid = false, Reason = "Expired" };
        if (!row.User.IsShellAccount)
            return new ActivationInfoDto { Valid = false, Reason = "AlreadyActive" };

        var name = $"{row.User.FirstName} {row.User.LastName}".Trim();
        return new ActivationInfoDto
        {
            Valid = true,
            Email = row.User.Email,
            FullName = string.IsNullOrWhiteSpace(name) ? null : name
        };
    }

    public async Task<LoginResponseDto?> ActivateAccountAsync(ActivateAccountDto dto)
    {
        var row = await _context.AccountActivationTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == dto.Token);

        // Reject anything not currently claimable (missing/expired/used, or already-activated account).
        if (row == null || !row.IsValid || !row.User.IsShellAccount)
            return null;

        var user = row.User;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.IsShellAccount = false;

        await ConsumeActivationTokensAsync(user.Id);
        await _context.SaveChangesAsync();
        await LinkSeasonTicketsByEmailAsync(user);

        // Log the fan straight in so they land in the app with their new password.
        return new LoginResponseDto
        {
            Token = GenerateJwtToken(user),
            User = MapToUserDto(user),
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
    }

    /// <summary>Marks all outstanding activation tokens for a user as used (single-use, and a claimed
    /// account should have no live tokens left).</summary>
    private async Task ConsumeActivationTokensAsync(int userId)
    {
        await _context.AccountActivationTokens
            .Where(t => t.UserId == userId && !t.IsUsed)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.IsUsed, true)
                .SetProperty(t => t.UsedAt, DateTime.UtcNow));
    }

    /// <summary>
    /// Links unclaimed season tickets to a fan by email (<c>HolderEmail == User.Email</c>, case-insensitive).
    /// Runs on register and login so passes bought or ingested at any time attach on next contact. Only
    /// fills empty links — never reassigns a pass already owned by another account. A single set-based
    /// UPDATE, so it is cheap and never fails the surrounding auth flow.
    /// </summary>
    private async Task LinkSeasonTicketsByEmailAsync(User user)
    {
        try
        {
            var email = user.Email.ToLower();
            await _context.SeasonTickets
                .Where(st => st.UserId == null
                             && st.HolderEmail != null
                             && st.HolderEmail.ToLower() == email)
                .ExecuteUpdateAsync(s => s.SetProperty(st => st.UserId, user.Id));
        }
        catch
        {
            // Linking is best-effort; a failure here must not block login/registration.
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user != null ? MapToUserDto(user) : null;
    }

    public async Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return null;

        user.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? null : dto.FirstName.Trim();
        user.LastName = string.IsNullOrWhiteSpace(dto.LastName) ? null : dto.LastName.Trim();
        user.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber) ? null : dto.PhoneNumber.Trim();
        user.Oib = string.IsNullOrWhiteSpace(dto.Oib) ? null : dto.Oib.Trim();

        await _context.SaveChangesAsync();
        return MapToUserDto(user);
    }

    public async Task<StaffMemberStatsDto?> GetStaffMemberStatsAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return null;

        // Every order this member touched at any workflow stage.
        var handledOrders = _context.Orders.Where(o =>
            o.AcceptedByUserId == userId ||
            o.InPreparationByUserId == userId ||
            o.PreparedByUserId == userId ||
            o.DeliveredByUserId == userId ||
            o.AssignedStaffId == userId);

        var stats = new StaffMemberStatsDto
        {
            UserId = user.Id,
            FullName = MapToUserDto(user).FullName,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,

            OrdersAccepted = await _context.Orders.CountAsync(o => o.AcceptedByUserId == userId),
            OrdersInPreparation = await _context.Orders.CountAsync(o => o.InPreparationByUserId == userId),
            OrdersPrepared = await _context.Orders.CountAsync(o => o.PreparedByUserId == userId),
            OrdersDelivered = await _context.Orders.CountAsync(o => o.DeliveredByUserId == userId),

            TotalOrdersHandled = await handledOrders.CountAsync(),
            RevenueDelivered = await _context.Orders
                .Where(o => o.DeliveredByUserId == userId)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m,

            EventsAssigned = await _context.EventStaffAssignments.CountAsync(a => a.StaffId == userId),
            UpcomingEventsAssigned = await _context.EventStaffAssignments
                .CountAsync(a => a.StaffId == userId && a.Event.EventDate >= DateTime.UtcNow),
        };

        // Latest workflow touch across the stamped timestamps.
        stats.LastOrderHandledAt = await handledOrders
            .Select(o => o.DeliveredAt ?? o.PreparedAt ?? o.InPreparationAt ?? o.AcceptedAt)
            .Where(d => d != null)
            .OrderByDescending(d => d)
            .FirstOrDefaultAsync();

        return stats;
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
            // Add custom claims for Supabase RLS compatibility
            new Claim("user_id", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("role", user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Role-based token lifetime. Waiters use the roaming Runner PWA and re-login per shift; they
        // get a shorter (default 4h) token to limit the exposure of a long-lived bearer token on a
        // mobile device. Refresh cannot happen offline anyway, so a fixed lifetime + re-login is the
        // model (see docs/staff-app-split-plan.md). Fixed-screen Bar/Admin sessions keep the longer
        // default. Both are tunable via JwtSettings without a code change.
        var defaultTokenHours = double.TryParse(jwtSettings["AccessTokenHours"], out var dh) ? dh : 24;
        var waiterTokenHours = double.TryParse(jwtSettings["WaiterAccessTokenHours"], out var wh) ? wh : 4;
        var tokenHours = user.Role == UserRole.Waiter ? waiterTokenHours : defaultTokenHours;

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(tokenHours),
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

        if (filter.ExcludeRole.HasValue)
        {
            query = query.Where(u => u.Role != filter.ExcludeRole.Value);
        }

        if (filter.CreatedAfter.HasValue)
        {
            query = query.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);
        }

        if (filter.CreatedBefore.HasValue)
        {
            query = query.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);
        }

        // Event/season scoping: a customer "belongs" to an event when they placed a drink order at it
        // or bought a ticket for it (matched by email); to a season when any of its events qualifies or
        // they hold a linked season pass. Event is the more specific filter and wins when both are set.
        if (filter.EventId.HasValue)
        {
            var eventId = filter.EventId.Value;
            query = query.Where(u =>
                _context.Orders.Any(o => o.CustomerId == u.Id && o.EventId == eventId) ||
                _context.Tickets.Any(t => t.EventId == eventId
                                          && t.CustomerEmail != null
                                          && t.CustomerEmail.ToLower() == u.Email.ToLower()));
        }
        else if (filter.SeasonId.HasValue)
        {
            var seasonId = filter.SeasonId.Value;
            query = query.Where(u =>
                _context.Orders.Any(o => o.CustomerId == u.Id && o.Event!.SeasonId == seasonId) ||
                _context.Tickets.Any(t => t.Event.SeasonId == seasonId
                                          && t.CustomerEmail != null
                                          && t.CustomerEmail.ToLower() == u.Email.ToLower()) ||
                _context.SeasonTickets.Any(st => st.UserId == u.Id && st.SeasonId == seasonId));
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
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            Role = createUserDto.Role,
            IsActive = createUserDto.IsActive,
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

        user.Username = updateUserDto.Username ?? user.Username;
        user.Email = updateUserDto.Email ?? user.Email;
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        if (updateUserDto.Role.HasValue)
        {
            user.Role = updateUserDto.Role.Value;
        }
        if (updateUserDto.IsActive.HasValue)
        {
            user.IsActive = updateUserDto.IsActive.Value;
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
        var fullName = $"{user.FirstName} {user.LastName}".Trim();
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Name = string.IsNullOrWhiteSpace(fullName) ? null : fullName,
            PhoneNumber = user.PhoneNumber,
            Oib = user.Oib,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
