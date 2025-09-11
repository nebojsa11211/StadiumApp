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
        return await _cache.GetOrCreateAsync("stadium-svg-layout", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            
            try
            {
                _logger.LogInformation("Generating stadium SVG layout");
                
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
        _cache.Remove("stadium-svg-layout");
        _logger.LogInformation("Stadium layout cache cleared");
        
        // Pre-generate the layout
        await GetStadiumLayoutAsync();
        _logger.LogInformation("Stadium layout cache refreshed");
    }

    /// <summary>
    /// Check if stadium structure data is available
    /// </summary>
    public async Task<bool> HasStadiumDataAsync()
    {
        try
        {
            var tribuneCount = await _context.Tribunes.CountAsync();
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
    /// Get tribunes from database with full hierarchy
    /// </summary>
    private async Task<List<Tribune>> GetTribunesFromDatabaseAsync()
    {
        try
        {
            return await _context.Tribunes
                .Include(t => t.Rings)
                    .ThenInclude(r => r.Sectors)
                        .ThenInclude(s => s.Seats)
                .OrderBy(t => t.Code)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tribunes from database");
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