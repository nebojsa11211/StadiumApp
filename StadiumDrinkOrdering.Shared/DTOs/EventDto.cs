using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>The kind of event (e.g. "Match", "Concert", "Other").</summary>
    public string EventType { get; set; } = "Match";
    /// <summary>Home side of a "Match" fixture (a resident club's name); null for non-match events.</summary>
    public string? HomeTeam { get; set; }
    /// <summary>Away/visiting side of a "Match" fixture; null for non-match events.</summary>
    public string? AwayTeam { get; set; }
    public DateTime? Date { get; set; }
    /// <summary>End of the event window, if set.</summary>
    public DateTime? EndDate { get; set; }
    /// <summary>Start of the ticket-sales window, if set (null = opens as soon as the event is on sale).</summary>
    public DateTime? TicketSalesStartDate { get; set; }
    /// <summary>End of the ticket-sales window, if set (null = stays open while the event is on sale).</summary>
    public DateTime? TicketSalesEndDate { get; set; }
    /// <summary>Start of the drink-ordering window, if set (null = opens as soon as the event goes live).</summary>
    public DateTime? DrinkSalesStartDate { get; set; }
    /// <summary>End of the drink-ordering window, if set (null = stays open while the event is live).</summary>
    public DateTime? DrinkSalesEndDate { get; set; }
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int AvailableSeats { get; set; }
    /// <summary>
    /// Of the sold seats (<see cref="Capacity"/> − <see cref="AvailableSeats"/>), how many come from
    /// season-pass–derived tickets. The remainder are ordinary single-event tickets.
    /// </summary>
    public int SeasonTicketsSold { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>The season this event belongs to, if any.</summary>
    public int? SeasonId { get; set; }
    /// <summary>Display name of the linked season (e.g. "2026/2027"), if any.</summary>
    public string? SeasonName { get; set; }

    /// <summary>Authoritative lifecycle state.</summary>
    public EventStatus Status { get; set; }
    /// <summary>Display name of <see cref="Status"/>.</summary>
    public string StatusName { get; set; } = string.Empty;
    /// <summary>High-level phase (Future / Active / Past) derived from <see cref="Status"/>.</summary>
    public EventPhase Phase { get; set; }
    /// <summary>
    /// Whether tickets/seats may currently be sold: the event is on sale AND the current time is
    /// within its configured sales window.
    /// </summary>
    public bool CanSellTickets { get; set; }
    /// <summary>Whether drink ordering is currently open for this event.</summary>
    public bool CanOrderDrinks { get; set; }
    /// <summary>
    /// True when the event is published and the current time falls within its
    /// [<see cref="Date"/>, <see cref="EndDate"/>] window (open-ended if no end is set).
    /// </summary>
    public bool IsCurrentlyLive { get; set; }

    /// <summary>
    /// True when the event has a poster image, fetchable from <c>GET events/{id}/image</c>.
    /// The bytes are never inlined here — this is only the flag the UI needs to decide whether to
    /// render an <c>&lt;img&gt;</c>.
    /// </summary>
    public bool HasPoster { get; set; }

    /// <summary>
    /// True once an admin has confirmed the poster's generated text is correct. Only approved
    /// posters are shown to fans.
    /// </summary>
    public bool PosterApproved { get; set; }

    /// <summary>
    /// True when the event's teams, kick-off or venue no longer match what is baked into the
    /// poster, so the artwork is showing details that have since changed.
    /// </summary>
    public bool PosterIsStale { get; set; }

    /// <summary>Pixel width of the stored poster, if any.</summary>
    public int? PosterWidth { get; set; }

    /// <summary>Pixel height of the stored poster, if any.</summary>
    public int? PosterHeight { get; set; }

    /// <summary>The prompt the poster was generated from, if any.</summary>
    public string? PosterPrompt { get; set; }
}

/// <summary>Request body for an event status transition.</summary>
public class TransitionEventStatusRequest
{
    [Required]
    public EventStatus NewStatus { get; set; }
}