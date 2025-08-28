using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string? Section { get; set; }
    public string? Row { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public bool IsActive { get; set; }
}

public class ValidateTicketDto
{
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
}

public class TicketValidationResultDto
{
    public bool IsValid { get; set; }
    public TicketDto? Ticket { get; set; }
    public string? ErrorMessage { get; set; }
}



