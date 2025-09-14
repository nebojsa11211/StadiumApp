using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Components.Stadium;

public partial class StadiumSvgRenderer : ComponentBase
{
    [Parameter] public StadiumSvgLayoutDto? Layout { get; set; }
    [Parameter] public Dictionary<string, SvgEventSeatStatusDto>? EventSeatStatus { get; set; }
    [Parameter] public bool ShowSeatCount { get; set; } = false;
    [Parameter] public EventCallback<SectorSvgDto> OnSectorClick { get; set; }
    [Parameter] public EventCallback<(SectorSvgDto sector, bool isHovering)> OnSectorHover { get; set; }

    private string GetSectorFillColor(SectorSvgDto sector)
    {
        // If we have event seat status, use occupancy-based colors
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var occupancyPercentage = seatStatus.OccupancyPercentage;

            if (occupancyPercentage >= 90)
                return "#F44336"; // Red - nearly full
            else if (occupancyPercentage >= 50)
                return "#FF9800"; // Orange - partially full
            else
                return "#4CAF50"; // Green - mostly available
        }

        // Use the sector's default fill color
        return sector.Style?.FillColor ?? "#4A90E2";
    }

    private string GetSectorStatusClass(SectorSvgDto sector)
    {
        var classes = new List<string> { "stadium-sector" };

        // Add occupancy status class if we have event data
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var occupancyPercentage = seatStatus.OccupancyPercentage;

            if (occupancyPercentage >= 90)
                classes.Add("status-full");
            else if (occupancyPercentage >= 50)
                classes.Add("status-partial");
            else
                classes.Add("status-available");
        }

        return string.Join(" ", classes);
    }

    private SectorSvgDto ConvertToSectorSvgDto(SectorSvgDto sector)
    {
        // Return the sector as-is since it's already the correct type
        return sector;
    }

    private async Task HandleSectorClick(SectorSvgDto sector)
    {
        if (OnSectorClick.HasDelegate)
        {
            await OnSectorClick.InvokeAsync(sector);
        }
    }

    private async Task HandleSectorHover(SectorSvgDto sector, bool isHovering)
    {
        if (OnSectorHover.HasDelegate)
        {
            await OnSectorHover.InvokeAsync((sector, isHovering));
        }
    }

    private string GetSectorDisplayText(SectorSvgDto sector)
    {
        var text = sector.Name;
        if (ShowSeatCount && sector.TotalSeats > 0)
        {
            text += $" {sector.TotalSeats - sector.OccupiedSeats}/{sector.TotalSeats}";
        }
        return text;
    }
}