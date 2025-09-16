using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// API Controller for dynamic stadium SVG layout generation
/// </summary>
[ApiController]
[Route("api/stadium-svg")]
[Authorize] // SECURITY: Default to authorized access, individual endpoints can override with [AllowAnonymous]
public class StadiumSvgController : ControllerBase
{
    private readonly IStadiumLayoutService _stadiumLayoutService;
    private readonly ILogger<StadiumSvgController> _logger;

    public StadiumSvgController(
        IStadiumLayoutService stadiumLayoutService,
        ILogger<StadiumSvgController> logger)
    {
        _stadiumLayoutService = stadiumLayoutService;
        _logger = logger;
    }

    /// <summary>
    /// Get the current stadium SVG layout
    /// </summary>
    [HttpGet("layout")]
    [AllowAnonymous]
    public async Task<ActionResult<StadiumSvgLayoutDto>> GetStadiumLayout()
    {
        try
        {
            _logger.LogInformation("Fetching stadium SVG layout");
            var layout = await _stadiumLayoutService.GetStadiumLayoutAsync();
            
            _logger.LogInformation("Successfully generated stadium layout with {StandCount} stands", layout.Stands.Count);
            return Ok(layout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stadium SVG layout");
            return StatusCode(500, new { Error = "Failed to generate stadium layout", Details = ex.Message });
        }
    }

    /// <summary>
    /// Get stadium layout with event-specific seat data
    /// </summary>
    [HttpGet("layout/event/{eventId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<StadiumSvgLayoutDto>> GetStadiumLayoutWithEventData(int eventId)
    {
        try
        {
            _logger.LogInformation("Fetching stadium SVG layout for event {EventId}", eventId);
            var layout = await _stadiumLayoutService.GetStadiumLayoutWithEventDataAsync(eventId);
            
            _logger.LogInformation("Successfully generated event-specific stadium layout for event {EventId}", eventId);
            return Ok(layout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stadium SVG layout for event {EventId}", eventId);
            return StatusCode(500, new { Error = "Failed to generate event-specific stadium layout", Details = ex.Message });
        }
    }

    /// <summary>
    /// Generate HNK Rijeka layout specifically (for testing/fallback)
    /// </summary>
    [HttpGet("layout/hnk-rijeka")]
    [AllowAnonymous]
    public async Task<ActionResult<StadiumSvgLayoutDto>> GetHNKRijekaLayout()
    {
        try
        {
            _logger.LogInformation("Generating HNK Rijeka stadium layout");
            var layout = await _stadiumLayoutService.GenerateHNKRijekaLayoutAsync();
            
            _logger.LogInformation("Successfully generated HNK Rijeka stadium layout");
            return Ok(layout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating HNK Rijeka stadium layout");
            return StatusCode(500, new { Error = "Failed to generate HNK Rijeka layout", Details = ex.Message });
        }
    }

    /// <summary>
    /// Refresh the stadium layout cache
    /// </summary>
    [HttpPost("layout/refresh")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RefreshStadiumLayout()
    {
        try
        {
            _logger.LogInformation("Refreshing stadium layout cache");
            await _stadiumLayoutService.RefreshLayoutCacheAsync();
            
            _logger.LogInformation("Successfully refreshed stadium layout cache");
            return Ok(new { Message = "Stadium layout cache refreshed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing stadium layout cache");
            return StatusCode(500, new { Error = "Failed to refresh stadium layout cache", Details = ex.Message });
        }
    }

    /// <summary>
    /// Check if stadium data is available
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetStadiumStatus()
    {
        try
        {
            var hasData = await _stadiumLayoutService.HasStadiumDataAsync();
            
            return Ok(new 
            { 
                HasStadiumData = hasData,
                Message = hasData 
                    ? "Stadium structure data is available" 
                    : "No stadium structure data found, using default HNK Rijeka layout",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking stadium status");
            return StatusCode(500, new { Error = "Failed to check stadium status", Details = ex.Message });
        }
    }
}