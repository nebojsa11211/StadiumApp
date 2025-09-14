using System.Net;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    /// <summary>
    /// Null implementation of IErrorNotificationService for services that don't need error notifications
    /// This is a temporary solution during the transition to the new error handling system
    /// </summary>
    public class NullErrorNotificationService : IErrorNotificationService
    {
        public Task ShowErrorAsync(string message, string? title = "Error", int durationMs = 8000)
        {
            // Console fallback for debugging
            Console.WriteLine($"[ERROR] {title}: {message}");
            return Task.CompletedTask;
        }

        public Task ShowWarningAsync(string message, string? title = "Warning", int durationMs = 6000)
        {
            Console.WriteLine($"[WARNING] {title}: {message}");
            return Task.CompletedTask;
        }

        public Task ShowSuccessAsync(string message, string? title = "Success", int durationMs = 5000)
        {
            Console.WriteLine($"[SUCCESS] {title}: {message}");
            return Task.CompletedTask;
        }

        public Task ShowInfoAsync(string message, string? title = "Information", int durationMs = 5000)
        {
            Console.WriteLine($"[INFO] {title}: {message}");
            return Task.CompletedTask;
        }

        public Task ShowApiErrorAsync(HttpStatusCode statusCode, string? details = null, string? endpoint = null)
        {
            Console.WriteLine($"[API ERROR] {statusCode} at {endpoint}: {details}");
            return Task.CompletedTask;
        }

        public Task ShowAuthenticationErrorAsync(string? returnUrl = null)
        {
            Console.WriteLine($"[AUTH ERROR] Authentication required, return URL: {returnUrl}");
            return Task.CompletedTask;
        }

        public Task ShowPermissionDeniedAsync(string? resource = null)
        {
            Console.WriteLine($"[PERMISSION DENIED] Resource: {resource}");
            return Task.CompletedTask;
        }

        public Task ShowNetworkErrorAsync(string? action = null)
        {
            Console.WriteLine($"[NETWORK ERROR] Action: {action}");
            return Task.CompletedTask;
        }

        public Task<bool> ShowRetryableErrorAsync(string message, Func<Task<bool>> retryAction, string retryButtonText = "Retry")
        {
            Console.WriteLine($"[RETRYABLE ERROR] {message}");
            return Task.FromResult(false);
        }

        public Task<bool> ShowConfirmationAsync(string message, string title = "Confirm Action")
        {
            Console.WriteLine($"[CONFIRMATION] {title}: {message}");
            return Task.FromResult(true);
        }

        public void ClearAllNotifications()
        {
            // No-op
        }

        public Task ShowLoadingAsync(string message = "Loading...")
        {
            Console.WriteLine($"[LOADING] {message}");
            return Task.CompletedTask;
        }

        public Task HideLoadingAsync()
        {
            return Task.CompletedTask;
        }
    }
}