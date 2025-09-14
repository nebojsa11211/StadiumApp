using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Stadium
{
    public interface IStadiumService
    {
        Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
        Task<StadiumSummaryDto?> GetStadiumSummaryAsync();
        Task<bool> ImportStadiumStructureAsync(Stream fileStream);
        Task<bool> ImportStadiumStructureAsync(string jsonContent);
        Task<string?> ExportStadiumStructureAsync();
        Task<bool> ClearStadiumStructureAsync();
    }
}