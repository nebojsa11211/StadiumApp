using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumOverview : ComponentBase, IDisposable
{
    [Inject] public IAdminApiService ApiService { get; set; } = default!;
    [Inject] public ILogger<StadiumOverview> Logger { get; set; } = default!;

    // Core data properties
    private StadiumViewerDto? stadiumData;
    private List<Event>? events;
    private Event? selectedEvent;
    private EventSeatStatusDto? eventSeatStatus;
    private StadiumSummaryDto? stadiumSummary;

    // UI state properties
    private bool isLoading = true;
    private string? errorMessage;
    private bool showLegend = false;
    private bool showOccupancy = true;
    private bool isInfoPanelCollapsed = false;
    private DateTime? lastUpdateTime;

    // Event and search properties
    private int selectedEventId = 0;
    private string searchSeatCode = "";
    private bool isSimulating = false;

    // Modal properties
    private bool showSectorModal = false;
    private StadiumSectorDto? selectedSector;

    protected override async Task OnInitializedAsync()
    {
        Logger.LogInformation("Initializing Stadium Overview page");
        lastUpdateTime = DateTime.Now;

        try
        {
            // Load data in parallel for better performance
            var loadDataTask = LoadStadiumData();
            var loadEventsTask = LoadEvents();

            await Task.WhenAll(loadDataTask, loadEventsTask);

            // Load summary after stadium data is available
            if (stadiumData != null)
            {
                await LoadStadiumSummary();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during initialization");
            errorMessage = "Failed to initialize stadium overview";
        }
        finally
        {
            isLoading = false;
            lastUpdateTime = DateTime.Now;
            StateHasChanged();
        }
    }

    private async Task LoadStadiumData()
    {
        try
        {
            Logger.LogInformation("Loading stadium data...");

            // Try the stadium viewer endpoint first (no authentication required)
            var viewerResponse = await ApiService.GetAsync<StadiumViewerDto>("StadiumViewer/overview");

            if (viewerResponse != null && viewerResponse.Stands != null && viewerResponse.Stands.Any())
            {
                Logger.LogInformation($"Stadium data loaded successfully: {viewerResponse.Name} with {viewerResponse.Stands.Count} stands");
                stadiumData = viewerResponse;
                errorMessage = null;
                return;
            }

            // Fallback: Try to load from stadium structure endpoint
            try
            {
                var layoutResponse = await ApiService.GetStadiumLayoutAsync();
                if (layoutResponse != null)
                {
                    Logger.LogInformation("Converting stadium layout to viewer format");
                    stadiumData = ConvertStadiumLayoutToViewer(layoutResponse);
                    errorMessage = null;
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Stadium layout endpoint failed");
            }

            // If we get here, no data was loaded
            errorMessage = "No stadium data available from any source";
            Logger.LogWarning("No stadium data could be loaded");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading stadium data");
            errorMessage = $"Error loading stadium data: {ex.Message}";
        }
    }

    private async Task LoadEvents()
    {
        try
        {
            Logger.LogInformation("Loading events...");
            var eventDtos = await ApiService.GetEventsAsync();

            if (eventDtos != null && eventDtos.Any())
            {
                events = eventDtos.Select(e => new Event
                {
                    Id = e.Id,
                    EventName = !string.IsNullOrWhiteSpace(e.Name) ? e.Name : $"Event {e.Id}",
                    EventDate = e.Date ?? DateTime.UtcNow.AddDays(1),
                    Description = e.Description,
                    TotalSeats = e.Capacity,
                    BaseTicketPrice = (decimal)e.BasePrice,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt
                }).ToList();

                Logger.LogInformation($"Loaded {events.Count} events");
            }
            else
            {
                Logger.LogWarning("No events received from API");
                events = new List<Event>();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading events");
            events = new List<Event>();
        }
    }

    private async Task LoadStadiumSummary()
    {
        try
        {
            Logger.LogInformation("Loading stadium summary...");
            stadiumSummary = await ApiService.GetStadiumSummaryAsync();

            if (stadiumSummary == null && stadiumData != null && stadiumData.Stands.Any())
            {
                // Generate summary from stadium data
                var totalSeats = stadiumData.Stands.SelectMany(s => s.Sectors).Sum(s => s.TotalSeats);
                var totalSectors = stadiumData.Stands.SelectMany(s => s.Sectors).Count();

                stadiumSummary = new StadiumSummaryDto
                {
                    TotalSeats = totalSeats,
                    TotalSectors = totalSectors,
                    TotalTribunes = stadiumData.Stands.Count,
                    StadiumName = stadiumData.Name ?? "Main Stadium"
                };

                Logger.LogInformation($"Generated stadium summary: {stadiumSummary.TotalSeats} total seats");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading stadium summary");

            // Generate basic summary as fallback
            if (stadiumData?.Stands?.Any() == true)
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
            }
        }
    }

    private async Task OnEventChanged()
    {
        try
        {
            if (selectedEventId > 0)
            {
                selectedEvent = events?.FirstOrDefault(e => e.Id == selectedEventId);
                Logger.LogInformation($"Event selected: {selectedEvent?.EventName}");

                await LoadEventSeatStatus(selectedEventId);
                lastUpdateTime = DateTime.Now;
            }
            else
            {
                selectedEvent = null;
                eventSeatStatus = null;
                Logger.LogInformation("Event selection cleared");
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error changing event");
        }
    }

    private async Task LoadEventSeatStatus(int eventId)
    {
        try
        {
            Logger.LogInformation($"Loading seat status for event {eventId}");
            eventSeatStatus = await ApiService.GetAsync<EventSeatStatusDto>($"StadiumViewer/event/{eventId}/seat-status");

            if (eventSeatStatus != null)
            {
                Logger.LogInformation($"Loaded seat status with {eventSeatStatus.SectorSummaries.Count} sectors");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error loading event seat status for event {eventId}");
            eventSeatStatus = null;
        }
    }

    private async Task SimulateTicketSales()
    {
        if (selectedEventId <= 0 || isSimulating) return;

        isSimulating = true;
        StateHasChanged();

        try
        {
            Logger.LogInformation($"Simulating ticket sales for event {selectedEventId}");
            var success = await ApiService.SimulateTicketSalesAsync(selectedEventId, 25);

            if (success)
            {
                await LoadEventSeatStatus(selectedEventId);
                lastUpdateTime = DateTime.Now;
                Logger.LogInformation("Ticket sales simulation completed successfully");
            }
            else
            {
                Logger.LogWarning("Ticket sales simulation failed");
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

    private async Task SearchSeat()
    {
        if (string.IsNullOrWhiteSpace(searchSeatCode)) return;

        try
        {
            Logger.LogInformation($"Searching for seat: {searchSeatCode}");
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
                    Logger.LogInformation($"Found seat {searchSeatCode} in sector {sector.Name}");
                }
                else
                {
                    Logger.LogWarning($"Seat {searchSeatCode} found but sector not in current data");
                }
            }
            else
            {
                Logger.LogInformation($"Seat {searchSeatCode} not found");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error searching for seat {searchSeatCode}");
        }
    }

    private async Task OpenSectorModal(StadiumSectorDto sector)
    {
        try
        {
            selectedSector = sector;
            showSectorModal = true;
            Logger.LogInformation($"Opening modal for sector {sector.Name}");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error opening sector modal for {sector.Name}");
        }
    }

    private void CloseSectorModal()
    {
        showSectorModal = false;
        selectedSector = null;
        StateHasChanged();
    }

    private void ToggleLegend()
    {
        showLegend = !showLegend;
        StateHasChanged();
    }

    private void ToggleOccupancy()
    {
        showOccupancy = !showOccupancy;
        StateHasChanged();
    }

    private void ToggleInfoPanel()
    {
        isInfoPanelCollapsed = !isInfoPanelCollapsed;
        StateHasChanged();
    }

    private async Task LoadStadiumLayout()
    {
        isLoading = true;
        errorMessage = null;
        StateHasChanged();

        try
        {
            await LoadStadiumData();
            if (stadiumData != null)
            {
                await LoadStadiumSummary();
            }
        }
        finally
        {
            isLoading = false;
            lastUpdateTime = DateTime.Now;
            StateHasChanged();
        }
    }

    private async Task GenerateDemoLayout()
    {
        try
        {
            Logger.LogInformation("Generating demo stadium layout");
            stadiumData = GenerateBasicStadiumLayout();
            await LoadStadiumSummary();
            errorMessage = null;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error generating demo layout");
            errorMessage = "Failed to generate demo layout";
        }
    }

    // Helper methods for data conversion and UI state
    private StadiumViewerDto ConvertStadiumLayoutToViewer(StadiumLayoutDto layout)
    {
        Logger.LogInformation("Converting stadium layout to viewer format");

        var viewer = new StadiumViewerDto
        {
            StadiumId = "main-stadium",
            Name = "Main Stadium",
            Stands = new List<StadiumStandDto>()
        };

        // Group sections by stand
        var standGroups = layout.Sections.GroupBy(s => DetermineStandCode(s.SectionName));

        foreach (var group in standGroups)
        {
            var stand = new StadiumStandDto
            {
                Id = group.Key.ToLower(),
                Name = GetStandName(group.Key),
                TribuneCode = group.Key,
                Sectors = new List<StadiumSectorDto>()
            };

            foreach (var section in group)
            {
                var sector = new StadiumSectorDto
                {
                    Id = section.SectionName,
                    Name = section.DisplayName,
                    TotalSeats = section.TotalSeats,
                    AvailableSeats = section.TotalSeats - section.ActiveOrders,
                    StandId = stand.Id,
                    FillColor = section.Color ?? GetDefaultSectorColor(group.Key),
                    HoverColor = "#d4d4d8"
                };

                stand.Sectors.Add(sector);
            }

            viewer.Stands.Add(stand);
        }

        Logger.LogInformation($"Converted to viewer format: {viewer.Stands.Count} stands");
        return viewer;
    }

    private StadiumViewerDto GenerateBasicStadiumLayout()
    {
        var viewer = new StadiumViewerDto
        {
            StadiumId = "demo-stadium",
            Name = "Demo Stadium",
            Stands = new List<StadiumStandDto>()
        };

        // Generate a more complex stadium with multiple tribunes for demonstration
        var stands = new[]
        {
            new { Code = "N", Name = "North Tribune", Color = "#3b82f6" },
            new { Code = "S", Name = "South Tribune", Color = "#10b981" },
            new { Code = "E", Name = "East Tribune", Color = "#f59e0b" },
            new { Code = "W", Name = "West Tribune", Color = "#8b5cf6" },
            new { Code = "NE", Name = "North-East Corner", Color = "#06b6d4" },
            new { Code = "NW", Name = "North-West Corner", Color = "#ec4899" },
            new { Code = "SE", Name = "South-East Corner", Color = "#eab308" },
            new { Code = "SW", Name = "South-West Corner", Color = "#a855f7" }
        };

        foreach (var standData in stands)
        {
            var stand = new StadiumStandDto
            {
                Id = standData.Code.ToLower(),
                Name = standData.Name,
                TribuneCode = standData.Code,
                Sectors = new List<StadiumSectorDto>()
            };

            // Generate 2-4 sectors per stand (corners have 2, main tribunes have 4)
            int sectorCount = standData.Code.Length > 1 ? 2 : 4;

            for (int i = 1; i <= sectorCount; i++)
            {
                var sector = new StadiumSectorDto
                {
                    Id = $"{standData.Code}{i}",
                    Name = $"Sector {standData.Code}{i}",
                    TotalSeats = standData.Code.Length > 1 ? 250 : 500, // Corners have fewer seats
                    AvailableSeats = standData.Code.Length > 1 ? 250 : 500,
                    StandId = stand.Id,
                    FillColor = standData.Color,
                    HoverColor = "#d4d4d8"
                };

                stand.Sectors.Add(sector);
            }

            viewer.Stands.Add(stand);
        }

        Logger.LogInformation($"Generated demo stadium with {viewer.Stands.Count} stands and {viewer.Stands.Sum(s => s.Sectors.Count)} sectors");
        return viewer;
    }

    // UI helper methods
    private string GetSectorCssClass(StadiumSectorDto sector)
    {
        var classes = new List<string> { "sector" };

        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var occupancyPercentage = GetOccupancyPercentage(sector);

            if (occupancyPercentage >= 90)
                classes.Add("status-full");
            else if (occupancyPercentage >= 50)
                classes.Add("status-partial");
            else
                classes.Add("status-available");
        }

        if (IsVipSector(sector))
        {
            classes.Add("sector-vip");
        }

        return string.Join(" ", classes);
    }

    private string GetStandPositionClass(StadiumStandDto stand)
    {
        // Map tribune codes to position classes
        var tribuneCode = stand.TribuneCode?.ToUpper() ?? "";

        return tribuneCode switch
        {
            "N" => "stand-position-north",
            "S" => "stand-position-south",
            "E" => "stand-position-east",
            "W" => "stand-position-west",
            _ => $"stand-position-{stand.Id}"
        };
    }

    private string GetSectorAriaLabel(StadiumSectorDto sector)
    {
        var label = $"Sector {sector.Name} with {sector.TotalSeats} total seats";

        if (IsVipSector(sector))
        {
            label += ", VIP section";
        }

        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var availableSeats = GetAvailableSeats(sector);
            var occupancyPercentage = GetOccupancyPercentage(sector);

            if (occupancyPercentage >= 90)
                label += $", nearly full, {availableSeats} seats available";
            else if (occupancyPercentage >= 50)
                label += $", partially occupied, {availableSeats} seats available";
            else
                label += $", mostly available, {availableSeats} seats available";
        }

        label += ". Click to view details.";
        return label;
    }

    private string GetSectorStatusClass(StadiumSectorDto sector)
    {
        if (selectedEventId > 0 && eventSeatStatus != null)
        {
            var occupancyPercentage = GetOccupancyPercentage(sector);

            if (occupancyPercentage >= 90) return "status-full";
            if (occupancyPercentage >= 50) return "status-partial";
            return "status-available";
        }

        return "";
    }

    private int GetAvailableSeats(StadiumSectorDto sector)
    {
        if (selectedEventId > 0 && eventSeatStatus?.SectorSummaries.TryGetValue(sector.Id, out var summary) == true)
        {
            return summary.FreeSeats;
        }
        return sector.AvailableSeats;
    }

    private int GetOccupancyPercentage(StadiumSectorDto sector)
    {
        if (selectedEventId > 0 && eventSeatStatus?.SectorSummaries.TryGetValue(sector.Id, out var summary) == true)
        {
            return (int)summary.OccupancyPercentage;
        }
        return 0;
    }

    private int GetOverallOccupancyPercentage()
    {
        if (eventSeatStatus?.SectorSummaries?.Any() != true || stadiumSummary == null)
            return 0;

        var totalSeats = stadiumSummary.TotalSeats;
        var occupiedSeats = eventSeatStatus.SectorSummaries.Values.Sum(s => s.SoldSeats + s.HeldSeats);

        return totalSeats > 0 ? (int)((double)occupiedSeats / totalSeats * 100) : 0;
    }

    private bool IsVipSector(StadiumSectorDto sector)
    {
        var name = sector.Name.ToUpper();
        return name.Contains("VIP") ||
               name.Contains("PREMIUM") ||
               name.Contains("LUXURY") ||
               name.Contains("EXECUTIVE") ||
               name.Contains("SUITE");
    }

    // Utility methods
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

    private string GetDefaultSectorColor(string standCode)
    {
        return standCode switch
        {
            "N" => "#3b82f6",
            "S" => "#10b981",
            "E" => "#f59e0b",
            "W" => "#8b5cf6",
            _ => "#6b7280"
        };
    }

    public void Dispose()
    {
        // Clean up any resources if needed
        Logger.LogInformation("Stadium Overview component disposed");
    }
}