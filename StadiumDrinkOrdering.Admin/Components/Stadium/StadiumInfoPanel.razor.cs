using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Components.Stadium
{
    public partial class StadiumInfoPanel : ComponentBase
    {
        [Parameter] public StadiumSummaryDto? Summary { get; set; }
        [Parameter] public bool IsLoading { get; set; }
        [Parameter] public bool IsCollapsed { get; set; } = false;
        [Parameter] public EventCallback<bool> IsCollapsedChanged { get; set; }

        private async Task ToggleCollapse()
        {
            IsCollapsed = !IsCollapsed;
            await IsCollapsedChanged.InvokeAsync(IsCollapsed);
        }
    }
}