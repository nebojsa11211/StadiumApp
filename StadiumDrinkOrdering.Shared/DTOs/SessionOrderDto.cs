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
