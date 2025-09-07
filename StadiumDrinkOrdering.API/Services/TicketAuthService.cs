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

    public async Task<TicketAuthResult> AuthenticateWithTicketAsync(string qrCodeToken)
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

            // Check if ticket is already used
            if (ticket.IsUsed && ticket.Status == "Used")
            {
                return new TicketAuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "This ticket has already been used. Each ticket can only be scanned once per event."
                };
            }

            // Check if event is still valid
            if (ticket.Event?.EventDate < DateTime.UtcNow.AddHours(-1)) // Allow 1 hour grace period
            {
                return new TicketAuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "This event has ended. Drink ordering is no longer available."
                };
            }

            // Check if there's already an active session for this ticket
            var existingSession = await GetActiveSessionByTokenAsync(qrCodeToken);
            if (existingSession != null)
            {
                // Update last accessed time
                await UpdateSessionAccessAsync(existingSession.SessionId);
                
                return new TicketAuthResult
                {
                    IsSuccess = true,
                    TicketSession = MapToTicketSessionDto(existingSession),
                    Ticket = MapToTicketInfoDto(ticket)
                };
            }

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
                ExpiresAt = ticket.Event?.EventDate.AddHours(4) ?? DateTime.UtcNow.AddHours(8), // Event end + 4 hours
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
        return session != null && session.IsActive && session.ExpiresAt > DateTime.UtcNow;
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

    private TicketSessionDto MapToTicketSessionDto(TicketSession session)
    {
        return new TicketSessionDto
        {
            Id = session.Id,
            SessionId = session.SessionId,
            QRCodeToken = session.QRCodeToken,
            TicketId = session.TicketId,
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
            IsActive = session.IsActive
        };
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