using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Orders
{
    public class OrderService : BaseApiService, IOrderService
    {
        public OrderService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService)
            : base(httpClient, loggingClient, errorNotificationService)
        {
        }

        public async Task<IEnumerable<OrderDto>?> GetOrdersAsync()
        {
            var result = await GetWithErrorHandlingAsync<IEnumerable<OrderDto>>("api/orders");
            return result.IsSuccess ? result.Data : Array.Empty<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            var result = await GetWithErrorHandlingAsync<OrderDto>($"api/orders/{id}");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order)
        {
            var result = await PutWithErrorHandlingAsync<OrderDto>($"api/orders/{id}", order);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var result = await DeleteWithErrorHandlingAsync($"api/orders/{id}");
            return result.IsSuccess;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var updateData = new { Status = status };
            var result = await PutWithErrorHandlingAsync<object>($"api/orders/{orderId}/status", updateData);
            return result.IsSuccess;
        }
    }
}