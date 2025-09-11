using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.Services;

/// <summary>
/// Interface for generating stadium SVG layouts from Tribune data
/// </summary>
public interface IStadiumLayoutGenerator
{
    /// <summary>
    /// Generate SVG layout from Tribune/Ring/Sector structure
    /// Uses HNK Rijeka coordinates for known sectors, generates generic layout for others
    /// </summary>
    Task<StadiumSvgLayoutDto> GenerateLayoutAsync(List<Tribune> tribunes);
    
    /// <summary>
    /// Generate HNK Rijeka specific layout (exact static SVG replica)
    /// </summary>
    Task<StadiumSvgLayoutDto> GenerateHNKRijekaLayoutAsync(List<Tribune> tribunes);
    
    /// <summary>
    /// Generate generic stadium layout for unknown structures
    /// </summary>
    Task<StadiumSvgLayoutDto> GenerateGenericLayoutAsync(List<Tribune> tribunes, LayoutType layoutType = LayoutType.Oval);
    
    /// <summary>
    /// Check if the provided tribunes match HNK Rijeka layout
    /// </summary>
    bool IsHNKRijekaLayout(List<Tribune> tribunes);
    
    /// <summary>
    /// Validate stadium structure for SVG generation
    /// </summary>
    ValidationResult ValidateStructure(List<Tribune> tribunes);
}

/// <summary>
/// Stadium layout types for generic generation
/// </summary>
public enum LayoutType
{
    /// <summary>HNK Rijeka specific layout (exact coordinates)</summary>
    HNKRijeka,
    
    /// <summary>Oval/circular stadium layout</summary>
    Oval,
    
    /// <summary>Rectangular stadium layout</summary>
    Rectangular,
    
    /// <summary>Horseshoe stadium layout (3 sides)</summary>
    Horseshoe,
    
    /// <summary>Bowl stadium layout (full circle)</summary>
    Bowl
}

/// <summary>
/// Layout generation options
/// </summary>
public class LayoutGenerationOptions
{
    /// <summary>Preferred layout type</summary>
    public LayoutType LayoutType { get; set; } = LayoutType.HNKRijeka;
    
    /// <summary>Use real seat counts from database</summary>
    public bool UseRealSeatCounts { get; set; } = true;
    
    /// <summary>Use default HNK Rijeka seat counts for missing data</summary>
    public bool UseDefaultSeatCounts { get; set; } = true;
    
    /// <summary>Include unavailable East Stand</summary>
    public bool IncludeEastStand { get; set; } = true;
    
    /// <summary>Force HNK Rijeka layout even for different structures</summary>
    public bool ForceHNKRijekaLayout { get; set; } = false;
    
    /// <summary>Cache generated layouts</summary>
    public bool EnableCaching { get; set; } = true;
    
    /// <summary>Cache duration for generated layouts</summary>
    public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(15);
}