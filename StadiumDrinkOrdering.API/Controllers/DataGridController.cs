using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DataGridController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataGridController> _logger;

    public DataGridController(ApplicationDbContext context, ILogger<DataGridController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("tables")]
    public IActionResult GetTables()
    {
        try
        {
            var tables = _context.Model.GetEntityTypes()
                .Select(t => new
                {
                    Name = t.GetTableName() ?? t.ShortName(),
                    EntityName = t.ClrType.Name,
                    Schema = t.GetSchema() ?? "dbo",
                    ColumnCount = t.GetProperties().Count(),
                    Columns = t.GetProperties().Select(p => new
                    {
                        Name = p.GetColumnName(),
                        Type = p.ClrType.Name,
                        IsNullable = p.IsNullable,
                        IsPrimaryKey = p.IsPrimaryKey(),
                        IsForeignKey = p.IsForeignKey(),
                        MaxLength = p.GetMaxLength()
                    }).ToList()
                })
                .OrderBy(t => t.Name)
                .ToList();

            return Ok(tables);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database tables");
            return StatusCode(500, new { message = "Error retrieving table information" });
        }
    }

    [HttpGet("table-data/{tableName}")]
    public async Task<IActionResult> GetTableData(
        string tableName,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortColumn = null,
        [FromQuery] string? sortDirection = "asc",
        [FromQuery] string? filters = null)
    {
        try
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(t => t.ClrType.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
            {
                return NotFound(new { message = $"Table '{tableName}' not found" });
            }

            var dbSetProperty = _context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericArguments()[0] == entityType.ClrType);

            if (dbSetProperty == null)
            {
                return NotFound(new { message = $"DbSet for table '{tableName}' not found" });
            }

            var dbSet = dbSetProperty.GetValue(_context);
            if (dbSet == null)
            {
                return NotFound(new { message = $"Unable to access table '{tableName}'" });
            }

            var query = ((IQueryable)dbSet).Cast<object>();

            // Apply filters
            if (!string.IsNullOrEmpty(filters))
            {
                var filterDict = JsonSerializer.Deserialize<Dictionary<string, string>>(filters);
                if (filterDict != null)
                {
                    foreach (var filter in filterDict)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.Value))
                        {
                            query = ApplyFilter(query, entityType.ClrType, filter.Key, filter.Value);
                        }
                    }
                }
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = ApplySort(query, entityType.ClrType, sortColumn, sortDirection);
            }

            // Apply pagination
            var skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            // Execute query and convert to dynamic objects
            var data = await query.ToListAsync();
            var result = ConvertToTableData(data, entityType);

            return Ok(new
            {
                data = result,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting table data for {TableName}", tableName);
            return StatusCode(500, new { message = $"Error retrieving data for table '{tableName}'" });
        }
    }

    [HttpGet("export/{tableName}")]
    public async Task<IActionResult> ExportTableToCsv(
        string tableName,
        [FromQuery] string? sortColumn = null,
        [FromQuery] string? sortDirection = "asc",
        [FromQuery] string? filters = null)
    {
        try
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(t => t.ClrType.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
            {
                return NotFound(new { message = $"Table '{tableName}' not found" });
            }

            var dbSetProperty = _context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericArguments()[0] == entityType.ClrType);

            if (dbSetProperty == null)
            {
                return NotFound(new { message = $"DbSet for table '{tableName}' not found" });
            }

            var dbSet = dbSetProperty.GetValue(_context);
            if (dbSet == null)
            {
                return NotFound(new { message = $"Unable to access table '{tableName}'" });
            }

            var query = ((IQueryable)dbSet).Cast<object>();

            // Apply filters
            if (!string.IsNullOrEmpty(filters))
            {
                var filterDict = JsonSerializer.Deserialize<Dictionary<string, string>>(filters);
                if (filterDict != null)
                {
                    foreach (var filter in filterDict)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.Value))
                        {
                            query = ApplyFilter(query, entityType.ClrType, filter.Key, filter.Value);
                        }
                    }
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = ApplySort(query, entityType.ClrType, sortColumn, sortDirection);
            }

            // Execute query
            var data = await query.ToListAsync();

            // Generate CSV
            var csv = GenerateCsv(data, entityType);
            var bytes = Encoding.UTF8.GetBytes(csv);

            return File(bytes, "text/csv", $"{tableName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting table {TableName} to CSV", tableName);
            return StatusCode(500, new { message = $"Error exporting table '{tableName}'" });
        }
    }

    private IQueryable<object> ApplyFilter(IQueryable<object> query, Type entityType, string propertyName, string filterValue)
    {
        var property = entityType.GetProperty(propertyName);
        if (property == null) return query;

        try
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
            var member = System.Linq.Expressions.Expression.Property(parameter, property);
            
            System.Linq.Expressions.Expression? comparison = null;

            if (property.PropertyType == typeof(string))
            {
                var constant = System.Linq.Expressions.Expression.Constant(filterValue.ToLower());
                var toLower = System.Linq.Expressions.Expression.Call(member, "ToLower", null);
                comparison = System.Linq.Expressions.Expression.Call(toLower, "Contains", null, constant);
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (int.TryParse(filterValue, out var intValue))
                {
                    var constant = System.Linq.Expressions.Expression.Constant(intValue);
                    comparison = System.Linq.Expressions.Expression.Equal(member, constant);
                }
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                if (decimal.TryParse(filterValue, out var decimalValue))
                {
                    var constant = System.Linq.Expressions.Expression.Constant(decimalValue);
                    comparison = System.Linq.Expressions.Expression.Equal(member, constant);
                }
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                if (bool.TryParse(filterValue, out var boolValue))
                {
                    var constant = System.Linq.Expressions.Expression.Constant(boolValue);
                    comparison = System.Linq.Expressions.Expression.Equal(member, constant);
                }
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                if (DateTime.TryParse(filterValue, out var dateValue))
                {
                    var startOfDay = dateValue.Date;
                    var endOfDay = startOfDay.AddDays(1);
                    var startConstant = System.Linq.Expressions.Expression.Constant(startOfDay);
                    var endConstant = System.Linq.Expressions.Expression.Constant(endOfDay);
                    
                    var greaterThanOrEqual = System.Linq.Expressions.Expression.GreaterThanOrEqual(member, startConstant);
                    var lessThan = System.Linq.Expressions.Expression.LessThan(member, endConstant);
                    comparison = System.Linq.Expressions.Expression.AndAlso(greaterThanOrEqual, lessThan);
                }
            }

            if (comparison != null)
            {
                var lambda = System.Linq.Expressions.Expression.Lambda(comparison, parameter);
                var castMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Cast" && m.IsGenericMethodDefinition)
                    .MakeGenericMethod(entityType);
                var whereMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(entityType);

                var castedQuery = castMethod.Invoke(null, new object[] { query });
                var filteredQuery = whereMethod.Invoke(null, new[] { castedQuery, lambda });
                
                var castBackMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Cast" && m.IsGenericMethodDefinition)
                    .MakeGenericMethod(typeof(object));
                
                return (IQueryable<object>)castBackMethod.Invoke(null, new[] { filteredQuery })!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to apply filter on {PropertyName} with value {FilterValue}", propertyName, filterValue);
        }

        return query;
    }

    private IQueryable<object> ApplySort(IQueryable<object> query, Type entityType, string propertyName, string? direction)
    {
        var property = entityType.GetProperty(propertyName);
        if (property == null) return query;

        try
        {
            // For SQLite decimal sorting, we need to handle it differently
            if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                _logger.LogWarning("Sorting by decimal property {PropertyName} not fully supported in SQLite. Data will be sorted on the client side.", propertyName);
                // Return query without server-side sorting for decimal columns
                // The client will need to handle sorting for decimal columns
                return query;
            }

            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "x");
            var member = System.Linq.Expressions.Expression.Property(parameter, property);
            var lambda = System.Linq.Expressions.Expression.Lambda(member, parameter);

            var methodName = direction?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
            
            var castMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Cast" && m.IsGenericMethodDefinition)
                .MakeGenericMethod(entityType);
            var orderByMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, property.PropertyType);

            var castedQuery = castMethod.Invoke(null, new object[] { query });
            var sortedQuery = orderByMethod.Invoke(null, new[] { castedQuery, lambda });
            
            var castBackMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Cast" && m.IsGenericMethodDefinition)
                .MakeGenericMethod(typeof(object));
            
            return (IQueryable<object>)castBackMethod.Invoke(null, new[] { sortedQuery })!;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to apply sort on {PropertyName}", propertyName);
            return query;
        }
    }

    private List<Dictionary<string, object?>> ConvertToTableData(IEnumerable<object> data, Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType)
    {
        var result = new List<Dictionary<string, object?>>();
        var properties = entityType.ClrType.GetProperties();

        foreach (var item in data)
        {
            var row = new Dictionary<string, object?>();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                
                // Handle navigation properties
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    if (value != null && !IsCollection(property.PropertyType))
                    {
                        // For single navigation properties, try to get a display value
                        var displayProperty = property.PropertyType.GetProperty("Name") 
                            ?? property.PropertyType.GetProperty("Title")
                            ?? property.PropertyType.GetProperty("Id");
                        
                        if (displayProperty != null)
                        {
                            value = displayProperty.GetValue(value)?.ToString() ?? "N/A";
                        }
                        else
                        {
                            value = value.ToString();
                        }
                    }
                    else if (IsCollection(property.PropertyType))
                    {
                        // For collection navigation properties, show count
                        if (value is ICollection collection)
                        {
                            value = $"[{collection.Count} items]";
                        }
                        else
                        {
                            value = "[Collection]";
                        }
                    }
                }
                
                row[property.Name] = value;
            }
            result.Add(row);
        }

        return result;
    }

    private bool IsCollection(Type type)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)) 
               && type != typeof(string);
    }

    private string GenerateCsv(IEnumerable<object> data, Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType)
    {
        var csv = new StringBuilder();
        var properties = entityType.ClrType.GetProperties()
            .Where(p => !p.PropertyType.IsClass || p.PropertyType == typeof(string))
            .ToList();

        // Add headers
        csv.AppendLine(string.Join(",", properties.Select(p => EscapeCsvField(p.Name))));

        // Add data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return EscapeCsvField(value?.ToString() ?? "");
            });
            csv.AppendLine(string.Join(",", values));
        }

        return csv.ToString();
    }

    private string EscapeCsvField(string field)
    {
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    [HttpDelete("clear-table/{tableName}")]
    public async Task<IActionResult> ClearTable(string tableName)
    {
        try
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(t => t.ClrType.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
            {
                return NotFound(new { message = $"Table '{tableName}' not found" });
            }

            // Check dependencies before deletion
            var dependencyResult = await CheckTableDependencies(tableName, "delete");
            if (!dependencyResult.CanProceed)
            {
                return BadRequest(new { message = dependencyResult.Message, dependencies = dependencyResult.Dependencies });
            }

            var dbSetProperty = _context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericArguments()[0] == entityType.ClrType);

            if (dbSetProperty == null)
            {
                return NotFound(new { message = $"DbSet for table '{tableName}' not found" });
            }

            var dbSet = dbSetProperty.GetValue(_context);
            if (dbSet == null)
            {
                return NotFound(new { message = $"Unable to access table '{tableName}'" });
            }

            // Get all entities and remove them
            var query = ((IQueryable)dbSet).Cast<object>();
            var entities = await query.ToListAsync();
            
            if (entities.Count == 0)
            {
                return Ok(new { message = "Table is already empty", deletedCount = 0 });
            }

            // Use reflection to call RemoveRange
            var removeRangeMethod = dbSet.GetType().GetMethod("RemoveRange", new Type[] { typeof(IEnumerable<object>) });
            if (removeRangeMethod != null)
            {
                removeRangeMethod.Invoke(dbSet, new object[] { entities });
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation("Cleared {Count} records from table {TableName}", entities.Count, tableName);
            
            return Ok(new { message = $"Successfully deleted {entities.Count} records from {tableName}", deletedCount = entities.Count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing table {TableName}", tableName);
            return StatusCode(500, new { message = $"Error clearing table '{tableName}': {ex.Message}" });
        }
    }

    [HttpPost("generate-data/{tableName}")]
    public async Task<IActionResult> GenerateData(string tableName, [FromBody] GenerateDataRequest request)
    {
        try
        {
            var entityType = _context.Model.GetEntityTypes()
                .FirstOrDefault(t => t.ClrType.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));

            if (entityType == null)
            {
                return NotFound(new { message = $"Table '{tableName}' not found" });
            }

            if (request.Count <= 0 || request.Count > 1000)
            {
                return BadRequest(new { message = "Count must be between 1 and 1000" });
            }

            // Check dependencies before generation
            var dependencyResult = await CheckTableDependencies(tableName, "generate");
            if (!dependencyResult.CanProceed)
            {
                return BadRequest(new { message = dependencyResult.Message, dependencies = dependencyResult.Dependencies });
            }

            var generatedCount = await GenerateDataForTable(tableName, entityType, request.Count);
            
            return Ok(new { 
                message = $"Successfully generated {generatedCount} records for {tableName}", 
                generatedCount = generatedCount 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating data for table {TableName}", tableName);
            return StatusCode(500, new { message = $"Error generating data for table '{tableName}': {ex.Message}" });
        }
    }

    [HttpGet("dependencies/{tableName}")]
    public async Task<IActionResult> GetTableDependencies(string tableName)
    {
        try
        {
            var dependencies = await CheckTableDependencies(tableName, "info");
            return Ok(dependencies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dependencies for table {TableName}", tableName);
            return StatusCode(500, new { message = $"Error getting dependencies for table '{tableName}': {ex.Message}" });
        }
    }

    private async Task<DependencyCheckResult> CheckTableDependencies(string tableName, string operation)
    {
        var result = new DependencyCheckResult { CanProceed = true, Dependencies = new List<string>() };

        switch (tableName.ToLower())
        {
            case "ticket":
                if (operation == "generate")
                {
                    var eventCount = await _context.Set<Event>().CountAsync();
                    if (eventCount == 0)
                    {
                        result.CanProceed = false;
                        result.Message = "Cannot generate tickets: No events exist. Please create events first.";
                        result.Dependencies.Add("Events required");
                    }
                }
                break;

            case "order":
                if (operation == "generate")
                {
                    var customerCount = await _context.Users.CountAsync(u => u.Role == UserRole.Customer);
                    var drinkCount = await _context.Set<Drink>().CountAsync();
                    
                    var issues = new List<string>();
                    if (customerCount == 0) issues.Add("No customers exist");
                    if (drinkCount == 0) issues.Add("No drinks exist");
                    
                    if (issues.Any())
                    {
                        result.CanProceed = false;
                        result.Message = $"Cannot generate orders: {string.Join(", ", issues)}. Please create required data first.";
                        result.Dependencies.AddRange(issues);
                    }
                }
                else if (operation == "delete")
                {
                    var dependentTickets = await _context.Set<Ticket>()
                        .AnyAsync(t => t.Orders.Any());
                    if (dependentTickets)
                    {
                        result.Dependencies.Add("Tickets reference orders");
                    }
                }
                break;

            case "event":
                if (operation == "delete")
                {
                    var dependentTickets = await _context.Set<Ticket>().CountAsync(t => t.EventId > 0);
                    var dependentOrders = await _context.Set<Order>().CountAsync(o => o.EventId.HasValue);
                    
                    if (dependentTickets > 0 || dependentOrders > 0)
                    {
                        result.CanProceed = false;
                        result.Message = $"Cannot delete events: {dependentTickets} tickets and {dependentOrders} orders depend on events. Delete them first.";
                        if (dependentTickets > 0) result.Dependencies.Add($"{dependentTickets} tickets");
                        if (dependentOrders > 0) result.Dependencies.Add($"{dependentOrders} orders");
                    }
                }
                break;

            case "user":
                if (operation == "delete")
                {
                    var dependentOrders = await _context.Set<Order>().CountAsync(o => o.CustomerId > 0);
                    if (dependentOrders > 0)
                    {
                        result.CanProceed = false;
                        result.Message = $"Cannot delete users: {dependentOrders} orders depend on users. Delete orders first.";
                        result.Dependencies.Add($"{dependentOrders} orders");
                    }
                }
                break;

            case "drink":
                if (operation == "delete")
                {
                    var dependentOrderItems = await _context.Set<OrderItem>().CountAsync(oi => oi.DrinkId > 0);
                    if (dependentOrderItems > 0)
                    {
                        result.CanProceed = false;
                        result.Message = $"Cannot delete drinks: {dependentOrderItems} order items depend on drinks. Delete orders first.";
                        result.Dependencies.Add($"{dependentOrderItems} order items");
                    }
                }
                break;
        }

        return result;
    }

    private async Task<int> GenerateDataForTable(string tableName, Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType, int count)
    {
        var random = new Random();
        var generatedCount = 0;

        switch (tableName.ToLower())
        {
            case "event":
                var events = new List<Event>();
                var eventTypes = new[] { "Football", "Basketball", "Concert", "Baseball", "Hockey" };
                var eventNames = new[] { "Championship Final", "Season Opener", "Classic Match", "Derby", "Tournament" };
                
                for (int i = 0; i < count; i++)
                {
                    var eventType = eventTypes[random.Next(eventTypes.Length)];
                    var eventName = eventNames[random.Next(eventNames.Length)];
                    
                    events.Add(new Event
                    {
                        EventName = $"{eventName} {i + 1}",
                        EventType = eventType,
                        EventDate = DateTime.UtcNow.AddDays(random.Next(1, 365)),
                        TotalSeats = random.Next(5000, 50000),
                        IsActive = true,
                        Description = $"Generated {eventType} event",
                        BaseTicketPrice = random.Next(25, 200)
                    });
                }
                
                _context.Set<Event>().AddRange(events);
                generatedCount = events.Count;
                break;

            case "ticket":
                var availableEvents = await _context.Set<Event>().ToListAsync();
                if (availableEvents.Count == 0)
                {
                    throw new InvalidOperationException("No events available for ticket generation");
                }

                var tickets = new List<Ticket>();
                for (int i = 0; i < count; i++)
                {
                    var eventItem = availableEvents[random.Next(availableEvents.Count)];
                    var ticketNumber = $"TIK{DateTime.Now:yyyyMMdd}{i + 1:D6}";
                    
                    tickets.Add(new Ticket
                    {
                        TicketNumber = ticketNumber,
                        EventId = eventItem.Id,
                        QRCode = $"data:qr/{ticketNumber}",
                        QRCodeToken = Guid.NewGuid().ToString(),
                        CustomerName = $"Customer {i + 1}",
                        CustomerEmail = $"customer{i + 1}@example.com",
                        CustomerPhone = $"555-{random.Next(1000, 9999)}",
                        Price = eventItem.BaseTicketPrice ?? random.Next(25, 200),
                        SeatNumber = $"{random.Next(1, 100)}",
                        Section = $"Section {random.Next(1, 10)}",
                        Row = $"{(char)('A' + random.Next(0, 26))}",
                        EventName = eventItem.EventName,
                        EventDate = eventItem.EventDate
                    });
                }
                
                _context.Set<Ticket>().AddRange(tickets);
                generatedCount = tickets.Count;
                break;

            case "drink":
                var drinks = new List<Drink>();
                var drinkCategories = new[] { DrinkCategory.Beer, DrinkCategory.SoftDrink, DrinkCategory.Water, DrinkCategory.Coffee, DrinkCategory.Cocktail, DrinkCategory.EnergyDrink, DrinkCategory.Juice };
                var brands = new[] { "Premium", "Classic", "Gold", "Select", "Special", "Deluxe" };
                
                for (int i = 0; i < count; i++)
                {
                    var category = drinkCategories[random.Next(drinkCategories.Length)];
                    var brand = brands[random.Next(brands.Length)];
                    
                    drinks.Add(new Drink
                    {
                        Name = $"{brand} {category} {i + 1}",
                        Price = Math.Round((decimal)(random.NextDouble() * 15 + 2), 2),
                        StockQuantity = random.Next(0, 100),
                        IsAvailable = random.Next(100) > 10, // 90% available
                        Description = $"Generated {category}",
                        Category = category,
                        ImageUrl = $"/images/drinks/{category.ToString().ToLower()}.jpg"
                    });
                }
                
                _context.Set<Drink>().AddRange(drinks);
                generatedCount = drinks.Count;
                break;

            case "user":
                var users = new List<User>();
                var roles = new[] { UserRole.Customer, UserRole.Admin, UserRole.Bartender, UserRole.Waiter };
                var firstNames = new[] { "John", "Jane", "Mike", "Sarah", "David", "Lisa", "Tom", "Emma" };
                var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller" };
                
                for (int i = 0; i < count; i++)
                {
                    var firstName = firstNames[random.Next(firstNames.Length)];
                    var lastName = lastNames[random.Next(lastNames.Length)];
                    var role = roles[random.Next(roles.Length)];
                    
                    users.Add(new User
                    {
                        Username = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}",
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@example.com",
                        PasswordHash = "GeneratedUser123!", // This would normally be hashed
                        Role = role,
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365))
                    });
                }
                
                _context.Users.AddRange(users);
                generatedCount = users.Count;
                break;

            case "order":
                var customers = await _context.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
                var availableDrinks = await _context.Set<Drink>().Where(d => d.IsAvailable).ToListAsync();
                
                if (customers.Count == 0 || availableDrinks.Count == 0)
                {
                    throw new InvalidOperationException("No customers or drinks available for order generation");
                }

                var orders = new List<Order>();
                var orderStatuses = Enum.GetValues<OrderStatus>();
                
                for (int i = 0; i < count; i++)
                {
                    var customer = customers[random.Next(customers.Count)];
                    var status = orderStatuses[random.Next(orderStatuses.Length)];
                    var ticketNumber = $"ORD{DateTime.Now:yyyyMMdd}{i + 1:D6}";
                    
                    var order = new Order
                    {
                        TicketNumber = ticketNumber,
                        SeatNumber = $"{random.Next(1, 100)}",
                        CustomerId = customer.Id,
                        TotalAmount = Math.Round((decimal)(random.NextDouble() * 50 + 10), 2),
                        Status = status,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-random.Next(1, 1440)), // Up to 24 hours ago
                        CustomerNotes = $"Generated order {i + 1}"
                    };

                    // Set status-specific timestamps
                    switch (status)
                    {
                        case OrderStatus.Accepted:
                            order.AcceptedAt = order.CreatedAt.AddMinutes(random.Next(1, 30));
                            break;
                        case OrderStatus.InPreparation:
                            order.AcceptedAt = order.CreatedAt.AddMinutes(random.Next(1, 30));
                            break;
                        case OrderStatus.Ready:
                            order.AcceptedAt = order.CreatedAt.AddMinutes(random.Next(1, 30));
                            order.PreparedAt = order.AcceptedAt?.AddMinutes(random.Next(5, 45));
                            break;
                        case OrderStatus.Delivered:
                            order.AcceptedAt = order.CreatedAt.AddMinutes(random.Next(1, 30));
                            order.PreparedAt = order.AcceptedAt?.AddMinutes(random.Next(5, 45));
                            order.DeliveredAt = order.PreparedAt?.AddMinutes(random.Next(5, 30));
                            break;
                    }

                    orders.Add(order);

                    // Add random order items
                    var itemCount = random.Next(1, 4);
                    for (int j = 0; j < itemCount; j++)
                    {
                        var drink = availableDrinks[random.Next(availableDrinks.Count)];
                        var quantity = random.Next(1, 4);
                        
                        order.OrderItems.Add(new OrderItem
                        {
                            DrinkId = drink.Id,
                            Quantity = quantity,
                            UnitPrice = drink.Price,
                            TotalPrice = drink.Price * quantity,
                            Order = order
                        });
                    }
                }
                
                _context.Set<Order>().AddRange(orders);
                generatedCount = orders.Count;
                break;

            default:
                throw new NotSupportedException($"Data generation for table '{tableName}' is not supported");
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Generated {Count} records for table {TableName}", generatedCount, tableName);
        
        return generatedCount;
    }

    public class GenerateDataRequest
    {
        public int Count { get; set; }
    }

    public class DependencyCheckResult
    {
        public bool CanProceed { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Dependencies { get; set; } = new List<string>();
    }
}