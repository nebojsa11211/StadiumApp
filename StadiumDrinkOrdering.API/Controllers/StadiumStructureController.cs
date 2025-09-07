using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Admin")] // Temporarily disabled for development
public class StadiumStructureController : ControllerBase
{
    private readonly IStadiumStructureService _stadiumStructureService;
    private readonly ILogger<StadiumStructureController> _logger;

    public StadiumStructureController(IStadiumStructureService stadiumStructureService, ILogger<StadiumStructureController> logger)
    {
        _stadiumStructureService = stadiumStructureService;
        _logger = logger;
    }

    [HttpPost("import/json")]
    public async Task<IActionResult> ImportFromJson(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File must be a JSON file");

        try
        {
            var success = await _stadiumStructureService.ImportFromJsonAsync(file);
            if (success)
                return Ok(new { message = "Stadium structure imported successfully" });
            else
                return BadRequest(new { message = "Failed to import stadium structure" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing stadium structure from JSON");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportFromCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("File must be a CSV file");

        try
        {
            var success = await _stadiumStructureService.ImportFromCsvAsync(file);
            if (success)
                return Ok(new { message = "Stadium structure imported successfully" });
            else
                return BadRequest(new { message = "CSV import not yet implemented" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing stadium structure from CSV");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearStructure()
    {
        try
        {
            var success = await _stadiumStructureService.ClearExistingStructureAsync();
            if (success)
                return Ok(new { message = "Stadium structure cleared successfully" });
            else
                return BadRequest(new { message = "Failed to clear stadium structure" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing stadium structure");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<StadiumSummaryDto>> GetSummary()
    {
        try
        {
            var summary = await _stadiumStructureService.GetStadiumSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting stadium summary");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("full-structure")]
    public async Task<IActionResult> GetFullStructure()
    {
        try
        {
            var structure = await _stadiumStructureService.GetFullStructureAsync();
            return Ok(structure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting full stadium structure");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("total-seats")]
    public async Task<ActionResult<int>> GetTotalSeats()
    {
        try
        {
            var count = await _stadiumStructureService.GetTotalSeatsCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total seats count");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("export/json")]
    public async Task<IActionResult> ExportToJson()
    {
        try
        {
            var stream = await _stadiumStructureService.ExportToJsonAsync();
            var fileName = $"stadium-structure-{DateTime.Now:yyyyMMdd-HHmmss}.json";
            
            return File(stream, "application/json", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting stadium structure to JSON");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportToCsv()
    {
        try
        {
            var stream = await _stadiumStructureService.ExportToCsvAsync();
            var fileName = $"stadium-structure-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
            
            return File(stream, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting stadium structure to CSV");
            return BadRequest(new { message = "CSV export not yet implemented" });
        }
    }

    [HttpPost("sectors/{sectorId}/generate-seats")]
    public async Task<IActionResult> GenerateSeats(int sectorId)
    {
        try
        {
            var success = await _stadiumStructureService.GenerateSeatsForSectorAsync(sectorId);
            if (success)
                return Ok(new { message = "Seats generated successfully" });
            else
                return BadRequest(new { message = "Failed to generate seats or sector not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating seats for sector {SectorId}", sectorId);
            return BadRequest(new { message = ex.Message });
        }
    }
}