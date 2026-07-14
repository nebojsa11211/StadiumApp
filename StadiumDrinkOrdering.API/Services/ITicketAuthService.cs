using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketAuthService
{
    /// <summary>
    /// Authenticates a ticket using its QR code token and resolves an ordering session.
    /// <paramref name="presentedSessionToken"/> is the SessionId the calling device already holds (from a
    /// previous claim, persisted client-side): when it matches the ticket's active session the session is
    /// resumed; otherwise a device with an active session elsewhere is rejected. Null on a first claim.
    /// </summary>
    Task<TicketAuthResult> AuthenticateWithTicketAsync(string qrCodeToken, string? presentedSessionToken = null);
    
    /// <summary>
    /// Validates if a ticket is currently valid for ordering
    /// </summary>
    Task<bool> IsTicketValidForOrderingAsync(string qrCodeToken);
    
    /// <summary>
    /// Gets ticket session by session ID
    /// </summary>
    Task<TicketSession?> GetTicketSessionAsync(string sessionId);
    
    /// <summary>
    /// Gets active ticket session by QR code token
    /// </summary>
    Task<TicketSession?> GetActiveSessionByTokenAsync(string qrCodeToken);
    
    /// <summary>
    /// Invalidates a ticket session (logout)
    /// </summary>
    Task<bool> InvalidateTicketSessionAsync(string sessionId);

    /// <summary>
    /// Staff re-issue: deactivates every active session for a ticket so a genuine holder who lost access
    /// (switched phones, cleared their browser) can re-claim it from a new device. Returns the number of
    /// sessions released.
    /// </summary>
    Task<int> ReleaseTicketSessionsAsync(int ticketId);
    
    /// <summary>
    /// Updates last accessed time for session
    /// </summary>
    Task UpdateSessionAccessAsync(string sessionId);
    
    /// <summary>
    /// Cleans up expired sessions
    /// </summary>
    Task CleanupExpiredSessionsAsync();
    
    /// <summary>
    /// Generates a secure session token
    /// </summary>
    string GenerateSessionToken();
    
    /// <summary>
    /// Validates session token format and extracts session ID
    /// </summary>
    bool TryParseSessionToken(string sessionToken, out string sessionId);
}