using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
// using StadiumDrinkOrdering.Admin.Components; - Components removed
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumOverview : ComponentBase, IDisposable
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
    private Dictionary<string, ViewerSeatStatusDto>? seatStatusMap;
    private bool isSimulating = false;

    // Stadium information panel properties
    private bool isInfoPanelCollapsed = false; // Start expanded for better UX
    private Event? selectedEvent;
    private DateTime? lastUpdateTime = DateTime.Now;
    private string? highlightedTribune;
    private StadiumSummaryDto? stadiumSummary;

    // Mobile interaction properties
    private bool showMobileControls = true;
    private double currentZoom = 1.0;
    private double translateX = 0;
    private double translateY = 0;
    private bool isDragging = false;
    private double lastTouchX = 0;
    private double lastTouchY = 0;
    private double lastMouseX = 0;
    private double lastMouseY = 0;
    private DateTime lastTouchTime = DateTime.MinValue;
    private bool isMultiTouch = false;

    protected override async Task OnInitializedAsync()
    {
        // Load dynamic stadium layout first (fast and essential)
        await LoadStadiumLayout();

        // Load stadium data for the info panel
        await LoadStadiumData();

        // Load stadium summary for info panel (after stadium data is loaded)
        await LoadStadiumSummary();

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
            var eventDtos = await ApiService.GetEventsAsync();

            if (eventDtos == null || !eventDtos.Any())
            {
                Logger?.LogWarning("No events received from API");
                events = new List<Event>();
                await InvokeAsync(StateHasChanged);
                return;
            }

            events = eventDtos.Select(e => new Event
            {
                Id = e.Id,
                EventName = !string.IsNullOrWhiteSpace(e.Name) ? e.Name : $"Event {e.Id}",
                EventDate = e.Date ?? DateTime.UtcNow.AddDays(1), // Use future date as fallback
                Description = e.Description,
                TotalSeats = e.Capacity,
                BaseTicketPrice = (decimal)e.BasePrice,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            }).ToList();

            Logger?.LogInformation($"Loaded {events.Count} events from API");
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            // Events loading failed - stadium should still work
            Logger?.LogError(ex, "Failed to load events in background");
            events = new List<Event>(); // Ensure events list is initialized
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task LoadStadiumData()
    {
        try
        {
            isLoading = true;
            StateHasChanged(); // Ensure UI shows loading state

            Logger.LogInformation("Starting to load stadium data from API...");

            // Try the new stadium viewer endpoint first (no authentication required)
            try
            {
                var viewerResponse = await ApiService.GetAsync<StadiumViewerDto>("StadiumViewer/overview");
                if (viewerResponse != null && viewerResponse.Stands != null && viewerResponse.Stands.Any())
                {
                    Logger.LogInformation($"Stadium data loaded successfully from viewer endpoint: {viewerResponse.Name} with {viewerResponse.Stands.Count} stands");
                    stadiumData = viewerResponse;
                    errorMessage = null; // Clear any previous error
                    return;
                }
                else if (viewerResponse != null)
                {
                    Logger.LogWarning($"Stadium viewer endpoint returned data but no stands: {viewerResponse.Name}");
                }
                else
                {
                    Logger.LogWarning("Stadium viewer endpoint returned null");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Stadium viewer endpoint failed: {Message}", ex.Message);
            }

            // Fallback to existing Stadium/layout endpoint (requires authentication)
            try
            {
                var layoutResponse = await ApiService.GetStadiumLayoutAsync();
                if (layoutResponse != null)
                {
                    Logger.LogInformation($"Stadium layout loaded from fallback, converting to viewer format");
                    stadiumData = ConvertStadiumLayoutToViewer(layoutResponse);
                    errorMessage = null; // Clear any previous error
                    return;
                }
                else
                {
                    Logger.LogWarning("Stadium layout endpoint returned null");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Stadium layout endpoint failed: {Message}", ex.Message);
            }

            // If we get here, both endpoints failed
            Logger.LogWarning("All stadium endpoints failed or returned no data");
            errorMessage = "Failed to load stadium data - no stadium structure found";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected exception loading stadium data: {Message}", ex.Message);
            errorMessage = $"Unexpected error loading stadium data: {ex.Message}";
        }
        finally
        {
            isLoading = false;
            Logger.LogInformation($"LoadStadiumData completed. stadiumData is null: {stadiumData == null}, errorMessage: {errorMessage}");
            StateHasChanged(); // Ensure UI updates after loading
        }
    }

    private async Task LoadStadiumSummary()
    {
        try
        {
            Logger.LogInformation("Loading stadium summary...");
            stadiumSummary = await ApiService.GetStadiumSummaryAsync();

            if (stadiumSummary != null)
            {
                Logger.LogInformation($"Stadium summary loaded: {stadiumSummary.TotalSeats} total seats, {stadiumSummary.TotalTribunes} tribunes");
            }
            else
            {
                Logger.LogWarning("Stadium summary returned null - may require authentication");

                // Generate basic summary from stadiumData if available
                if (stadiumData != null && stadiumData.Stands != null && stadiumData.Stands.Any())
                {
                    var totalSeats = stadiumData.Stands.SelectMany(s => s.Sectors).Sum(s => s.TotalSeats);
                    var totalSectors = stadiumData.Stands.SelectMany(s => s.Sectors).Count();

                    stadiumSummary = new StadiumSummaryDto
                    {
                        TotalSeats = totalSeats,
                        TotalSectors = totalSectors,
                        TotalTribunes = stadiumData.Stands.Count,
                        StadiumName = stadiumData.Name ?? "Main Stadium"
                    };

                    Logger.LogInformation($"Generated stadium summary from viewer data: {stadiumSummary.TotalSeats} total seats, {stadiumSummary.TotalTribunes} tribunes");
                }
                else
                {
                    Logger.LogWarning($"Cannot generate stadium summary - stadiumData is null: {stadiumData == null}, stands count: {stadiumData?.Stands?.Count ?? -1}");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading stadium summary: {Message}", ex.Message);

            // Generate basic summary from stadiumData if available and exception occurred
            if (stadiumData != null && stadiumData.Stands != null)
            {
                var totalSeats = stadiumData.Stands.SelectMany(s => s.Sectors).Sum(s => s.TotalSeats);
                var totalSectors = stadiumData.Stands.SelectMany(s => s.Sectors).Count();

                stadiumSummary = new StadiumSummaryDto
                {
                    TotalSeats = totalSeats,
                    TotalSectors = totalSectors,
                    TotalTribunes = stadiumData.Stands.Count,
                    StadiumName = stadiumData.Name ?? "Main Stadium"
                };

                Logger.LogInformation($"Generated fallback stadium summary from viewer data: {stadiumSummary.TotalSeats} total seats");
            }
        }
        finally
        {
            StateHasChanged();
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
            var eventDtos = await ApiService.GetEventsAsync();
            events = eventDtos?.Select(e => new Event
            {
                Id = e.Id,
                EventName = e.Name,
                EventDate = e.Date ?? DateTime.Now,
                Description = e.Description,
                TotalSeats = e.Capacity,
                BaseTicketPrice = (decimal)e.BasePrice,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            }).ToList() ?? new List<Event>();
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
            var response = await ApiService.GetAsync<EventSeatStatusDto>($"StadiumViewer/event/{eventId}/seat-status");
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
                var responseJson = response as dynamic;
                seatStatusMap = responseJson?.SoldSeats ?? new Dictionary<string, bool>();
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
            var success = await ApiService.SimulateTicketSalesAsync(selectedEventId, 25);
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
            var response = await ApiService.GetAsync<StadiumSectorDto>($"StadiumViewer/sector/{sector.Id}/seats");
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
            var response = await ApiService.PostAsync<SearchSeatResultDto>("StadiumViewer/search-seat", request);
            
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

    /* ========================================
       Mobile Touch & Gesture Interaction Methods
       Responsive Pan, Zoom, and Navigation
       ======================================== */

    /// <summary>
    /// Handle touch start for mobile gesture recognition
    /// </summary>
    private async Task HandleTouchStart(TouchEventArgs e)
    {
        try
        {
            if (e.Touches.Length == 1)
            {
                // Single touch - potential pan
                var touch = e.Touches[0];
                lastTouchX = touch.ClientX;
                lastTouchY = touch.ClientY;
                lastTouchTime = DateTime.UtcNow;
                isDragging = true;
                isMultiTouch = false;
            }
            else if (e.Touches.Length == 2)
            {
                // Multi-touch - potential pinch zoom
                isMultiTouch = true;
                isDragging = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling touch start");
        }
    }

    /// <summary>
    /// Handle touch move for panning and zooming
    /// </summary>
    private async Task HandleTouchMove(TouchEventArgs e)
    {
        try
        {
            if (isDragging && e.Touches.Length == 1)
            {
                var touch = e.Touches[0];
                var deltaX = touch.ClientX - lastTouchX;
                var deltaY = touch.ClientY - lastTouchY;

                // Update translation
                translateX += deltaX * 0.5; // Reduce sensitivity for smoother movement
                translateY += deltaY * 0.5;

                // Constrain translation to reasonable bounds
                var maxTranslate = 200 * currentZoom;
                translateX = Math.Max(-maxTranslate, Math.Min(maxTranslate, translateX));
                translateY = Math.Max(-maxTranslate, Math.Min(maxTranslate, translateY));

                lastTouchX = touch.ClientX;
                lastTouchY = touch.ClientY;

                await ApplyTransformation();
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling touch move");
        }
    }

    /// <summary>
    /// Handle touch end and detect tap gestures
    /// </summary>
    private async Task HandleTouchEnd(TouchEventArgs e)
    {
        try
        {
            var touchDuration = DateTime.UtcNow - lastTouchTime;

            // Detect quick tap for toggle legend
            if (touchDuration.TotalMilliseconds < 200 && !isMultiTouch)
            {
                // Quick tap - could be sector selection or legend toggle
                // Let normal click handlers deal with sector selection
            }

            isDragging = false;
            isMultiTouch = false;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling touch end");
        }
    }

    /// <summary>
    /// Handle mouse wheel for desktop zoom
    /// </summary>
    private async Task HandleMouseWheel(WheelEventArgs e)
    {
        try
        {
            if (e.DeltaY < 0)
            {
                await ZoomIn();
            }
            else if (e.DeltaY > 0)
            {
                await ZoomOut();
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling mouse wheel");
        }
    }

    /// <summary>
    /// Handle mouse down for desktop dragging
    /// </summary>
    private void HandleMouseDown(MouseEventArgs e)
    {
        try
        {
            if (e.Button == 0) // Left mouse button
            {
                isDragging = true;
                lastMouseX = e.ClientX;
                lastMouseY = e.ClientY;
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling mouse down");
        }
    }

    /// <summary>
    /// Handle mouse move for desktop panning
    /// </summary>
    private async Task HandleMouseMove(MouseEventArgs e)
    {
        try
        {
            if (isDragging)
            {
                var deltaX = e.ClientX - lastMouseX;
                var deltaY = e.ClientY - lastMouseY;

                translateX += deltaX * 0.3;
                translateY += deltaY * 0.3;

                // Constrain translation
                var maxTranslate = 150 * currentZoom;
                translateX = Math.Max(-maxTranslate, Math.Min(maxTranslate, translateX));
                translateY = Math.Max(-maxTranslate, Math.Min(maxTranslate, translateY));

                lastMouseX = e.ClientX;
                lastMouseY = e.ClientY;

                await ApplyTransformation();
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling mouse move");
        }
    }

    /// <summary>
    /// Handle mouse up to stop dragging
    /// </summary>
    private void HandleMouseUp(MouseEventArgs e)
    {
        isDragging = false;
    }

    /// <summary>
    /// Zoom in on the stadium view
    /// </summary>
    private async Task ZoomIn()
    {
        try
        {
            currentZoom = Math.Min(3.0, currentZoom * 1.2);
            await ApplyTransformation();
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error zooming in");
        }
    }

    /// <summary>
    /// Zoom out on the stadium view
    /// </summary>
    private async Task ZoomOut()
    {
        try
        {
            currentZoom = Math.Max(0.5, currentZoom * 0.8);
            await ApplyTransformation();
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error zooming out");
        }
    }

    /// <summary>
    /// Reset zoom and position to default
    /// </summary>
    private async Task ResetZoom()
    {
        try
        {
            currentZoom = 1.0;
            translateX = 0;
            translateY = 0;
            await ApplyTransformation();
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error resetting zoom");
        }
    }

    /// <summary>
    /// Toggle legend visibility for mobile
    /// </summary>
    private void ToggleLegendMobile()
    {
        showLegend = !showLegend;
        StateHasChanged();
    }

    /// <summary>
    /// Apply transformation to the SVG element
    /// </summary>
    private async Task ApplyTransformation()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("applyStadiumTransform",
                svgContainer, currentZoom, translateX, translateY);
        }
        catch (JSDisconnectedException)
        {
            Logger.LogWarning("Cannot apply transformation: Blazor circuit disconnected");
        }
        catch (JSException jsEx)
        {
            Logger.LogWarning(jsEx, "JavaScript error applying transformation");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error applying stadium transformation");
        }
    }

    /// <summary>
    /// Handle device orientation change
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            try
            {
                // Initialize mobile controls and gesture support
                await JSRuntime.InvokeVoidAsync("initializeStadiumGestures", svgContainer);

                // Setup orientation change listener
                await JSRuntime.InvokeVoidAsync("setupOrientationChangeListener",
                    DotNetObjectReference.Create(this));
            }
            catch (JSDisconnectedException)
            {
                Logger.LogWarning("Cannot initialize gestures: Blazor circuit disconnected");
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error initializing mobile gestures");
            }
        }
    }

    /// <summary>
    /// Handle orientation change from JavaScript
    /// </summary>
    [JSInvokable]
    public async Task OnOrientationChanged()
    {
        try
        {
            // Reset transformation on orientation change for better UX
            await Task.Delay(200); // Wait for orientation animation
            await ResetZoom();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling orientation change");
        }
    }

    /// <summary>
    /// Handle visibility change (tab switching, app backgrounding)
    /// </summary>
    [JSInvokable]
    public void OnVisibilityChanged(bool isVisible)
    {
        try
        {
            if (!isVisible)
            {
                // Stop any ongoing animations or updates when not visible
                isDragging = false;
                isMultiTouch = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error handling visibility change");
        }
    }

    /// <summary>
    /// Check if device has touch support
    /// </summary>
    private async Task<bool> IsTouchDevice()
    {
        try
        {
            return await JSRuntime.InvokeAsync<bool>("isTouchDevice");
        }
        catch (Exception)
        {
            return false; // Assume non-touch if detection fails
        }
    }

    /// <summary>
    /// Get viewport dimensions for responsive calculations
    /// </summary>
    private async Task<(int width, int height)> GetViewportDimensions()
    {
        try
        {
            var dimensions = await JSRuntime.InvokeAsync<int[]>("getViewportDimensions");
            return (dimensions[0], dimensions[1]);
        }
        catch (Exception)
        {
            return (1024, 768); // Default dimensions
        }
    }

    /// <summary>
    /// Dispose method to clean up event listeners
    /// </summary>
    public void Dispose()
    {
        try
        {
            // Clean up JavaScript event listeners
            _ = JSRuntime.InvokeVoidAsync("cleanupStadiumGestures");
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error cleaning up gesture listeners");
        }
    }
}