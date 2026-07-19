using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace StadiumDrinkOrdering.API.Services;

public class TicketAuthService : ITicketAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TicketAuthService> _logger;

    public TicketAuthService(ApplicationDbContext context, ILogger<TicketAuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TicketAuthResult> AuthenticateWithTicketAsync(string qrCodeToken, string? presentedSessionToken = null)
    {
        try
        {
            // Find the ticket by QR code token
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Seat)
                    .ThenInclude(s => s.Section)
                .FirstOrDefaultAsync(t => t.QRCodeToken == qrCodeToken && t.IsActive);

            if (ticket == null)
            {
                return new TicketAuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid ticket. Please check your QR code and try again."
                };
            }

            // Gate drink ordering on the event's lifecycle status (Phase 2 only: Active).
            // This is the authoritative source of truth — replaces ad-hoc EventDate math and blocks
            // both future (not started) and past (ended/cancelled) events. Because reuse is now governed
            // by "one active session per live event" (below) rather than a sticky IsUsed flag, this
            // live-event gate is also what blocks a genuine second use *after* the match is over.
            if (ticket.Event == null || !ticket.Event.AreDrinkSalesOpenAt(DateTime.UtcNow))
            {
                return new TicketAuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = ticket.Event != null
                        ? ticket.Event.DrinkSalesBlockedReason(DateTime.UtcNow)!
                        : "This ticket is not linked to a valid event."
                };
            }

            // Device binding: a ticket may have at most one *active* ordering session at a time, and the
            // random SessionId is the per-device secret. Only the device that created the session holds it
            // (the customer app persists it in localStorage keyed by the QR), so it is presented here on a
            // re-scan/reload from the SAME device.
            //  • Same device (token matches) → resume the live session. This covers the legitimate "fan
            //    reopens the QR / refreshes /order" case.
            //  • Different device (no token, or a token that doesn't match) → reject. A second phone that
            //    only has the QR cannot present the SessionId, so it is turned away rather than sharing the
            //    session. Staff can hand access over via ReleaseTicketSessionsAsync (re-issue).
            var existingSession = await GetActiveSessionByTokenAsync(qrCodeToken);
            if (existingSession != null)
            {
                if (!string.IsNullOrEmpty(presentedSessionToken) &&
                    string.Equals(presentedSessionToken, existingSession.SessionId, StringComparison.Ordinal))
                {
                    await UpdateSessionAccessAsync(existingSession.SessionId);

                    return new TicketAuthResult
                    {
                        IsSuccess = true,
                        TicketSession = MapToTicketSessionDto(existingSession),
                        Ticket = MapToTicketInfoDto(ticket)
                    };
                }

                _logger.LogWarning(
                    "Blocked second-device access for ticket {TicketId}: an active session already exists on another device.",
                    ticket.Id);

                return new TicketAuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "This ticket is already in use on another device. If this is your ticket, ask staff at the bar to reset it."
                };
            }

            // No active session → this device claims the ticket. Reaching here means either a first-ever
            // scan or a re-claim after the previous session was released by staff (re-issue). The
            // live-event gate above already blocked post-match reuse, so we do NOT reject on the sticky
            // IsUsed flag — doing so would make a released ticket permanently unusable.

            // Create new session
            var session = new TicketSession
            {
                SessionId = GenerateSessionToken(),
                QRCodeToken = qrCodeToken,
                TicketId = ticket.Id,
                EventId = ticket.EventId,
                SeatId = ticket.SeatId,
                SeatNumber = ticket.SeatNumber ?? ticket.Seat?.SeatNumber.ToString() ?? "",
                Section = ticket.Section ?? ticket.Seat?.Section?.SectionName ?? "",
                Row = ticket.Row ?? ticket.Seat?.RowNumber.ToString() ?? "",
                CustomerName = ticket.CustomerName,
                CustomerEmail = ticket.CustomerEmail,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = ComputeSessionExpiry(ticket.Event),
                LastAccessedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.TicketSessions.Add(session);

            // Mark ticket as used
            ticket.IsUsed = true;
            ticket.UsedAt = DateTime.UtcNow;
            ticket.Status = "Used";

            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new ticket session for ticket {TicketId}, seat {SeatNumber}", 
                ticket.Id, session.SeatNumber);

            return new TicketAuthResult
            {
                IsSuccess = true,
                TicketSession = MapToTicketSessionDto(session),
                Ticket = MapToTicketInfoDto(ticket)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating ticket with token {Token}", qrCodeToken);
            return new TicketAuthResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while validating your ticket. Please try again."
            };
        }
    }

    public async Task<bool> IsTicketValidForOrderingAsync(string qrCodeToken)
    {
        var session = await GetActiveSessionByTokenAsync(qrCodeToken);
        return session != null
            && session.IsActive
            && session.ExpiresAt > DateTime.UtcNow
            && session.Ticket?.Event != null
            && session.Ticket.Event.AreDrinkSalesOpenAt(DateTime.UtcNow);
    }

    public async Task<TicketSession?> GetTicketSessionAsync(string sessionId)
    {
        return await _context.TicketSessions
            .Include(s => s.Ticket)
                .ThenInclude(t => t.Event)
            .Include(s => s.Seat)
                .ThenInclude(s => s.Section)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive);
    }

    public async Task<TicketSession?> GetActiveSessionByTokenAsync(string qrCodeToken)
    {
        return await _context.TicketSessions
            .Include(s => s.Ticket)
                .ThenInclude(t => t.Event)
            .Include(s => s.Seat)
                .ThenInclude(s => s.Section)
            .FirstOrDefaultAsync(s => s.QRCodeToken == qrCodeToken && s.IsActive && s.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<bool> InvalidateTicketSessionAsync(string sessionId)
    {
        try
        {
            var session = await _context.TicketSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session != null)
            {
                session.IsActive = false;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Invalidated ticket session {SessionId}", sessionId);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<int> ReleaseTicketSessionsAsync(int ticketId)
    {
        try
        {
            var active = await _context.TicketSessions
                .Where(s => s.TicketId == ticketId && s.IsActive)
                .ToListAsync();

            foreach (var session in active)
                session.IsActive = false;

            if (active.Count > 0)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation(
                    "Released {Count} active session(s) for ticket {TicketId} (staff re-issue).",
                    active.Count, ticketId);
            }

            return active.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing sessions for ticket {TicketId}", ticketId);
            throw;
        }
    }

    public async Task UpdateSessionAccessAsync(string sessionId)
    {
        try
        {
            var session = await _context.TicketSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session != null)
            {
                session.LastAccessedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating session access for {SessionId}", sessionId);
        }
    }

    public async Task CleanupExpiredSessionsAsync()
    {
        try
        {
            var expiredSessions = await _context.TicketSessions
                .Where(s => s.ExpiresAt < DateTime.UtcNow || !s.IsActive)
                .ToListAsync();

            foreach (var session in expiredSessions)
            {
                session.IsActive = false;
            }

            if (expiredSessions.Any())
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired ticket sessions", expiredSessions.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired sessions");
        }
    }

    public string GenerateSessionToken()
    {
        // Generate a secure random session ID
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        
        var sessionId = Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        return $"ticket_session_{sessionId}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }

    public bool TryParseSessionToken(string sessionToken, out string sessionId)
    {
        sessionId = string.Empty;
        
        if (string.IsNullOrEmpty(sessionToken) || !sessionToken.StartsWith("ticket_session_"))
        {
            return false;
        }

        sessionId = sessionToken;
        return true;
    }

    /// <summary>
    /// A session's cleanup expiry. Tied to the event's *end* (or start when no explicit end is set), plus a
    /// 4-hour grace. Floored to "now + 4h" so a past-dated-but-still-Active event (common in test/simulated
    /// data) does not create an already-expired session — that bug made every re-scan fall through to the
    /// old "already used" reject. Live-ness itself is re-checked via <see cref="Event.AreDrinkSalesOpenAt"/>
    /// on each resolve, so a generous expiry here only affects the background cleanup sweep.
    /// </summary>
    private static DateTime ComputeSessionExpiry(Event? evt)
    {
        var baseline = evt?.EventEndDate ?? evt?.EventDate ?? DateTime.UtcNow;
        if (baseline < DateTime.UtcNow)
            baseline = DateTime.UtcNow;
        return baseline.AddHours(4);
    }

    private TicketSessionDto MapToTicketSessionDto(TicketSession session)
    {
        return new TicketSessionDto
        {
            Id = session.Id,
            SessionId = session.SessionId,
            QRCodeToken = session.QRCodeToken,
            TicketId = session.TicketId,
            TicketNumber = session.Ticket?.TicketNumber ?? "",
            EventId = session.EventId,
            EventName = session.Event?.EventName ?? session.Ticket?.Event?.EventName ?? "",
            EventDate = session.Event?.EventDate ?? session.Ticket?.Event?.EventDate ?? DateTime.MinValue,
            SeatNumber = session.SeatNumber,
            Section = session.Section,
            Row = session.Row,
            CustomerName = session.CustomerName,
            CustomerEmail = session.CustomerEmail,
            CreatedAt = session.CreatedAt,
            ExpiresAt = session.ExpiresAt,
            LastAccessedAt = session.LastAccessedAt,
            IsActive = session.IsActive,
            CanOrderDrinks = IsEventOrderable(session)
        };
    }

    private static bool IsEventOrderable(TicketSession session)
    {
        var evt = session.Event ?? session.Ticket?.Event;
        return evt is not null && evt.AreDrinkSalesOpenAt(DateTime.UtcNow);
    }

    private TicketInfoDto MapToTicketInfoDto(Ticket ticket)
    {
        return new TicketInfoDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            EventId = ticket.EventId,
            EventName = ticket.Event?.EventName ?? ticket.EventName,
            EventDate = ticket.Event?.EventDate ?? ticket.EventDate,
            SeatNumber = ticket.SeatNumber ?? ticket.Seat?.SeatNumber.ToString() ?? "",
            Section = ticket.Section ?? ticket.Seat?.Section?.SectionName,
            Row = ticket.Row ?? ticket.Seat?.RowNumber.ToString(),
            CustomerName = ticket.CustomerName,
            CustomerEmail = ticket.CustomerEmail,
            IsUsed = ticket.IsUsed,
            UsedAt = ticket.UsedAt,
            Status = ticket.Status
        };
    }
}