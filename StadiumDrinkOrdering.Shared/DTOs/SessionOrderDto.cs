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
}
