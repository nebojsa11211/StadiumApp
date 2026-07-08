using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// The physical stadium / venue identity for the deployment. This is effectively a singleton
/// (one venue per installation) and gives meaning to the pre-existing <see cref="Event.VenueId"/>
/// column. It carries the branding surfaced to customers (stadium name, address, photo, contacts)
/// and owns the collection of <see cref="Club"/>s that are resident at the stadium.
/// </summary>
public class Venue
{
    public int Id { get; set; }

    /// <summary>Display name of the stadium (e.g. "Stadion Maksimir").</summary>
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

    /// <summary>Optional geo coordinates for map links / directions.</summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>
    /// Officially stated capacity. Optional: the true seat count is computed from the stadium
    /// structure, but the official figure can legitimately differ (standing areas, closed sections),
    /// so it is stored separately rather than derived.
    /// </summary>
    public int? OfficialCapacity { get; set; }

    [StringLength(200)]
    public string? ContactEmail { get; set; }

    [StringLength(50)]
    public string? ContactPhone { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    /// <summary>Optional stadium photo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? Photo { get; set; }

    [StringLength(100)]
    public string? PhotoContentType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Email of the admin who last saved the profile (audit).</summary>
    [StringLength(200)]
    public string? UpdatedBy { get; set; }

    // Navigation
    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
}

/// <summary>
/// A club/team resident at the <see cref="Venue"/>. A stadium can host more than one (e.g. two
/// teams sharing a ground), hence a collection rather than fields on the venue. This is the club
/// <em>identity</em> (logo, colours, founding) and is distinct from the free-text
/// <see cref="Event.HomeTeam"/>/<see cref="Event.AwayTeam"/> labels used per fixture.
/// </summary>
public class Club
{
    public int Id { get; set; }

    public int VenueId { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Abbreviation shown where space is tight (e.g. "GNK").</summary>
    [StringLength(50)]
    public string? ShortName { get; set; }

    /// <summary>Club crest/logo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? Logo { get; set; }

    [StringLength(100)]
    public string? LogoContentType { get; set; }

    /// <summary>Brand colours as hex (e.g. "#1d4ed8"). Stored now; app theming is a later phase.</summary>
    [StringLength(16)]
    public string? PrimaryColor { get; set; }

    [StringLength(16)]
    public string? SecondaryColor { get; set; }

    public int? FoundedYear { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    /// <summary>Marks the principal home club when several share the venue.</summary>
    public bool IsPrimary { get; set; }

    /// <summary>Sort order for display in lists / headers.</summary>
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public virtual Venue? Venue { get; set; }
}
