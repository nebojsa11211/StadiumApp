using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A team the venue plays host to but does not house — the visiting side of a fixture — held as a
/// managed directory entry with its crest, so an opponent's badge is uploaded once and reused every
/// season thereafter.
///
/// This supersedes the earlier name-keyed <c>TeamCrest</c> cache, which could only ever be created
/// as a side effect of building a fixture and offered no way to correct a crest or a misspelt name.
/// A <see cref="Team"/> is editable in its own right (see the admin Teams page) while keeping the
/// same normalized-name lookup, so a fixture typed against a known opponent still pre-fills.
///
/// Distinct from <see cref="Club"/>, which models the clubs <em>resident</em> at the venue and owns
/// venue-level concerns (primary club, display order, branding colours used for theming).
/// </summary>
public class Team
{
    public int Id { get; set; }

    /// <summary>Display name as an admin entered it (e.g. "NK Osijek").</summary>
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// <see cref="Name"/> lower-cased and trimmed. Unique, and the key fixtures are matched on, so
    /// "nk osijek" typed on an event finds the same team as "NK Osijek".
    /// </summary>
    [Required]
    [StringLength(150)]
    public string NormalizedName { get; set; } = string.Empty;

    /// <summary>Abbreviation shown where space is tight (e.g. "OSI").</summary>
    [StringLength(50)]
    public string? ShortName { get; set; }

    /// <summary>Crest/logo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? Logo { get; set; }

    [StringLength(100)]
    public string? LogoContentType { get; set; }

    /// <summary>Brand colours as hex (e.g. "#1d4ed8"), for poster and fixture-card accents.</summary>
    [StringLength(16)]
    public string? PrimaryColor { get; set; }

    [StringLength(16)]
    public string? SecondaryColor { get; set; }

    public int? FoundedYear { get; set; }

    /// <summary>Home city/town, to disambiguate similarly named clubs in a long dropdown.</summary>
    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Normalises a team name for storage/lookup. Null/blank yields null (no lookup key).</summary>
    public static string? Normalize(string? name)
        => string.IsNullOrWhiteSpace(name) ? null : name.Trim().ToLowerInvariant();
}
