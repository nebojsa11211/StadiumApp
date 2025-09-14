using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("[controller]")]
public class QRCodeController : ControllerBase
{
    private readonly IQRCodeService _qrCodeService;

    public QRCodeController(IQRCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateQRCode([FromBody] ValidateQRCodeDto request)
    {
        try
        {
            var isValid = await _qrCodeService.ValidateQRCodeAsync(request.QRToken);
            
            if (!isValid)
            {
                return BadRequest(new { message = "Invalid or expired QR code" });
            }

            var ticket = await _qrCodeService.GetTicketByQRTokenAsync(request.QRToken);
            if (ticket == null)
            {
                return BadRequest(new { message = "Ticket not found" });
            }

            return Ok(new
            {
                message = "QR Code is valid",
                ticket = new
                {
                    ticketId = ticket.Id,
                    ticketNumber = ticket.TicketNumber,
                    eventName = ticket.Event?.EventName,
                    eventDate = ticket.Event?.EventDate,
                    seatInfo = new
                    {
                        section = ticket.Seat?.Section?.SectionName ?? ticket.Section,
                        row = ticket.Seat?.RowNumber.ToString() ?? ticket.Row,
                        seat = ticket.Seat?.SeatNumber.ToString() ?? ticket.SeatNumber
                    }
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while validating QR code", error = ex.Message });
        }
    }

    [HttpPost("create-session")]
    public async Task<IActionResult> CreateOrderSession([FromBody] CreateOrderSessionDto request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var session = await _qrCodeService.CreateOrderSessionFromQRAsync(request.QRToken, ipAddress, userAgent);
            
            if (session == null)
            {
                return BadRequest(new { message = "Invalid QR code or unable to create session" });
            }

            return Ok(new
            {
                message = "Order session created successfully",
                sessionToken = session.SessionToken,
                expiresAt = session.ExpiresAt,
                ticket = await _qrCodeService.GetTicketByQRTokenAsync(request.QRToken)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating order session", error = ex.Message });
        }
    }

    [HttpGet("ticket/{qrToken}")]
    public async Task<IActionResult> GetTicketByQRToken(string qrToken)
    {
        try
        {
            var ticket = await _qrCodeService.GetTicketByQRTokenAsync(qrToken);
            
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }

            return Ok(new
            {
                ticketId = ticket.Id,
                ticketNumber = ticket.TicketNumber,
                eventName = ticket.Event?.EventName,
                eventDate = ticket.Event?.EventDate,
                seatInfo = new
                {
                    section = ticket.Seat?.Section?.SectionName ?? ticket.Section,
                    row = ticket.Seat?.RowNumber.ToString() ?? ticket.Row,
                    seat = ticket.Seat?.SeatNumber.ToString() ?? ticket.SeatNumber
                },
                qrCode = ticket.QRCode,
                isUsed = ticket.IsUsed,
                isActive = ticket.IsActive
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving ticket", error = ex.Message });
        }
    }

    [HttpPost("regenerate/{ticketId}")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> RegenerateQRCode(int ticketId)
    {
        try
        {
            // This would be implemented to regenerate QR codes for admin purposes
            return Task.FromResult<IActionResult>(Ok(new { message = "QR Code regeneration not yet implemented" }));
        }
        catch (Exception ex)
        {
            return Task.FromResult<IActionResult>(StatusCode(500, new { message = "An error occurred while regenerating QR code", error = ex.Message }));
        }
    }
}

// DTOs for the QR Code endpoints
public class ValidateQRCodeDto
{
    public string QRToken { get; set; } = string.Empty;
}

public class CreateOrderSessionDto
{
    public string QRToken { get; set; } = string.Empty;
}