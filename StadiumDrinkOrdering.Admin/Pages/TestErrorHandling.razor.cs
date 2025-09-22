using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class TestErrorHandling : ComponentBase
{
    [Inject] private IErrorNotificationService ErrorNotificationService { get; set; } = default!;
    [Inject] private IAdminApiService ApiService { get; set; } = default!;

    private async Task TestErrorNotification()
    {
        await ErrorNotificationService.ShowErrorAsync(
            "This is a test error message! 🚨 The error handling system is working correctly.",
            "Test Error"
        );
    }

    private async Task TestWarningNotification()
    {
        await ErrorNotificationService.ShowWarningAsync(
            "This is a test warning message! ⚠️ Something needs your attention.",
            "Test Warning"
        );
    }

    private async Task TestSuccessNotification()
    {
        await ErrorNotificationService.ShowSuccessAsync(
            "This is a test success message! ✅ Everything worked perfectly.",
            "Test Success"
        );
    }

    private async Task TestInfoNotification()
    {
        await ErrorNotificationService.ShowInfoAsync(
            "This is a test info message! ℹ️ Here's some useful information.",
            "Test Information"
        );
    }

    private async Task TestAuthError()
    {
        await ErrorNotificationService.ShowAuthenticationErrorAsync("/test-error-handling");
    }

    private async Task TestApiConnection()
    {
        try
        {
            var drinks = await ApiService.GetDrinksAsync();
            if (drinks != null && drinks.Any())
            {
                await ErrorNotificationService.ShowSuccessAsync(
                    $"✅ API is working! Found {drinks.Count()} drinks.",
                    "API Connection Test"
                );
            }
            else
            {
                await ErrorNotificationService.ShowWarningAsync(
                    "⚠️ API responded but returned no drinks. This could be normal if no drinks are configured.",
                    "API Connection Test"
                );
            }
        }
        catch (Exception ex)
        {
            // This will be handled by the error handling system automatically
            await ErrorNotificationService.ShowErrorAsync(
                $"❌ API connection failed: {ex.Message}",
                "API Connection Test"
            );
        }
    }

    private async Task TestDrinksFetch()
    {
        var drinks = await ApiService.GetDrinksAsync();
        if (drinks != null && drinks.Any())
        {
            await ErrorNotificationService.ShowSuccessAsync(
                $"🥤 Successfully loaded {drinks.Count()} drinks!",
                "Drinks Test"
            );
        }
        else
        {
            await ErrorNotificationService.ShowInfoAsync(
                "🥤 No drinks returned (this may have triggered an error notification if API is down)",
                "Drinks Test"
            );
        }
    }

    private async Task TestOrdersFetch()
    {
        var orders = await ApiService.GetOrdersAsync();
        if (orders != null && orders.Any())
        {
            await ErrorNotificationService.ShowSuccessAsync(
                $"📦 Successfully loaded {orders.Count()} orders!",
                "Orders Test"
            );
        }
        else
        {
            await ErrorNotificationService.ShowInfoAsync(
                "📦 No orders returned (this may have triggered an error notification if API is down)",
                "Orders Test"
            );
        }
    }
}