using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Drinks
{
    public class DrinkService : BaseApiService, IDrinkService
    {
        public DrinkService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService)
            : base(httpClient, loggingClient, errorNotificationService)
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
    }
}