using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await AuthStateService.InitializeAsync();
    }

    private void HandleNewOrder(string orderInfo)
    {
        // Handle new order notifications
        // Format: "orderId:customerName:seatNumber"
        StateHasChanged();
    }

    private void HandleOrderStatusChanged(string statusUpdate)
    {
        // Handle real-time order status updates
        // Format: "orderId:status"
        StateHasChanged();
    }

    private void HandleNotification(string notification)
    {
        // Handle real-time notifications
        // Format: "type:message"
        StateHasChanged();
    }

    private void HandleSystemAlert(string alert)
    {
        // Handle system alerts
        // Format: "severity:message"
        StateHasChanged();
    }

    private async Task HandleLogout()
    {
        Console.WriteLine("HandleLogout called");
        await AuthStateService.LogoutAsync();
        Console.WriteLine("LogoutAsync completed");
        NavigationManager.NavigateTo("/login", forceLoad: true);
        Console.WriteLine("Navigation triggered");
    }
}