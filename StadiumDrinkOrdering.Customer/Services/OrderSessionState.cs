using StadiumDrinkOrdering.Customer.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Customer.Services;

/// <summary>
/// The single drink-ordering session for the walk-up mobile flow. Scoped to the Blazor circuit, so the
/// active session and its cart survive navigation across /order → /cart → /track.
///
/// Backed by the real, working API surface: entry is a <c>TicketAuth/validate</c> (anonymous) that
/// yields a ticket-session token; the cart is held client-side here; checkout and status tracking go
/// through the anonymous, session-gated <c>customer/session</c> endpoints.
/// </summary>
public class OrderSessionState
{
    private readonly IApiService _api;

    public OrderSessionState(IApiService api) => _api = api;

    // --- session identity + seat/event (from validate) ---
    public string? SessionToken { get; private set; }
    public string EventName { get; private set; } = "";
    public DateTime? EventDate { get; private set; }
    public string SeatSection { get; private set; } = "";
    public string SeatRow { get; private set; } = "";
    public string SeatNumber { get; private set; } = "";

    /// <summary>Name of the ticket holder, surfaced on the landing/welcome banner after a scan. Empty for
    /// anonymous tickets that carry no holder name.</summary>
    public string CustomerName { get; private set; } = "";

    /// <summary>Human-readable ticket number (e.g. "TK001" / "SEA-6-E17"), shown on the welcome banner.</summary>
    public string TicketNumber { get; private set; } = "";

    /// <summary>
    /// True only while the event is live (Active). A session survives after the match ends, but
    /// ordering must be blocked — the /order and /cart pages use this to gate the menu and checkout. The API
    /// re-checks on order placement, so this is UX only; it is never the sole guard.
    /// </summary>
    public bool CanOrderDrinks { get; private set; }

    /// <summary>
    /// Id of the fan's most recently placed order while it is still in flight (not Delivered/Cancelled).
    /// Lets the ordering flow offer a jump back to <c>/track/{id}</c> after the fan leaves the tracking
    /// page to order more. Set on checkout, refreshed when a live order is opened on /track, and cleared
    /// once it reaches a terminal state. Persisted to sessionStorage at the page level so it survives a
    /// hard reload.
    /// </summary>
    public int? ActiveOrderId { get; private set; }

    // --- client-side cart ---
    public List<CartLine> Items { get; private set; } = new();

