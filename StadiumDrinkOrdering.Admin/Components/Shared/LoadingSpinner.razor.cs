using Microsoft.AspNetCore.Components;

namespace StadiumDrinkOrdering.Admin.Components.Shared;

public partial class LoadingSpinnerBase : ComponentBase
{
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public bool Overlay { get; set; } = false;
    [Parameter] public string Size { get; set; } = "Medium"; // Small, Medium, Large
    [Parameter] public string Message { get; set; } = "";
    [Parameter] public string Color { get; set; } = "primary"; // primary, success, warning, danger
}