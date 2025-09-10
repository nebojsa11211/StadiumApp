using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class StadiumInfoPanelBase : ComponentBase
{
    [Parameter] public StadiumViewerDto? StadiumData { get; set; }
    [Parameter] public EventSeatStatusDto? EventSeatStatus { get; set; }
    [Parameter] public Event? SelectedEvent { get; set; }
    [Parameter] public bool IsCollapsed { get; set; } = false;
    [Parameter] public EventCallback OnTogglePanel { get; set; }
    [Parameter] public EventCallback<StadiumStandDto> OnTribuneSelected { get; set; }
    [Parameter] public EventCallback<(StadiumStandDto, bool)> OnTribuneHovered { get; set; }

    protected bool showTechnical = false;

    // Computed properties
    protected int TotalCapacity => StadiumData?.Stands.Sum(s => s.Sectors.Sum(sec => sec.TotalSeats)) ?? 0;
    protected int TotalSectors => StadiumData?.Stands.Sum(s => s.Sectors.Count) ?? 0;
    protected int AverageSeatsPerSector => TotalSectors > 0 ? TotalCapacity / TotalSectors : 0;

    protected int TotalSoldSeats => EventSeatStatus?.SectorSummaries.Values.Sum(s => s.SoldSeats) ?? 0;
    protected int TotalReservedSeats => EventSeatStatus?.SectorSummaries.Values.Sum(s => s.HeldSeats) ?? 0;
    protected int TotalAvailableSeats => EventSeatStatus?.SectorSummaries.Values.Sum(s => s.FreeSeats) ?? 0;
    protected decimal OverallOccupancyPercentage 
    {
        get
        {
            if (TotalCapacity == 0) return 0;
            return Math.Round((decimal)(TotalSoldSeats + TotalReservedSeats) / TotalCapacity * 100, 1);
        }
    }

    protected async Task TogglePanel()
    {
        IsCollapsed = !IsCollapsed;
        if (OnTogglePanel.HasDelegate)
            await OnTogglePanel.InvokeAsync();
    }

    protected async Task OnTribuneClick(StadiumStandDto stand)
    {
        if (OnTribuneSelected.HasDelegate)
            await OnTribuneSelected.InvokeAsync(stand);
    }

    protected async Task OnTribuneHover(StadiumStandDto stand, bool isHovering)
    {
        if (OnTribuneHovered.HasDelegate)
            await OnTribuneHovered.InvokeAsync((stand, isHovering));
    }

    protected decimal GetTribuneOccupancy(StadiumStandDto stand)
    {
        if (EventSeatStatus == null || !stand.Sectors.Any()) return 0;

        var tribuneTotal = stand.Sectors.Sum(s => s.TotalSeats);
        if (tribuneTotal == 0) return 0;

        var tribuneOccupied = stand.Sectors
            .Where(s => EventSeatStatus.SectorSummaries.ContainsKey(s.Id))
            .Sum(s => EventSeatStatus.SectorSummaries[s.Id].SoldSeats + EventSeatStatus.SectorSummaries[s.Id].HeldSeats);

        return Math.Round((decimal)tribuneOccupied / tribuneTotal * 100, 1);
    }
}