    public bool HasSession => !string.IsNullOrEmpty(SessionToken);
    public int Count => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.TotalPrice);

    public string SeatText
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(SeatSection)) parts.Add(SeatSection);
            if (!string.IsNullOrWhiteSpace(SeatRow)) parts.Add($"Red {SeatRow}");
            if (!string.IsNullOrWhiteSpace(SeatNumber)) parts.Add($"Sjed. {SeatNumber}");
            return parts.Count > 0 ? string.Join(" · ", parts) : "Tvoje mjesto";
        }
    }

    /// <summary>Remember an in-flight order so the ordering flow can offer a jump back to /track/{id}.
    /// No-op for a non-positive id or when it's already the active order (avoids a redundant re-render).</summary>
    public void SetActiveOrder(int orderId)
    {
        if (orderId <= 0 || ActiveOrderId == orderId) return;
        ActiveOrderId = orderId;
        OnChange?.Invoke();
    }

    /// <summary>Forget the active order once it's delivered/cancelled, so the "track your order" affordance
    /// disappears from the ordering flow.</summary>
    public void ClearActiveOrder()
    {
        if (ActiveOrderId == null) return;
        ActiveOrderId = null;
        OnChange?.Invoke();
    }

    public event Action? OnChange;

    /// <summary>
    /// Entry: resolve a scanned token (single-event ticket QR OR season pass token) into a ticket
    /// session bound to the seat. Also used by the season home to start ordering from a derived ticket.
    /// </summary>
    public async Task<(bool Ok, string? Error)> InitFromQrAsync(string qrToken, string? existingSessionToken = null)
    {
        var resp = await _api.ResolveAccessAsync(qrToken, existingSessionToken);
        if (resp is { Success: true } && !string.IsNullOrEmpty(resp.SessionToken))
        {
            SessionToken = resp.SessionToken;
            EventName = resp.Ticket?.EventName ?? resp.TicketSession?.EventName ?? "Utakmica";
            EventDate = resp.Ticket?.EventDate ?? resp.TicketSession?.EventDate;
            SeatSection = resp.SeatInfo?.SectionName ?? resp.SeatInfo?.Section ?? resp.Ticket?.Section ?? "";
            SeatRow = resp.SeatInfo?.Row ?? resp.Ticket?.Row ?? "";
            SeatNumber = resp.SeatInfo?.SeatNumber ?? resp.Ticket?.SeatNumber ?? "";
            CustomerName = resp.Ticket?.CustomerName ?? resp.TicketSession?.CustomerName ?? "";
            TicketNumber = resp.Ticket?.TicketNumber ?? resp.TicketSession?.TicketNumber ?? "";
            // Resolve only succeeds for a live event, so ordering is open; honour the flag if the API sends it.
            CanOrderDrinks = resp.TicketSession?.CanOrderDrinks ?? true;
            OnChange?.Invoke();
            return (true, null);
        }
        return (false, resp?.ErrorMessage ?? "Ulaznicu nije moguće potvrditi. Pokušaj ponovno.");
    }

    /// <summary>Rehydrate the session (seat/event) from a stored token after a hard reload. Cart is not restored.</summary>
    public async Task<bool> InitFromTokenAsync(string sessionToken)
    {
        var s = await _api.GetTicketSessionAsync(sessionToken);
        if (s == null || !s.IsActive) return false;
        SessionToken = s.SessionId;
        EventName = s.EventName;
        EventDate = s.EventDate;
        SeatSection = string.IsNullOrWhiteSpace(s.Section) ? SeatSection : s.Section;
        SeatRow = string.IsNullOrWhiteSpace(s.Row) ? SeatRow : s.Row;
        SeatNumber = string.IsNullOrWhiteSpace(s.SeatNumber) ? SeatNumber : s.SeatNumber;
        CustomerName = string.IsNullOrWhiteSpace(s.CustomerName) ? CustomerName : s.CustomerName;
        TicketNumber = string.IsNullOrWhiteSpace(s.TicketNumber) ? TicketNumber : s.TicketNumber;
        CanOrderDrinks = s.CanOrderDrinks;
        OnChange?.Invoke();
        return true;
    }

    public int QtyOf(int drinkId) => Items.FirstOrDefault(i => i.DrinkId == drinkId)?.Quantity ?? 0;

    /// <summary>Add (or increment) a drink in the client cart.</summary>
    public void AddDrink(DrinkDto drink, int quantity = 1, string? note = null)
    {
        if (quantity <= 0) return;
        var line = Items.FirstOrDefault(i => i.DrinkId == drink.Id);
        if (line == null)
        {
            line = new CartLine { DrinkId = drink.Id, DrinkName = drink.Name, UnitPrice = drink.Price, Quantity = 0, SpecialInstructions = note };
            Items.Add(line);
        }
        if (!string.IsNullOrWhiteSpace(note)) line.SpecialInstructions = note;
        line.Quantity += quantity;
        line.TotalPrice = line.UnitPrice * line.Quantity;
        OnChange?.Invoke();
    }

    public void SetQty(int drinkId, int quantity)
    {
        var line = Items.FirstOrDefault(i => i.DrinkId == drinkId);
        if (line == null) return;
        if (quantity <= 0)
        {
            Items.Remove(line);
        }
        else
        {
            line.Quantity = quantity;
            line.TotalPrice = line.UnitPrice * quantity;
        }
        OnChange?.Invoke();
    }

    public void Remove(int drinkId)
    {
        Items.RemoveAll(i => i.DrinkId == drinkId);
        OnChange?.Invoke();
    }

    /// <summary>Place the order for the scanned seat. Keeps the session (so the fan can order again).
    /// When <paramref name="payWithWallet"/> is true the order is charged to the ticket owner's HALFTIME
    /// wallet; otherwise it is created unpaid and settled at the bar / on delivery, with the fan's chosen
    /// offline <paramref name="paymentMethod"/> (cash / card) recorded on the order for staff.</summary>
    public async Task<SessionOrderResultDto?> CheckoutAsync(bool payWithWallet = false, PaymentMethod? paymentMethod = null, string? customerNotes = null)
    {
        if (!HasSession || Items.Count == 0) return new SessionOrderResultDto { Success = false, Error = "Košarica je prazna." };

        var request = new SessionOrderRequest
        {
            SessionToken = SessionToken!,
            CustomerNotes = customerNotes,
            PayWithWallet = payWithWallet,
            PaymentMethod = paymentMethod,
            Items = Items.Select(i => new SessionOrderItemDto
            {
                DrinkId = i.DrinkId,
                Quantity = i.Quantity,
                SpecialInstructions = i.SpecialInstructions
            }).ToList()
        };

        var result = await _api.CreateSessionOrderAsync(request);
        if (result is { Success: true })
        {
            Items = new();
            if (result.OrderId > 0) ActiveOrderId = result.OrderId;
            OnChange?.Invoke();
        }
        return result;
    }
}
