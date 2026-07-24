using System.Net.Http.Json;

namespace StadiumDrinkOrdering.UI;

/// <summary>
/// Typed HttpClient that fetches the API's anonymous <c>/api/system/info</c> endpoint. Used by the
/// small database badge in each frontend app. Never throws: on any failure it returns null so the
/// badge simply hides itself rather than break a page.
/// </summary>
public class SystemInfoClient
{
    private readonly HttpClient _httpClient;

    public SystemInfoClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<SystemInfoDto?> GetSystemInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SystemInfoDto>("api/system/info", cancellationToken);
        }
        catch
        {
            // Diagnostic-only; a badge should never surface an error.
            return null;
        }
    }
}
