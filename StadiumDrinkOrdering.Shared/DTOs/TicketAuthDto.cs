using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class ValidateTicketRequest
{
    [Required(ErrorMessage = "QR Code Token is required")]
    [StringLength(100, ErrorMessage = "QR Code Token must be less than 100 characters")]
    public string QRCodeToken { get; set; } = string.Empty;
}

public class ValidateTicketResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SessionToken { get; set; }
    public TicketSessionDto? TicketSession { get; set; }
    public TicketInfoDto? Ticket { get; set; }
    public SeatInfoDto? SeatInfo { get; set; }
}

public class TicketSessionDto
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string QRCodeToken { get; set; } = string.Empty;
    public int TicketId { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Row { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public bool IsActive { get; set; }
}

public class TicketInfoDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string? Section { get; set; }
    public string? Row { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class SeatInfoDto
{
    public string SeatNumber { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Row { get; set; } = string.Empty;
    public string? SectionName { get; set; }
    public string? TribuneName { get; set; }
}

public class LogoutTicketRequest
{
    [Required]
    public string SessionId { get; set; } = string.Empty;
}

public class TicketAuthResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public TicketSessionDto? TicketSession { get; set; }
    public TicketInfoDto? Ticket { get; set; }
}