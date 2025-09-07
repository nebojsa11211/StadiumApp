using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IDemoDataService
{
    Task<bool> GenerateComprehensiveDemoDataAsync();
    Task<bool> GenerateEventWithTicketsAsync(string eventName, DateTime eventDate, int ticketCount = 50);
    Task<bool> GenerateTestOrdersAsync(int orderCount = 20);
    Task<bool> GenerateStaffAssignmentsAsync();
    Task<bool> ClearDemoDataAsync();
    Task<bool> GenerateDemoDataForEventAsync(int eventId);
}

public class DemoDataService : IDemoDataService
{
    private readonly ApplicationDbContext _context;
    private readonly IQRCodeService _qrCodeService;
    private readonly ILogger<DemoDataService> _logger;

    public DemoDataService(ApplicationDbContext context, IQRCodeService qrCodeService, ILogger<DemoDataService> logger)
    {
        _context = context;
        _qrCodeService = qrCodeService;
        _logger = logger;
    }

    public async Task<bool> GenerateComprehensiveDemoDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting comprehensive demo data generation...");

            // Generate staff users
            await GenerateStaffUsersAsync();
            
            // Generate multiple events
            await GenerateEventsAsync();
            
            // Skip additional seat generation - stadium structure already preserved
            _logger.LogInformation("Skipping seat generation - stadium structure already preserved");
            
            // Generate tickets with QR codes for all events
            await GenerateTicketsForAllEventsAsync();
            
            // Generate staff assignments
            await GenerateStaffAssignmentsAsync();
            
            // Generate test orders
            await GenerateTestOrdersAsync(30);
            
            // Generate order sessions
            await GenerateOrderSessionsAsync();
            
            // Generate notifications
            await GenerateNotificationsAsync();
            
            // Generate analytics data
            await GenerateAnalyticsDataAsync();

