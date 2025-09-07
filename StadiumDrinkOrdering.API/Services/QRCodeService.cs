using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using StadiumDrinkOrdering.Shared.Models;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;

namespace StadiumDrinkOrdering.API.Services;

public interface IQRCodeService
{
    Task<string> GenerateQRCodeAsync(Ticket ticket);
    Task<byte[]> GenerateQRCodeImageAsync(string qrData);
    Task<bool> ValidateQRCodeAsync(string qrToken);
    Task<Ticket?> GetTicketByQRTokenAsync(string qrToken);
    Task<OrderSession?> CreateOrderSessionFromQRAsync(string qrToken, string ipAddress, string userAgent);
}

public class QRCodeService : IQRCodeService
{
    private readonly ApplicationDbContext _context;

    public QRCodeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateQRCodeAsync(Ticket ticket)
    {
        // Create QR token if it doesn't exist
        if (string.IsNullOrEmpty(ticket.QRCodeToken))
        {
            ticket.QRCodeToken = Guid.NewGuid().ToString();
        }

        // Create QR data payload
        var qrData = new
        {
            TicketId = ticket.Id,
            Token = ticket.QRCodeToken,
            EventId = ticket.EventId,
            SeatId = ticket.SeatId,
            Timestamp = DateTime.UtcNow,
            ValidUntil = ticket.Event?.EventDate ?? DateTime.UtcNow.AddDays(1)
        };

        var qrDataJson = System.Text.Json.JsonSerializer.Serialize(qrData);
        
        // Generate QR code image as base64
        var qrCodeImage = await GenerateQRCodeImageAsync(qrDataJson);
        var base64String = Convert.ToBase64String(qrCodeImage);
        
        // Store the base64 QR code in the ticket
        ticket.QRCode = $"data:image/png;base64,{base64String}";
        
        await _context.SaveChangesAsync();
        
        return ticket.QRCode;
    }

    public async Task<byte[]> GenerateQRCodeImageAsync(string qrData)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20);
        
        return await Task.FromResult(qrCodeBytes);
    }

    public async Task<bool> ValidateQRCodeAsync(string qrToken)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Event)
            .FirstOrDefaultAsync(t => t.QRCodeToken == qrToken && t.IsActive);

        if (ticket == null)
            return false;

        // Check if ticket is already used
        if (ticket.IsUsed)
            return false;

        // Check if event date has passed
        if (ticket.Event?.EventDate < DateTime.UtcNow.AddHours(-2)) // Allow 2 hours grace period
            return false;

        return true;
    }

    public async Task<Ticket?> GetTicketByQRTokenAsync(string qrToken)
    {
        return await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Seat)
                .ThenInclude(s => s.Section)
            .FirstOrDefaultAsync(t => t.QRCodeToken == qrToken && t.IsActive);
    }

    public async Task<OrderSession?> CreateOrderSessionFromQRAsync(string qrToken, string ipAddress, string userAgent)
    {
        var ticket = await GetTicketByQRTokenAsync(qrToken);
        if (ticket == null || !await ValidateQRCodeAsync(qrToken))
            return null;

        // Check if there's already an active session for this ticket
        var existingSession = await _context.OrderSessions
            .FirstOrDefaultAsync(s => s.TicketId == ticket.Id && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

        if (existingSession != null)
        {
            // Extend existing session
            existingSession.LastActivity = DateTime.UtcNow;
            existingSession.ExpiresAt = DateTime.UtcNow.AddHours(2);
            await _context.SaveChangesAsync();
            return existingSession;
        }

        // Create new session
        var session = new OrderSession
        {
            TicketId = ticket.Id,
            SessionToken = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            IsActive = true,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            LastActivity = DateTime.UtcNow
        };

        _context.OrderSessions.Add(session);
        await _context.SaveChangesAsync();

        return session;
    }
}