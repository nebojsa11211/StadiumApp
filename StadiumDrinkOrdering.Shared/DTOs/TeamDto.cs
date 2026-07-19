using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// A visiting team as returned to clients. The crest bytes are never inlined here — a directory of
/// a few hundred teams would be tens of megabytes of base64 on every list call — so callers render
/// the logo from <c>GET api/teams/{id}/logo</c> and use <see cref="HasLogo"/> to know whether to.
/// </summary>
public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public int? FoundedYear { get; set; }
    public string? City { get; set; }
    public string? Website { get; set; }
    public bool HasLogo { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Create/update payload for a team. The crest is uploaded separately as multipart.</summary>
public class TeamUpsertDto
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? ShortName { get; set; }

    [StringLength(16)]
    public string? PrimaryColor { get; set; }

    [StringLength(16)]
    public string? SecondaryColor { get; set; }

    [Range(1800, 2200)]
    public int? FoundedYear { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }
}
