using System.Text;
using System.Text.Json;
using System.Net;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Base
{
    public abstract class BaseApiService
    {
        protected readonly HttpClient HttpClient;
        protected readonly ICentralizedLoggingClient LoggingClient;
        protected readonly IErrorNotificationService ErrorNotificationService;
        protected readonly ITokenStorageService? TokenStorage;
        protected readonly JsonSerializerOptions JsonOptions;

        protected BaseApiService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService? errorNotificationService = null, ITokenStorageService? tokenStorage = null)
        {
            HttpClient = httpClient;
            LoggingClient = loggingClient;
            ErrorNotificationService = errorNotificationService ?? new NullErrorNotificationService();
            TokenStorage = tokenStorage;
            JsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        protected void SetAuthorizationHeader()
        {
            if (TokenStorage != null && !string.IsNullOrEmpty(TokenStorage.Token))
            {
                HttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenStorage.Token);
            }
            else
            {
                HttpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        protected StringContent CreateJsonContent(object data)
        {
            var json = JsonSerializer.Serialize(data, JsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected T? DeserializeResponse<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        protected async Task LogErrorAsync(Exception ex, string action, string details, string category = "SystemError")
        {
            await LoggingClient.LogErrorAsync(ex, action, category,
                details: details, source: "Admin");
        }

        /// <summary>
        /// Handles API response and shows user-friendly error messages
        /// </summary>
        protected async Task<ApiResponse<T>> HandleApiResponseAsync<T>(HttpResponseMessage response, string operation, bool showUserNotification = true)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = DeserializeResponse<T>(content);
                return ApiResponse<T>.Success(data!);
            }

            // Get error details
            var errorContent = await response.Content.ReadAsStringAsync();
            var endpoint = response.RequestMessage?.RequestUri?.ToString();

            // Show user notification for API error (unless disabled)
            if (showUserNotification)
            {
                await ErrorNotificationService.ShowApiErrorAsync(response.StatusCode, errorContent, endpoint);
            }

            // Log the error for debugging
            await LogErrorAsync(
                new HttpRequestException($"HTTP {response.StatusCode}: {response.ReasonPhrase}"),
                operation,
                $"Endpoint: {endpoint}, Response: {errorContent}"
            );

            return ApiResponse<T>.Failure(response.StatusCode, errorContent, endpoint);
        }

        /// <summary>
        /// Handles exceptions and shows user-friendly error messages
        /// </summary>
        protected async Task<ApiResponse<T>> HandleExceptionAsync<T>(Exception ex, string operation, string? endpoint = null, bool showUserNotification = true)
        {
            // Show user notification for exception (unless disabled)
            if (showUserNotification)
            {
                if (ex is UnauthorizedAccessException)
                {
                    await ErrorNotificationService.ShowAuthenticationErrorAsync();
                }
                else if (ex is TimeoutException || ex is TaskCanceledException)
                {
                    await ErrorNotificationService.ShowErrorAsync(
                        "⏱️ The request is taking longer than expected. Please try again.",
                        "Request Timeout"
                    );
                }
                else if (ex is HttpRequestException)
                {
                    await ErrorNotificationService.ShowNetworkErrorAsync(operation);
                }
                else
                {
                    await ErrorNotificationService.ShowErrorAsync(
                        "An unexpected error occurred. Please try again or contact support.",
                        "Error"
                    );
                }
            }

            // Log the exception
            await LogErrorAsync(ex, operation, $"Endpoint: {endpoint}");

            return ApiResponse<T>.Exception(ex, endpoint);
        }

        /// <summary>
        /// Enhanced GET method with error handling
        /// </summary>
        protected async Task<ApiResponse<T>> GetWithErrorHandlingAsync<T>(string endpoint, bool showUserNotification = true)
        {
            try
            {
                var response = await HttpClient.GetAsync(endpoint);
                return await HandleApiResponseAsync<T>(response, $"GET {endpoint}", showUserNotification);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync<T>(ex, $"GET {endpoint}", endpoint, showUserNotification);
            }
        }

        /// <summary>
        /// Enhanced POST method with error handling
        /// </summary>
        protected async Task<ApiResponse<T>> PostWithErrorHandlingAsync<T>(string endpoint, object? data = null, bool showUserNotification = true)
        {
            try
            {
                var content = data != null ? CreateJsonContent(data) : null;
                var response = await HttpClient.PostAsync(endpoint, content);
                return await HandleApiResponseAsync<T>(response, $"POST {endpoint}", showUserNotification);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync<T>(ex, $"POST {endpoint}", endpoint, showUserNotification);
            }
        }

        /// <summary>
        /// Enhanced PUT method with error handling
        /// </summary>
        protected async Task<ApiResponse<T>> PutWithErrorHandlingAsync<T>(string endpoint, object? data = null, bool showUserNotification = true)
        {
            try
            {
                var content = data != null ? CreateJsonContent(data) : null;
                var response = await HttpClient.PutAsync(endpoint, content);
                return await HandleApiResponseAsync<T>(response, $"PUT {endpoint}", showUserNotification);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync<T>(ex, $"PUT {endpoint}", endpoint, showUserNotification);
            }
        }

        /// <summary>
        /// Enhanced DELETE method with error handling
        /// </summary>
        protected async Task<ApiResponse> DeleteWithErrorHandlingAsync(string endpoint, bool showUserNotification = true)
        {
            try
            {
                var response = await HttpClient.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    return ApiResponse.Success();
                }

                // Get error details
                var errorContent = await response.Content.ReadAsStringAsync();
                var endpointUri = response.RequestMessage?.RequestUri?.ToString();

                // Show user notification for API error (unless disabled)
                if (showUserNotification)
                {
                    await ErrorNotificationService.ShowApiErrorAsync(response.StatusCode, errorContent, endpointUri);
                }

                // Log the error
                await LogErrorAsync(
                    new HttpRequestException($"HTTP {response.StatusCode}: {response.ReasonPhrase}"),
                    $"DELETE {endpoint}",
                    $"Endpoint: {endpointUri}, Response: {errorContent}"
                );

                return ApiResponse.Failure(response.StatusCode, errorContent, endpointUri);
            }
            catch (Exception ex)
            {
                // Show user notification for exception (unless disabled)
                if (showUserNotification)
                {
                    if (ex is UnauthorizedAccessException)
                    {
                        await ErrorNotificationService.ShowAuthenticationErrorAsync();
                    }
                    else if (ex is TimeoutException || ex is TaskCanceledException)
                    {
                        await ErrorNotificationService.ShowErrorAsync(
                            "⏱️ The request is taking longer than expected. Please try again.",
                            "Request Timeout"
                        );
                    }
                    else if (ex is HttpRequestException)
                    {
                        await ErrorNotificationService.ShowNetworkErrorAsync($"DELETE {endpoint}");
                    }
                    else
                    {
                        await ErrorNotificationService.ShowErrorAsync(
                            "An unexpected error occurred while deleting. Please try again.",
                            "Delete Error"
                        );
                    }
                }

                // Log the exception
                await LogErrorAsync(ex, $"DELETE {endpoint}", $"Endpoint: {endpoint}");

                return ApiResponse.Exception(ex, endpoint);
            }
        }
    }
}