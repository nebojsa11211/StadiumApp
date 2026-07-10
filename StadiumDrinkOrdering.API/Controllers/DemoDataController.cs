using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.API.Services;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("[controller]")]
[Route("demo-data")] // Support both routes for compatibility
[Authorize(Roles = "Admin")] // SECURITY: Admin-only access to demo data operations
public class DemoDataController : ControllerBase
{
    private readonly ILogger<DemoDataController> _logger;
    private readonly IDemoDataService _demoDataService;

    public DemoDataController(ILogger<DemoDataController> logger, IDemoDataService demoDataService)
    {
        _logger = logger;
        _demoDataService = demoDataService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { message = "DemoDataController is working", timestamp = DateTime.UtcNow });
    }

    [HttpPost("generate/{eventId}")]
    public async Task<IActionResult> GenerateDemoDataForEvent(int eventId)
    {
        try
        {
            _logger.LogInformation("Generating demo data for event {EventId}", eventId);
            var result = await _demoDataService.GenerateDemoDataForEventAsync(eventId);
            
            if (result)
            {
                return Ok(new { 
                    success = true, 
                    message = $"Demo data generated successfully for event {eventId}", 
                    timestamp = DateTime.UtcNow 
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = $"Failed to generate demo data for event {eventId}. Event may not exist.", 
                    timestamp = DateTime.UtcNow 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating demo data for event {EventId}", eventId);
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while generating demo data", 
                error = ex.Message 
            });
        }
    }

    [HttpPost("generate-comprehensive")]
    public async Task<IActionResult> GenerateComprehensiveDemoData()
    {
        try
        {
            _logger.LogInformation("Generating comprehensive demo data");
            var result = await _demoDataService.GenerateComprehensiveDemoDataAsync();
            
            if (result)
            {
                return Ok(new { 
                    success = true, 
                    message = "Comprehensive demo data generated successfully", 
                    timestamp = DateTime.UtcNow 
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Failed to generate comprehensive demo data", 
                    timestamp = DateTime.UtcNow 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating comprehensive demo data");
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while generating demo data", 
                error = ex.Message 
            });
        }
    }

    [HttpPost("generate-drink-sales-completed")]
    public async Task<IActionResult> GenerateDrinkSalesForCompletedEvents()
    {
        try
        {
            _logger.LogInformation("Generating drink sales for completed events");
            var seeded = await _demoDataService.GenerateDrinkSalesForCompletedEventsAsync();

            return Ok(new
            {
                success = true,
                eventsSeeded = seeded,
                message = seeded > 0
                    ? $"Generated drink sales for {seeded} completed event(s)."
                    : "No completed events needed drink sales (none found, or all already had sales).",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating drink sales for completed events");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while generating drink sales for completed events",
                error = ex.Message
            });
        }
    }

    [HttpPost("backfill-ticket-statuses")]
    public async Task<IActionResult> BackfillPastEventTicketStatuses()
    {
        try
        {
            _logger.LogInformation("Backfilling ticket lifecycle statuses for past events");
            var updated = await _demoDataService.BackfillPastEventTicketStatusesAsync();

            return Ok(new
            {
                success = true,
                ticketsUpdated = updated,
                message = updated > 0
                    ? $"Backfilled lifecycle status (85% Used / 10% Active / 5% Cancelled) for {updated} past-event ticket(s)."
                    : "No past-event tickets found to update.",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error backfilling ticket statuses for past events");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while backfilling ticket statuses",
                error = ex.Message
            });
        }
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearDemoData()
    {
        try
        {
            _logger.LogInformation("Clearing demo data");
            var result = await _demoDataService.ClearDemoDataAsync();
            
            if (result)
            {
                return Ok(new { 
                    success = true, 
                    message = "Demo data cleared successfully", 
                    timestamp = DateTime.UtcNow 
                });
            }
            else
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Failed to clear demo data", 
                    timestamp = DateTime.UtcNow 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing demo data");
            return StatusCode(500, new { 
                success = false, 
                message = "An error occurred while clearing demo data", 
                error = ex.Message 
            });
        }
    }
}