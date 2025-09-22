using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Components.Stadium;

public partial class StadiumSvgRenderer : ComponentBase
{
    [Parameter] public StadiumSvgLayoutDto? Layout { get; set; }
    [Parameter] public Dictionary<string, SvgEventSeatStatusDto>? EventSeatStatus { get; set; }
    [Parameter] public bool ShowSeatCount { get; set; } = false;
    [Parameter] public EventCallback<SectorSvgDto> OnSectorClick { get; set; }
    [Parameter] public EventCallback<(SectorSvgDto sector, bool isHovering)> OnSectorHover { get; set; }

    // Enhanced visual properties
    private readonly Dictionary<string, string> _tribuneGradients = new()
    {
        ["N"] = "url(#north-gradient)",
        ["S"] = "url(#south-gradient)",
        ["E"] = "url(#east-gradient)",
        ["W"] = "url(#west-gradient)"
    };

    private readonly Dictionary<string, string> _tribuneAccentColors = new()
    {
        ["N"] = "#60a5fa",
        ["S"] = "#34d399",
        ["E"] = "#fbbf24",
        ["W"] = "#a78bfa"
    };

    private readonly HashSet<string> _vipTypes = new() { "VIP", "PREMIUM", "CORPORATE", "BOX" };

    private string GetSectorFillGradient(SectorSvgDto sector, string tribuneCode)
    {
        // Priority 1: Event occupancy status with enhanced gradients
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var occupancyPercentage = seatStatus.OccupancyPercentage;

            if (occupancyPercentage >= 90)
                return "url(#full-gradient)"; // Red gradient - nearly full
            else if (occupancyPercentage >= 50)
                return "url(#partial-gradient)"; // Orange gradient - partially full
            else
                return "url(#available-gradient)"; // Green gradient - mostly available
        }

        // Priority 2: VIP/Premium sections get special treatment
        if (IsVipSector(sector))
        {
            return sector.Name.ToUpper().Contains("PREMIUM") ? "url(#premium-gradient)" : "url(#vip-gradient)";
        }

        // Priority 3: Tribune-specific gradients
        if (_tribuneGradients.TryGetValue(tribuneCode.ToUpper(), out var gradient))
        {
            return gradient;
        }

        // Fallback: Use sector's custom color or default
        var customColor = sector.Style?.FillColor;
        return !string.IsNullOrEmpty(customColor) ? customColor : "url(#north-gradient)";
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

        // Add VIP styling
        if (IsVipSector(sector))
        {
            classes.Add("sector-vip");
        }

        // Add premium styling
        if (sector.Name.ToUpper().Contains("PREMIUM"))
        {
            classes.Add("sector-premium");
        }

        return string.Join(" ", classes);
    }

    private string GetTribuneClass(string tribuneCode)
    {
        return $"tribune-{tribuneCode.ToLower()}";
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

    // Enhanced visual helper methods
    private string GetSectorStroke(SectorSvgDto sector)
    {
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var occupancyPercentage = seatStatus.OccupancyPercentage;
            if (occupancyPercentage >= 90) return "#dc2626";
            if (occupancyPercentage >= 50) return "#ea580c";
            return "#16a34a";
        }
        return IsVipSector(sector) ? "#f59e0b" : "#e2e8f0";
    }

    private string GetSectorAccentColor(SectorSvgDto sector, string tribuneCode)
    {
        if (_tribuneAccentColors.TryGetValue(tribuneCode.ToUpper(), out var accentColor))
        {
            return accentColor;
        }
        return "#3b82f6";
    }

    private int GetSectorBorderRadius(SectorSvgDto sector)
    {
        if (IsVipSector(sector)) return 12;
        return sector.Style?.BorderRadius ?? 8;
    }

    private string GetEnhancedAriaLabel(SectorSvgDto sector)
    {
        var label = $"Sector {sector.Name}";

        if (IsVipSector(sector))
        {
            label += ", VIP section";
        }

        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var occupancyPercentage = seatStatus.OccupancyPercentage;
            var availableSeats = sector.TotalSeats - sector.OccupiedSeats;

            if (occupancyPercentage >= 90)
                label += $", nearly full, {availableSeats} seats available";
            else if (occupancyPercentage >= 50)
                label += $", partially occupied, {availableSeats} seats available";
            else
                label += $", mostly available, {availableSeats} seats available";
        }
        else if (sector.TotalSeats > 0)
        {
            label += $", {sector.TotalSeats} total seats";
        }

        return label;
    }

    private double GetOccupancyPercentage(SectorSvgDto sector)
    {
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            return seatStatus.OccupancyPercentage;
        }
        return 0;
    }

    private string GetOccupancyBarColor(SectorSvgDto sector)
    {
        var percentage = GetOccupancyPercentage(sector);
        if (percentage >= 90) return "#dc2626";
        if (percentage >= 50) return "#ea580c";
        return "#16a34a";
    }

    private string GetDynamicFontSize(SectorSvgDto sector)
    {
        var width = sector.Bounds.Width;
        var height = sector.Bounds.Height;
        var minDimension = Math.Min(width, height);

        // Calculate font size based on sector dimensions
        var fontSize = Math.Max(8, Math.Min(16, minDimension / 8));
        return $"{fontSize}px";
    }

    private string GetTextColor(SectorSvgDto sector)
    {
        if (IsVipSector(sector)) return "#92400e";
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var percentage = seatStatus.OccupancyPercentage;
            if (percentage >= 90) return "#7f1d1d";
            if (percentage >= 50) return "#9a3412";
            return "#14532d";
        }
        return "#1f2937";
    }

    private string GetSeatCountColor(SectorSvgDto sector)
    {
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var percentage = seatStatus.OccupancyPercentage;
            if (percentage >= 90) return "#991b1b";
            if (percentage >= 50) return "#c2410c";
            return "#166534";
        }
        return "#6b7280";
    }

    private string GetSeatCountText(SectorSvgDto sector)
    {
        if (EventSeatStatus != null && EventSeatStatus.TryGetValue(sector.Code, out var seatStatus))
        {
            var available = sector.TotalSeats - sector.OccupiedSeats;
            return $"{available}/{sector.TotalSeats}";
        }
        return sector.TotalSeats > 0 ? $"{sector.TotalSeats}" : "";
    }

    private bool IsVipSector(SectorSvgDto sector)
    {
        var name = sector.Name.ToUpper();
        return _vipTypes.Any(vip => name.Contains(vip)) ||
               name.Contains("LUXURY") ||
               name.Contains("SUITE") ||
               name.Contains("EXECUTIVE");
    }

    private double GetTextWidth(SectorSvgDto sector)
    {
        // Approximate text width calculation
        var fontSize = double.Parse(GetDynamicFontSize(sector).Replace("px", ""));
        return sector.Name.Length * fontSize * 0.6;
    }

    private double GetTextHeight(SectorSvgDto sector)
    {
        var fontSize = double.Parse(GetDynamicFontSize(sector).Replace("px", ""));
        return fontSize + (ShowSeatCount ? fontSize * 0.7 : 0);
    }

    private string GetSeatCountFontSize(SectorSvgDto sector)
    {
        var baseFontSize = int.Parse(GetDynamicFontSize(sector).Replace("px", ""));
        var seatCountFontSize = (int)(baseFontSize * 0.7);
        return $"{seatCountFontSize}px";
    }
}