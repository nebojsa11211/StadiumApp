using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services
{
    public class TicketingDataImportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketingDataImportService> _logger;

        public TicketingDataImportService(ApplicationDbContext context, ILogger<TicketingDataImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ImportFromJsonFileAsync(string jsonFilePath)
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    _logger.LogError($"File not found: {jsonFilePath}");
                    return false;
                }

                var jsonString = await File.ReadAllTextAsync(jsonFilePath);
                var jsonData = JsonDocument.Parse(jsonString);

                var strategy = _context.Database.CreateExecutionStrategy();
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        _logger.LogInformation("Starting ticketing data import with transaction...");

                        // Import Events
                        await ImportEventsAsync(jsonData);
                        
                        // Import Customers
                        await ImportCustomersAsync(jsonData);
                        
                        // Import Staff
                        await ImportStaffAsync(jsonData);
                        
                        // Import Orders and Tickets
                        await ImportOrdersAndTicketsAsync(jsonData);

                        await transaction.CommitAsync();
                        _logger.LogInformation("Ticketing data import completed successfully!");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Import failed, transaction rolled back");
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import ticketing data");
                return false;
            }
        }

        private async Task ImportEventsAsync(JsonDocument jsonData)
        {
            _logger.LogInformation("Importing events...");
            var events = jsonData.RootElement.GetProperty("events");
            int importedCount = 0;
            
            // Get all existing event IDs in one query
            var existingEventIds = (await _context.Events
                .Select(e => e.Id)
                .ToListAsync()).ToHashSet();
            
            var newEvents = new List<Event>();
            
            foreach (var eventElement in events.EnumerateArray())
            {
                var eventId = eventElement.GetProperty("id").GetInt32();
                
                // Check if event already exists
                if (!existingEventIds.Contains(eventId))
                {
                    var newEvent = new Event
                    {
                        Id = eventId,
                        EventName = eventElement.GetProperty("name").GetString() ?? string.Empty,
                        Description = eventElement.GetProperty("description").GetString(),
                        EventDate = DateTime.Parse(eventElement.GetProperty("eventDate").GetString() ?? DateTime.Now.ToString()).ToUniversalTime(),
                        EventType = eventElement.GetProperty("eventType").GetString() ?? "Other",
                        TotalSeats = eventElement.GetProperty("capacity").GetInt32(),
                        BaseTicketPrice = eventElement.GetProperty("basePrice").GetDecimal(),
                        IsActive = eventElement.GetProperty("isActive").GetBoolean(),
                        CreatedAt = DateTime.Parse(eventElement.GetProperty("createdAt").GetString() ?? DateTime.Now.ToString()).ToUniversalTime()
                    };
                    
                    newEvents.Add(newEvent);
                    importedCount++;
                }
            }
            
            if (newEvents.Any())
            {
                _context.Events.AddRange(newEvents);
                await _context.SaveChangesAsync();
            }
            
            _logger.LogInformation($"Imported {importedCount} new events out of {events.GetArrayLength()} total");
        }

        private async Task ImportCustomersAsync(JsonDocument jsonData)
        {
            _logger.LogInformation("Importing customers...");
            var customers = jsonData.RootElement.GetProperty("customers");
            int importedCount = 0;
            
            // Get all existing user emails in one query
            var existingEmails = (await _context.Users
                .Select(u => u.Email)
                .ToListAsync()).ToHashSet();
            
            var newUsers = new List<User>();
            
            foreach (var customerElement in customers.EnumerateArray())
            {
                var email = customerElement.GetProperty("email").GetString();
                
                // Check if user already exists by email
                if (email != null && !existingEmails.Contains(email))
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!");
                    
                    var newUser = new User
                    {
                        Username = customerElement.GetProperty("username").GetString() ?? string.Empty,
                        Email = email,
                        PasswordHash = passwordHash,
                        Role = UserRole.Customer,
                        CreatedAt = DateTime.Parse(customerElement.GetProperty("registeredDate").GetString() ?? DateTime.Now.ToString()).ToUniversalTime()
                    };
                    
                    newUsers.Add(newUser);
                    importedCount++;
                }
            }
            
            if (newUsers.Any())
            {
                _context.Users.AddRange(newUsers);
                await _context.SaveChangesAsync();
            }
            
            _logger.LogInformation($"Imported {importedCount} new customers out of {customers.GetArrayLength()} total");
        }

        private async Task ImportStaffAsync(JsonDocument jsonData)
        {
            _logger.LogInformation("Importing staff...");
            var staff = jsonData.RootElement.GetProperty("staff");
            int importedCount = 0;
            
            foreach (var staffElement in staff.EnumerateArray())
            {
                var email = staffElement.GetProperty("email").GetString();
                
                // Check if user already exists by email
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser == null)
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("StaffPassword123!");
                    
                    var roleString = staffElement.GetProperty("role").GetString() ?? "Bartender";
                    var role = roleString.ToLower() switch
                    {
                        "admin" => UserRole.Admin,
                        "waiter" => UserRole.Waiter,
                        "bartender" => UserRole.Bartender,
                        _ => UserRole.Bartender
                    };
                    
                    var newUser = new User
                    {
                        Username = staffElement.GetProperty("username").GetString() ?? string.Empty,
                        Email = email ?? string.Empty,
                        PasswordHash = passwordHash,
                        Role = role,
                        CreatedAt = DateTime.Parse(staffElement.GetProperty("hireDate").GetString() ?? DateTime.Now.ToString()).ToUniversalTime()
                    };
                    
                    _context.Users.Add(newUser);
                    importedCount++;
                }
            }
            
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Imported {importedCount} new staff members out of {staff.GetArrayLength()} total");
        }

        private async Task ImportOrdersAndTicketsAsync(JsonDocument jsonData)
        {
            _logger.LogInformation("Importing orders and tickets...");
            var orders = jsonData.RootElement.GetProperty("orders");
            var customers = jsonData.RootElement.GetProperty("customers");
            var staff = jsonData.RootElement.GetProperty("staff");
            int ordersImported = 0;
            int ticketsImported = 0;
            
            // Create email to ID mapping for users
            var customerEmailMap = customers.EnumerateArray()
                .ToDictionary(c => c.GetProperty("email").GetString(), c => c.GetProperty("id").GetInt32());
            var staffEmailMap = staff.EnumerateArray()
                .ToDictionary(s => s.GetProperty("email").GetString(), s => s.GetProperty("id").GetInt32());
            
            foreach (var orderElement in orders.EnumerateArray())
            {
                var orderId = orderElement.GetProperty("id").GetInt32();
                
                // Check if order already exists
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (existingOrder == null)
                {
                    // Get actual user IDs from database
                    var customerJsonId = orderElement.GetProperty("customerId").GetInt32();
                    var customerEmail = customers.EnumerateArray()
                        .First(c => c.GetProperty("id").GetInt32() == customerJsonId)
                        .GetProperty("email").GetString();
                    
                    var customerUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == customerEmail);
                    if (customerUser == null)
                    {
                        _logger.LogWarning($"Customer with email {customerEmail} not found, skipping order {orderId}");
                        continue;
                    }
                    
                    var statusString = orderElement.GetProperty("status").GetString() ?? "Pending";
                    var orderStatus = statusString switch
                    {
                        "Completed" => OrderStatus.Delivered,
                        "Confirmed" => OrderStatus.Accepted,
                        "Cancelled" => OrderStatus.Cancelled,
                        _ => OrderStatus.Pending
                    };
                    
                    var newOrder = new Order
                    {
                        Id = orderId,
                        CustomerId = customerUser.Id,
                        EventId = orderElement.GetProperty("eventId").GetInt32(),
                        TotalAmount = orderElement.GetProperty("totalAmount").GetDecimal(),
                        Status = orderStatus,
                        TicketNumber = $"ORD-{orderId}",
                        SeatNumber = "Multiple",
                        CreatedAt = DateTime.Parse(orderElement.GetProperty("orderDate").GetString() ?? DateTime.Now.ToString()).ToUniversalTime()
                    };
                    
                    // Add served by if exists
                    if (orderElement.TryGetProperty("servedBy", out var servedBy))
                    {
                        var staffJsonId = servedBy.GetInt32();
                        var staffEmail = staff.EnumerateArray()
                            .First(s => s.GetProperty("id").GetInt32() == staffJsonId)
                            .GetProperty("email").GetString();
                        
                        var staffUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == staffEmail);
                        if (staffUser != null)
                        {
                            newOrder.AssignedStaffId = staffUser.Id;
                        }
                    }
                    
                    _context.Orders.Add(newOrder);
                    await _context.SaveChangesAsync();
                    ordersImported++;
                    
                    // Import tickets for this order
                    if (orderElement.TryGetProperty("tickets", out var tickets))
                    {
                        foreach (var ticketElement in tickets.EnumerateArray())
                        {
                            var ticket = new Ticket
                            {
                                TicketNumber = ticketElement.GetProperty("id").GetString() ?? $"TKT-{Guid.NewGuid()}",
                                EventId = newOrder.EventId.Value,
                                SeatNumber = ticketElement.GetProperty("seatNumber").GetString(),
                                Section = ticketElement.GetProperty("sector").GetString(),
                                Row = ticketElement.GetProperty("row").GetInt32().ToString(),
                                Price = ticketElement.GetProperty("price").GetDecimal(),
                                QRCode = ticketElement.GetProperty("qrCode").GetString() ?? string.Empty,
                                QRCodeToken = ticketElement.GetProperty("qrCode").GetString() ?? Guid.NewGuid().ToString(),
                                IsUsed = ticketElement.GetProperty("isUsed").GetBoolean(),
                                Status = ticketElement.GetProperty("isUsed").GetBoolean() ? "Used" : "Active",
                                PurchaseDate = newOrder.CreatedAt,
                                CustomerEmail = customerUser.Email
                            };
                            
                            _context.Tickets.Add(ticket);
                            ticketsImported++;
                        }
                    }
                    
                    // Import beverage orders if exist
                    if (orderElement.TryGetProperty("beverageOrders", out var beverageOrders))
                    {
                        foreach (var beverageElement in beverageOrders.EnumerateArray())
                        {
                            // Check if drink exists, if not create it
                            var drinkName = beverageElement.GetProperty("drinkName").GetString();
                            var drink = await _context.Drinks.FirstOrDefaultAsync(d => d.Name == drinkName);
                            if (drink == null)
                            {
                                var drinkCategory = drinkName switch
                                {
                                    "Beer" => DrinkCategory.Beer,
                                    "Champagne" => DrinkCategory.Cocktail,
                                    "Soft Drink" => DrinkCategory.SoftDrink,
                                    "Water" => DrinkCategory.Water,
                                    "Coffee" => DrinkCategory.Coffee,
                                    _ => DrinkCategory.SoftDrink
                                };
                                    
                                drink = new Drink
                                {
                                    Name = drinkName ?? "Unknown",
                                    Category = drinkCategory,
                                    Price = beverageElement.GetProperty("price").GetDecimal() / beverageElement.GetProperty("quantity").GetInt32(),
                                    IsAvailable = true
                                };
                                _context.Drinks.Add(drink);
                                await _context.SaveChangesAsync();
                            }
                            
                            var orderItem = new OrderItem
                            {
                                OrderId = orderId,
                                DrinkId = drink.Id,
                                Quantity = beverageElement.GetProperty("quantity").GetInt32()
                            };
                            
                            _context.OrderItems.Add(orderItem);
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                }
            }
            
            _logger.LogInformation($"Imported {ordersImported} new orders with {ticketsImported} tickets");
        }
    }
}