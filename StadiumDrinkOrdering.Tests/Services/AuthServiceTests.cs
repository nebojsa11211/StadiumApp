using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Tests.Services;

public class AuthServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        var configDict = new Dictionary<string, string?>
        {
            ["JwtSettings:SecretKey"] = "ThisIsAVerySecretKeyForJWTTokenGenerationWithAtLeast32Characters",
            ["JwtSettings:Issuer"] = "StadiumApp",
            ["JwtSettings:Audience"] = "StadiumAppUsers"
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        _authService = new AuthService(_context, _configuration);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_CreatesNewUser()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "newuser@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Username = "newuser"
        };

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(registerDto.Email, result.Email);
        Assert.Equal(registerDto.Username, result.Username);
        Assert.Equal(UserRole.Customer, result.Role);

        // Verify user was saved to database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
        Assert.NotNull(user);
        Assert.True(BCrypt.Net.BCrypt.Verify(registerDto.Password, user.PasswordHash));
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ReturnsNull()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "existing@stadium.com",
            Username = "existing",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var registerDto = new RegisterDto
        {
            Email = "existing@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Username = "newuser"
        };

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var password = "password123";
        var user = new User
        {
            Email = "user@stadium.com",
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = user.Email,
            Password = password
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.Equal(user.Email, result.User.Email);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);

        // Verify last login was updated
        var updatedUser = await _context.Users.FirstAsync(u => u.Id == user.Id);
        Assert.NotNull(updatedUser.LastLoginAt);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Email = "user@stadium.com",
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Email = user.Email,
            Password = "wrongpassword"
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Email = "user@stadium.com",
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _authService.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(UserRole.Admin, result.Role);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _authService.GetUserByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}