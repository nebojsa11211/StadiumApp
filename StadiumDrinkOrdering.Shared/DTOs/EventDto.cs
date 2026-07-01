using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }
    public int AvailableSeats { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>Authoritative lifecycle state.</summary>
    public EventStatus Status { get; set; }
    /// <summary>Display name of <see cref="Status"/>.</summary>
    public string StatusName { get; set; } = string.Empty;
    /// <summary>High-level phase (Future / Active / Past) derived from <see cref="Status"/>.</summary>
    public EventPhase Phase { get; set; }
    /// <summary>Whether tickets/seats may currently be sold for this event.</summary>
    public bool CanSellTickets { get; set; }
    /// <summary>Whether drink ordering is currently open for this event.</summary>
    public bool CanOrderDrinks { get; set; }
}

/// <summary>Request body for an event status transition.</summary>
public class TransitionEventStatusRequest
{
    [Required]
    public EventStatus NewStatus { get; set; }
}