using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// TODO: Re-enable authorization after testing: [Authorize(Policy = "AdminOnly")]
public class StadiumSectorOverlayController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StadiumSectorOverlayController> _logger;

    public StadiumSectorOverlayController(
        ApplicationDbContext context,
        ILogger<StadiumSectorOverlayController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all non-deleted sector overlays
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StadiumSectorOverlay>>> GetAll()
    {
        try
        {
            var sectors = await _context.StadiumSectorOverlays
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.SectorCode)
                .ToListAsync();

            return Ok(sectors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sector overlays");
            return StatusCode(500, "Error retrieving sector overlays");
        }
    }

    /// <summary>
    /// Get a specific sector overlay by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StadiumSectorOverlay>> GetById(int id)
    {
        try
        {
            var sector = await _context.StadiumSectorOverlays
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (sector == null)
            {
                return NotFound($"Sector overlay with ID {id} not found");
            }

            return Ok(sector);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sector overlay {Id}", id);
            return StatusCode(500, "Error retrieving sector overlay");
        }
    }

    /// <summary>
    /// Create a new sector overlay
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<StadiumSectorOverlay>> Create([FromBody] StadiumSectorOverlay sector)
    {
        try
        {
            // Check for duplicate sector code
            var exists = await _context.StadiumSectorOverlays
                .AnyAsync(s => s.SectorCode == sector.SectorCode && !s.IsDeleted);

            if (exists)
            {
                return BadRequest($"Sector with code '{sector.SectorCode}' already exists");
            }

            // Set timestamps
            sector.CreatedDate = DateTime.UtcNow;
            sector.ModifiedDate = DateTime.UtcNow;
            sector.IsDeleted = false;

            _context.StadiumSectorOverlays.Add(sector);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created sector overlay: {SectorCode}", sector.SectorCode);

            return CreatedAtAction(nameof(GetById), new { id = sector.Id }, sector);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sector overlay");
            return StatusCode(500, "Error creating sector overlay");
        }
    }

    /// <summary>
    /// Update an existing sector overlay
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] StadiumSectorOverlay sector)
    {
        try
        {
            var existingSector = await _context.StadiumSectorOverlays
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (existingSector == null)
            {
                return NotFound($"Sector overlay with ID {id} not found");
            }

            // Check for duplicate sector code (excluding current sector)
            var duplicateExists = await _context.StadiumSectorOverlays
                .AnyAsync(s => s.SectorCode == sector.SectorCode && s.Id != id && !s.IsDeleted);

            if (duplicateExists)
            {
                return BadRequest($"Sector with code '{sector.SectorCode}' already exists");
            }

            // Update properties
            existingSector.SectorCode = sector.SectorCode;
            existingSector.Name = sector.Name;
            existingSector.TopPercent = sector.TopPercent;
            existingSector.LeftPercent = sector.LeftPercent;
            existingSector.WidthPercent = sector.WidthPercent;
            existingSector.HeightPercent = sector.HeightPercent;
            existingSector.ShapeType = sector.ShapeType;
            existingSector.ShapeData = sector.ShapeData;
            existingSector.Rows = sector.Rows;
            existingSector.SeatsPerRow = sector.SeatsPerRow;
            existingSector.UseVariableSeating = sector.UseVariableSeating;
            existingSector.VariableSeatingData = sector.VariableSeatingData;
            existingSector.Type = sector.Type;
            existingSector.Color = sector.Color;
            existingSector.ModifiedDate = DateTime.UtcNow;
            existingSector.StadiumSectionId = sector.StadiumSectionId;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated sector overlay: {SectorCode}", sector.SectorCode);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sector overlay {Id}", id);
            return StatusCode(500, "Error updating sector overlay");
        }
    }

    /// <summary>
    /// Soft delete a sector overlay
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var sector = await _context.StadiumSectorOverlays
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (sector == null)
            {
                return NotFound($"Sector overlay with ID {id} not found");
            }

            // Soft delete
            sector.IsDeleted = true;
            sector.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted sector overlay: {SectorCode}", sector.SectorCode);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sector overlay {Id}", id);
            return StatusCode(500, "Error deleting sector overlay");
        }
    }

    /// <summary>
    /// Bulk create/update sector overlays (useful for importing)
    /// </summary>
    [HttpPost("bulk")]
    public async Task<ActionResult<IEnumerable<StadiumSectorOverlay>>> BulkUpsert([FromBody] List<StadiumSectorOverlay> sectors)
    {
        try
        {
            var results = new List<StadiumSectorOverlay>();

            foreach (var sector in sectors)
            {
                var existing = await _context.StadiumSectorOverlays
                    .FirstOrDefaultAsync(s => s.SectorCode == sector.SectorCode && !s.IsDeleted);

                if (existing != null)
                {
                    // Update existing
                    existing.Name = sector.Name;
                    existing.TopPercent = sector.TopPercent;
                    existing.LeftPercent = sector.LeftPercent;
                    existing.WidthPercent = sector.WidthPercent;
                    existing.HeightPercent = sector.HeightPercent;
                    existing.ShapeType = sector.ShapeType;
                    existing.ShapeData = sector.ShapeData;
                    existing.Rows = sector.Rows;
                    existing.SeatsPerRow = sector.SeatsPerRow;
                    existing.UseVariableSeating = sector.UseVariableSeating;
                    existing.VariableSeatingData = sector.VariableSeatingData;
                    existing.Type = sector.Type;
                    existing.Color = sector.Color;
                    existing.ModifiedDate = DateTime.UtcNow;
                    existing.StadiumSectionId = sector.StadiumSectionId;

                    results.Add(existing);
                }
                else
                {
                    // Create new
                    sector.CreatedDate = DateTime.UtcNow;
                    sector.ModifiedDate = DateTime.UtcNow;
                    sector.IsDeleted = false;

                    _context.StadiumSectorOverlays.Add(sector);
                    results.Add(sector);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Bulk upserted {Count} sector overlays", sectors.Count);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk upserting sector overlays");
            return StatusCode(500, "Error bulk upserting sector overlays");
        }
    }

    /// <summary>
    /// Clear all sector overlays (soft delete all)
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAll()
    {
        try
        {
            var sectors = await _context.StadiumSectorOverlays
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            foreach (var sector in sectors)
            {
                sector.IsDeleted = true;
                sector.ModifiedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Cleared all sector overlays ({Count} sectors)", sectors.Count);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing sector overlays");
            return StatusCode(500, "Error clearing sector overlays");
        }
    }
}
