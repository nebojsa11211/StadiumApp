using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StadiumDrinkOrdering.API.Controllers;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Tests.Controllers;

public class EventControllerTests
{
    private readonly Mock<IEventService> _mockEventService;
    private readonly Mock<ILogger<EventController>> _mockLogger;
    private readonly EventController _controller;

    public EventControllerTests()
    {
        _mockEventService = new Mock<IEventService>();
        _mockLogger = new Mock<ILogger<EventController>>();
        _controller = new EventController(_mockEventService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetEvents_ReturnsAllEvents_WhenActiveOnlyIsFalse()
    {
        // Arrange
        var events = new List<Event>
        {
            new Event { Id = 1, EventName = "Test Event 1", IsActive = true, EventDate = DateTime.Now.AddDays(7), EventType = "Football", TotalSeats = 50000 },
            new Event { Id = 2, EventName = "Test Event 2", IsActive = false, EventDate = DateTime.Now.AddDays(14), EventType = "Concert", TotalSeats = 30000 }
        };
        _mockEventService.Setup(s => s.GetEventsAsync(false))
            .ReturnsAsync(events);

        // Act
        var result = await _controller.GetEvents(false);

        // Assert
        Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);
        Assert.Equal(2, returnedEvents.Count());
    }

    [Fact]
    public async Task GetEvents_ReturnsActiveEventsOnly_WhenActiveOnlyIsTrue()
    {
        // Arrange
        var activeEvents = new List<Event>
        {
            new Event { Id = 1, EventName = "Active Event 1", IsActive = true, EventDate = DateTime.Now.AddDays(7), EventType = "Football", TotalSeats = 50000 },
            new Event { Id = 3, EventName = "Active Event 2", IsActive = true, EventDate = DateTime.Now.AddDays(21), EventType = "Basketball", TotalSeats = 20000 }
        };
        _mockEventService.Setup(s => s.GetEventsAsync(true))
            .ReturnsAsync(activeEvents);

        // Act
        var result = await _controller.GetEvents(true);

        // Assert
        Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);
        Assert.Equal(2, returnedEvents.Count());
        Assert.All(returnedEvents, e => Assert.True(e.IsActive));
    }

    [Fact]
    public async Task GetActiveEvents_ReturnsOnlyActiveEvents()
    {
        // Arrange
        var activeEvents = new List<Event>
        {
            new Event { Id = 1, EventName = "Active Event", IsActive = true, EventDate = DateTime.Now.AddDays(7), EventType = "Football", TotalSeats = 50000 }
        };
        _mockEventService.Setup(s => s.GetActiveEventsAsync())
            .ReturnsAsync(activeEvents);

        // Act
        var result = await _controller.GetActiveEvents();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);
        Assert.Single(returnedEvents);
        Assert.True(returnedEvents.First().IsActive);
    }

    [Fact]
    public async Task GetEvents_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _mockEventService.Setup(s => s.GetEventsAsync(It.IsAny<bool>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetEvents();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Equal("Internal server error", statusResult.Value);
    }
}