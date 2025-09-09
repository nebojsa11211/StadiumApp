using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class StadiumViewerDto
{
    public string StadiumId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public StadiumFieldDto Field { get; set; } = new();
    public List<StadiumStandDto> Stands { get; set; } = new();
    public StadiumCoordinateSystem CoordinateSystem { get; set; } = new();
}

public class StadiumFieldDto
{
    public List<PointDto> Polygon { get; set; } = new();
    public string FillColor { get; set; } = "#4a9d4a";
    public string StrokeColor { get; set; } = "#ffffff";
}

public class StadiumStandDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TribuneCode { get; set; } = string.Empty; // N, S, E, W
    public List<StadiumSectorDto> Sectors { get; set; } = new();
    public List<PointDto> Polygon { get; set; } = new();
}

public class StadiumSectorDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string StandId { get; set; } = string.Empty;
    public List<PointDto> Polygon { get; set; } = new();
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal OccupancyPercentage { get; set; }
    public string FillColor { get; set; } = "#e3e3e3";
    public string HoverColor { get; set; } = "#d0d0d0";
    public decimal PriceMultiplier { get; set; } = 1.0m;
    public List<StadiumRowDto>? Rows { get; set; } // Loaded on demand
}

public class StadiumRowDto
{
    public int Index { get; set; }
    public int RowNumber { get; set; }
    public List<ViewerSeatDto> Seats { get; set; } = new();
}

public class ViewerSeatDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // e.g., "N1A-R1S1"
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; } = 1.2;
    public string Status { get; set; } = "free"; // free, sold, held, blocked
    public bool IsAccessible { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
}

public class PointDto
{
    public double X { get; set; }
    public double Y { get; set; }

    public PointDto() { }
    
    public PointDto(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public class StadiumCoordinateSystem
{
    public double Width { get; set; } = 1000;  // Stadium width in coordinate units
    public double Height { get; set; } = 800;  // Stadium height in coordinate units
    public double OriginX { get; set; } = 0;   // Origin X coordinate
    public double OriginY { get; set; } = 0;   // Origin Y coordinate
    public string Unit { get; set; } = "normalized"; // "normalized", "meters", "pixels"
}

public class EventSeatStatusDto
{
    public int EventId { get; set; }
    public Dictionary<string, SeatStatusSummaryDto> SectorSummaries { get; set; } = new();
    public List<ViewerSeatStatusDto>? SeatStatuses { get; set; } // Loaded on demand for specific sectors
}

public class SeatStatusSummaryDto
{
    public string SectorId { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int SoldSeats { get; set; }
    public int HeldSeats { get; set; }
    public int BlockedSeats { get; set; }
    public int FreeSeats { get; set; }
    public decimal OccupancyPercentage { get; set; }
}

public class ViewerSeatStatusDto
{
    public string SeatId { get; set; } = string.Empty;
    public string Status { get; set; } = "free"; // free, sold, held, blocked
    public DateTime? ReservedUntil { get; set; }
}

public class SeatSelectionRequestDto
{
    public string SectorId { get; set; } = string.Empty;
    public List<string> SeatIds { get; set; } = new();
    public int? EventId { get; set; }
    public string SessionId { get; set; } = string.Empty;
}

public class StadiumViewerOptionsDto
{
    public bool ShowSeatNumbers { get; set; } = false;
    public bool ShowOccupancy { get; set; } = true;
    public string RenderMode { get; set; } = "auto"; // "svg", "canvas", "auto"
    public int DotModeThreshold { get; set; } = 15000; // Switch to dots when seats > threshold
    public bool EnableAnimations { get; set; } = true;
    public bool ShowMinimap { get; set; } = true;
    public string ColorScheme { get; set; } = "default"; // "default", "colorblind", "high-contrast"
}

public class SearchSeatRequestDto
{
    public string SeatCode { get; set; } = string.Empty;
}

public class SearchSeatResultDto
{
    public string SeatId { get; set; } = string.Empty;
    public string SectorId { get; set; } = string.Empty;
    public string SectorName { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool IsAccessible { get; set; }
}