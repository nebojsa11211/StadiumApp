using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class TestNotifications : ComponentBase
{
    [Inject] private IErrorNotificationService ErrorService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private async Task TestErrorNotification()
    {
        await ErrorService.ShowErrorAsync("This is a test error message!", "Test Error");
    }

    private async Task TestTimeoutNotification()
    {
        await ErrorService.ShowErrorAsync(
            "⏱️ The request is taking longer than expected. Please try again.",
            "Request Timeout"
        );
    }

    private async Task TestApiErrorNotification()
    {
        await ErrorService.ShowApiErrorAsync(System.Net.HttpStatusCode.BadRequest, "Test details", "/test/endpoint");
    }

    private async Task TestJavaScriptDirect()
    {
        try
        {
            // Test the exact same call that ErrorNotificationService makes
            await JSRuntime.InvokeVoidAsync("notificationManager.showToast",
                "error",
                "Direct JavaScript test message",
                "JavaScript Test",
                5000);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", $"JavaScript test failed: {ex.Message}");
            await JSRuntime.InvokeVoidAsync("alert", $"JavaScript test failed: {ex.Message}");
        }
    }
}