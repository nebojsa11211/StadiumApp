using StadiumDrinkOrdering.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    // Human-readable full seat location (e.g. "Sector A · Row 5 · Seat 12"), computed server-side
    // from the linked Seat/Section, falling back to the legacy SeatNumber string when unlinked.
    public string SeatPath { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? EventId { get; set; }
    public int? SeatId { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? InPreparationAt { get; set; }
    public DateTime? PreparedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public int? AcceptedByUserId { get; set; }
    public int? InPreparationByUserId { get; set; }
    public int? PreparedByUserId { get; set; }
    public int? DeliveredByUserId { get; set; }
    public string? AcceptedByUserName { get; set; }
    public string? InPreparationByUserName { get; set; }
    public string? PreparedByUserName { get; set; }
    public string? DeliveredByUserName { get; set; }
    public string? Notes { get; set; }
    public string? CustomerNotes { get; set; }
    public string? AcceptedByUser { get; set; }
    // Delivery-exception fields — populated when a runner reported "couldn't deliver". Surfaced on the
    // Bar returns triage so the bartender sees why it came back, how many attempts, and by whom.
    public int DeliveryAttempts { get; set; }
    public DeliveryFailureReason? LastDeliveryFailureReason { get; set; }
    public DateTime? LastDeliveryAttemptAt { get; set; }
    // Slim, self-contained views of the linked event and seat. These deliberately do NOT expose the
    // EF entities: an entity carries its navigation collections (Event.Orders, Seat.Section.Seats, …),
    // which EF's navigation fixup populates for every order in the same query — serializing that
    // turned a list of N orders into an O(N²) payload (121 orders ≈ 48 MB, the full list never
    // finished). Keep these flat and additive.
    public OrderEventDto? Event { get; set; }
    public OrderSeatDto? Seat { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public PaymentDto? Payment { get; set; }
}

/// <summary>The bits of the linked <see cref="Models.Event"/> an order view needs (name, date, season scope).</summary>
public class OrderEventDto
{
    public int Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int? SeasonId { get; set; }
}

/// <summary>The linked seat with its section flattened in, so no entity graph is dragged along.</summary>
public class OrderSeatDto
{
    public int Id { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public int SectionId { get; set; }
    public string? SectionCode { get; set; }
    public string? SectionName { get; set; }
}

public class CreateOrderDto
{
    [StringLength(50)]
    public string? TicketNumber { get; set; } = string.Empty;
    
    public int? TicketSessionId { get; set; }
    
    [StringLength(500)]
    public string? CustomerNotes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();

    /// <summary>
    /// How the order is paid. <see cref="Models.PaymentMethod.DigitalWallet"/> charges the fan's
    /// wallet for the total at creation time (order is only placed if the debit succeeds). Null keeps
    /// the legacy behaviour — the order is created unpaid.
    /// </summary>
    public PaymentMethod? PaymentMethod { get; set; }
}

public class UpdateOrderStatusDto
{
    [Required]
    public OrderStatus Status { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Optional client-generated id for the action that produced this update. Set by the Runner
    /// PWA's offline outbox so a retried request (e.g. a "Delivered" whose response was lost in a
    /// dead zone) can be correlated/de-duplicated. Safe to omit for server-rendered callers.
    /// </summary>
    public Guid? ClientActionId { get; set; }
}

/// <summary>
/// A runner reporting they couldn't hand an order over at the seat. Carries the reason from the
/// Runner's reason sheet plus an optional free-text note, and (like <see cref="UpdateOrderStatusDto"/>)
/// an optional client-generated id so the Runner's offline outbox can retry idempotently.
/// </summary>
public class ReportDeliveryFailedDto
{
    [Required]
    public DeliveryFailureReason Reason { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public Guid? ClientActionId { get; set; }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public DrinkDto? Drink { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class CreateOrderItemDto
{
    [Required]
    public int DrinkId { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
    
    [StringLength(200)]
    public string? SpecialInstructions { get; set; }
}



