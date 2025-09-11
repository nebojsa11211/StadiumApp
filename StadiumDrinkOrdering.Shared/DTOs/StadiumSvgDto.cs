using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// DTO for dynamic SVG stadium layout generation
/// Maps Tribune/Ring/Sector structure to visual coordinates
/// </summary>
public class StadiumSvgLayoutDto
{
    public string Name { get; set; } = "HNK Rijeka Stadium";
    public string Description { get; set; } = string.Empty;
    public FieldSvgDto Field { get; set; } = new();
    public List<StandSvgDto> Stands { get; set; } = new();
    public ViewBoxDto ViewBox { get; set; } = new(1200, 900);
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// SVG Stand representation (Tribune)
/// </summary>
public class StandSvgDto
{
    public string Code { get; set; } = string.Empty; // N, S, E, W
    public string Name { get; set; } = string.Empty; // "North Stand", "South Stand"
    public string CssClass { get; set; } = string.Empty; // "north-stand", "south-stand"
    public string DataAttribute { get; set; } = string.Empty; // "north", "south"
    public List<SectorSvgDto> Sectors { get; set; } = new();
    public int TotalSeats => Sectors.Sum(s => s.TotalSeats);
    public int OccupiedSeats => Sectors.Sum(s => s.OccupiedSeats);
}

/// <summary>
/// SVG Sector representation with exact coordinates and styling
/// </summary>
public class SectorSvgDto
{
    public string Code { get; set; } = string.Empty; // I2, I3, S5, Z10, etc.
    public string Name { get; set; } = string.Empty; // Display name
    public RectangleDto Bounds { get; set; } = new(); // X, Y, Width, Height
    public SvgPointDto TextCenter { get; set; } = new(); // Text positioning
    public SectorStyleDto Style { get; set; } = new(); // Colors, etc.
    public int TotalSeats { get; set; } // Total seats in sector
    public int OccupiedSeats { get; set; } = 0; // Occupied seats for occupancy calculation
    public string ElementId => $"sector-{Code}";
    public string AriaLabel => $"Sector {Code}";
    public double OccupancyPercentage => TotalSeats > 0 ? (double)OccupiedSeats / TotalSeats : 0;
}

/// <summary>
/// Rectangle coordinates for SVG rendering
/// </summary>
public class RectangleDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public RectangleDto() { }
    
    public RectangleDto(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}

/// <summary>
/// SVG Point coordinates (integer-based for SVG elements)
/// </summary>
public class SvgPointDto
{
    public int X { get; set; }
    public int Y { get; set; }

    public SvgPointDto() { }
    
    public SvgPointDto(int x, int y)
    {
        X = x;
        Y = y;
    }
}

/// <summary>
/// Sector styling for SVG rendering
/// </summary>
public class SectorStyleDto
{
    public string FillColor { get; set; } = "#4A90E2";
    public string StrokeColor { get; set; } = "#2E5C9A";
    public int StrokeWidth { get; set; } = 2;
    public int BorderRadius { get; set; } = 6;
    public string FontSize { get; set; } = "16px";
    public string FontWeight { get; set; } = "600";
    public string TextColor { get; set; } = "white";
    public string TextShadow { get; set; } = "1px 1px 2px rgba(0,0,0,0.3)";

    public SectorStyleDto() { }
    
    public SectorStyleDto(string fillColor, string strokeColor)
    {
        FillColor = fillColor;
        StrokeColor = strokeColor;
    }
}

/// <summary>
/// Football field SVG representation (stays exactly the same as static)
/// </summary>
public class FieldSvgDto
{
    public RectangleDto Bounds { get; set; } = new(350, 250, 500, 320);
    public string GradientId { get; set; } = "field-gradient";
    public string FilterId { get; set; } = "sector-shadow";
    public string StrokeColor { get; set; } = "#ffffff";
    public int StrokeWidth { get; set; } = 3;
}

/// <summary>
/// SVG ViewBox configuration
/// </summary>
public class ViewBoxDto
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string ViewBoxString => $"0 0 {Width} {Height}";

    public ViewBoxDto() { }
    
    public ViewBoxDto(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

/// <summary>
/// Coordinate mapping configuration for different stadium layouts
/// </summary>
public class SectorCoordinates
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string FillColor { get; set; } = "#4A90E2";
    public string StrokeColor { get; set; } = "#2E5C9A";

    public SvgPointDto TextCenter => new(X + Width / 2, Y + Height / 2);

    public SectorCoordinates() { }
    
    public SectorCoordinates(int x, int y, int width, int height, string fillColor, string strokeColor)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        FillColor = fillColor;
        StrokeColor = strokeColor;
    }
    
    public RectangleDto ToRectangle() => new(X, Y, Width, Height);
    public SectorStyleDto ToStyle() => new(FillColor, StrokeColor);
}

/// <summary>
/// SVG Event seat status for real-time occupancy display
/// </summary>
public class SvgEventSeatStatusDto
{
    public string SectorCode { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int OccupiedSeats { get; set; }
    public int AvailableSeats => TotalSeats - OccupiedSeats;
    public double OccupancyPercentage => TotalSeats > 0 ? (double)OccupiedSeats / TotalSeats : 0;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}