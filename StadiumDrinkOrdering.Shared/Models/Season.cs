using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A stadium season, e.g. "2026/2027", grouping the events (matches) played within its
/// date window. Season tickets are sold against a season and grant entry to every event
/// linked to it. Nullable <see cref="ExternalSeasonId"/> lets seasons originate from the
/// external ticketing system while internally-created seasons remain valid.
/// </summary>
public class Season
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty; // e.g. "2026/2027"

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// The season used as the default selection in UIs. At most one season should be current;
    /// setting a new current season clears the flag on the others.
    /// </summary>
    public bool IsCurrent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifier of this season in the external ticketing system, if it originated there.
    /// Unique (nullable) so external seasons map 1:1 and ingestion is idempotent.
    /// </summary>
    [StringLength(100)]
    public string? ExternalSeasonId { get; set; }

    /// <summary>Name of the system that created this season (e.g. "TicketingSimulator"), if external.</summary>
    [StringLength(100)]
    public string? SourceSystem { get; set; }

    // Navigation properties
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    public virtual ICollection<SeasonTicket> SeasonTickets { get; set; } = new List<SeasonTicket>();
}
