using System;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Real-time payload broadcast over SignalR ("OrderStatusChanged") when a drink order
/// transitions to a new status, so staff/admin dashboards can update live.
/// Sent as a single object so all fields (including <see cref="EventId"/> and
/// <see cref="Timestamp"/>) survive the round trip.
/// </summary>
public class OrderStatusChangedNotification
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int? EventId { get; set; }
    public DateTime Timestamp { get; set; }
}
