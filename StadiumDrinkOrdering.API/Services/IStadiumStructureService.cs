using Microsoft.AspNetCore.Http;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IStadiumStructureService
{
    Task<bool> ImportFromJsonAsync(IFormFile jsonFile);
    Task<bool> ImportFromCsvAsync(IFormFile csvFile);
    Task<bool> ClearExistingStructureAsync();
    Task<StadiumSummaryDto> GetStadiumSummaryAsync();
    Task<List<Tribune>> GetFullStructureAsync();
    Task<int> GetTotalSeatsCountAsync();
    Task<bool> GenerateSeatsForSectorAsync(int sectorId);
    Task<MemoryStream> ExportToJsonAsync();
    Task<MemoryStream> ExportToCsvAsync();
}