using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Drinks
{
    public class DrinkService : BaseApiService, IDrinkService
    {
        public DrinkService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, errorNotificationService, tokenStorage)
        {
        }

        public async Task<IEnumerable<DrinkDto>?> GetDrinksAsync()
        {
            var result = await GetWithErrorHandlingAsync<IEnumerable<DrinkDto>>("drinks");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<DrinkDto?> GetDrinkAsync(int id)
        {
            var result = await GetWithErrorHandlingAsync<DrinkDto>($"drinks/{id}");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto)
        {
            var result = await PostWithErrorHandlingAsync<DrinkDto>("drinks", createDrinkDto);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<DrinkDto?> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto)
        {
            var result = await PutWithErrorHandlingAsync<DrinkDto>($"drinks/{id}", updateDrinkDto);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            var result = await DeleteWithErrorHandlingAsync($"drinks/{id}");
            return result.IsSuccess;
        }

        public async Task<DrinkDto?> RestockDrinkAsync(int id, RestockDrinkDto restockDto)
        {
            var result = await PostWithErrorHandlingAsync<DrinkDto>($"drinks/{id}/restock", restockDto);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<IEnumerable<StockMovementDto>?> GetStockMovementsAsync(int id, int take = 50)
        {
            var result = await GetWithErrorHandlingAsync<IEnumerable<StockMovementDto>>($"drinks/{id}/stock-movements?take={take}");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<IEnumerable<CategoryDto>?> GetCategoriesAsync()
        {
            var result = await GetWithErrorHandlingAsync<IEnumerable<CategoryDto>>("categories");
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<CategoryDto?> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var result = await PostWithErrorHandlingAsync<CategoryDto>("categories", createCategoryDto);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var result = await PutWithErrorHandlingAsync<CategoryDto>($"categories/{id}", updateCategoryDto);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<(bool Success, string? Error)> DeleteCategoryAsync(int id)
        {
            // Suppress the generic error toast so the page can surface the API's specific
            // "category in use" message from the response body instead.
            var result = await DeleteWithErrorHandlingAsync($"categories/{id}", showUserNotification: false);
            return (result.IsSuccess, result.IsSuccess ? null : result.ErrorMessage);
        }
    }
}