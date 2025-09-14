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
    }
}