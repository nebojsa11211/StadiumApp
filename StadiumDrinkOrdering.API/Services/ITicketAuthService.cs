using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketAuthService
{
    /// <summary>
    /// Authenticates a ticket using its QR code token and creates a session
    /// </summary>
    Task<TicketAuthResult> AuthenticateWithTicketAsync(string qrCodeToken);
    
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