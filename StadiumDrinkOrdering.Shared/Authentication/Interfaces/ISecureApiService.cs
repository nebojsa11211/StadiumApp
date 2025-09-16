using System.Text.Json;

namespace StadiumDrinkOrdering.Shared.Authentication.Interfaces;

/// <summary>
/// Unified interface for making authenticated API calls
/// </summary>
public interface ISecureApiService
{
    /// <summary>
    /// Gets the base URL for API calls
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Event fired when authentication fails (401/403 responses)
    /// </summary>
    event Action? OnAuthenticationFailed;

    /// <summary>
    /// Makes an authenticated GET request
    /// </summary>
    Task<T?> GetAsync<T>(string endpoint);

    /// <summary>
    /// Makes an authenticated GET request returning raw response
    /// </summary>
    Task<HttpResponseMessage> GetAsync(string endpoint);

    /// <summary>
    /// Makes an authenticated POST request with JSON body
    /// </summary>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);

    /// <summary>
    /// Makes an authenticated POST request returning raw response
    /// </summary>
    Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data);

    /// <summary>
    /// Makes an authenticated POST request without body
    /// </summary>
    Task<HttpResponseMessage> PostAsync(string endpoint);

    /// <summary>
    /// Makes an authenticated PUT request with JSON body
    /// </summary>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);

    /// <summary>
    /// Makes an authenticated PUT request returning raw response
    /// </summary>
    Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data);

    /// <summary>
    /// Makes an authenticated DELETE request
    /// </summary>
    Task<HttpResponseMessage> DeleteAsync(string endpoint);

    /// <summary>
    /// Makes an authenticated PATCH request with JSON body
    /// </summary>
    Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data);

    /// <summary>
    /// Makes an authenticated PATCH request returning raw response
    /// </summary>
    Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T data);

    /// <summary>
    /// Uploads a file with authentication
    /// </summary>
    Task<HttpResponseMessage> UploadFileAsync(string endpoint, Stream fileStream, string fileName, string? contentType = null);

    /// <summary>
    /// Downloads a file with authentication
    /// </summary>
    Task<Stream?> DownloadFileAsync(string endpoint);

    /// <summary>
    /// Makes a multipart form data request with authentication
    /// </summary>
    Task<HttpResponseMessage> PostMultipartAsync(string endpoint, MultipartFormDataContent content);

    /// <summary>
    /// Sets custom JSON serializer options
    /// </summary>
    void SetJsonOptions(JsonSerializerOptions options);

    /// <summary>
    /// Adds a custom header to all requests
    /// </summary>
    void AddDefaultHeader(string name, string value);

    /// <summary>
    /// Removes a custom header from all requests
    /// </summary>
    void RemoveDefaultHeader(string name);

    /// <summary>
    /// Checks if the service has a valid authentication token
    /// </summary>
    bool HasValidToken();

    /// <summary>
    /// Gets the current authentication token
    /// </summary>
    string? GetCurrentToken();
}