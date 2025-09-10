using Microsoft.AspNetCore.Components;

namespace StadiumDrinkOrdering.Admin.Components.Shared;

public partial class ProgressBarBase : ComponentBase
{
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public double Value { get; set; } = 0; // 0-100
    [Parameter] public string Size { get; set; } = "Medium"; // Small, Medium, Large
    [Parameter] public string Color { get; set; } = "primary"; // primary, success, warning, danger
    [Parameter] public string Label { get; set; } = "";
    [Parameter] public string SubLabel { get; set; } = "";
    [Parameter] public bool ShowPercentage { get; set; } = true;
    [Parameter] public bool Animated { get; set; } = true;
    [Parameter] public bool Striped { get; set; } = false;

    protected override void OnParametersSet()
    {
        // Clamp value between 0 and 100
        Value = Math.Max(0, Math.Min(100, Value));
    }
}