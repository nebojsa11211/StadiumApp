using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StadiumDrinkOrdering.API.Controllers;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Tests.Controllers;

public class ShoppingCartControllerTests : IDisposable
{
    private readonly Mock<IShoppingCartService> _mockCartService;
    private readonly Mock<ILogger<ShoppingCartController>> _mockLogger;
    private readonly ShoppingCartController _controller;
    private readonly ApplicationDbContext _context;

    public ShoppingCartControllerTests()
    {
        _mockCartService = new Mock<IShoppingCartService>();
        _mockLogger = new Mock<ILogger<ShoppingCartController>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        _controller = new ShoppingCartController(_mockCartService.Object, _context, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCart_WithValidSessionId_ReturnsCart()
    {
        // Arrange
        var sessionId = "test-session-123";
        var cart = new ShoppingCart
        {
            SessionId = sessionId,
            Items = new List<StadiumDrinkOrdering.Shared.Models.CartItem>
            {
                new StadiumDrinkOrdering.Shared.Models.CartItem 
                { 
                    Id = 1, 
                    EventId = 1, 
                    SeatNumber = 1, 
                    SeatCode = "A1", 
                    SectorId = 1,
                    RowNumber = 1,
                    Price = 50.00m 
                }
            }
        };

        _mockCartService.Setup(s => s.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        // Add test event to context
        var testEvent = new Event { Id = 1, EventName = "Test Event", EventType = "Test", EventDate = DateTime.Now.AddDays(7), TotalSeats = 1000 };
        _context.Events.Add(testEvent);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCart(sessionId);

        // Assert
        Assert.IsType<ActionResult<StadiumDrinkOrdering.Shared.DTOs.ShoppingCartDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var cartDto = Assert.IsType<StadiumDrinkOrdering.Shared.DTOs.ShoppingCartDto>(okResult.Value);
        Assert.Equal(sessionId, cartDto.SessionId);
        Assert.Single(cartDto.Items);
    }

    [Fact]
    public async Task GetCart_WithEmptySessionId_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetCart(string.Empty);

        // Assert
        Assert.IsType<ActionResult<StadiumDrinkOrdering.Shared.DTOs.ShoppingCartDto>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Session ID is required", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCart_WithNullCart_ReturnsEmptyCart()
    {
        // Arrange
        var sessionId = "empty-session";
        _mockCartService.Setup(s => s.GetCartAsync(sessionId))
            .ReturnsAsync((ShoppingCart?)null);

        // Act
        var result = await _controller.GetCart(sessionId);

        // Assert
        Assert.IsType<ActionResult<StadiumDrinkOrdering.Shared.DTOs.ShoppingCartDto>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var cartDto = Assert.IsType<StadiumDrinkOrdering.Shared.DTOs.ShoppingCartDto>(okResult.Value);
        Assert.Equal(sessionId, cartDto.SessionId);
        Assert.Empty(cartDto.Items);
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}