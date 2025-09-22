using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Orders
{
    public class OrderService : BaseApiService, IOrderService
    {
        public OrderService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, errorNotificationService, tokenStorage)
        {
        }

        public async Task<IEnumerable<OrderDto>?> GetOrdersAsync()
        {
            try
            {
                // Set authorization header before making the request
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync("orders");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<IEnumerable<OrderDto>>(json) ?? Array.Empty<OrderDto>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await ErrorNotificationService.ShowAuthenticationErrorAsync();
                    throw new UnauthorizedAccessException("Authentication required. Please log in again.");
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetOrders", "Failed to retrieve orders list");
            }
            return Array.Empty<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            var result = await GetWithErrorHandlingAsync<OrderDto>($"orders/{id}");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order)
        {
            var result = await PutWithErrorHandlingAsync<OrderDto>($"orders/{id}", order);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var result = await DeleteWithErrorHandlingAsync($"orders/{id}");
            return result.IsSuccess;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var updateData = new { Status = status };
            var result = await PutWithErrorHandlingAsync<object>($"orders/{orderId}/status", updateData);
            return result.IsSuccess;
        }
    }
}