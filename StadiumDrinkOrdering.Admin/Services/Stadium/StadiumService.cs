using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Stadium
{
    public class StadiumService : BaseApiService, IStadiumService
    {
        public StadiumService(HttpClient httpClient, ICentralizedLoggingClient loggingClient)
            : base(httpClient, loggingClient)
        {
        }

        public async Task<StadiumLayoutDto?> GetStadiumLayoutAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("StadiumStructure/full-structure");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<StadiumLayoutDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetStadiumLayout", "Failed to retrieve stadium layout");
            }
            return null;
        }

        public async Task<StadiumSummaryDto?> GetStadiumSummaryAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("StadiumStructure/summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<StadiumSummaryDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetStadiumSummary", "Failed to retrieve stadium summary");
            }
            return null;
        }

        public async Task<bool> ImportStadiumStructureAsync(Stream fileStream)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(fileStream), "file", "stadium.json");
                var response = await HttpClient.PostAsync("StadiumStructure/import/json", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ImportStadiumStructure",
                    "Failed to import stadium structure from stream");
                return false;
            }
        }

        public async Task<bool> ImportStadiumStructureAsync(string jsonContent)
        {
            try
            {
                var content = CreateJsonContent(jsonContent);
                var response = await HttpClient.PostAsync("StadiumStructure/import/json", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ImportStadiumStructure", "Failed to import stadium structure");
            }
            return false;
        }

        public async Task<string?> ExportStadiumStructureAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("StadiumStructure/export/json");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ExportStadiumStructure", "Failed to export stadium structure");
            }
            return null;
        }

        public async Task<bool> ClearStadiumStructureAsync()
        {
            try
            {
                var response = await HttpClient.DeleteAsync("StadiumStructure/clear");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ClearStadiumStructure", "Failed to clear stadium structure");
            }
            return false;
        }
    }
}