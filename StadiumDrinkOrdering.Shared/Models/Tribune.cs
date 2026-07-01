using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

public class Tribune
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(1)]
    public string Code { get; set; } = string.Empty; // N, S, E, W
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<Ring> Rings { get; set; } = new List<Ring>();
}

public class Ring
{
    public int Id { get; set; }
    public int TribuneId { get; set; }
    
    [Required]
    public int Number { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Tribune? Tribune { get; set; }
    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();
}

public class Sector
{
    public int Id { get; set; }
    public int RingId { get; set; }
    
    [Required]
    [StringLength(5)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue)]
    public int TotalRows { get; set; }
    
    [Range(1, int.MaxValue)]
    public int SeatsPerRow { get; set; }
    
    public int StartRow { get; set; } = 1;
    public int StartSeat { get; set; } = 1;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual Ring? Ring { get; set; }
    public virtual ICollection<StadiumSeatNew> Seats { get; set; } = new List<StadiumSeatNew>();
}

public class StadiumSeatNew
{
    public int Id { get; set; }

    // A seat belongs to EITHER a hierarchy Sector (legacy JSON import) OR a drawing-tool
    // sector overlay (the source of truth going forward). Both are optional.
    public int? SectorId { get; set; }
    public int? StadiumSectorOverlayId { get; set; }

    [Required]
    public int RowNumber { get; set; }

    [Required]
    public int SeatNumber { get; set; }

    [Required]
    [StringLength(60)]
    public string UniqueCode { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Sector? Sector { get; set; }
    public virtual StadiumSectorOverlay? StadiumSectorOverlay { get; set; }
}