using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Service for accessing dynamic stadium SVG layouts
/// </summary>
public interface IStadiumSvgService
{
    /// <summary>
    /// Get the current stadium SVG layout
    /// </summary>
    Task<StadiumSvgLayoutDto?> GetStadiumLayoutAsync();
    
    /// <summary>
    /// Get stadium layout with event-specific occupancy data
    /// </summary>
    Task<StadiumSvgLayoutDto?> GetStadiumLayoutWithEventDataAsync(int eventId);
    
    /// <summary>
    /// Generate HNK Rijeka layout specifically
    /// </summary>
    Task<StadiumSvgLayoutDto?> GetHNKRijekaLayoutAsync();
    
    /// <summary>
    /// Refresh the stadium layout cache
    /// </summary>
    Task<bool> RefreshStadiumLayoutAsync();
    
    /// <summary>
    /// Check if stadium data is available
    /// </summary>
    Task<bool> HasStadiumDataAsync();
}