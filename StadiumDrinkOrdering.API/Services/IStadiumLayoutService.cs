using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Service for managing stadium SVG layouts
/// </summary>
public interface IStadiumLayoutService
{
    /// <summary>
    /// Get the current stadium SVG layout (with caching)
    /// </summary>
    Task<StadiumSvgLayoutDto> GetStadiumLayoutAsync();
    
    /// <summary>
    /// Generate HNK Rijeka layout specifically
    /// </summary>
    Task<StadiumSvgLayoutDto> GenerateHNKRijekaLayoutAsync();
    
    /// <summary>
    /// Get stadium layout with event seat status
    /// </summary>
    Task<StadiumSvgLayoutDto> GetStadiumLayoutWithEventDataAsync(int eventId);
    
    /// <summary>
    /// Refresh the layout cache
    /// </summary>
    Task RefreshLayoutCacheAsync();
    
    /// <summary>
    /// Check if stadium structure data is available
    /// </summary>
    Task<bool> HasStadiumDataAsync();
}