using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// A drink order placed by an anonymous walk-up fan, authenticated only by possession of a valid
/// <c>TicketSession</c> token (obtained by scanning their ticket QR). No login required.
/// </summary>
public class SessionOrderRequest
{
    public string SessionToken { get; set; } = string.Empty;
    public List<SessionOrderItemDto> Items { get; set; } = new();
    public string? CustomerNotes { get; set; }

    /// <summary>When true, charge the order to the HALFTIME wallet of the account behind the scanned ticket
    /// (resolved server-side from the ticket — the client never supplies a wallet/user id). When false the
    /// order is created unpaid and settled at the bar / on delivery, as before.</summary>
    public bool PayWithWallet { get; set; }

    /// <summary>The fan's chosen offline method (<see cref="PaymentMethod.Cash"/> or a card) when
    /// <see cref="PayWithWallet"/> is false. No money moves at checkout — it's recorded as a Pending payment
    /// on the order so staff know how it will be settled at the bar / on delivery. Ignored when paying by
    /// wallet (that path is resolved server-side). Null keeps the legacy "unpaid, method unspecified"
    /// behaviour.</summary>
    public PaymentMethod? PaymentMethod { get; set; }
}

public class SessionOrderItemDto
{
    public int DrinkId { get; set; }
    public int Quantity { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class SessionOrderResultDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public int OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string SeatPath { get; set; } = string.Empty;

    /// <summary>True when a wallet payment was declined for insufficient balance. No order was created and no
    /// money moved — the fan can top up (or switch to another method) and retry.</summary>
    public bool InsufficientFunds { get; set; }
}

/// <summary>
/// One drink order in the fan's own history, trimmed to what the account view shows. Deliberately much
/// slimmer than <see cref="OrderDto"/>, which carries staff-only fields and nested entities that an
/// anonymous walk-up caller has no business receiving.
/// </summary>
public class SessionOrderHistoryItemDto
{
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    /// <summary>Where it was delivered — the seat as it read on the order.</summary>
    public string SeatPath { get; set; } = string.Empty;

    /// <summary>What was in it, one line per drink ("2 × Pivo").</summary>
    public List<SessionOrderHistoryLineDto> Items { get; set; } = new();
}

public class SessionOrderHistoryLineDto
{
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

/// <summary>Page of the fan's order history for a scanned ticket, plus the lifetime totals for that ticket.</summary>
public class SessionOrderHistoryDto
{
    public List<SessionOrderHistoryItemDto> Orders { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    /// <summary>Sum of every non-cancelled order on this ticket — not just the current page.</summary>
    public decimal TotalSpent { get; set; }
}
