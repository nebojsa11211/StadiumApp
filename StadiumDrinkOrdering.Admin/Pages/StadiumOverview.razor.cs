using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;
using System.Text.Json;
using System.Net.Http;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumOverview : ComponentBase, IDisposable
{
    [Inject] public IAdminApiService ApiService { get; set; } = default!;
    [Inject] public ILogger<StadiumOverview> Logger { get; set; } = default!;
    [Inject] public HttpClient HttpClient { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public IConfiguration Configuration { get; set; } = default!;

    // Core data properties
    private StadiumViewerDto? stadiumData;
    private List<Event>? events;
    private Event? selectedEvent;
    private EventSeatStatusDto? eventSeatStatus;

    // Overlay configuration
    private List<SectorOverlay>? sectorOverlays;

    // Caching for performance - eliminates N+1 query problem
    private Dictionary<string, int>? _sectorCodeToIdCache;
    private Dictionary<string, string>? _sectorCodeToNameCache;

    // UI state properties
    private bool isLoading = true;
    private string? errorMessage;

    // Event selection
    private int selectedEventId = 0;

    // Modal properties
    private bool showSectorModal = false;
    private string selectedSectorCode = string.Empty;
    private string selectedSectorName = string.Empty;
    private List<StadiumSeat>? selectedSectorSeats;

    // Race condition protection
    private bool _isModalOpening = false;

    protected override async Task OnInitializedAsync()
    {
        Logger.LogInformation("Initializing Stadium Overview - Image Overlay System");

        try
        {
            // Load overlay configuration
            await LoadSectorOverlayConfig();

            // Load stadium data and events
            var loadDataTask = LoadStadiumData();
            var loadEventsTask = LoadEvents();

            await Task.WhenAll(loadDataTask, loadEventsTask);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during stadium initialization");
            errorMessage = "Failed to initialize stadium overview";
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private async Task LoadSectorOverlayConfig()
    {
        try
        {
            Logger.LogInformation("Loading sector overlay configuration from database API...");

            // Load sectors from database API (same as StadiumDrawingTool)
            var apiBaseUrl = Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";
            Logger.LogInformation($"Loading sectors from API: {apiBaseUrl}/api/StadiumSectorOverlay");

            var response = await HttpClient.GetAsync($"{apiBaseUrl}/api/StadiumSectorOverlay");

            Logger.LogInformation($"API Response Status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var dbSectors = await response.Content.ReadFromJsonAsync<List<StadiumDrinkOrdering.Shared.Models.StadiumSectorOverlay>>(jsonOptions);

                Logger.LogInformation($"Deserialized {dbSectors?.Count ?? 0} sectors from API");

                // Convert database entities to local overlay format with shape data
                sectorOverlays = dbSectors?.Select(db =>
                {
                    // Parse vertex coordinates from ShapeData JSON (if available)
                    List<VertexCoordinate>? vertices = null;
                    if (!string.IsNullOrEmpty(db.ShapeData))
                    {
                        try
                        {
                            vertices = JsonSerializer.Deserialize<List<VertexCoordinate>>(db.ShapeData, jsonOptions);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning(ex, $"Failed to parse ShapeData for sector {db.SectorCode}");
                        }
                    }

                    return new SectorOverlay
                    {
                        SectorCode = db.SectorCode,
                        Name = db.Name,
                        TopPercent = db.TopPercent,
                        LeftPercent = db.LeftPercent,
                        WidthPercent = db.WidthPercent,
                        HeightPercent = db.HeightPercent,
                        Type = db.Type,
                        ShapeType = db.ShapeType ?? "rectangle",
                        VertexCoordinates = vertices,
                        ShapeData = db.ShapeData
                    };
                }).ToList();

                Logger.LogInformation($"✅ Loaded {sectorOverlays?.Count ?? 0} sectors from database for stadium overview");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Logger.LogWarning($"Failed to load sectors: {response.StatusCode} - {errorContent}");
                sectorOverlays = new List<SectorOverlay>();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading sector overlay configuration from database");
            sectorOverlays = new List<SectorOverlay>();
        }
    }

    private async Task<bool> LoadStadiumData()
    {
        try
        {
            Logger.LogInformation("Loading stadium data...");

            var viewerResponse = await ApiService.GetAsync<StadiumViewerDto>("StadiumViewer/overview");

            if (viewerResponse != null && viewerResponse.Stands != null && viewerResponse.Stands.Any())
            {
                Logger.LogInformation($"Stadium data loaded: {viewerResponse.Name} with {viewerResponse.Stands.Count} stands");
                stadiumData = viewerResponse;

                // Build sector code to ID cache for performance
                BuildSectorCache();

                return true;
            }

            Logger.LogWarning("No stadium data available");
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading stadium data");
            return false;
        }
    }

    private void BuildSectorCache()
    {
        // Note: This caching strategy is ready but currently not needed since
        // the image-based overlay system uses hardcoded sector overlay JSON configuration.
        // The overlay sector codes (A1, A2, etc.) don't directly map to database IDs.
        // Future enhancement: Match overlay codes to actual sector IDs for real data.

        _sectorCodeToIdCache = new Dictionary<string, int>();
        _sectorCodeToNameCache = new Dictionary<string, string>();

        Logger.LogInformation("Sector cache initialized (currently using static overlay config)");
    }

    private async Task LoadEvents()
    {
        try
        {
            Logger.LogInformation("Loading events...");

            var eventsResponse = await ApiService.GetAsync<List<Event>>("Events/active");

            if (eventsResponse != null && eventsResponse.Any())
            {
                events = eventsResponse.OrderByDescending(e => e.EventDate).ToList();
                Logger.LogInformation($"Loaded {events.Count} active events");
            }
            else
            {
                Logger.LogWarning("No active events found");
                events = new List<Event>();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading events");
            events = new List<Event>();
        }
    }

    private async Task OnEventChanged()
    {
        if (selectedEventId > 0)
        {
            selectedEvent = events?.FirstOrDefault(e => e.Id == selectedEventId);

            if (selectedEvent != null)
            {
                await LoadEventSeatStatus(selectedEventId);
            }
        }
        else
        {
            selectedEvent = null;
            eventSeatStatus = null;
        }

        StateHasChanged();
    }

    private async Task LoadEventSeatStatus(int eventId)
    {
        try
        {
            Logger.LogInformation($"Loading seat status for event {eventId}...");

            var statusResponse = await ApiService.GetAsync<EventSeatStatusDto>($"StadiumViewer/event/{eventId}/status");

            if (statusResponse != null)
            {
                eventSeatStatus = statusResponse;
                Logger.LogInformation($"Loaded seat status for event {eventId}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error loading seat status for event {eventId}");
        }
    }

    private string GetSectorOccupancyClass(string sectorCode)
    {
        // Note: This is a placeholder implementation since the overlay system
        // uses static sector codes that don't directly map to real database sectors.
        // For a demo, we can return different classes based on the sector code pattern
        // to show the visual system working.

        if (string.IsNullOrEmpty(sectorCode))
            return "";

        // Demo logic: Show visual variety
        return sectorCode switch
        {
            "A1" or "A2" or "B1" => "available",
            "A3" or "VIP1" or "VIP2" => "vip",
            "A4" or "A5" => "partial",
            "B2" or "B3" or "B4" => "full",
            _ => "available"
        };
    }

    private async Task OpenSectorModal(string sectorCode)
    {
        // Race condition protection - prevent multiple concurrent opens
        if (_isModalOpening)
        {
            Logger.LogDebug($"Modal already opening, ignoring duplicate request for {sectorCode}");
            return;
        }

        try
        {
            _isModalOpening = true;
            Logger.LogInformation($"Opening sector modal for {sectorCode}...");

            // Note: This is a placeholder since the overlay system uses static codes.
            // For now, we'll show an empty modal or generate demo seats.
            selectedSectorCode = sectorCode;
            selectedSectorName = $"Sector {sectorCode}";

            // Generate demo seats for visual demonstration
            selectedSectorSeats = GenerateDemoSeats(sectorCode);
            showSectorModal = true;

            Logger.LogInformation($"Opened modal for sector {sectorCode} with {selectedSectorSeats?.Count ?? 0} seats");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error opening sector modal for {sectorCode}");
        }
        finally
        {
            _isModalOpening = false;
            StateHasChanged();
        }
    }

    private List<StadiumSeat> GenerateDemoSeats(string sectorCode)
    {
        // Generate demo seats for demonstration
        var seats = new List<StadiumSeat>();
        for (int row = 1; row <= 10; row++)
        {
            for (int seat = 1; seat <= 15; seat++)
            {
                seats.Add(new StadiumSeat
                {
                    Id = row * 100 + seat,
                    Section = sectorCode,
                    RowNumber = row,
                    SeatNumber = seat,
                    IsActive = (row + seat) % 3 != 0 // Make some seats inactive for demo
                });
            }
        }
        return seats;
    }

    private void CloseSectorModal()
    {
        showSectorModal = false;
        selectedSectorCode = string.Empty;
        selectedSectorName = string.Empty;
        selectedSectorSeats = null;
        StateHasChanged();
    }

    private async Task HandleSectorKeyDown(KeyboardEventArgs e, string sectorCode)
    {
        // Handle Enter or Space key to activate sector
        if (e.Key == "Enter" || e.Key == " ")
        {
            // Prevent default space key scrolling
            await OpenSectorModal(sectorCode);
        }
    }

    // SVG rendering helper methods
    private const double CANVAS_WIDTH = 1170.0;
    private const double CANVAS_HEIGHT = 898.0;

    /// <summary>
    /// Generates SVG path data for a sector based on its shape type
    /// </summary>
    private string GenerateSvgPath(SectorOverlay sector)
    {
        if (sector == null) return string.Empty;

        return sector.ShapeType?.ToLower() switch
        {
            "rectangle" or null or "" => GenerateRectanglePath(sector),
            "triangle" => GeneratePolygonPath(sector.VertexCoordinates),
            "rhombus" => GeneratePolygonPath(sector.VertexCoordinates),
            "custompolygon" => GeneratePolygonPath(sector.VertexCoordinates),
            _ => GenerateRectanglePath(sector) // Fallback to rectangle
        };
    }

    /// <summary>
    /// Generates SVG path for rectangular sectors (converts percentages to pixels)
    /// </summary>
    private string GenerateRectanglePath(SectorOverlay sector)
    {
        // Convert percentages to pixels
        double x = (sector.LeftPercent / 100.0) * CANVAS_WIDTH;
        double y = (sector.TopPercent / 100.0) * CANVAS_HEIGHT;
        double width = (sector.WidthPercent / 100.0) * CANVAS_WIDTH;
        double height = (sector.HeightPercent / 100.0) * CANVAS_HEIGHT;

        // Create path for rectangle: M (move to top-left) -> L (line to corners) -> Z (close)
        return $"M {x:F2} {y:F2} L {(x + width):F2} {y:F2} L {(x + width):F2} {(y + height):F2} L {x:F2} {(y + height):F2} Z";
    }

    /// <summary>
    /// Generates SVG path for polygon-based shapes (triangle, rhombus, custom polygon)
    /// </summary>
    private string GeneratePolygonPath(List<VertexCoordinate>? vertices)
    {
        if (vertices == null || vertices.Count < 3)
            return string.Empty;

        var pathBuilder = new System.Text.StringBuilder();

        // Convert first vertex from percentage to pixels and move to it
        double x0 = (vertices[0].X / 100.0) * CANVAS_WIDTH;
        double y0 = (vertices[0].Y / 100.0) * CANVAS_HEIGHT;
        pathBuilder.Append($"M {x0:F2} {y0:F2}");

        // Line to each subsequent vertex
        for (int i = 1; i < vertices.Count; i++)
        {
            double x = (vertices[i].X / 100.0) * CANVAS_WIDTH;
            double y = (vertices[i].Y / 100.0) * CANVAS_HEIGHT;
            pathBuilder.Append($" L {x:F2} {y:F2}");
        }

        // Close the path (line back to first point)
        pathBuilder.Append(" Z");

        return pathBuilder.ToString();
    }

    /// <summary>
    /// Calculates center point for label placement (works for all shapes)
    /// </summary>
    private (double X, double Y) CalculateLabelCenter(SectorOverlay sector)
    {
        if (sector.VertexCoordinates != null && sector.VertexCoordinates.Count >= 3)
        {
            // For polygons: calculate centroid
            double sumX = 0, sumY = 0;
            foreach (var vertex in sector.VertexCoordinates)
            {
                sumX += (vertex.X / 100.0) * CANVAS_WIDTH;
                sumY += (vertex.Y / 100.0) * CANVAS_HEIGHT;
            }
            return (sumX / sector.VertexCoordinates.Count, sumY / sector.VertexCoordinates.Count);
        }
        else
        {
            // For rectangles: use center of bounding box
            double x = (sector.LeftPercent / 100.0) * CANVAS_WIDTH;
            double y = (sector.TopPercent / 100.0) * CANVAS_HEIGHT;
            double width = (sector.WidthPercent / 100.0) * CANVAS_WIDTH;
            double height = (sector.HeightPercent / 100.0) * CANVAS_HEIGHT;
            return (x + width / 2, y + height / 2);
        }
    }

    public void Dispose()
    {
        // Clear references to prevent memory leaks
        stadiumData = null;
        events = null;
        selectedEvent = null;
        eventSeatStatus = null;
        sectorOverlays = null;
        selectedSectorSeats = null;

        // Clear caches
        _sectorCodeToIdCache?.Clear();
        _sectorCodeToIdCache = null;
        _sectorCodeToNameCache?.Clear();
        _sectorCodeToNameCache = null;

        Logger.LogInformation("StadiumOverview component disposed and resources cleared");
    }

    // Sector overlay model (matches database structure with shape support)
    private class SectorOverlay
    {
        public string SectorCode { get; set; } = string.Empty;
        public string? Name { get; set; }
        public double TopPercent { get; set; }
        public double LeftPercent { get; set; }
        public double WidthPercent { get; set; }
        public double HeightPercent { get; set; }
        public string? Type { get; set; }
        public string ShapeType { get; set; } = "rectangle";
        public List<VertexCoordinate>? VertexCoordinates { get; set; }
        public string? ShapeData { get; set; }
    }

    // Vertex coordinate model for polygon shapes
    private class VertexCoordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
