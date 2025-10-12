using Microsoft.Extensions.Caching.Memory;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Stadium layout service with caching and fallback mechanisms
/// </summary>
public class StadiumLayoutService : IStadiumLayoutService
{
    private readonly ApplicationDbContext _context;
    private readonly IStadiumLayoutGenerator _layoutGenerator;
    private readonly IMemoryCache _cache;
    private readonly ILogger<StadiumLayoutService> _logger;

    public StadiumLayoutService(
        ApplicationDbContext context,
        IStadiumLayoutGenerator layoutGenerator,
        IMemoryCache cache,
        ILogger<StadiumLayoutService> logger)
    {
        _context = context;
        _layoutGenerator = layoutGenerator;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Get the current stadium SVG layout (with caching)
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GetStadiumLayoutAsync()
    {
        _logger.LogInformation("GetStadiumLayoutAsync called - using memory cache");

        return await _cache.GetOrCreateAsync("stadium-svg-layout-v4-optimized", async entry =>
        {
            _logger.LogInformation("Cache miss - generating stadium SVG layout");
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

            try
            {
                _logger.LogInformation("Generating stadium SVG layout from database");

                // Try to get tribune structure from database
                var tribunes = await GetTribunesFromDatabaseAsync();

                if (tribunes.Any())
                {
                    _logger.LogInformation("Found {TribuneCount} tribunes in database, generating dynamic layout", tribunes.Count);
                    return await _layoutGenerator.GenerateLayoutAsync(tribunes);
                }
                else
                {
                    _logger.LogWarning("No tribune data found in database, generating default HNK Rijeka layout");
                    return await GenerateHNKRijekaLayoutAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating stadium layout, falling back to default HNK Rijeka layout");
                return await GenerateHNKRijekaLayoutAsync();
            }
        });
    }

    /// <summary>
    /// Generate HNK Rijeka layout specifically
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GenerateHNKRijekaLayoutAsync()
    {
        try
        {
            // Try to get real tribune data, but generate with empty list if not available
            var tribunes = await GetTribunesFromDatabaseAsync();
            return await _layoutGenerator.GenerateHNKRijekaLayoutAsync(tribunes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating HNK Rijeka layout with database data, using empty data");
            // Fallback to generating with empty tribune list
            return await _layoutGenerator.GenerateHNKRijekaLayoutAsync(new List<Tribune>());
        }
    }

    /// <summary>
    /// Get stadium layout with event seat status
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GetStadiumLayoutWithEventDataAsync(int eventId)
    {
        var layout = await GetStadiumLayoutAsync();
        
        try
        {
            // Get event seat status data
            var eventSeatStatus = await GetEventSeatStatusAsync(eventId);
            
            // Update sector occupancy data
            foreach (var stand in layout.Stands)
            {
                foreach (var sector in stand.Sectors)
                {
                    if (eventSeatStatus.TryGetValue(sector.Code, out var status))
                    {
                        sector.OccupiedSeats = status.OccupiedSeats;
                        // Don't overwrite TotalSeats from layout - use database value which is more accurate
                    }
                }
            }
            
            _logger.LogInformation("Updated layout with event {EventId} seat status for {SectorCount} sectors", 
                eventId, eventSeatStatus.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading event seat status for event {EventId}", eventId);
            // Return layout without event data rather than failing
        }

        return layout;
    }

    /// <summary>
    /// Refresh the layout cache
    /// </summary>
    public async Task RefreshLayoutCacheAsync()
    {
        // Force clear all possible cache entries (including old versions)
        _cache.Remove("stadium-svg-layout");
        _cache.Remove("stadium-svg-layout-v2");
        _cache.Remove("stadium-svg-layout-v3");
        _cache.Remove("stadium-svg-layout-v4-optimized");
        _logger.LogInformation("Stadium layout cache cleared (all versions)");

        // Check current database state before regenerating
        var hasData = await HasStadiumDataAsync();
        _logger.LogInformation("Database has stadium data: {HasData}", hasData);

        // DEBUG: Direct database query to see what's actually in Tribunes table (OPTIMIZED)
        try
        {
            var allTribunes = await _context.Tribunes
                .AsNoTracking()
                .Include(t => t.Rings)
                    .ThenInclude(r => r.Sectors)
                .ToListAsync();
            _logger.LogInformation("DIRECT DATABASE QUERY (OPTIMIZED) - Found {Count} total tribunes", allTribunes.Count);
            foreach (var tribune in allTribunes)
            {
                var ringCount = tribune.Rings?.Count ?? 0;
                var sectorCount = tribune.Rings?.SelectMany(r => r.Sectors)?.Count() ?? 0;
                var calculatedSeats = tribune.Rings?.SelectMany(r => r.Sectors)?.Sum(s => s.TotalRows * s.SeatsPerRow) ?? 0;
                _logger.LogInformation("DIRECT DATABASE QUERY - Tribune: Code={Code}, Name={Name}, Rings={RingCount}, Sectors={SectorCount}, CalculatedSeats={CalculatedSeats}",
                    tribune.Code, tribune.Name, ringCount, sectorCount, calculatedSeats);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in direct database query");
        }

        if (hasData)
        {
            var tribunes = await GetTribunesFromDatabaseAsync();
            _logger.LogInformation("Found {TribuneCount} tribunes with sector codes: {SectorCodes}",
                tribunes.Count,
                string.Join(", ", tribunes.SelectMany(t => t.Rings).SelectMany(r => r.Sectors).Select(s => s.Code)));
        }

        // Pre-generate the layout
        var layout = await GetStadiumLayoutAsync();
        _logger.LogInformation("Stadium layout cache refreshed with {StandCount} stands and sectors: {SectorCodes}",
            layout.Stands.Count,
            string.Join(", ", layout.Stands.SelectMany(s => s.Sectors).Select(sec => sec.Code)));
    }

    /// <summary>
    /// Check if stadium structure data is available
    /// </summary>
    public async Task<bool> HasStadiumDataAsync()
    {
        try
        {
            // Check Tribunes table (where imported data is actually stored)
            var tribuneCount = await _context.Tribunes.CountAsync();
            _logger.LogInformation("Found {TribuneCount} tribunes in database", tribuneCount);
            return tribuneCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking stadium data availability");
            return false;
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Get tribunes from database with full hierarchy (from imported Tribune/Ring/Sector structure)
    /// OPTIMIZED: No seat loading - uses calculated counts from sector metadata
    /// </summary>
    private async Task<List<Tribune>> GetTribunesFromDatabaseAsync()
    {
        try
        {
            _logger.LogInformation("Loading tribune structure from database (optimized - no seat loading)");

            // PERFORMANCE OPTIMIZATION: Remove .ThenInclude(s => s.Seats) to avoid loading 80,000+ seat records
            // We only need seat COUNTS, which we calculate from sector metadata (TotalRows × SeatsPerRow)
            var tribunes = await _context.Tribunes
                .AsNoTracking()  // Read-only query - no change tracking overhead
                .Include(t => t.Rings)
                    .ThenInclude(r => r.Sectors)
                // REMOVED: .ThenInclude(s => s.Seats) - Don't load individual seats for overview!
                .OrderBy(t => t.Code)
                .ToListAsync();

            if (!tribunes.Any())
            {
                _logger.LogWarning("No tribunes found in database - stadium structure not imported");
                return new List<Tribune>();
            }

            _logger.LogInformation("Found {TribuneCount} tribunes from database", tribunes.Count);

            // Debug: Log all tribune and sector details using CALCULATED seat counts
            foreach (var tribune in tribunes)
            {
                var sectorCodes = tribune.Rings?.SelectMany(r => r.Sectors)?.Select(s => s.Code) ?? Enumerable.Empty<string>();
                // OPTIMIZED: Calculate seat count from metadata instead of counting actual seat records
                var calculatedSeatCount = tribune.Rings?.SelectMany(r => r.Sectors)?.Sum(s => s.TotalRows * s.SeatsPerRow) ?? 0;

                _logger.LogInformation("Tribune {Code} ({Name}): {RingCount} rings, sectors: [{SectorCodes}], {CalculatedSeats} seats (calculated)",
                    tribune.Code, tribune.Name, tribune.Rings?.Count ?? 0,
                    string.Join(", ", sectorCodes), calculatedSeatCount);
            }

            var totalSectors = tribunes.SelectMany(t => t.Rings).SelectMany(r => r.Sectors).Count();
            // OPTIMIZED: Calculate total seats from sector metadata (TotalRows × SeatsPerRow)
            var totalCalculatedSeats = tribunes.SelectMany(t => t.Rings).SelectMany(r => r.Sectors).Sum(s => s.TotalRows * s.SeatsPerRow);

            _logger.LogInformation("Loaded tribune structure (OPTIMIZED): {TribuneCount} tribunes, {SectorCount} total sectors, {CalculatedSeats} calculated seats",
                tribunes.Count, totalSectors, totalCalculatedSeats);

            return tribunes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tribune structure from database");
            return new List<Tribune>();
        }
    }


    /// <summary>
    /// Get event seat status data for occupancy calculation
    /// </summary>
    private async Task<Dictionary<string, SvgEventSeatStatusDto>> GetEventSeatStatusAsync(int eventId)
    {
        try
        {
            // This would integrate with the ticketing system to get real occupancy data
            // For now, return empty dictionary - future enhancement
            
            // Example implementation:
            // var tickets = await _context.Tickets
            //     .Where(t => t.EventId == eventId && t.IsActive)
            //     .GroupBy(t => t.SectorCode)
            //     .Select(g => new { SectorCode = g.Key, Count = g.Count() })
            //     .ToListAsync();
            
            var result = new Dictionary<string, SvgEventSeatStatusDto>();
            
            _logger.LogDebug("Event seat status query not yet implemented for event {EventId}", eventId);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading event seat status for event {EventId}", eventId);
            return new Dictionary<string, SvgEventSeatStatusDto>();
        }
    }

    #endregion
}