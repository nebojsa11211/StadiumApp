using System.Text.Json;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Service for accessing dynamic stadium SVG layouts from API
/// </summary>
public class StadiumSvgService : IStadiumSvgService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StadiumSvgService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public StadiumSvgService(HttpClient httpClient, ILogger<StadiumSvgService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Get the current stadium SVG layout
    /// </summary>
    public async Task<StadiumSvgLayoutDto?> GetStadiumLayoutAsync()
    {
        try
        {
            _logger.LogInformation("Fetching stadium SVG layout from API");
            
            var response = await _httpClient.GetAsync("stadium-svg/layout");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var layout = JsonSerializer.Deserialize<StadiumSvgLayoutDto>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully fetched stadium layout with {StandCount} stands", 
                    layout?.Stands?.Count ?? 0);
                
                return layout;
            }
            else
            {
                _logger.LogWarning("Failed to fetch stadium layout: {StatusCode} - {ReasonPhrase}", 
                    response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stadium SVG layout");
            return null;
        }
    }

    /// <summary>
    /// Get stadium layout with event-specific occupancy data
    /// </summary>
    public async Task<StadiumSvgLayoutDto?> GetStadiumLayoutWithEventDataAsync(int eventId)
    {
        try
        {
            _logger.LogInformation("Fetching stadium SVG layout with event data for event {EventId}", eventId);
            
            var response = await _httpClient.GetAsync($"stadium-svg/layout/event/{eventId}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var layout = JsonSerializer.Deserialize<StadiumSvgLayoutDto>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully fetched event-specific stadium layout for event {EventId}", eventId);
                return layout;
            }
            else
            {
                _logger.LogWarning("Failed to fetch event-specific stadium layout for event {EventId}: {StatusCode} - {ReasonPhrase}", 
                    eventId, response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stadium SVG layout for event {EventId}", eventId);
            return null;
        }
    }

    /// <summary>
    /// Generate HNK Rijeka layout specifically
    /// </summary>
    public async Task<StadiumSvgLayoutDto?> GetHNKRijekaLayoutAsync()
    {
        try
        {
            _logger.LogInformation("Fetching HNK Rijeka stadium layout from API");
            
            var response = await _httpClient.GetAsync("stadium-svg/layout/hnk-rijeka");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var layout = JsonSerializer.Deserialize<StadiumSvgLayoutDto>(json, _jsonOptions);
                
                _logger.LogInformation("Successfully fetched HNK Rijeka stadium layout");
                return layout;
            }
            else
            {
                _logger.LogWarning("Failed to fetch HNK Rijeka stadium layout: {StatusCode} - {ReasonPhrase}", 
                    response.StatusCode, response.ReasonPhrase);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching HNK Rijeka stadium layout");
            return null;
        }
    }

    /// <summary>
    /// Refresh the stadium layout cache
    /// </summary>
    public async Task<bool> RefreshStadiumLayoutAsync()
    {
        try
        {
            _logger.LogInformation("Refreshing stadium layout cache via API");
            
            var response = await _httpClient.PostAsync("stadium-svg/layout/refresh", null);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully refreshed stadium layout cache");
                return true;
            }
            else
            {
                _logger.LogWarning("Failed to refresh stadium layout cache: {StatusCode} - {ReasonPhrase}", 
                    response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing stadium layout cache");
            return false;
        }
    }

    /// <summary>
    /// Check if stadium data is available
    /// </summary>
    public async Task<bool> HasStadiumDataAsync()
    {
        try
        {
            _logger.LogDebug("Checking stadium data availability via API");
            
            var response = await _httpClient.GetAsync("stadium-svg/status");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var status = JsonSerializer.Deserialize<JsonElement>(json, _jsonOptions);
                
                if (status.TryGetProperty("hasStadiumData", out var hasDataElement))
                {
                    var hasData = hasDataElement.GetBoolean();
                    _logger.LogDebug("Stadium data availability: {HasData}", hasData);
                    return hasData;
                }
            }
            
            _logger.LogWarning("Failed to check stadium data availability: {StatusCode} - {ReasonPhrase}", 
                response.StatusCode, response.ReasonPhrase);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking stadium data availability");
            return false;
        }
    }
}