using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Shared.Constants;

/// <summary>
/// HNK Rijeka Stadium exact coordinate constants
/// These coordinates are copied EXACTLY from the static SVG to ensure pixel-perfect consistency
/// </summary>
public static class HNKRijekaStadiumConstants
{
    /// <summary>
    /// EXACT sector coordinates from current static SVG - DO NOT MODIFY
    /// Each coordinate matches the static SVG rectangles precisely
    /// </summary>
    public static readonly Dictionary<string, SectorCoordinates> SECTOR_COORDINATES = new()
    {
        // North Stand - I Sectors (EXACT from static SVG lines 148-197)
        ["I4"] = new(400, 120, 120, 100, "#4A90E2", "#2E5C9A"),
        ["I3"] = new(540, 120, 120, 100, "#4A90E2", "#2E5C9A"),
        ["I2"] = new(680, 120, 120, 100, "#4A90E2", "#2E5C9A"),
        
        // West Stand - S Sectors (EXACT from static SVG lines 200-251)
        ["S5"] = new(150, 260, 120, 90, "#F39C12", "#D68910"),
        ["S4"] = new(150, 365, 120, 90, "#F39C12", "#D68910"),
        ["S3"] = new(150, 470, 120, 90, "#F39C12", "#D68910"),
        
        // South Stand - Z Sectors Top Row (EXACT from static SVG lines 278-362)
        ["Z10"] = new(280, 600, 95, 80, "#4A90E2", "#2E5C9A"),
        ["Z8"] = new(385, 600, 95, 80, "#4A90E2", "#2E5C9A"),
        ["Z6A"] = new(490, 600, 95, 80, "#4A90E2", "#2E5C9A"),
        ["Z4"] = new(595, 600, 95, 80, "#4A90E2", "#2E5C9A"),
        ["Z2"] = new(700, 600, 95, 80, "#4A90E2", "#2E5C9A"),
        
        // South Stand - Z Sectors Bottom Row (EXACT from static SVG lines 365-414)
        ["Z9"] = new(350, 700, 100, 75, "#4A90E2", "#2E5C9A"),
        ["Z5A"] = new(470, 700, 100, 75, "#4A90E2", "#2E5C9A"),
        ["Z3"] = new(590, 700, 100, 75, "#4A90E2", "#2E5C9A"),
    };
    
    /// <summary>
    /// Football field coordinates (EXACT from static SVG lines 104-142)
    /// </summary>
    public static readonly FieldCoordinates FIELD = new(350, 250, 500, 320);
    
    /// <summary>
    /// East Stand unavailable areas (EXACT from static SVG lines 254-273)
    /// These are rendered as gray rectangles in the static version
    /// </summary>
    public static readonly RectangleDto[] EAST_UNAVAILABLE = 
    {
        new(930, 280, 120, 80),
        new(930, 380, 120, 80),
        new(930, 480, 120, 80)
    };
    
    /// <summary>
    /// Stadium name and metadata
    /// </summary>
    public static readonly string STADIUM_NAME = "HNK Rijeka Stadium";
    public static readonly string STADIUM_DESCRIPTION = "HNK Rijeka Stadium with 14 sectors including North (I2, I3, I4), West (S3, S4, S5), South (Z2, Z3, Z4, Z5A, Z6A, Z8, Z9, Z10), and East stands. Professional football stadium layout.";
    
    /// <summary>
    /// ViewBox dimensions (EXACT from static SVG)
    /// </summary>
    public static readonly ViewBoxDto VIEW_BOX = new(1200, 900);
    
    /// <summary>
    /// Tribune CSS class mappings
    /// </summary>
    public static readonly Dictionary<string, string> TRIBUNE_CSS_CLASSES = new()
    {
        ["N"] = "north-stand",
        ["S"] = "south-stand", 
        ["E"] = "east-stand",
        ["W"] = "west-stand"
    };
    
    /// <summary>
    /// Tribune data attribute mappings
    /// </summary>
    public static readonly Dictionary<string, string> TRIBUNE_DATA_ATTRIBUTES = new()
    {
        ["N"] = "north",
        ["S"] = "south",
        ["E"] = "east", 
        ["W"] = "west"
    };
    
    /// <summary>
    /// Default seat counts for HNK Rijeka sectors
    /// These are reasonable estimates based on sector size
    /// </summary>
    public static readonly Dictionary<string, int> DEFAULT_SEAT_COUNTS = new()
    {
        // North Stand - larger sectors
        ["I4"] = 480, // 20 rows × 24 seats
        ["I3"] = 480, // 20 rows × 24 seats  
        ["I2"] = 480, // 20 rows × 24 seats
        
        // West Stand - medium sectors
        ["S5"] = 360, // 18 rows × 20 seats
        ["S4"] = 360, // 18 rows × 20 seats
        ["S3"] = 360, // 18 rows × 20 seats
        
        // South Stand - varied sizes
        ["Z10"] = 304, // 16 rows × 19 seats
        ["Z8"] = 304,  // 16 rows × 19 seats
        ["Z6A"] = 304, // 16 rows × 19 seats
        ["Z4"] = 304,  // 16 rows × 19 seats
        ["Z2"] = 304,  // 16 rows × 19 seats
        ["Z9"] = 300,  // 15 rows × 20 seats
        ["Z5A"] = 300, // 15 rows × 20 seats
        ["Z3"] = 300,  // 15 rows × 20 seats
    };
    
    /// <summary>
    /// Total stadium capacity (sum of all sectors)
    /// </summary>
    public static readonly int TOTAL_CAPACITY = DEFAULT_SEAT_COUNTS.Values.Sum();
    
    /// <summary>
    /// Check if a sector code exists in HNK Rijeka layout
    /// </summary>
    public static bool HasSector(string sectorCode) => SECTOR_COORDINATES.ContainsKey(sectorCode);
    
    /// <summary>
    /// Get sector coordinates for HNK Rijeka layout
    /// </summary>
    public static SectorCoordinates? GetSectorCoordinates(string sectorCode) 
        => SECTOR_COORDINATES.TryGetValue(sectorCode, out var coords) ? coords : null;
    
    /// <summary>
    /// Get default seat count for HNK Rijeka sector
    /// </summary>
    public static int GetDefaultSeatCount(string sectorCode) 
        => DEFAULT_SEAT_COUNTS.TryGetValue(sectorCode, out var count) ? count : 200; // fallback
}

/// <summary>
/// Field coordinates helper
/// </summary>
public record FieldCoordinates(int X, int Y, int Width, int Height)
{
    public RectangleDto ToRectangle() => new(X, Y, Width, Height);
}