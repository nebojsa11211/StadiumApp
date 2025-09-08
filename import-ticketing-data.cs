using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

public class TicketingDataImporter
{
    private readonly ApplicationDbContext _context;
    private readonly JsonDocument _jsonData;

    public TicketingDataImporter(ApplicationDbContext context, string jsonFilePath)
    {
        _context = context;
        var jsonString = File.ReadAllText(jsonFilePath);
        _jsonData = JsonDocument.Parse(jsonString);
    }

    public async Task ImportAllDataAsync()
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            Console.WriteLine("Starting data import...");

            // Import Events
            await ImportEventsAsync();
            
            // Import Customers
            await ImportCustomersAsync();
            
            // Import Staff
            await ImportStaffAsync();
            
            // Import Orders and Tickets
            await ImportOrdersAndTicketsAsync();

            await transaction.CommitAsync();
            Console.WriteLine("Data import completed successfully!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error during import: {ex.Message}");
            throw;
        }
    }

    private async Task ImportEventsAsync()
    {
        Console.WriteLine("Importing events...");
        var events = _jsonData.RootElement.GetProperty("events");
        
        foreach (var eventElement in events.EnumerateArray())
        {
            var eventId = eventElement.GetProperty("id").GetInt32();
            
            // Check if event already exists
            var existingEvent = await _context.Events.FindAsync(eventId);
            if (existingEvent == null)
            {
                var newEvent = new Event
                {
                    Id = eventId,
                    Name = eventElement.GetProperty("name").GetString(),
                    Description = eventElement.GetProperty("description").GetString(),
                    EventDate = DateTime.Parse(eventElement.GetProperty("eventDate").GetString()),
                    EventType = eventElement.GetProperty("eventType").GetString(),
                    Venue = eventElement.GetProperty("venue").GetString(),
                    Capacity = eventElement.GetProperty("capacity").GetInt32(),
                    BasePrice = eventElement.GetProperty("basePrice").GetDecimal(),
                    PremiumPrice = eventElement.GetProperty("premiumPrice").GetDecimal(),
                    VipPrice = eventElement.GetProperty("vipPrice").GetDecimal(),
                    IsActive = eventElement.GetProperty("isActive").GetBoolean(),
                    CreatedAt = DateTime.Parse(eventElement.GetProperty("createdAt").GetString())
                };
                
                _context.Events.Add(newEvent);
            }
        }
        
        await _context.SaveChangesAsync();
        Console.WriteLine($"Imported {events.GetArrayLength()} events");
    }

    private async Task ImportCustomersAsync()
    {
        Console.WriteLine("Importing customers...");
        var customers = _jsonData.RootElement.GetProperty("customers");
        
        foreach (var customerElement in customers.EnumerateArray())
        {
            var customerId = customerElement.GetProperty("id").GetInt32();
            
            // Check if user already exists
            var existingUser = await _context.Users.FindAsync(customerId);
            if (existingUser == null)
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!");
                
                var newUser = new User
                {
                    Id = customerId,
                    Username = customerElement.GetProperty("username").GetString(),
                    Email = customerElement.GetProperty("email").GetString(),
                    PasswordHash = passwordHash,
                    FirstName = customerElement.GetProperty("firstName").GetString(),
                    LastName = customerElement.GetProperty("lastName").GetString(),
                    PhoneNumber = customerElement.GetProperty("phoneNumber").GetString(),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Parse(customerElement.GetProperty("registeredDate").GetString())
                };
                
                _context.Users.Add(newUser);
            }
        }
        
        await _context.SaveChangesAsync();
        Console.WriteLine($"Imported {customers.GetArrayLength()} customers");
    }

    private async Task ImportStaffAsync()
    {
        Console.WriteLine("Importing staff...");
        var staff = _jsonData.RootElement.GetProperty("staff");
        
        foreach (var staffElement in staff.EnumerateArray())
        {
            var staffId = staffElement.GetProperty("id").GetInt32();
            
            // Check if user already exists
            var existingUser = await _context.Users.FindAsync(staffId);
            if (existingUser == null)
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("StaffPassword123!");
                
                var newUser = new User
                {
                    Id = staffId,
                    Username = staffElement.GetProperty("username").GetString(),
                    Email = staffElement.GetProperty("email").GetString(),
                    PasswordHash = passwordHash,
                    FirstName = staffElement.GetProperty("firstName").GetString(),
                    LastName = staffElement.GetProperty("lastName").GetString(),
                    Role = staffElement.GetProperty("role").GetString(),
                    IsActive = true,
                    CreatedAt = DateTime.Parse(staffElement.GetProperty("hireDate").GetString())
                };
                
                _context.Users.Add(newUser);
            }
        }
        
        await _context.SaveChangesAsync();
        Console.WriteLine($"Imported {staff.GetArrayLength()} staff members");
    }

    private async Task ImportOrdersAndTicketsAsync()
    {
        Console.WriteLine("Importing orders and tickets...");
        var orders = _jsonData.RootElement.GetProperty("orders");
        
        foreach (var orderElement in orders.EnumerateArray())
        {
            var orderId = orderElement.GetProperty("id").GetInt32();
            
            // Check if order already exists
            var existingOrder = await _context.Orders.FindAsync(orderId);
            if (existingOrder == null)
            {
                var newOrder = new Order
                {
                    Id = orderId,
                    UserId = orderElement.GetProperty("customerId").GetInt32(),
                    EventId = orderElement.GetProperty("eventId").GetInt32(),
                    OrderDate = DateTime.Parse(orderElement.GetProperty("orderDate").GetString()),
                    TotalAmount = orderElement.GetProperty("totalAmount").GetDecimal(),
                    Status = orderElement.GetProperty("status").GetString(),
                    PaymentStatus = orderElement.GetProperty("paymentStatus").GetString(),
                    PaymentMethod = orderElement.GetProperty("paymentMethod").GetString(),
                    CreatedAt = DateTime.Parse(orderElement.GetProperty("orderDate").GetString())
                };
                
                // Add served by if exists
                if (orderElement.TryGetProperty("servedBy", out var servedBy))
                {
                    newOrder.AssignedStaffId = servedBy.GetInt32();
                }
                
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                
                // Import tickets for this order
                if (orderElement.TryGetProperty("tickets", out var tickets))
                {
                    foreach (var ticketElement in tickets.EnumerateArray())
                    {
                        var ticket = new Ticket
                        {
                            TicketNumber = ticketElement.GetProperty("id").GetString(),
                            OrderId = orderId,
                            EventId = newOrder.EventId.Value,
                            SeatNumber = ticketElement.GetProperty("seatNumber").GetString(),
                            TribuneCode = ticketElement.GetProperty("tribuneCode").GetString(),
                            Ring = ticketElement.GetProperty("ring").GetInt32(),
                            Sector = ticketElement.GetProperty("sector").GetString(),
                            Row = ticketElement.GetProperty("row").GetInt32(),
                            Seat = ticketElement.GetProperty("seat").GetInt32(),
                            PriceCategory = ticketElement.GetProperty("priceCategory").GetString(),
                            Price = ticketElement.GetProperty("price").GetDecimal(),
                            QrCode = ticketElement.GetProperty("qrCode").GetString(),
                            IsUsed = ticketElement.GetProperty("isUsed").GetBoolean(),
                            IsValid = true,
                            PurchaseDate = newOrder.OrderDate
                        };
                        
                        _context.Tickets.Add(ticket);
                    }
                }
                
                // Import beverage orders if exist
                if (orderElement.TryGetProperty("beverageOrders", out var beverageOrders))
                {
                    foreach (var beverageElement in beverageOrders.EnumerateArray())
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = orderId,
                            DrinkName = beverageElement.GetProperty("drinkName").GetString(),
                            Quantity = beverageElement.GetProperty("quantity").GetInt32(),
                            Price = beverageElement.GetProperty("price").GetDecimal() / beverageElement.GetProperty("quantity").GetInt32()
                        };
                        
                        _context.OrderItems.Add(orderItem);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
        }
        
        Console.WriteLine($"Imported {orders.GetArrayLength()} orders with tickets");
    }
}

// Main program
public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("StadiumDrinkOrdering.API/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("StadiumDrinkOrdering.API/appsettings.Development.json", optional: true)
            .Build();
        
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        var serviceProvider = services.BuildServiceProvider();
        
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Import the data
            var importer = new TicketingDataImporter(context, "ticketing-test-data.json");
            await importer.ImportAllDataAsync();
        }
    }
}