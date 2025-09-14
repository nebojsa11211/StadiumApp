using System.Net;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    public interface IErrorNotificationService
    {
        // Primary notification methods
        Task ShowErrorAsync(string message, string? title = "Error", int durationMs = 8000);
        Task ShowWarningAsync(string message, string? title = "Warning", int durationMs = 6000);
        Task ShowSuccessAsync(string message, string? title = "Success", int durationMs = 5000);
        Task ShowInfoAsync(string message, string? title = "Information", int durationMs = 5000);

        // Specialized API error methods
        Task ShowApiErrorAsync(HttpStatusCode statusCode, string? details = null, string? endpoint = null);
        Task ShowAuthenticationErrorAsync(string? returnUrl = null);
        Task ShowPermissionDeniedAsync(string? resource = null);
        Task ShowNetworkErrorAsync(string? action = null);

        // Interactive notifications
        Task<bool> ShowRetryableErrorAsync(string message, Func<Task<bool>> retryAction, string retryButtonText = "Retry");
        Task<bool> ShowConfirmationAsync(string message, string title = "Confirm Action");

        // Management methods
        void ClearAllNotifications();
        Task ShowLoadingAsync(string message = "Loading...");
        Task HideLoadingAsync();
    }
}