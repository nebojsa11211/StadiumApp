using Microsoft.AspNetCore.Components;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class SurveyPrompt : ComponentBase
{
    // Demonstrates how a parent component can supply parameters
    [Parameter]
    public string? Title { get; set; }
}