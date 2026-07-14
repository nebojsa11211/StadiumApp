using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Runner.Models;

// Minimal client-side mirror of the API's contracts. Matched to the server by property name
// (JSON is camelCase, case-insensitive) and by numeric enum value. Kept deliberately small — only
// what the Runner reads/writes. See StadiumDrinkOrdering.Shared for the authoritative server types.

public enum OrderStatus
{
    Pending = 1,
    Accepted = 2,
    InPreparation = 3,
    Ready = 4,
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7,
    DeliveryFailed = 8
}

// Why a runner couldn't complete a delivery — mirrors the server enum by numeric value.
public enum DeliveryFailureReason
{
    CustomerNotAtSeat = 1,
    CustomerRefused = 2,
    WrongSeat = 3,
    Other = 99
}

public enum UserRole
{
    Customer = 1,
    Admin = 2,
    Bartender = 3,
    Waiter = 4
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string SeatPath { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

public class OrderItemDto
{
    public int Quantity { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
    public Guid? ClientActionId { get; set; }
}

// A runner reporting they couldn't hand an order over at the seat. ClientActionId carries the
// outbox action id so a retried send is idempotent server-side (mirrors UpdateOrderStatusDto).
public class ReportDeliveryFailedDto
{
    public DeliveryFailureReason Reason { get; set; }
    public string? Notes { get; set; }
    public Guid? ClientActionId { get; set; }
}

// Batch claim: a runner grabbing several Ready orders from the pool in one trip.
public class BatchClaimRequestDto
{
    public List<int> OrderIds { get; set; } = new();
}

public class BatchClaimResultDto
{
    public List<int> Claimed { get; set; } = new();
    public List<int> Taken { get; set; } = new();
    public List<int> NotFound { get; set; } = new();
}
