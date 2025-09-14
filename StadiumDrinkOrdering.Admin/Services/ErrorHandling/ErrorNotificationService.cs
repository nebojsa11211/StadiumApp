using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using StadiumDrinkOrdering.Admin.Services.Auth;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    public class ErrorNotificationService : IErrorNotificationService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigation;
        private readonly IAuthService _authService;
        private readonly ICentralizedLoggingClient _loggingClient;
        private readonly Dictionary<string, DateTime> _lastErrorTime = new();
        private readonly TimeSpan _errorThrottleInterval = TimeSpan.FromSeconds(3);

        public ErrorNotificationService(
            IJSRuntime jsRuntime,
            NavigationManager navigation,
            IAuthService authService,
            ICentralizedLoggingClient loggingClient)
        {
            _jsRuntime = jsRuntime;
            _navigation = navigation;
            _authService = authService;
            _loggingClient = loggingClient;
        }

        public async Task ShowErrorAsync(string message, string? title = "Error", int durationMs = 8000)
        {
            await ShowNotificationAsync("error", message, title, durationMs);
        }

        public async Task ShowWarningAsync(string message, string? title = "Warning", int durationMs = 6000)
        {
            await ShowNotificationAsync("warning", message, title, durationMs);
        }

        public async Task ShowSuccessAsync(string message, string? title = "Success", int durationMs = 5000)
        {
            await ShowNotificationAsync("success", message, title, durationMs);
        }

        public async Task ShowInfoAsync(string message, string? title = "Information", int durationMs = 5000)
        {
            await ShowNotificationAsync("info", message, title, durationMs);
        }

        public async Task ShowApiErrorAsync(HttpStatusCode statusCode, string? details = null, string? endpoint = null)
        {
            // Throttle error messages for the same endpoint
            if (IsThrottled(endpoint ?? statusCode.ToString()))
                return;

            // Log the API error for debugging
            await _loggingClient.LogErrorAsync(
                new HttpRequestException($"API Error: {statusCode}"),
                "ApiError",
                "SystemError",
                details: $"Endpoint: {endpoint}, Details: {details}",
                source: "Admin"
            );

            // Handle special cases
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    await ShowAuthenticationErrorAsync();
                    return;

                case HttpStatusCode.Forbidden:
                    await ShowPermissionDeniedAsync(endpoint);
                    return;
            }

            // Show appropriate user message
            var errorInfo = ErrorMessageMappings.GetErrorInfo(statusCode, endpoint);
            var severityType = errorInfo.Severity switch
            {
                ErrorSeverity.Critical => "error",
                ErrorSeverity.Error => "error",
                ErrorSeverity.Warning => "warning",
                ErrorSeverity.Info => "info",
                ErrorSeverity.Success => "success",
                _ => "error"
            };

            await ShowNotificationAsync(severityType, errorInfo.Message, errorInfo.Title, 8000);
        }

        public async Task ShowAuthenticationErrorAsync(string? returnUrl = null)
        {
            await _authService.LogoutAsync(); // Clear invalid tokens

            await ShowErrorAsync(
                "🔐 Your session has expired. Redirecting to login page...",
                "Authentication Required",
                durationMs: 3000
            );

            // Redirect after showing message
            await Task.Delay(1000);
            var loginUrl = "/login";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                loginUrl += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";
            }
            _navigation.NavigateTo(loginUrl, forceLoad: true);
        }

        public async Task ShowPermissionDeniedAsync(string? resource = null)
        {
            var message = string.IsNullOrEmpty(resource)
                ? "❌ You don't have permission to perform this action."
                : $"❌ You don't have permission to access {resource}.";

            await ShowErrorAsync(message, "Access Denied");
        }

        public async Task ShowNetworkErrorAsync(string? action = null)
        {
            var message = string.IsNullOrEmpty(action)
                ? "🌐 Unable to connect to the server. Please check your internet connection."
                : $"🌐 Unable to connect while {action}. Please check your internet connection.";

            await ShowErrorAsync(message, "Connection Error");
        }

        public async Task<bool> ShowRetryableErrorAsync(string message, Func<Task<bool>> retryAction, string retryButtonText = "Retry")
        {
            // For now, just show the error. We'll enhance this with retry buttons later
            await ShowErrorAsync(message, "Error");
            return false;
        }

        public async Task<bool> ShowConfirmationAsync(string message, string title = "Confirm Action")
        {
            // For now, just show as info. We'll enhance this with confirmation dialogs later
            await ShowInfoAsync(message, title);
            return true;
        }

        public void ClearAllNotifications()
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await _jsRuntime.InvokeVoidAsync("notificationManager.clearAll");
                }
                catch (Exception ex)
                {
                    // Ignore JS errors during disposal
                    Console.WriteLine($"Error clearing notifications: {ex.Message}");
                }
            });
        }

        public async Task ShowLoadingAsync(string message = "Loading...")
        {
            await ShowInfoAsync(message, "Please Wait", durationMs: 0); // 0 duration means no auto-close
        }

        public async Task HideLoadingAsync()
        {
            ClearAllNotifications();
        }

        private async Task ShowNotificationAsync(string type, string message, string? title, int durationMs)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("notificationManager.showToast", type, message, title, durationMs);
            }
            catch (Exception ex)
            {
                // Fallback to console if JS fails
                Console.WriteLine($"[{type.ToUpper()}] {title}: {message}");

                // Log the JS error
                await _loggingClient.LogErrorAsync(ex, "ShowNotification", "UIError",
                    details: $"Failed to show {type} notification: {message}", source: "Admin");
            }
        }

        private bool IsThrottled(string key)
        {
            if (_lastErrorTime.TryGetValue(key, out var lastTime))
            {
                if (DateTime.UtcNow - lastTime < _errorThrottleInterval)
                {
                    return true;
                }
            }

            _lastErrorTime[key] = DateTime.UtcNow;
            return false;
        }
    }
}