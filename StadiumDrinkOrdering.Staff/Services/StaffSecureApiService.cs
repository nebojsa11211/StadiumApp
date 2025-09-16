using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using System.Text.Json;

namespace StadiumDrinkOrdering.Staff.Services;

/// <summary>
/// Staff implementation of secure API service that wraps the existing StaffApiService
/// </summary>
public class StaffSecureApiService : ISecureApiService
{
    private readonly IStaffApiService _staffApiService;
    private readonly ITokenStorageService _tokenStorage;
    private readonly HttpClient _httpClient;
    private JsonSerializerOptions _jsonOptions;

    public string BaseUrl { get; }

    public event Action? OnAuthenticationFailed;

    public StaffSecureApiService(
        IStaffApiService staffApiService,
        ITokenStorageService tokenStorage,
        HttpClient httpClient,
        string baseUrl)
    {
        _staffApiService = staffApiService;
        _tokenStorage = tokenStorage;
        _httpClient = httpClient;
        BaseUrl = baseUrl.TrimEnd('/');

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        SetupHttpClient();
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                OnAuthenticationFailed?.Invoke();
            }

            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        await EnsureAuthenticatedAsync();
        return await _httpClient.GetAsync(endpoint);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var response = await PostAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
            }

            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
    {
        await EnsureAuthenticatedAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint)
    {
        await EnsureAuthenticatedAsync();
        return await _httpClient.PostAsync(endpoint, null);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var response = await PutAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
            }

            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
    {
        await EnsureAuthenticatedAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        return await _httpClient.PutAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        await EnsureAuthenticatedAsync();
        return await _httpClient.DeleteAsync(endpoint);
    }

    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        try
        {
            var response = await PatchAsync(endpoint, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
            }

            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T data)
    {
        await EnsureAuthenticatedAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        return await _httpClient.PatchAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> UploadFileAsync(string endpoint, Stream fileStream, string fileName, string? contentType = null)
    {
        await EnsureAuthenticatedAsync();
        var formData = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileStream);

        if (!string.IsNullOrEmpty(contentType))
        {
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        }

        formData.Add(streamContent, "file", fileName);
        return await _httpClient.PostAsync(endpoint, formData);
    }

    public async Task<Stream?> DownloadFileAsync(string endpoint)
    {
        try
        {
            await EnsureAuthenticatedAsync();
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HttpResponseMessage> PostMultipartAsync(string endpoint, MultipartFormDataContent content)
    {
        await EnsureAuthenticatedAsync();
        return await _httpClient.PostAsync(endpoint, content);
    }

    public void SetJsonOptions(JsonSerializerOptions options)
    {
        _jsonOptions = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void AddDefaultHeader(string name, string value)
    {
        if (_httpClient.DefaultRequestHeaders.Contains(name))
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
        }
        _httpClient.DefaultRequestHeaders.Add(name, value);
    }

    public void RemoveDefaultHeader(string name)
    {
        if (_httpClient.DefaultRequestHeaders.Contains(name))
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
        }
    }

    public bool HasValidToken()
    {
        return !string.IsNullOrEmpty(_tokenStorage.Token);
    }

    public string? GetCurrentToken()
    {
        return _tokenStorage.Token;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            SetAuthorizationHeader(token);
            // Also set token in legacy API service for backward compatibility
            _staffApiService.Token = token;
        }
    }

    private void SetupHttpClient()
    {
        _httpClient.BaseAddress = new Uri(BaseUrl + "/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}