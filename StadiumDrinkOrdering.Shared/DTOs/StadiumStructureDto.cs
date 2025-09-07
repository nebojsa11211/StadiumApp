using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

// Import DTOs
public class StadiumImportDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TribuneImportDto> Tribunes { get; set; } = new();
}

public class TribuneImportDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RingImportDto> Rings { get; set; } = new();
}

public class RingImportDto
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SectorImportDto> Sectors { get; set; } = new();
}

public class SectorImportDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Rows { get; set; }
    public int SeatsPerRow { get; set; }
    public int StartRow { get; set; } = 1;
    public int StartSeat { get; set; } = 1;
}

// Summary DTO
public class StadiumSummaryDto
{
    public int TotalTribunes { get; set; }
    public int TotalRings { get; set; }
    public int TotalSectors { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int OccupiedSeats { get; set; }
}

// Validation Result
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}