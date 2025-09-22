using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Stadium
{
    public class StadiumService : BaseApiService, IStadiumService
    {
        public StadiumService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, null, tokenStorage)
        {
        }

        public async Task<StadiumLayoutDto?> GetStadiumLayoutAsync()
        {
            try
            {
                SetAuthorizationHeader();
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
                SetAuthorizationHeader();
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
                SetAuthorizationHeader();
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
                SetAuthorizationHeader();
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
                SetAuthorizationHeader();
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
                SetAuthorizationHeader();
                var response = await HttpClient.DeleteAsync("StadiumStructure/clear");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ClearStadiumStructure", "Failed to clear stadium structure");
            }
            return false;
        }

        public async Task<bool> RefreshStadiumCacheAsync()
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.PostAsync("stadium-svg/layout/refresh", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "RefreshStadiumCache", "Failed to refresh stadium layout cache");
            }
            return false;
        }
    }
}