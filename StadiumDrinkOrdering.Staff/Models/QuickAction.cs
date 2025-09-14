using Microsoft.AspNetCore.Components;

namespace StadiumDrinkOrdering.Staff.Models;

public class QuickAction
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string IconClass { get; set; } = string.Empty;
    public string ButtonClass { get; set; } = "btn-primary";
    public int BadgeCount { get; set; }
    public bool IsDisabled { get; set; }
    public EventCallback OnClick { get; set; }

    public static QuickAction CreateLink(string title, string url, string iconClass = "", string buttonClass = "btn-primary", string? description = null, int badgeCount = 0)
    {
        return new QuickAction
        {
            Title = title,
            Url = url,
            IconClass = iconClass,
            ButtonClass = buttonClass,
            Description = description,
            BadgeCount = badgeCount
        };
    }

    public static QuickAction CreateButton(string title, EventCallback onClick, string iconClass = "", string buttonClass = "btn-primary", string? description = null, int badgeCount = 0, bool isDisabled = false)
    {
        return new QuickAction
        {
            Title = title,
            OnClick = onClick,
            IconClass = iconClass,
            ButtonClass = buttonClass,
            Description = description,
            BadgeCount = badgeCount,
            IsDisabled = isDisabled
        };
    }
}