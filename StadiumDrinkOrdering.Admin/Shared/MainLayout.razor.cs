using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class MainLayout : LayoutComponentBase, IAsyncDisposable
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private string currentTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
    private IJSObjectReference? jsModule;

    protected override void OnInitialized()
    {
        // Only subscribe to authentication state changes
        // Authentication initialization is handled in App.razor.cs
        AuthStateService.OnAuthenticationStateChanged += OnAuthStateChanged;
    }

    private void OnAuthStateChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/server-time.js");
            await jsModule.InvokeVoidAsync("startServerTime", "server-time-value");
        }
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
        // Use normal navigation instead of forceLoad to prevent refresh loops
        NavigationManager.NavigateTo("/login");
        Console.WriteLine("Navigation triggered");
    }

    public async ValueTask DisposeAsync()
    {
        // Unsubscribe from authentication state changes
        AuthStateService.OnAuthenticationStateChanged -= OnAuthStateChanged;

        if (jsModule is not null)
        {
            await jsModule.DisposeAsync();
        }
    }
}