using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;

namespace StadiumDrinkOrdering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestDataController> _logger;

        public TestDataController(ApplicationDbContext context, ILogger<TestDataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("create-test-orders")]
        public async Task<IActionResult> CreateTestOrders()
        {
            try
            {
                _logger.LogInformation("Creating test orders...");

                // Create a test event first
                var testEvent = new Event
                {
                    Id = 9999,
                    EventName = "Test Event",
                    Description = "Test Event for Orders",
                    EventDate = DateTime.UtcNow.AddDays(30),
                    EventType = "Test",
                    TotalSeats = 1000,
                    BaseTicketPrice = 50.00m,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == 9999);
                if (existingEvent == null)
                {
                    _context.Events.Add(testEvent);
                    await _context.SaveChangesAsync();
                }

                // Create test customers
                var testCustomers = new List<User>();
                for (int i = 1; i <= 3; i++)
                {
                    var email = $"testcustomer{i}@test.com";
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                    if (existingUser == null)
                    {
                        var customer = new User
                        {
                            Username = $"testcustomer{i}",
                            Email = email,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!"),
                            Role = UserRole.Customer,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Users.Add(customer);
                        testCustomers.Add(customer);
                    }
                    else
                    {
                        testCustomers.Add(existingUser);
                    }
                }
                await _context.SaveChangesAsync();

                // Create test orders
                var testOrders = new List<Order>();
                for (int i = 1; i <= 5; i++)
                {
                    var order = new Order
                    {
                        Id = 8000 + i,
                        CustomerId = testCustomers[i % testCustomers.Count].Id,
                        EventId = 9999,
                        TotalAmount = 50.00m * i,
                        Status = OrderStatus.Delivered,
                        TicketNumber = $"TEST-{8000 + i}",
                        SeatNumber = $"T{i}-1",
                        CreatedAt = DateTime.UtcNow.AddDays(-i)
                    };
                    testOrders.Add(order);
                }

                _context.Orders.AddRange(testOrders);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created {testOrders.Count} test orders successfully!");

                return Ok(new { 
                    message = $"Created {testOrders.Count} test orders successfully",
                    orders = testOrders.Select(o => new { o.Id, o.TotalAmount, o.Status, o.TicketNumber })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test orders");
                return StatusCode(500, new { message = "Failed to create test orders", error = ex.Message });
            }
        }
    }
}