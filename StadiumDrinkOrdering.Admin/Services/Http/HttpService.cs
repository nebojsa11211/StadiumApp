using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Http
{
    public class HttpService : BaseApiService, IHttpService
    {
        private readonly ITokenStorageService _tokenStorage;

        public HttpService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, errorNotificationService)
        {
            _tokenStorage = tokenStorage;
        }

        private void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_tokenStorage.Token))
            {
                HttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenStorage.Token);
            }
            else
            {
                HttpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<T>(json);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await ErrorNotificationService.ShowAuthenticationErrorAsync();
                    throw new UnauthorizedAccessException("Authentication required. Please log in again.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await ErrorNotificationService.ShowErrorAsync($"Request failed: {response.StatusCode}", "API Error");
                    throw new HttpRequestException($"HTTP {response.StatusCode}: {errorContent}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw unauthorized exceptions
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetAsync", $"Failed to GET from {endpoint}");
                await ErrorNotificationService.ShowErrorAsync($"Network error: {ex.Message}", "Connection Error");
                throw;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                SetAuthorizationHeader();
                var content = data != null ? CreateJsonContent(data) : null;
                var response = await HttpClient.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<T>(responseJson);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await ErrorNotificationService.ShowAuthenticationErrorAsync();
                    throw new UnauthorizedAccessException("Authentication required. Please log in again.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await ErrorNotificationService.ShowErrorAsync($"Request failed: {response.StatusCode}", "API Error");
                    throw new HttpRequestException($"HTTP {response.StatusCode}: {errorContent}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw unauthorized exceptions
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "PostAsync", $"Failed to POST to {endpoint}");
                await ErrorNotificationService.ShowErrorAsync($"Network error: {ex.Message}", "Connection Error");
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            SetAuthorizationHeader();
            return await HttpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null)
        {
            SetAuthorizationHeader();
            var content = data != null ? CreateJsonContent(data) : null;
            return await HttpClient.PostAsync(endpoint, content);
        }

        public async Task<(bool success, string errorMessage)> DeleteAsync(string endpoint)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.DeleteAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, errorContent);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "DeleteAsync", $"Failed to DELETE {endpoint}");
                return (false, ex.Message);
            }
        }
    }
}