using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;

namespace StadiumDrinkOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuickOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<QuickOrderController> _logger;

        public QuickOrderController(ApplicationDbContext context, ILogger<QuickOrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("create-simple-orders")]
        public async Task<IActionResult> CreateSimpleOrders()
        {
            try
        {
                _logger.LogInformation("Starting simple order creation...");

                // Use raw SQL to avoid EF complexity
                var eventSql = @"
                    INSERT INTO ""Events"" (""Id"", ""EventName"", ""Description"", ""EventDate"", ""EventType"", ""TotalSeats"", ""BaseTicketPrice"", ""IsActive"", ""CreatedAt"")
                    VALUES (9999, 'Quick Test Event', 'Simple test event for orders', @eventDate, 'Test', 100, 25.00, true, @createdAt)
                    ON CONFLICT (""Id"") DO NOTHING;
                ";

                var userSql = @"
                    INSERT INTO ""Users"" (""Username"", ""Email"", ""PasswordHash"", ""Role"", ""CreatedAt"")
                    VALUES ('quicktest', 'quicktest@test.com', @passwordHash, @role, @createdAt)
                    ON CONFLICT (""Email"") DO NOTHING;
                ";

                var orderSql = @"
                    INSERT INTO ""Orders"" (""Id"", ""CustomerId"", ""EventId"", ""TotalAmount"", ""Status"", ""TicketNumber"", ""SeatNumber"", ""CreatedAt"")
                    SELECT 9001, u.""Id"", 9999, 75.00, @status, 'QUICK-9001', 'A1-1', @createdAt
                    FROM ""Users"" u WHERE u.""Email"" = 'quicktest@test.com'
                    ON CONFLICT (""Id"") DO NOTHING;
                ";

                // Execute raw SQL commands
                await _context.Database.ExecuteSqlRawAsync(eventSql, 
                    new Npgsql.NpgsqlParameter("@eventDate", DateTime.UtcNow.AddDays(7)),
                    new Npgsql.NpgsqlParameter("@createdAt", DateTime.UtcNow));

                await _context.Database.ExecuteSqlRawAsync(userSql,
                    new Npgsql.NpgsqlParameter("@passwordHash", BCrypt.Net.BCrypt.HashPassword("TestPass123!")),
                    new Npgsql.NpgsqlParameter("@role", (int)UserRole.Customer),
                    new Npgsql.NpgsqlParameter("@createdAt", DateTime.UtcNow));

                await _context.Database.ExecuteSqlRawAsync(orderSql,
                    new Npgsql.NpgsqlParameter("@status", (int)OrderStatus.Delivered),
                    new Npgsql.NpgsqlParameter("@createdAt", DateTime.UtcNow));

                _logger.LogInformation("Simple orders created successfully!");

                return Ok(new { 
                    message = "Simple orders created successfully using raw SQL",
                    orders = new[] { new { Id = 9001, Amount = 75.00, Status = "Delivered", Ticket = "QUICK-9001" }}
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating simple orders");
                return StatusCode(500, new { message = "Failed to create simple orders", error = ex.Message });
            }
        }
    }
}