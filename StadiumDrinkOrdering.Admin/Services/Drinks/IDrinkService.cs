using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Drinks
{
    public interface IDrinkService
    {
        Task<IEnumerable<DrinkDto>?> GetDrinksAsync();
        Task<DrinkDto?> GetDrinkAsync(int id);
        Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto);
        Task<DrinkDto?> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto);
        Task<bool> DeleteDrinkAsync(int id);

        Task<DrinkDto?> RestockDrinkAsync(int id, RestockDrinkDto restockDto);
        Task<IEnumerable<StockMovementDto>?> GetStockMovementsAsync(int id, int take = 50);

        Task<IEnumerable<CategoryDto>?> GetCategoriesAsync();
        Task<CategoryDto?> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<(bool Success, string? Error)> DeleteCategoryAsync(int id);
    }
}