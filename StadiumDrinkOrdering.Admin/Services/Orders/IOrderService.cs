using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Services.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>?> GetOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int id);
        Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}