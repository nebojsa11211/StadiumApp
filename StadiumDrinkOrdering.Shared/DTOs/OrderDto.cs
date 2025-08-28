using StadiumDrinkOrdering.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? PreparedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? AcceptedByUserName { get; set; }
    public string? PreparedByUserName { get; set; }
    public string? DeliveredByUserName { get; set; }
    public string? Notes { get; set; }
    public string? CustomerNotes { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
    public PaymentDto? Payment { get; set; }
}

public class CreateOrderDto
{
    [Required]
    [StringLength(50)]
    public string TicketNumber { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? CustomerNotes { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

public class UpdateOrderStatusDto
{
    [Required]
    public OrderStatus Status { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SpecialInstructions { get; set; }
}

public class CreateOrderItemDto
{
    [Required]
    public int DrinkId { get; set; }
    
    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
    
    [StringLength(200)]
    public string? SpecialInstructions { get; set; }
}



