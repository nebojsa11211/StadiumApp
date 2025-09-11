using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumOverview : ComponentBase
{
    [Inject] public IAdminApiService ApiService { get; set; } = default!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] public ILogger<StadiumOverview> Logger { get; set; } = default!;
    [Inject] public IStadiumSvgService StadiumSvgService { get; set; } = default!;

    // Stadium viewer properties
    private StadiumViewerDto? stadiumData;
    private EventSeatStatusDto? eventSeatStatus;
    private StadiumSvgLayoutDto? stadiumLayout;
    private Dictionary<string, SvgEventSeatStatusDto> svgEventSeatStatus = new();
    private bool isLoading = true;
    private string? errorMessage;
    private bool showLegend = false;
    private bool showOccupancy = true;
    private string? hoveredSector;
    private string searchSeatCode = "";
    private double tooltipX;
    private double tooltipY;

    // Modal properties
    private bool showSectorModal = false;
    private StadiumSectorDto? selectedSector;
    private StadiumSectorDto? sectorWithSeats;
    private List<string> selectedSeats = new();
    private ElementReference svgContainer;
    private ElementReference seatCanvas;

    // Existing properties for event management
    private List<Event>? events;
    private int selectedEventId = 0;
    private Dictionary<string, SeatStatusDto>? seatStatusMap;
    private bool isSimulating = false;

    // Stadium information panel properties
    private bool isInfoPanelCollapsed = false; // Start expanded for better UX
    private Event? selectedEvent;
    private DateTime? lastUpdateTime = DateTime.Now;
    private string? highlightedTribune;

    protected override async Task OnInitializedAsync()
    {
        // Load dynamic stadium layout first (fast and essential)
        await LoadStadiumLayout();
        
        // Load stadium data for the info panel
        await LoadStadiumData();
        
        // Don't wait for events - they can timeout without affecting stadium display
        _ = LoadEventsBackground();
    }
    
    private async Task LoadStadiumLayout()
    {
        try
        {
            isLoading = true;
            StateHasChanged();
            
            Logger.LogInformation("Loading dynamic stadium layout using HNK Rijeka coordinates...");
            
            // Load the stadium layout using our dynamic service
            stadiumLayout = await StadiumSvgService.GetStadiumLayoutAsync();
            
            if (stadiumLayout != null)
            {
                Logger.LogInformation($"Dynamic stadium layout loaded successfully: {stadiumLayout.Name}");
            }
            else
            {
                Logger.LogWarning("Stadium layout service returned null");
                errorMessage = "Failed to generate dynamic stadium layout";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception loading dynamic stadium layout: {Message}", ex.Message);
            errorMessage = $"Error loading dynamic stadium layout: {ex.Message}";
        }
        finally
        {
            isLoading = false;
            Logger.LogInformation($"LoadStadiumLayout completed. stadiumLayout is null: {stadiumLayout == null}");
            StateHasChanged();
        }
    }

    private async Task LoadEventsBackground()
    {
        try
        {
            await Task.Delay(100); // Small delay to ensure stadium renders first
            events = await ApiService.GetEventsAsync();
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // Events loading failed - stadium should still work
            Logger?.LogError(ex, "Failed to load events in background");
        }
    }

    private async Task LoadStadiumData()
    {
        try
        {
            isLoading = true;
            StateHasChanged(); // Ensure UI shows loading state
            
            Logger.LogInformation("Starting to load stadium data from API...");
            
            // Try the new stadium viewer endpoint first, then fallback to Stadium/layout
            try
            {
                var viewerResponse = await ApiService.GetAsync<StadiumViewerDto>("stadium-viewer/overview");
                if (viewerResponse != null)
                {
                    Logger.LogInformation($"Stadium data loaded successfully: {viewerResponse.Name}");
                    stadiumData = viewerResponse;
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Stadium viewer endpoint failed, trying fallback");
            }

            // Fallback to existing Stadium/layout endpoint
            var layoutResponse = await ApiService.GetStadiumLayoutAsync();
            if (layoutResponse != null)
            {
                Logger.LogInformation($"Stadium layout loaded, converting to viewer format");
                stadiumData = ConvertStadiumLayoutToViewer(layoutResponse);
            }
            else
            {
                Logger.LogWarning("Both stadium endpoints returned null");
                errorMessage = "Failed to load stadium data - no stadium structure found";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Exception loading stadium data: {Message}", ex.Message);
            errorMessage = $"Error loading stadium layout: {ex.Message}";
        }
        finally
        {
            isLoading = false;
            Logger.LogInformation($"LoadStadiumData completed. stadiumData is null: {stadiumData == null}");
            StateHasChanged(); // Ensure UI updates after loading
        }
    }

    private StadiumViewerDto ConvertStadiumLayoutToViewer(StadiumLayoutDto layout)
    {
        var viewer = new StadiumViewerDto
        {
            StadiumId = "main-stadium",
            Name = "Main Stadium",
            CoordinateSystem = new StadiumCoordinateSystem
            {
                Width = 1200,
                Height = 900,
                OriginX = 600,
                OriginY = 450,
                Unit = "normalized"
            }
        };

        // Generate professional oval field
        var centerX = viewer.CoordinateSystem.Width / 2.0;
        var centerY = viewer.CoordinateSystem.Height / 2.0;
        var fieldRadiusX = 180.0;
        var fieldRadiusY = 120.0;

        // Create oval field using multiple points for smooth curves
        var fieldPoints = new List<PointDto>();
        for (int i = 0; i < 32; i++)
        {
            var angle = (2 * Math.PI * i) / 32;
            var x = centerX + fieldRadiusX * Math.Cos(angle);
            var y = centerY + fieldRadiusY * Math.Sin(angle);
            fieldPoints.Add(new PointDto(x, y));
        }

        viewer.Field = new StadiumFieldDto
        {
            Polygon = fieldPoints,
            FillColor = "#2d5a2d",
            StrokeColor = "#ffffff"
        };

        // Group sections into stands (tribunes)
        var standMap = new Dictionary<string, List<StadiumSectionDto>>();
        foreach (var section in layout.Sections)
        {
            var standCode = DetermineStandCode(section.SectionName);
            if (!standMap.ContainsKey(standCode))
                standMap[standCode] = new List<StadiumSectionDto>();
            standMap[standCode].Add(section);
        }

        // Create stands with curved layout
        foreach (var kvp in standMap)
        {
            var stand = new StadiumStandDto
            {
                Id = kvp.Key.ToLower(),
                Name = GetStandName(kvp.Key),
                TribuneCode = kvp.Key,
                Sectors = new List<StadiumSectorDto>()
            };

            // Generate stand polygon
            stand.Polygon = GenerateStandPolygon(kvp.Key, viewer.CoordinateSystem, centerX, centerY, fieldRadiusX, fieldRadiusY);

            // Create sectors for this stand
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                var section = kvp.Value[i];
                var sector = new StadiumSectorDto
                {
                    Id = section.SectionName,
                    Name = section.DisplayName,
                    TotalSeats = section.TotalSeats,
                    AvailableSeats = section.TotalSeats - section.ActiveOrders,
                    StandId = stand.Id,
                    FillColor = section.Color ?? "#e3e3e3",
                    HoverColor = "#d4d4d8"
                };

                // Generate sector polygon
                sector.Polygon = GenerateSectorPolygon(kvp.Key, kvp.Value.Count, i, viewer.CoordinateSystem, centerX, centerY, fieldRadiusX, fieldRadiusY);
                
                stand.Sectors.Add(sector);
            }

            viewer.Stands.Add(stand);
        }

        return viewer;
    }

    private string DetermineStandCode(string sectionName)
    {
        if (string.IsNullOrEmpty(sectionName)) return "N";
        var firstChar = sectionName.Substring(0, 1).ToUpper();
        return new[] { "N", "S", "E", "W" }.Contains(firstChar) ? firstChar : "N";
    }

    private string GetStandName(string standCode)
    {
        return standCode switch
        {
            "N" => "North Tribune",
            "S" => "South Tribune", 
            "E" => "East Tribune",
            "W" => "West Tribune",
            _ => "Tribune"
        };
    }

    private List<PointDto> GenerateStandPolygon(string standCode, StadiumCoordinateSystem coords, double centerX, double centerY, double fieldRadiusX, double fieldRadiusY)
    {
        var innerRadiusX = fieldRadiusX + 60;
        var innerRadiusY = fieldRadiusY + 40;
        var outerRadiusX = fieldRadiusX + 160;
        var outerRadiusY = fieldRadiusY + 120;
        
        var points = new List<PointDto>();
        
        // Get tribune angles
        var (startAngle, endAngle) = GetStandAngles(standCode);
        var angleStep = (endAngle - startAngle) / 20;
        
        // Inner arc
        for (int i = 0; i <= 20; i++)
        {
            var angle = startAngle + angleStep * i;
            var x = centerX + innerRadiusX * Math.Cos(angle);
            var y = centerY + innerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        // Outer arc (reverse order)
        for (int i = 20; i >= 0; i--)
        {
            var angle = startAngle + angleStep * i;
            var x = centerX + outerRadiusX * Math.Cos(angle);
            var y = centerY + outerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        return points;
    }

    private List<PointDto> GenerateSectorPolygon(string standCode, int totalSectors, int sectorIndex, StadiumCoordinateSystem coords, double centerX, double centerY, double fieldRadiusX, double fieldRadiusY)
    {
        var innerRadiusX = fieldRadiusX + 60;
        var innerRadiusY = fieldRadiusY + 40;
        var outerRadiusX = fieldRadiusX + 160;
        var outerRadiusY = fieldRadiusY + 120;
        
        var points = new List<PointDto>();
        
        // Get stand angle range and calculate sector portion
        var (standStartAngle, standEndAngle) = GetStandAngles(standCode);
        var sectorAngleRange = (standEndAngle - standStartAngle) / totalSectors;
        var sectorStartAngle = standStartAngle + sectorAngleRange * sectorIndex;
        var sectorEndAngle = sectorStartAngle + sectorAngleRange;
        
        var angleStep = sectorAngleRange / 8;
        
        // Inner arc
        for (int i = 0; i <= 8; i++)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + innerRadiusX * Math.Cos(angle);
            var y = centerY + innerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        // Outer arc (reverse order)
        for (int i = 8; i >= 0; i--)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + outerRadiusX * Math.Cos(angle);
            var y = centerY + outerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        return points;
    }
    
    private (double startAngle, double endAngle) GetStandAngles(string standCode)
    {
        return standCode switch
        {
            "N" => (-2.8, -0.35), // North stand (top)
            "E" => (-0.35, 2.45), // East stand (right)
            "S" => (0.35, 2.8),   // South stand (bottom) 
            "W" => (2.45, -2.8),  // West stand (left)
            _ => (-Math.PI / 2, Math.PI / 2)
        };
    }

    private async Task LoadEvents()
    {
        try
        {
            events = await ApiService.GetEventsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading events");
        }
    }

    private async Task OnEventChanged()
    {
        if (selectedEventId > 0)
        {
            selectedEvent = events?.FirstOrDefault(e => e.Id == selectedEventId);
            await LoadEventSeatStatus(selectedEventId);
            await LoadSeatStatusForEvent(); // Keep existing functionality
        }
        else
        {
            selectedEventId = 0;
            selectedEvent = null;
            eventSeatStatus = null;
            seatStatusMap = null;
        }
        StateHasChanged();
    }

    private async Task LoadEventSeatStatus(int eventId)
    {
        try
        {
            var response = await ApiService.GetAsync<EventSeatStatusDto>($"stadium-viewer/event/{eventId}/seat-status");
            eventSeatStatus = response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading event seat status");
        }
    }

    private async Task LoadSeatStatusForEvent()
    {
        try
        {
            var response = await ApiService.GetSeatStatusForEventAsync(selectedEventId);
            if (response != null)
            {
                seatStatusMap = response.SoldSeats;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading seat status");
            seatStatusMap = null;
        }
    }

    private async Task SimulateTicketSales()
    {
        if (selectedEventId <= 0) return;
        
        isSimulating = true;
        StateHasChanged();

        try
        {
            var success = await ApiService.SimulateTicketSalesAsync(selectedEventId, 25, 75.00m);
            if (success)
            {
                await LoadSeatStatusForEvent();
                await LoadEventSeatStatus(selectedEventId);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error simulating ticket sales");
        }
        finally
        {
            isSimulating = false;
            StateHasChanged();
        }
    }

    private async Task OpenSectorModal(StadiumSectorDto sector)
    {
        selectedSector = sector;
        selectedSeats.Clear();
        showSectorModal = true;

        try
        {
            var response = await ApiService.GetAsync<StadiumSectorDto>($"stadium-viewer/sector/{sector.Id}/seats");
            if (response != null)
            {
                sectorWithSeats = response;
                StateHasChanged();
                await Task.Delay(100);
                await RenderSeatsOnCanvas();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading sector seats");
        }
    }

    private void CloseSectorModal()
    {
        showSectorModal = false;
        selectedSector = null;
        sectorWithSeats = null;
        selectedSeats.Clear();
    }

    private async Task RenderSeatsOnCanvas()
    {
        if (sectorWithSeats?.Rows != null)
        {
            await JSRuntime.InvokeVoidAsync("renderSeatsOnCanvas", seatCanvas, sectorWithSeats, selectedEventId);
        }
    }

    private async Task SearchSeat()
    {
        if (string.IsNullOrWhiteSpace(searchSeatCode))
            return;

        try
        {
            var request = new { seatCode = searchSeatCode };
            var response = await ApiService.PostAsync<SearchSeatResultDto>("stadium-viewer/search-seat", request);
            
            if (response != null)
            {
                var sector = stadiumData?.Stands
                    .SelectMany(s => s.Sectors)
                    .FirstOrDefault(s => s.Id == response.SectorId);
                    
                if (sector != null)
                {
                    await OpenSectorModal(sector);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error searching for seat");
        }
    }

    private void OnSectorHover(StadiumSectorDto sector, bool isHovering)
    {
        if (isHovering)
        {
            hoveredSector = sector.Id;
            tooltipX = 500;
            tooltipY = 300;
        }
        else
        {
            hoveredSector = null;
        }
    }

    private StadiumSectorDto? GetHoveredSectorData()
    {
        if (hoveredSector == null || stadiumData == null)
            return null;

        return stadiumData.Stands
            .SelectMany(s => s.Sectors)
            .FirstOrDefault(s => s.Id == hoveredSector);
    }

    private SeatStatusSummaryDto? GetSectorSummary(string sectorId)
    {
        if (eventSeatStatus?.SectorSummaries.TryGetValue(sectorId, out var summary) == true)
        {
            return summary;
        }
        return null;
    }

    private string GetSectorFillColor(StadiumSectorDto sector)
    {
        if (selectedEventId == 0 || eventSeatStatus == null)
        {
            return sector.FillColor; // Use sector's original color when no event selected
        }

        var summary = GetSectorSummary(sector.Id);
        if (summary == null)
            return sector.FillColor;

        if (summary.OccupancyPercentage >= 90)
            return "#F44336"; // Red - nearly full
        else if (summary.OccupancyPercentage >= 50)
            return "#FF9800"; // Orange - partially full
        else
            return "#4CAF50"; // Green - mostly available
    }

    private string GetPolygonPoints(List<PointDto> points)
    {
        return string.Join(" ", points.Select(p => $"{GetRoundedValue(p.X)},{GetRoundedValue(p.Y)}"));
    }
    
    private string GetOptimizedPathData(List<PointDto> points)
    {
        if (!points.Any()) return "";
        
        var first = points.First();
        var pathData = $"M {GetRoundedValue(first.X)} {GetRoundedValue(first.Y)}";
        
        for (int i = 1; i < points.Count; i++)
        {
            pathData += $" L {GetRoundedValue(points[i].X)} {GetRoundedValue(points[i].Y)}";
        }
        
        return pathData + " Z";
    }
    
    private double GetRoundedValue(double value)
    {
        return Math.Round(value, 2); // Round to 2 decimal places for precision balance
    }

    private double GetSectorCenterX(List<PointDto> polygon)
    {
        return polygon.Average(p => p.X);
    }

    private double GetSectorCenterY(List<PointDto> polygon)
    {
        return polygon.Average(p => p.Y);
    }

    // Enhanced SVG helper methods for better accessibility and visual feedback
    private string GetEnhancedSectorAriaLabel(StadiumSectorDto sector)
    {
        var baseLabel = $"Sector {sector.Name} with {sector.TotalSeats} total seats";
        
        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var summary = GetSectorSummary(sector.Id);
            if (summary != null)
            {
                var status = summary.OccupancyPercentage >= 90 ? "fully occupied" :
                            summary.OccupancyPercentage >= 50 ? "partially occupied" : "mostly available";
                return $"{baseLabel}. Event selected: {summary.FreeSeats} available, {summary.SoldSeats} sold, {summary.HeldSeats} reserved. Status: {status}. Click to view seat details.";
            }
        }
        
        return $"{baseLabel}. Click to view seat details.";
    }
    
    private string GetSectorStatusClass(StadiumSectorDto sector)
    {
        var classes = new List<string> { "sector-main" };
        
        if (hoveredSector == sector.Id) classes.Add("hovered");
        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var summary = GetSectorSummary(sector.Id);
            if (summary != null)
            {
                classes.Add(summary.OccupancyPercentage >= 90 ? "status-full" :
                           summary.OccupancyPercentage >= 50 ? "status-partial" : "status-available");
            }
        }
        
        return string.Join(" ", classes);
    }
    
    private string GetOccupancyClass(decimal percentage)
    {
        return percentage >= 90 ? "occupancy-full" :
               percentage >= 50 ? "occupancy-partial" : "occupancy-available";
    }

    private string GetSectorStrokeColor(StadiumSectorDto sector)
    {
        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var summary = GetSectorSummary(sector.Id);
            if (summary != null)
            {
                if (summary.OccupancyPercentage >= 90) return "#D32F2F"; // Dark red
                if (summary.OccupancyPercentage >= 50) return "#F57C00"; // Dark orange  
                return "#388E3C"; // Dark green
            }
        }
        return "#555"; // Default dark gray
    }

    private string GetSectorTextColor(StadiumSectorDto sector)
    {
        var fillColor = GetSectorFillColor(sector);
        // Use white text on dark backgrounds, dark text on light backgrounds
        if (fillColor == "#F44336" || fillColor == "#FF9800") // Red or orange
            return "white";
        if (fillColor == "#4CAF50") // Green
            return "white";
        return "#333"; // Default dark text
    }

    private string GetSectorFontSize(StadiumSectorDto sector)
    {
        // Calculate font size based on sector area (polygon bounds)
        var minX = sector.Polygon.Min(p => p.X);
        var maxX = sector.Polygon.Max(p => p.X);
        var minY = sector.Polygon.Min(p => p.Y);
        var maxY = sector.Polygon.Max(p => p.Y);
        
        var width = maxX - minX;
        var height = maxY - minY;
        var area = width * height;
        
        // Scale font size based on area, with reasonable min/max bounds - increased 30% more for better visibility
        var fontSize = Math.Max(21, Math.Min(36, area * 0.0026));
        return fontSize.ToString("F0");
    }

    // Removed - replaced with CSS classes for better performance

    // Stadium Information Panel Event Handlers
    private async Task HandleInfoPanelToggle()
    {
        isInfoPanelCollapsed = !isInfoPanelCollapsed;
        StateHasChanged();
        await Task.Delay(300); // Allow CSS transition to complete
        // Optionally trigger map resize/redraw
    }

    private async Task OnTribuneSelected(StadiumStandDto stand)
    {
        highlightedTribune = stand.TribuneCode;
        StateHasChanged();
        
        // Optional: Scroll to or highlight tribune on map
        try
        {
            await JSRuntime.InvokeVoidAsync("highlightTribune", stand.TribuneCode);
        }
        catch (JSDisconnectedException)
        {
            Logger.LogWarning("Could not highlight tribune on map: Blazor circuit disconnected");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Could not highlight tribune on map: {TribuneCode}", stand.TribuneCode);
        }
    }

    private async Task OnTribuneHovered((StadiumStandDto stand, bool isHovering) tribuneData)
    {
        try
        {
            if (tribuneData.isHovering)
            {
                // Optional: Add hover effects on the map
                await JSRuntime.InvokeVoidAsync("hoverTribune", tribuneData.stand.TribuneCode);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("unhoverTribune");
            }
        }
        catch (JSDisconnectedException)
        {
            Logger.LogWarning("Could not invoke JavaScript function for tribune hover: Blazor circuit disconnected");
        }
        catch (Exception ex)
        {
            // Handle circuit disconnection gracefully
            Logger.LogWarning(ex, "Could not invoke JavaScript function for tribune hover. Circuit may be disconnected.");
        }
    }

    private Event? GetSelectedEvent()
    {
        return selectedEvent;
    }

    /// <summary>
    /// Handler for sector clicks from dynamic SVG component
    /// </summary>
    private async Task OnSectorClicked(SectorSvgDto sector)
    {
        Logger.LogInformation($"Sector clicked: {sector.Code} - {sector.Name}");
        
        // Convert to existing StadiumSectorDto format for compatibility
        var stadiumSector = new StadiumSectorDto
        {
            Id = sector.Code,
            Name = sector.Name,
            TotalSeats = sector.TotalSeats
        };
        
        await OpenSectorModal(stadiumSector);
    }
}