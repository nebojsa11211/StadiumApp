using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StadiumController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StadiumController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("layout")]
    [AllowAnonymous]
    public async Task<ActionResult<StadiumLayoutDto>> GetStadiumLayout()
    {
        var seats = await _context.StadiumSeats
            .Where(s => s.IsActive)
            .ToListAsync();

        var activeOrders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Drink)
            .Where(o => o.Status == OrderStatus.Pending || 
                       o.Status == OrderStatus.Accepted || 
                       o.Status == OrderStatus.InPreparation || 
                       o.Status == OrderStatus.Ready)
            .ToListAsync();

        var sections = seats
            .GroupBy(s => s.Section)
            .Select(g => new StadiumSectionDto
            {
                SectionName = g.Key,
                DisplayName = $"Section {g.Key}",
                Color = GetSectionColor(g.Key),
                TotalSeats = g.Count(),
                ActiveOrders = activeOrders.Count(o => o.SeatNumber.StartsWith(g.Key)),
                Seats = g.Select(s => new StadiumSeatDto
                {
                    Id = s.Id,
                    Section = s.Section,
                    RowNumber = s.RowNumber,
                    SeatNumber = s.SeatNumber,
                    XCoordinate = s.XCoordinate,
                    YCoordinate = s.YCoordinate,
                    IsActive = s.IsActive,
                    HasActiveOrder = activeOrders.Any(o => o.SeatNumber == $"{s.Section}{s.RowNumber}-{s.SeatNumber}"),
                    ActiveOrder = activeOrders
                        .Where(o => o.SeatNumber == $"{s.Section}{s.RowNumber}-{s.SeatNumber}")
                        .Select(o => new OrderDto
                        {
                            Id = o.Id,
                            TicketNumber = o.TicketNumber,
                            SeatNumber = o.SeatNumber,
                            TotalAmount = o.TotalAmount,
                            Status = o.Status,
                            CreatedAt = o.CreatedAt,
                            OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                            {
                                Id = oi.Id,
                                DrinkId = oi.DrinkId,
                                DrinkName = oi.Drink.Name,
                                Quantity = oi.Quantity,
                                UnitPrice = oi.UnitPrice,
                                TotalPrice = oi.TotalPrice,
                                SpecialInstructions = oi.SpecialInstructions
                            }).ToList()
                        })
                        .FirstOrDefault()
                }).ToList()
            })
            .ToList();

        var layout = new StadiumLayoutDto
        {
            Sections = sections,
            TotalSeats = seats.Count,
            ActiveOrders = activeOrders.Count,
            Width = 800,
            Height = 600
        };

        return Ok(layout);
    }

    [HttpGet("section/{section}/seats")]
    [Authorize(Roles = "Admin,Bartender,Waiter")]
    public async Task<ActionResult<List<StadiumSeatDto>>> GetSectionSeats(string section)
    {
        var seats = await _context.StadiumSeats
            .Where(s => s.Section == section && s.IsActive)
            .ToListAsync();

        var activeOrders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Drink)
            .Where(o => (o.Status == OrderStatus.Pending || 
                        o.Status == OrderStatus.Accepted || 
                        o.Status == OrderStatus.InPreparation || 
                        o.Status == OrderStatus.Ready) &&
                        o.SeatNumber.StartsWith(section))
            .ToListAsync();

        var seatDtos = seats.Select(s => new StadiumSeatDto
        {
            Id = s.Id,
            Section = s.Section,
            RowNumber = s.RowNumber,
            SeatNumber = s.SeatNumber,
            XCoordinate = s.XCoordinate,
            YCoordinate = s.YCoordinate,
            IsActive = s.IsActive,
            HasActiveOrder = activeOrders.Any(o => o.SeatNumber == $"{s.Section}{s.RowNumber}-{s.SeatNumber}"),
            ActiveOrder = activeOrders
                .Where(o => o.SeatNumber == $"{s.Section}{s.RowNumber}-{s.SeatNumber}")
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    TicketNumber = o.TicketNumber,
                    SeatNumber = o.SeatNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        DrinkId = oi.DrinkId,
                        DrinkName = oi.Drink.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice,
                        SpecialInstructions = oi.SpecialInstructions
                    }).ToList()
                })
                .FirstOrDefault()
        }).ToList();

        return Ok(seatDtos);
    }

    [HttpGet("seat-orders")]
    [Authorize(Roles = "Admin,Bartender,Waiter")]
    public async Task<ActionResult<List<SeatOrderDto>>> GetSeatOrders()
    {
        var activeOrders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Drink)
            .Where(o => o.Status == OrderStatus.Pending || 
                       o.Status == OrderStatus.Accepted || 
                       o.Status == OrderStatus.InPreparation || 
                       o.Status == OrderStatus.Ready)
            .ToListAsync();

        var seatOrders = new List<SeatOrderDto>();

        foreach (var order in activeOrders)
        {
            var seatParts = order.SeatNumber.Split('-');
            if (seatParts.Length == 2)
            {
                var section = seatParts[0][0].ToString();
                var seatNumber = int.Parse(seatParts[0].Substring(1));
                var seat = await _context.StadiumSeats
                    .FirstOrDefaultAsync(s => s.Section == section && 
                                            s.RowNumber == seatNumber && 
                                            s.SeatNumber == int.Parse(seatParts[1]));

                if (seat != null)
                {
                    seatOrders.Add(new SeatOrderDto
                    {
                        Seat = new StadiumSeatDto
                        {
                            Id = seat.Id,
                            Section = seat.Section,
                            RowNumber = seat.RowNumber,
                            SeatNumber = seat.SeatNumber,
                            XCoordinate = seat.XCoordinate,
                            YCoordinate = seat.YCoordinate,
                            IsActive = seat.IsActive,
                            HasActiveOrder = true
                        },
                        Order = new OrderDto
                        {
                            Id = order.Id,
                            TicketNumber = order.TicketNumber,
                            SeatNumber = order.SeatNumber,
                            TotalAmount = order.TotalAmount,
                            Status = order.Status,
                            CreatedAt = order.CreatedAt,
                            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                            {
                                Id = oi.Id,
                                DrinkId = oi.DrinkId,
                                DrinkName = oi.Drink.Name,
                                Quantity = oi.Quantity,
                                UnitPrice = oi.UnitPrice,
                                TotalPrice = oi.TotalPrice,
                                SpecialInstructions = oi.SpecialInstructions
                            }).ToList()
                        }
                    });
                }
            }
        }

        return Ok(seatOrders);
    }

    private static string GetSectionColor(string section)
    {
        return section switch
        {
            "A" => "#28a745",
            "B" => "#007bff",
            "C" => "#ffc107",
            "VIP" => "#dc3545",
            _ => "#6c757d"
        };
    }
}
