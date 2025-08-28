using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Hubs;

[Authorize(Roles = "Admin,Bartender,Waiter")]
public class BartenderHub : Hub
{
    public async Task JoinSection(string section)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"section-{section}");
    }

    public async Task LeaveSection(string section)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"section-{section}");
    }

    public async Task SendOrderUpdate(OrderDto order)
    {
        var section = ExtractSectionFromSeat(order.SeatNumber);
        await Clients.Group($"section-{section}").SendAsync("OrderUpdated", order);
        await Clients.All.SendAsync("OrderUpdated", order);
    }

    public async Task SendNewOrder(OrderDto order)
    {
        var section = ExtractSectionFromSeat(order.SeatNumber);
        await Clients.Group($"section-{section}").SendAsync("NewOrder", order);
        await Clients.All.SendAsync("NewOrder", order);
    }

    public async Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber)
    {
        var section = ExtractSectionFromSeat(seatNumber);
        await Clients.Group($"section-{section}").SendAsync("OrderStatusChanged", orderId, newStatus, seatNumber);
        await Clients.All.SendAsync("OrderStatusChanged", orderId, newStatus, seatNumber);
    }

    public async Task SendSeatHighlight(string seatNumber, bool highlight)
    {
        var section = ExtractSectionFromSeat(seatNumber);
        await Clients.Group($"section-{section}").SendAsync("SeatHighlight", seatNumber, highlight);
        await Clients.All.SendAsync("SeatHighlight", seatNumber, highlight);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    private static string ExtractSectionFromSeat(string seatNumber)
    {
        if (string.IsNullOrEmpty(seatNumber))
            return "Unknown";

        var parts = seatNumber.Split('-');
        if (parts.Length > 0)
        {
            var sectionPart = parts[0];
            return sectionPart.Length > 0 ? sectionPart[0].ToString() : "Unknown";
        }

        return "Unknown";
    }
}
