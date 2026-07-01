using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
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
    /// Returns a summary of the seats generated for a drawing-tool sector overlay (per-row counts
    /// and availability). Seats are owned directly by the overlay, so this is a straight lookup.
    /// </summary>
    [HttpGet("{id}/persisted-seats")]
    public async Task<ActionResult<SectorPersistedSeatsDto>> GetPersistedSeatsSummary(int id)
    {
        try
        {
            var overlay = await _context.StadiumSectorOverlays
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (overlay == null)
            {
                return NotFound($"Sector overlay with ID {id} not found");
            }

            var dto = new SectorPersistedSeatsDto
            {
                SectorCode = overlay.SectorCode,
                MatchedSectorId = overlay.Id,
                MatchedSectorName = overlay.Name
            };

            dto.Rows = await _context.StadiumSeatsNew
                .AsNoTracking()
                .Where(s => s.StadiumSectorOverlayId == id)
                .GroupBy(s => s.RowNumber)
                .Select(g => new PersistedRowDto
                {
                    RowNumber = g.Key,
                    SeatCount = g.Count(),
                    AvailableCount = g.Count(x => x.IsAvailable)
                })
                .OrderBy(r => r.RowNumber)
                .ToListAsync();

            dto.TotalSeats = dto.Rows.Sum(r => r.SeatCount);
            dto.AvailableSeats = dto.Rows.Sum(r => r.AvailableCount);
            dto.IsPersisted = dto.Rows.Count > 0;

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving persisted seat summary for overlay {Id}", id);
            return StatusCode(500, "Error retrieving persisted seat summary");
        }
    }

    /// <summary>
    /// Returns the generated seats (with their unique codes) for a single row of an overlay sector.
    /// Fetched lazily per row to stay cheap for very large sectors.
    /// </summary>
    [HttpGet("{id}/persisted-seats/{row:int}")]
    public async Task<ActionResult<IEnumerable<PersistedSeatDto>>> GetPersistedSeatsForRow(int id, int row)
    {
        try
        {
            var seats = await _context.StadiumSeatsNew
                .AsNoTracking()
                .Where(s => s.StadiumSectorOverlayId == id && s.RowNumber == row)
                .OrderBy(s => s.SeatNumber)
                .Select(s => new PersistedSeatDto
                {
                    Id = s.Id,
                    RowNumber = s.RowNumber,
                    SeatNumber = s.SeatNumber,
                    UniqueCode = s.UniqueCode,
                    IsAvailable = s.IsAvailable
                })
                .ToListAsync();

            return Ok(seats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving persisted seats for overlay {Id} row {Row}", id, row);
            return StatusCode(500, "Error retrieving persisted seats");
        }
    }

    /// <summary>
    /// Generates seats for every overlay that currently has none (one-time backfill for sectors
    /// created before seat generation existed). Returns how many sectors/seats were created.
    /// </summary>
    [HttpPost("generate-missing-seats")]
    public async Task<IActionResult> GenerateMissingSeats()
    {
        try
        {
            var overlays = await _context.StadiumSectorOverlays
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            var sectorsTouched = 0;
            var seatsGenerated = 0;

            foreach (var overlay in overlays)
            {
                var hasSeats = await _context.StadiumSeatsNew
                    .AnyAsync(s => s.StadiumSectorOverlayId == overlay.Id);
                if (hasSeats)
                    continue;

                var seats = BuildSeatsForOverlay(overlay);
                if (seats.Count == 0)
                    continue;

                _context.StadiumSeatsNew.AddRange(seats);
                await _context.SaveChangesAsync();

                sectorsTouched++;
                seatsGenerated += seats.Count;
            }

            _logger.LogInformation("Backfill generated {Seats} seats across {Sectors} overlay sectors", seatsGenerated, sectorsTouched);
            return Ok(new { sectorsTouched, seatsGenerated });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating missing overlay seats");
            return StatusCode(500, "Error generating missing overlay seats");
        }
    }

    /// <summary>
    /// Fields that determine the seat layout of an overlay. When any of these change on update,
    /// the sector's seats are regenerated.
    /// </summary>
    private static bool SeatingLayoutChanged(StadiumSectorOverlay before, StadiumSectorOverlay after) =>
        before.SectorCode != after.SectorCode ||
        before.Rows != after.Rows ||
        before.SeatsPerRow != after.SeatsPerRow ||
        before.UseVariableSeating != after.UseVariableSeating ||
        before.VariableSeatingData != after.VariableSeatingData;

    /// <summary>
    /// Builds the in-memory seat records for an overlay from its (uniform or variable) row layout.
    /// Each seat gets a unique code of the form "{SectorCode}-R{row}-S{seat}".
    /// </summary>
    private static List<StadiumSeatNew> BuildSeatsForOverlay(StadiumSectorOverlay overlay)
    {
        var now = DateTime.UtcNow;
        var seats = new List<StadiumSeatNew>();

        foreach (var (row, seatsInRow) in EnumerateRows(overlay))
        {
            for (var seat = 1; seat <= seatsInRow; seat++)
            {
                seats.Add(new StadiumSeatNew
                {
                    StadiumSectorOverlayId = overlay.Id,
                    SectorId = null,
                    RowNumber = row,
                    SeatNumber = seat,
                    UniqueCode = $"{overlay.SectorCode}-R{row}-S{seat}",
                    IsAvailable = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }

        return seats;
    }

    /// <summary>
    /// Yields (rowNumber, seatsInThatRow) for an overlay, honoring variable-seating patterns when
    /// configured, otherwise a uniform Rows x SeatsPerRow grid (rows and seats are 1-based).
    /// </summary>
    private static IEnumerable<(int Row, int Seats)> EnumerateRows(StadiumSectorOverlay overlay)
    {
        if (overlay.UseVariableSeating && !string.IsNullOrEmpty(overlay.VariableSeatingData))
        {
            List<RowPattern>? patterns = null;
            try
            {
                patterns = JsonSerializer.Deserialize<List<RowPattern>>(
                    overlay.VariableSeatingData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                patterns = null;
            }

            if (patterns is { Count: > 0 })
            {
                foreach (var pattern in patterns.OrderBy(p => p.FromRow))
                {
                    for (var row = pattern.FromRow; row <= pattern.ToRow; row++)
                    {
                        yield return (row, pattern.SeatsPerRow);
                    }
                }
                yield break;
            }
        }

        for (var row = 1; row <= overlay.Rows; row++)
        {
            yield return (row, overlay.SeatsPerRow);
        }
    }

    /// <summary>
    /// Replaces all persisted seats for an overlay with a freshly generated set. Delete and insert
    /// are saved separately so the unique code index never sees old and new rows at once.
    /// </summary>
    private async Task RegenerateOverlaySeatsAsync(StadiumSectorOverlay overlay)
    {
        var existing = await _context.StadiumSeatsNew
            .Where(s => s.StadiumSectorOverlayId == overlay.Id)
            .ToListAsync();

        if (existing.Count > 0)
        {
            _context.StadiumSeatsNew.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        var seats = BuildSeatsForOverlay(overlay);
        if (seats.Count > 0)
        {
            _context.StadiumSeatsNew.AddRange(seats);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Create a new sector overlay (and generate its seats)
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

            // Generate the actual seats for this sector (drawing tool is the source of truth)
            await RegenerateOverlaySeatsAsync(sector);

            _logger.LogInformation("Created sector overlay: {SectorCode} with generated seats", sector.SectorCode);

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

            // Snapshot the layout-defining fields before applying the update so we can decide
            // whether the seats need to be regenerated.
            var before = new StadiumSectorOverlay
            {
                SectorCode = existingSector.SectorCode,
                Rows = existingSector.Rows,
                SeatsPerRow = existingSector.SeatsPerRow,
                UseVariableSeating = existingSector.UseVariableSeating,
                VariableSeatingData = existingSector.VariableSeatingData
            };

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

            // Regenerate seats only when the layout changed, or when the sector has none yet
            // (avoids wiping seats on a simple move/recolor).
            var hasSeats = await _context.StadiumSeatsNew.AnyAsync(s => s.StadiumSectorOverlayId == existingSector.Id);
            if (SeatingLayoutChanged(before, existingSector) || !hasSeats)
            {
                await RegenerateOverlaySeatsAsync(existingSector);
            }

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

            // Remove the sector's generated seats (they have no meaning without the sector)
            var seats = await _context.StadiumSeatsNew
                .Where(s => s.StadiumSectorOverlayId == sector.Id)
                .ToListAsync();
            if (seats.Count > 0)
            {
                _context.StadiumSeatsNew.RemoveRange(seats);
            }

            // Soft delete
            sector.IsDeleted = true;
            sector.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted sector overlay: {SectorCode} and its {Count} seats", sector.SectorCode, seats.Count);

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
            // Overlays whose seats must be (re)generated after the batch is saved.
            var needsRegen = new List<StadiumSectorOverlay>();

            foreach (var sector in sectors)
            {
                var existing = await _context.StadiumSectorOverlays
                    .FirstOrDefaultAsync(s => s.SectorCode == sector.SectorCode && !s.IsDeleted);

                if (existing != null)
                {
                    var before = new StadiumSectorOverlay
                    {
                        SectorCode = existing.SectorCode,
                        Rows = existing.Rows,
                        SeatsPerRow = existing.SeatsPerRow,
                        UseVariableSeating = existing.UseVariableSeating,
                        VariableSeatingData = existing.VariableSeatingData
                    };

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

                    if (SeatingLayoutChanged(before, existing))
                    {
                        needsRegen.Add(existing);
                    }

                    results.Add(existing);
                }
                else
                {
                    // Create new
                    sector.CreatedDate = DateTime.UtcNow;
                    sector.ModifiedDate = DateTime.UtcNow;
                    sector.IsDeleted = false;

                    _context.StadiumSectorOverlays.Add(sector);
                    needsRegen.Add(sector);
                    results.Add(sector);
                }
            }

            await _context.SaveChangesAsync();

            // Ids are assigned after save; (re)generate seats for new/changed sectors.
            foreach (var overlay in needsRegen)
            {
                await RegenerateOverlaySeatsAsync(overlay);
            }

            _logger.LogInformation("Bulk upserted {Count} sector overlays ({Regen} with seat (re)generation)", sectors.Count, needsRegen.Count);

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

            // Remove all overlay-owned generated seats.
            var overlaySeats = await _context.StadiumSeatsNew
                .Where(s => s.StadiumSectorOverlayId != null)
                .ToListAsync();
            if (overlaySeats.Count > 0)
            {
                _context.StadiumSeatsNew.RemoveRange(overlaySeats);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Cleared all sector overlays ({Count} sectors) and {Seats} seats", sectors.Count, overlaySeats.Count);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing sector overlays");
            return StatusCode(500, "Error clearing sector overlays");
        }
    }
}
