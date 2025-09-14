using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketAuthController : ControllerBase
{
    private readonly ITicketAuthService _ticketAuthService;
    private readonly ILogger<TicketAuthController> _logger;

    public TicketAuthController(ITicketAuthService ticketAuthService, ILogger<TicketAuthController> logger)
    {
        _ticketAuthService = ticketAuthService;
        _logger = logger;
    }

    /// <summary>
    /// Validates a ticket QR code and creates an authentication session
    /// </summary>
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateTicket([FromBody] ValidateTicketRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateTicketResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid request data"
                });
            }

            _logger.LogInformation("Validating ticket with token: {Token}", request.QRCodeToken);

            var result = await _ticketAuthService.AuthenticateWithTicketAsync(request.QRCodeToken);
            
            if (result.IsSuccess)
            {
                var sessionToken = result.TicketSession?.SessionId;
                
                _logger.LogInformation("Ticket validation successful for seat {SeatNumber}", 
                    result.TicketSession?.SeatNumber);

                return Ok(new ValidateTicketResponse
                {
                    Success = true,
                    SessionToken = sessionToken,
                    TicketSession = result.TicketSession,
                    Ticket = result.Ticket,
                    SeatInfo = new SeatInfoDto
                    {
                        SeatNumber = result.TicketSession?.SeatNumber ?? "",
                        Section = result.TicketSession?.Section ?? "",
                        Row = result.TicketSession?.Row ?? ""
                    }
                });
            }
            else
            {
                _logger.LogWarning("Ticket validation failed: {ErrorMessage}", result.ErrorMessage);
                
                return BadRequest(new ValidateTicketResponse
                {
                    Success = false,
                    ErrorMessage = result.ErrorMessage
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating ticket");
            return StatusCode(500, new ValidateTicketResponse
            {
                Success = false,
                ErrorMessage = "An internal error occurred while validating your ticket"
            });
        }
    }

    /// <summary>
    /// Gets the current ticket session information
    /// </summary>
    [HttpGet("session/{sessionToken}")]
    public async Task<IActionResult> GetSession(string sessionToken)
    {
        try
        {
            if (!_ticketAuthService.TryParseSessionToken(sessionToken, out var sessionId))
            {
                return BadRequest(new { Success = false, ErrorMessage = "Invalid session token format" });
            }

            var session = await _ticketAuthService.GetTicketSessionAsync(sessionId);
            
            if (session == null || !session.IsActive || session.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound(new { Success = false, ErrorMessage = "Session not found or expired" });
            }

            // Update last accessed time
            await _ticketAuthService.UpdateSessionAccessAsync(sessionId);

            return Ok(new
            {
                Success = true,
                Session = new TicketSessionDto
                {
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
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving session {SessionToken}", sessionToken);
            return StatusCode(500, new { Success = false, ErrorMessage = "Error retrieving session" });
        }
    }

    /// <summary>
    /// Logs out a ticket session (invalidates it)
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutTicket([FromBody] LogoutTicketRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, ErrorMessage = "Invalid request data" });
            }

            var success = await _ticketAuthService.InvalidateTicketSessionAsync(request.SessionId);
            
            if (success)
            {
                _logger.LogInformation("Successfully logged out session {SessionId}", request.SessionId);
                return Ok(new { Success = true, Message = "Successfully logged out" });
            }
            else
            {
                return NotFound(new { Success = false, ErrorMessage = "Session not found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out session {SessionId}", request.SessionId);
            return StatusCode(500, new { Success = false, ErrorMessage = "Error logging out" });
        }
    }

    /// <summary>
    /// Checks if a QR code token is valid for ordering
    /// </summary>
    [HttpGet("check/{qrCodeToken}")]
    public async Task<IActionResult> CheckTicketValidity(string qrCodeToken)
    {
        try
        {
            var isValid = await _ticketAuthService.IsTicketValidForOrderingAsync(qrCodeToken);
            
            return Ok(new 
            { 
                Success = true, 
                IsValid = isValid,
                Message = isValid ? "Ticket is valid for ordering" : "Ticket is not valid for ordering"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ticket validity for {Token}", qrCodeToken);
            return StatusCode(500, new { Success = false, ErrorMessage = "Error checking ticket validity" });
        }
    }

    /// <summary>
    /// Administrative endpoint to cleanup expired sessions
    /// </summary>
    [HttpPost("cleanup-expired")]
    public async Task<IActionResult> CleanupExpiredSessions()
    {
        try
        {
            await _ticketAuthService.CleanupExpiredSessionsAsync();
            return Ok(new { Success = true, Message = "Expired sessions cleaned up successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired sessions");
            return StatusCode(500, new { Success = false, ErrorMessage = "Error cleaning up sessions" });
        }
    }
}