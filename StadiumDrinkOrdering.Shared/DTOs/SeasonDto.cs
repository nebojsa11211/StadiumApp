using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class SeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>Number of events linked to this season.</summary>
    public int EventCount { get; set; }
    /// <summary>Number of active season tickets (passes) sold for this season.</summary>
    public int SeasonTicketCount { get; set; }
    /// <summary>Origin system if the season was ingested externally (else null).</summary>
    public string? SourceSystem { get; set; }
}

/// <summary>A single season pass (annual ticket) with its fixed seat, for the admin drill-down.</summary>
public class SeasonTicketDto
{
    public int Id { get; set; }
    public string SeasonTicketNumber { get; set; } = string.Empty;
    public string? HolderName { get; set; }
    public string? HolderEmail { get; set; }
    public string SectionCode { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string? SourceSystem { get; set; }
}

public class CreateSeasonDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool IsCurrent { get; set; }
}

public class UpdateSeasonDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsCurrent { get; set; }
}
