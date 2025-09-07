using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
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
}