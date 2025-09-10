using Microsoft.AspNetCore.Components;

namespace StadiumDrinkOrdering.Admin.Components.Shared;

public partial class SuccessAnimationBase : ComponentBase
{
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public bool Overlay { get; set; } = false;
    [Parameter] public string Size { get; set; } = "Medium"; // Small, Medium, Large
    [Parameter] public string Message { get; set; } = "Success!";
    [Parameter] public string SubMessage { get; set; } = "";
    [Parameter] public int AutoHideDelay { get; set; } = 3000;
    [Parameter] public EventCallback OnAnimationComplete { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (IsVisible && AutoHideDelay > 0)
        {
            await Task.Delay(AutoHideDelay);
            IsVisible = false;
            await OnAnimationComplete.InvokeAsync();
            StateHasChanged();
        }
    }
}