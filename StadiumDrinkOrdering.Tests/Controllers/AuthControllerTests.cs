using Microsoft.AspNetCore.Mvc;
using Moq;
using StadiumDrinkOrdering.API.Controllers;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "admin@stadium.com",
            Password = "admin123"
        };
        var expectedResponse = new LoginResponseDto
        {
            Token = "mock-jwt-token",
            User = new UserDto { Id = 1, Email = "admin@stadium.com", Role = UserRole.Admin }
        };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        Assert.IsType<ActionResult<LoginResponseDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal(expectedResponse.Token, response.Token);
        Assert.Equal(expectedResponse.User.Email, response.User.Email);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "invalid@email.com",
            Password = "wrongpassword"
        };
        _mockAuthService.Setup(s => s.LoginAsync(loginDto))
            .ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        Assert.IsType<ActionResult<LoginResponseDto>>(result);
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result.Result);
        Assert.Equal("Invalid email or password", unauthorizedResult.Value);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "newuser@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Username = "newuser"
        };
        var expectedUser = new UserDto 
        { 
            Id = 2, 
            Email = "newuser@stadium.com", 
            Username = "newuser",
            Role = UserRole.Customer
        };
        _mockAuthService.Setup(s => s.RegisterAsync(registerDto))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsType<ActionResult<UserDto>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var user = Assert.IsType<UserDto>(createdResult.Value);
        Assert.Equal(expectedUser.Email, user.Email);
        Assert.Equal(expectedUser.Username, user.Username);
    }

    [Fact]
    public async Task Register_WithExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "existing@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123",
            Username = "existing"
        };
        _mockAuthService.Setup(s => s.RegisterAsync(registerDto))
            .ReturnsAsync((UserDto?)null);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsType<ActionResult<UserDto>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("User with this email or username already exists", badRequestResult.Value);
    }
}