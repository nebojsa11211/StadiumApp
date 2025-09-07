using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.API.Services;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerAnalyticsController : ControllerBase
{
    private readonly ICustomerAnalyticsService _customerAnalyticsService;
    private readonly ILogger<CustomerAnalyticsController> _logger;

    public CustomerAnalyticsController(
        ICustomerAnalyticsService customerAnalyticsService,
        ILogger<CustomerAnalyticsController> logger)
    {
        _customerAnalyticsService = customerAnalyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Get paginated customer analytics with filtering and sorting
    /// </summary>
    [HttpPost("search")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedCustomerAnalyticsDto>> GetCustomerAnalytics(
        [FromBody] CustomerAnalyticsFilterDto filter)
    {
        try
        {
            if (filter == null)
            {
                return BadRequest("Filter cannot be null");
            }

            // Validate page size
            if (filter.PageSize > 100)
            {
                filter.PageSize = 100; // Limit to prevent performance issues
            }

            var result = await _customerAnalyticsService.GetCustomerAnalyticsAsync(filter);
            
            if (result == null)
            {
                return NotFound("No customer analytics data found");
            }

            _logger.LogInformation("Customer analytics retrieved: {Count} customers, page {Page}/{TotalPages}",
                result.Customers.Count, result.Page, result.TotalPages);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid customer analytics filter parameters");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer analytics");
            return StatusCode(500, "An error occurred while retrieving customer analytics");
        }
    }

    /// <summary>
    /// Get detailed spending information for a specific customer
    /// </summary>
    [HttpGet("customer/{customerEmail}/details")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CustomerSpendingDetailDto>> GetCustomerSpendingDetails(
        [FromRoute] string customerEmail)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                return BadRequest("Customer email is required");
            }

            // Validate email format
            if (!new EmailAddressAttribute().IsValid(customerEmail))
            {
                return BadRequest("Invalid email format");
            }

            var result = await _customerAnalyticsService.GetCustomerSpendingDetailsAsync(customerEmail);
            
            if (result == null)
            {
                return NotFound($"No spending details found for customer: {customerEmail}");
            }

            _logger.LogInformation("Customer spending details retrieved for: {CustomerEmail}", customerEmail);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer spending details for {CustomerEmail}", customerEmail);
            return StatusCode(500, "An error occurred while retrieving customer spending details");
        }
    }

    /// <summary>
    /// Get customer analytics summary statistics
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<CustomerAnalyticsSummaryDto>> GetCustomerAnalyticsSummary()
    {
        try
        {
            var summary = await _customerAnalyticsService.GetCustomerAnalyticsSummaryAsync();
            
            if (summary == null)
            {
                return NotFound("No customer analytics summary available");
            }

            _logger.LogInformation("Customer analytics summary retrieved: {TotalCustomers} customers, ${TotalRevenue} revenue",
                summary.TotalCustomers, summary.TotalRevenue);

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer analytics summary");
            return StatusCode(500, "An error occurred while retrieving customer analytics summary");
        }
    }

    /// <summary>
    /// Export customer analytics data to CSV format
    /// </summary>
    [HttpPost("export")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ExportCustomerAnalytics([FromBody] CustomerAnalyticsFilterDto filter)
    {
        try
        {
            if (filter == null)
            {
                return BadRequest("Filter cannot be null");
            }

            // Override pagination for export - get all matching records
            filter.Page = 1;
            filter.PageSize = int.MaxValue;

            var csvData = await _customerAnalyticsService.ExportCustomerAnalyticsAsync(filter);
            
            if (string.IsNullOrEmpty(csvData))
            {
                return NotFound("No data available for export");
            }

            var fileName = $"customer_analytics_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvData);

            _logger.LogInformation("Customer analytics export generated: {FileName}", fileName);

            return File(bytes, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting customer analytics data");
            return StatusCode(500, "An error occurred while exporting customer analytics data");
        }
    }

    /// <summary>
    /// Get top spending customers
    /// </summary>
    [HttpGet("top-customers")]
    public async Task<ActionResult<List<CustomerAnalyticsDto>>> GetTopSpendingCustomers(
        [FromQuery] int limit = 10)
    {
        try
        {
            if (limit <= 0 || limit > 100)
            {
                return BadRequest("Limit must be between 1 and 100");
            }

            var topCustomers = await _customerAnalyticsService.GetTopSpendingCustomersAsync(limit);
            
            if (topCustomers == null || !topCustomers.Any())
            {
                return Ok(new List<CustomerAnalyticsDto>());
            }

            _logger.LogInformation("Top {Limit} spending customers retrieved", limit);
            return Ok(topCustomers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving top spending customers");
            return StatusCode(500, "An error occurred while retrieving top spending customers");
        }
    }

    /// <summary>
    /// Get customer spending trends over time
    /// </summary>
    [HttpGet("spending-trends")]
    public async Task<ActionResult<Dictionary<string, decimal>>> GetCustomerSpendingTrends(
        [FromQuery] int days = 30)
    {
        try
        {
            if (days <= 0 || days > 365)
            {
                return BadRequest("Days must be between 1 and 365");
            }

            var trends = await _customerAnalyticsService.GetCustomerSpendingTrendsAsync(days);
            
            if (trends == null || !trends.Any())
            {
                return Ok(new Dictionary<string, decimal>());
            }

            _logger.LogInformation("Customer spending trends retrieved for {Days} days", days);
            return Ok(trends);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer spending trends");
            return StatusCode(500, "An error occurred while retrieving customer spending trends");
        }
    }

    /// <summary>
    /// Get customer acquisition metrics
    /// </summary>
    [HttpGet("acquisition")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Dictionary<string, object>>> GetCustomerAcquisitionMetrics(
        [FromQuery] int months = 12)
    {
        try
        {
            if (months <= 0 || months > 36)
            {
                return BadRequest("Months must be between 1 and 36");
            }

            var metrics = await _customerAnalyticsService.GetCustomerAcquisitionMetricsAsync(months);
            
            if (metrics == null || !metrics.Any())
            {
                return Ok(new Dictionary<string, object>());
            }

            _logger.LogInformation("Customer acquisition metrics retrieved for {Months} months", months);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer acquisition metrics");
            return StatusCode(500, "An error occurred while retrieving customer acquisition metrics");
        }
    }

    /// <summary>
    /// Get customer retention analysis
    /// </summary>
    [HttpGet("retention")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Dictionary<string, object>>> GetCustomerRetentionAnalysis()
    {
        try
        {
            var analysis = await _customerAnalyticsService.GetCustomerRetentionAnalysisAsync();
            
            if (analysis == null || !analysis.Any())
            {
                return Ok(new Dictionary<string, object>());
            }

            _logger.LogInformation("Customer retention analysis retrieved");
            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer retention analysis");
            return StatusCode(500, "An error occurred while retrieving customer retention analysis");
        }
    }
}