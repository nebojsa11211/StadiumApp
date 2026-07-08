using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>Read model for the venue branding profile plus its resident clubs.</summary>
public class VenueDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? OfficialCapacity { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Website { get; set; }

    /// <summary>True when a stadium photo is stored; fetch it from the photo endpoint.</summary>
    public bool HasPhoto { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public List<ClubDto> Clubs { get; set; } = new();
}

/// <summary>Write model for the venue's own (non-club) fields.</summary>
public class UpdateVenueDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? AddressLine1 { get; set; }
    [StringLength(200)]
    public string? AddressLine2 { get; set; }
    [StringLength(100)]
    public string? City { get; set; }
    [StringLength(20)]
    public string? PostalCode { get; set; }
    [StringLength(100)]
    public string? Country { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? OfficialCapacity { get; set; }

    [StringLength(200)]
    public string? ContactEmail { get; set; }
    [StringLength(50)]
    public string? ContactPhone { get; set; }
    [StringLength(300)]
    public string? Website { get; set; }
}

/// <summary>Read model for a resident club.</summary>
public class ClubDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public int? FoundedYear { get; set; }
    public string? Website { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }

    /// <summary>True when a logo is stored; fetch it from the club logo endpoint.</summary>
    public bool HasLogo { get; set; }
}

/// <summary>Write model for creating/updating a resident club.</summary>
public class ClubUpsertDto
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? ShortName { get; set; }
    [StringLength(16)]
    public string? PrimaryColor { get; set; }
    [StringLength(16)]
    public string? SecondaryColor { get; set; }
    public int? FoundedYear { get; set; }
    [StringLength(300)]
    public string? Website { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
}
