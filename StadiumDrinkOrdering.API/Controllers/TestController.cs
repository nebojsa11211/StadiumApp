using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly ApplicationDbContext _context;

    public TestController(ILogger<TestController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("demo-test")]
    public IActionResult DemoTest()
    {
        return Ok(new { message = "TestController is working perfectly", timestamp = DateTime.UtcNow });
    }

    [HttpPost("demo-generate")]
    public IActionResult DemoGenerate()
    {
        _logger.LogInformation("Demo generation called from TestController");
        return Ok(new { message = "Demo generation placeholder from TestController", timestamp = DateTime.UtcNow });
    }

    [HttpPost("create-test-tickets")]
    public async Task<IActionResult> CreateTestTickets()
    {
        try
        {
            _logger.LogInformation("Creating test tickets");

            // Create some test events first
            var events = new List<Event>();
            for (int i = 1; i <= 3; i++)
            {
                var evt = new Event
                {
                    EventName = $"Test Event {i}",
                    EventType = "Football",
                    EventDate = DateTime.Now.AddDays(30 + i),
                    TotalSeats = 50000,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Description = $"Test event {i} for demo purposes",
                    BaseTicketPrice = 45.00m + (i * 10)
                };
                events.Add(evt);
                _context.Events.Add(evt);
            }

            await _context.SaveChangesAsync();

            // Create test tickets
            var tickets = new List<Ticket>();
            var customers = new[] { "John Smith", "Jane Doe", "Mike Johnson", "Sarah Wilson", "Tom Brown" };
            var emails = new[] { "john@test.com", "jane@test.com", "mike@test.com", "sarah@test.com", "tom@test.com" };
            var sections = new[] { "A", "B", "C", "D" };

            int ticketCounter = 1;
            foreach (var evt in events)
            {
                for (int t = 0; t < 10; t++) // 10 tickets per event
                {
                    var customerIndex = t % customers.Length;
                    var ticket = new Ticket
                    {
                        TicketNumber = $"TICKET{ticketCounter:D3}",
                        EventId = evt.Id,
                        EventName = evt.EventName,
                        EventDate = evt.EventDate,
                        Section = sections[t % sections.Length],
                        Row = $"Row {(t / 4) + 1}",
                        SeatNumber = $"{(t % 4) + 1}",
                        IsActive = true,
                        CustomerName = customers[customerIndex],
                        CustomerEmail = emails[customerIndex],
                        PurchaseDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 15)),
                        Price = evt.BaseTicketPrice ?? 50.00m
                    };
                    tickets.Add(ticket);
                    _context.Tickets.Add(ticket);
                    ticketCounter++;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { 
                message = $"Successfully created {tickets.Count} test tickets across {events.Count} events", 
                eventsCreated = events.Count,
                ticketsCreated = tickets.Count,
                timestamp = DateTime.UtcNow 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test tickets");
            return StatusCode(500, new { message = "Error creating test tickets", error = ex.Message });
        }
    }
}