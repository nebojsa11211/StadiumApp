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

    /// <summary>
    /// Builds the QR-code image for a ticket as a data-URI PNG WITHOUT persisting the image
    /// (the <c>Ticket.QRCode</c> column is too small to hold a base64 image). Ensures the ticket
    /// has a validation token (saving only the short token if it was missing), encodes the same
    /// deep link as <see cref="GenerateQRCodeAsync"/>, and returns the image for display.
    /// </summary>
    Task<string> GetQrImageDataUriAsync(Ticket ticket);

    Task<byte[]> GenerateQRCodeImageAsync(string qrData);
    Task<bool> ValidateQRCodeAsync(string qrToken);
    Task<Ticket?> GetTicketByQRTokenAsync(string qrToken);
    Task<OrderSession?> CreateOrderSessionFromQRAsync(string qrToken, string ipAddress, string userAgent);
}

public class QRCodeService : IQRCodeService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public QRCodeService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> GenerateQRCodeAsync(Ticket ticket)
    {
        // Create QR token if it doesn't exist
        if (string.IsNullOrEmpty(ticket.QRCodeToken))
        {
            ticket.QRCodeToken = Guid.NewGuid().ToString();
        }

        // Encode a deep link to the mobile customer app so a phone camera scan opens the ordering
        // flow directly: {CustomerApp:BaseUrl}/t/{token} -> the resolver validates and forwards to /order.
        var baseUrl = (_configuration["CustomerApp:BaseUrl"] ?? "https://localhost:7020").TrimEnd('/');
        var deepLink = $"{baseUrl}/t/{ticket.QRCodeToken}";

        // Generate QR code image as base64
        var qrCodeImage = await GenerateQRCodeImageAsync(deepLink);
        var base64String = Convert.ToBase64String(qrCodeImage);
        
        // Store the base64 QR code in the ticket
        ticket.QRCode = $"data:image/png;base64,{base64String}";
        
        await _context.SaveChangesAsync();
        
        return ticket.QRCode;
    }

    public async Task<string> GetQrImageDataUriAsync(Ticket ticket)
    {
        // Ensure a validation token exists (short — fits the column). Ingested tickets already have
        // one, so this normally saves nothing.
        if (string.IsNullOrEmpty(ticket.QRCodeToken))
        {
            ticket.QRCodeToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();
        }

        var baseUrl = (_configuration["CustomerApp:BaseUrl"] ?? "https://localhost:7020").TrimEnd('/');
        var deepLink = $"{baseUrl}/t/{ticket.QRCodeToken}";

        var qrCodeImage = await GenerateQRCodeImageAsync(deepLink);
        return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
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