            _logger.LogInformation("Comprehensive demo data generation completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating comprehensive demo data");
            return false;
        }
    }

    private async Task GenerateStaffUsersAsync()
    {
        var staffUsers = new[]
        {
            // Staff Users
            new User { Id = 10, Username = "alex.bartender", Email = "alex.martinez@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Bartender, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 11, Username = "sarah.bartender", Email = "sarah.johnson@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Bartender, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 12, Username = "mike.waiter", Email = "mike.wilson@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 13, Username = "emma.waiter", Email = "emma.davis@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 14, Username = "david.waiter", Email = "david.brown@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 15, Username = "lisa.supervisor", Email = "lisa.supervisor@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Admin, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(60, 365)) },
            
            // Customer Users
            new User { Id = 20, Username = "john.customer", Email = "john.smith@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 7)) },
            new User { Id = 21, Username = "mary.johnson", Email = "mary.johnson@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 14)) },
            new User { Id = 22, Username = "robert.williams", Email = "robert.williams@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)) },
            new User { Id = 23, Username = "jennifer.garcia", Email = "jennifer.garcia@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 5)) },
            new User { Id = 24, Username = "michael.davis", Email = "michael.davis@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 21)) },
            new User { Id = 25, Username = "jessica.martinez", Email = "jessica.martinez@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 3)) },
            new User { Id = 26, Username = "william.anderson", Email = "william.anderson@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 10)) },
            new User { Id = 27, Username = "ashley.taylor", Email = "ashley.taylor@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 15)) },
            new User { Id = 28, Username = "james.thomas", Email = "james.thomas@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 8)) },
            new User { Id = 29, Username = "amanda.rodriguez", Email = "amanda.rodriguez@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 12)) }
        };

        foreach (var user in staffUsers)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                _context.Users.Add(user);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Generated {Count} users (staff + customers)", staffUsers.Length);
    }

    private async Task GenerateEventsAsync()
    {
        var events = new[]
        {
            // Upcoming Events
            new Event { Id = 10, EventName = "Premier League Derby Match", EventType = "Football", EventDate = DateTime.Today.AddDays(3).AddHours(15), TotalSeats = 350, BaseTicketPrice = 85.00m, Description = "Intense local derby match between city rivals", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new Event { Id = 11, EventName = "Champions League Semi-Final", EventType = "Football", EventDate = DateTime.Today.AddDays(8).AddHours(20), TotalSeats = 400, BaseTicketPrice = 120.00m, Description = "European Champions League semi-final showdown", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-21) },
            new Event { Id = 12, EventName = "Rock Legends World Tour", EventType = "Concert", EventDate = DateTime.Today.AddDays(12).AddHours(19), TotalSeats = 280, BaseTicketPrice = 95.00m, Description = "World-famous rock band performing greatest hits", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-28) },
            new Event { Id = 13, EventName = "NBA Basketball Exhibition", EventType = "Basketball", EventDate = DateTime.Today.AddDays(18).AddHours(19), TotalSeats = 250, BaseTicketPrice = 75.00m, Description = "Professional basketball exhibition match", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Event { Id = 14, EventName = "International Rugby Final", EventType = "Rugby", EventDate = DateTime.Today.AddDays(25).AddHours(16), TotalSeats = 320, BaseTicketPrice = 90.00m, Description = "International rugby championship final", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-35) },
            
            // Recent Past Events (for analytics data)
            new Event { Id = 15, EventName = "Summer Festival Concert", EventType = "Concert", EventDate = DateTime.Today.AddDays(-5).AddHours(18), TotalSeats = 300, BaseTicketPrice = 65.00m, Description = "Summer music festival main event", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-42) },
            new Event { Id = 16, EventName = "Cup Quarter-Final", EventType = "Football", EventDate = DateTime.Today.AddDays(-12).AddHours(15), TotalSeats = 280, BaseTicketPrice = 55.00m, Description = "Exciting cup quarter-final match", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-49) },
            new Event { Id = 17, EventName = "Jazz Night Special", EventType = "Concert", EventDate = DateTime.Today.AddDays(-8).AddHours(20), TotalSeats = 200, BaseTicketPrice = 45.00m, Description = "Special jazz performance evening", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-38) },
            
            // Far Future Events  
            new Event { Id = 18, EventName = "Season Finale Championship", EventType = "Football", EventDate = DateTime.Today.AddDays(45).AddHours(17), TotalSeats = 400, BaseTicketPrice = 110.00m, Description = "End of season championship match", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-7) },
            new Event { Id = 19, EventName = "Pop Stars United Concert", EventType = "Concert", EventDate = DateTime.Today.AddDays(60).AddHours(19), TotalSeats = 350, BaseTicketPrice = 80.00m, Description = "Multiple pop stars collaboration concert", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) }
        };

        foreach (var evt in events)
        {
            var existingEvent = await _context.Events.FindAsync(evt.Id);
            if (existingEvent == null)
            {
                _context.Events.Add(evt);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Generated {Count} events with realistic dates", events.Length);
    }

    private async Task GenerateAdditionalSeatsAsync()
    {
        // Skip seat generation since stadium structure is already preserved
        var existingSeatCount = await _context.Seats.CountAsync();
        _logger.LogInformation("Stadium already has {SeatCount} seats, skipping additional seat generation", existingSeatCount);
        return;
    }

    private async Task GenerateTicketsForAllEventsAsync()
    {
        var events = await _context.Events.ToListAsync();
        var seats = await _context.Seats.ToListAsync();
        var maxTicketId = await _context.Tickets.MaxAsync(t => (int?)t.Id) ?? 5;

        foreach (var evt in events)
        {
            var ticketsToGenerate = Math.Min(20, seats.Count); // Generate up to 20 tickets per event
            var selectedSeats = seats.Take(ticketsToGenerate).ToList();

            foreach (var seat in selectedSeats)
            {
                maxTicketId++;
                var ticket = new Ticket
                {
                    Id = maxTicketId,
                    TicketNumber = $"TK{evt.Id:D2}{maxTicketId:D3}",
                    EventId = evt.Id,
                    SeatId = seat.Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = evt.BaseTicketPrice ?? 50.00m,
                    PurchaseDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                    CustomerName = $"Customer {maxTicketId}",
                    CustomerEmail = $"customer{maxTicketId}@example.com",
                    Status = "Active",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    // Legacy fields for compatibility
                    SeatNumber = seat.SeatCode,
                    Section = seat.Section?.SectionCode ?? "A",
                    Row = seat.RowNumber.ToString(),
                    EventName = evt.EventName,
                    EventDate = evt.EventDate
                };

                _context.Tickets.Add(ticket);
            }
        }

        await _context.SaveChangesAsync();

        // Generate QR codes for all tickets
        var tickets = await _context.Tickets.Include(t => t.Event).ToListAsync();
        foreach (var ticket in tickets.Where(t => string.IsNullOrEmpty(t.QRCode)))
        {
            try
            {
                await _qrCodeService.GenerateQRCodeAsync(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to generate QR code for ticket {TicketId}", ticket.Id);
            }
        }
    }

    public async Task<bool> GenerateStaffAssignmentsAsync()
    {
        try
        {
            var events = await _context.Events.ToListAsync();
            var staff = await _context.Users.Where(u => u.Role == UserRole.Bartender || u.Role == UserRole.Waiter || u.Role == UserRole.Admin).ToListAsync();

            foreach (var evt in events)
            {
                foreach (var staffMember in staff.Take(3)) // Assign first 3 staff to each event
                {
                    var assignment = new EventStaffAssignment
                    {
                        EventId = evt.Id,
                        StaffId = staffMember.Id,
                        Role = staffMember.Username?.Contains("bartender") == true ? "Bartender" : 
                               staffMember.Username?.Contains("supervisor") == true ? "Supervisor" : "Waiter",
                        ShiftStart = evt.EventDate.AddHours(-2),
                        ShiftEnd = evt.EventDate.AddHours(4),
                        AssignedSections = "[\"A\", \"B\"]",
                        IsActive = true
                    };

                    var existingAssignment = await _context.EventStaffAssignments
                        .FirstOrDefaultAsync(esa => esa.EventId == evt.Id && esa.StaffId == staffMember.Id);

                    if (existingAssignment == null)
                    {
                        _context.EventStaffAssignments.Add(assignment);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating staff assignments");
            return false;
        }
    }

    public async Task<bool> GenerateTestOrdersAsync(int orderCount = 20)
    {
        try
        {
            var customers = await _context.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
            var drinks = await _context.Drinks.Where(d => d.IsAvailable).ToListAsync();
            var tickets = await _context.Tickets.Take(10).ToListAsync();

            for (int i = 0; i < orderCount; i++)
            {
                var customer = customers[Random.Shared.Next(customers.Count)];
                var ticket = tickets[Random.Shared.Next(tickets.Count)];
                
                var order = new Order
                {
                    TicketNumber = ticket.TicketNumber,
                    SeatNumber = ticket.SeatNumber ?? "A1",
                    CustomerId = customer.Id,
                    TotalAmount = 0, // Will be calculated
                    Status = (OrderStatus)Random.Shared.Next(1, 8),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(1, 120)),
                    EventId = ticket.EventId,
                    SeatId = ticket.SeatId
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add order items
                var itemCount = Random.Shared.Next(1, 4);
                decimal totalAmount = 0;

                for (int j = 0; j < itemCount; j++)
                {
                    var drink = drinks[Random.Shared.Next(drinks.Count)];
                    var quantity = Random.Shared.Next(1, 3);
                    var totalPrice = drink.Price * quantity;
                    totalAmount += totalPrice;

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        DrinkId = drink.Id,
                        Quantity = quantity,
                        UnitPrice = drink.Price,
                        TotalPrice = totalPrice
                    };

                    _context.OrderItems.Add(orderItem);
                }

                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating test orders");
            return false;
        }
    }

    private async Task GenerateOrderSessionsAsync()
    {
        var tickets = await _context.Tickets.Take(5).ToListAsync();

        foreach (var ticket in tickets)
        {
            var session = new OrderSession
            {
                TicketId = ticket.Id,
                SessionToken = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(10, 60)),
                ExpiresAt = DateTime.UtcNow.AddHours(Random.Shared.Next(1, 3)),
                IsActive = Random.Shared.NextDouble() > 0.3, // 70% active
                IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}",
                UserAgent = "Mozilla/5.0 (Mobile)",
                CartData = "[]",
                CartTotal = 0,
                ItemCount = 0,
                LastActivity = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(1, 30))
            };

            _context.OrderSessions.Add(session);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GenerateNotificationsAsync()
    {
        var users = await _context.Users.ToListAsync();
        var events = await _context.Events.ToListAsync();

        var notifications = new[]
        {
            new Notification { UserId = null, Type = "SystemUpdate", Title = "System Maintenance", Message = "System will be updated tonight at 2 AM", Priority = "High", CreatedAt = DateTime.UtcNow.AddHours(-2) },
            new Notification { UserId = users.First().Id, Type = "OrderReady", Title = "Order Ready", Message = "Your order #123 is ready for pickup", Priority = "Normal", CreatedAt = DateTime.UtcNow.AddMinutes(-30) },
            new Notification { UserId = null, Type = "EventAlert", Title = "New Event", Message = "New concert added this weekend!", Priority = "Normal", EventId = events.First().Id, CreatedAt = DateTime.UtcNow.AddHours(-1) }
        };

        foreach (var notification in notifications)
        {
            _context.Notifications.Add(notification);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GenerateAnalyticsDataAsync()
    {
        _logger.LogInformation("Generating analytics data...");
        
        var events = await _context.Events.ToListAsync();

        foreach (var evt in events)
        {
            // Check if analytics already exists
            var existingAnalytics = await _context.EventAnalytics
                .FirstOrDefaultAsync(a => a.EventId == evt.Id);
                
            if (existingAnalytics != null)
                continue;

            var analytics = new EventAnalytics
            {
                EventId = evt.Id,
                TotalTicketsSold = Random.Shared.Next(50, Math.Min(300, evt.TotalSeats / 10)),
                TotalOrders = Random.Shared.Next(20, 100),
                TotalDrinksSold = Random.Shared.Next(100, 500),
                TicketRevenue = Random.Shared.Next(3000, 20000),
                DrinksRevenue = Random.Shared.Next(800, 4000),
                AverageOrderValue = Random.Shared.Next(25, 150),
                PeakOrderTime = evt.EventDate.AddHours(-2).AddMinutes(Random.Shared.Next(0, 120)),
                MostPopularDrink = new[] { "Beer", "Soft Drink", "Water", "Coffee" }[Random.Shared.Next(4)],
                CalculatedAt = DateTime.UtcNow
            };
            
            // Calculate total revenue from individual components
            analytics.TotalRevenue = analytics.TicketRevenue + analytics.DrinksRevenue;

            _context.EventAnalytics.Add(analytics);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Analytics data generated for {Count} events", events.Count);
    }

    public async Task<bool> GenerateEventWithTicketsAsync(string eventName, DateTime eventDate, int ticketCount = 50)
    {
        try
        {
            var evt = new Event
            {
                EventName = eventName,
                EventType = "Custom",
                EventDate = eventDate,
                TotalSeats = ticketCount,
                BaseTicketPrice = 50.00m,
                Description = $"Custom event: {eventName}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            // Generate tickets
            var seats = await _context.Seats.Take(ticketCount).ToListAsync();
            for (int i = 0; i < Math.Min(ticketCount, seats.Count); i++)
            {
                var ticket = new Ticket
                {
                    TicketNumber = $"CUSTOM{evt.Id:D2}{i + 1:D3}",
                    EventId = evt.Id,
                    SeatId = seats[i].Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = evt.BaseTicketPrice ?? 50.00m,
                    PurchaseDate = DateTime.UtcNow,
                    CustomerName = $"Customer {i + 1}",
                    CustomerEmail = $"customer{i + 1}@example.com",
                    Status = "Active",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    SeatNumber = seats[i].SeatCode,
                    Section = seats[i].Section?.SectionCode ?? "A",
                    Row = seats[i].RowNumber.ToString(),
                    EventName = evt.EventName,
                    EventDate = evt.EventDate
                };

                _context.Tickets.Add(ticket);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating event with tickets");
            return false;
        }
    }

    public async Task<bool> ClearDemoDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting comprehensive data cleanup...");

            // Clear in proper dependency order
            _context.OrderItems.RemoveRange(await _context.OrderItems.ToListAsync());
            _context.Orders.RemoveRange(await _context.Orders.ToListAsync());
            _context.Payments.RemoveRange(await _context.Payments.ToListAsync());
            _context.CartItems.RemoveRange(await _context.CartItems.ToListAsync());
            _context.ShoppingCarts.RemoveRange(await _context.ShoppingCarts.ToListAsync());
            _context.SeatReservations.RemoveRange(await _context.SeatReservations.ToListAsync());
            _context.TicketSessions.RemoveRange(await _context.TicketSessions.ToListAsync());
            _context.OrderSessions.RemoveRange(await _context.OrderSessions.ToListAsync());
            _context.Notifications.RemoveRange(await _context.Notifications.ToListAsync());
            _context.EventAnalytics.RemoveRange(await _context.EventAnalytics.ToListAsync());
            _context.EventStaffAssignments.RemoveRange(await _context.EventStaffAssignments.ToListAsync());
            _context.Tickets.RemoveRange(await _context.Tickets.ToListAsync());
            _context.Events.RemoveRange(await _context.Events.ToListAsync());
            
            // Clear all users except admin (ID = 1)
            _context.Users.RemoveRange(await _context.Users.Where(u => u.Id != 1).ToListAsync());
            
            // Clear drinks except seeded ones (let's keep all seeded drinks)
            _context.Drinks.RemoveRange(await _context.Drinks.Where(d => d.Id > 8).ToListAsync()); 

            // Clear logs but keep recent system logs
            var oldLogs = await _context.LogEntries.Where(l => l.Timestamp < DateTime.UtcNow.AddHours(-24)).ToListAsync();
            _context.LogEntries.RemoveRange(oldLogs);

            await _context.SaveChangesAsync();

            // Keep stadium structure intact:
            // - Tribunes, Rings, Sectors, StadiumSeatsNew
            // - StadiumSeats, StadiumSections, Seats
            _logger.LogInformation("Comprehensive data cleanup completed. Stadium structure preserved.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing data comprehensively");
            return false;
        }
    }

    public async Task<bool> GenerateDemoDataForEventAsync(int eventId)
    {
        try
        {
            _logger.LogInformation("Generating demo data for event {EventId}", eventId);

            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
            {
                _logger.LogWarning("Event {EventId} not found", eventId);
                return false;
            }

            // Generate demo tickets for the event
            var availableSeats = await _context.Seats.Where(s => !_context.Tickets.Any(t => t.SeatId == s.Id && t.EventId == eventId)).Take(20).ToListAsync();
            
            for (int i = 0; i < availableSeats.Count; i++)
            {
                var seat = availableSeats[i];
                var ticket = new Ticket
                {
                    TicketNumber = $"DEMO{eventId:D2}{i + 1:D3}",
                    EventId = eventId,
                    SeatId = seat.Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = 45.00m + (i % 3 * 15.00m), // Varying prices
                    PurchaseDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 30)),
                    CustomerName = $"Demo Customer {i + 1}",
                    CustomerEmail = $"demo{i + 1}@example.com",
                    Status = "Active",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    SeatNumber = seat.SeatCode,
                    Section = seat.Section?.SectionCode ?? "A",
                    Row = seat.RowNumber.ToString()
                };

                // Set event details from the event
                var eventEntity = await _context.Events.FindAsync(eventId);
                if (eventEntity != null)
                {
                    ticket.EventName = eventEntity.EventName;
                    ticket.EventDate = eventEntity.EventDate;
                }

                _context.Tickets.Add(ticket);
            }

            // Generate demo orders for the event
            for (int i = 0; i < 5; i++)
            {
                var order = new Order
                {
                    EventId = eventId,
                    TicketNumber = $"ORD{eventId:D2}{i + 1:D3}",
                    SeatNumber = $"A{i + 1}",
                    CustomerId = 104 + (i % 2), // Use demo customers
                    TotalAmount = 25.50m + (i * 5.25m),
                    Status = (OrderStatus)((i % 5) + 1), // Various statuses (1-5)
                    CreatedAt = DateTime.UtcNow.AddHours(-Random.Shared.Next(1, 24)),
                    Notes = $"Demo order {i + 1} for event {eventId}",
                    CustomerNotes = $"Generated demo order",
                    DeliveryNotes = $"Section {(char)('A' + (i % 4))}, Row {i + 1}"
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Demo data generated successfully for event {EventId}", eventId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating demo data for event {EventId}", eventId);
            return false;
        }
    }
}