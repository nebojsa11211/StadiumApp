using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using System.Net.Http;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Logs : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private IConsoleLoggingToggleService ToggleService { get; set; } = default!;

    private PagedLogsDto? logs;
    private LogSummaryDto? logSummary;
    private bool isLoading = true;
    private string selectedLevel = "";
    private string? statusMessage;
    private bool isSuccess = false;
    private bool isConsoleToSystemLoggingEnabled = false;
    private HashSet<int> expandedMessages = new HashSet<int>();
    
    // Date filtering properties
    private string selectedDateRange = "last24hours";
    private DateTime? fromDate;
    private DateTime? toDate;
    private bool showCustomDateRange = false;
    private DateTime customFromDate = DateTime.UtcNow.AddDays(-7);
    private DateTime customToDate = DateTime.UtcNow;
    private string currentFilterDescription = "Last 24 Hours";

    protected override async Task OnInitializedAsync()
    {
        // Initialize console-to-system logging state using toggle service
        try
        {
            isConsoleToSystemLoggingEnabled = await ToggleService.GetIsEnabledAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Logs] Failed to get console-to-system logging state: {ex.Message}");
            isConsoleToSystemLoggingEnabled = false;
        }
        
        // Load filter preferences from localStorage if available
        await LoadFilterPreferences();
        
        // Set default to last 24 hours
        await SetDateRange("last24hours");
        await ApiService.LogUserActionAsync("ViewLogsPage", "UserAction", "Admin viewed System Logs page");
    }

    private async Task LoadData()
    {
        isLoading = true;
        
        var filter = new LogFilterDto 
        { 
            Page = 1, 
            PageSize = 100,
            Level = selectedLevel,
            FromDate = fromDate,
            ToDate = toDate
        };

        try 
        {
            logs = await ApiService.GetLogsAsync(filter);
            logSummary = await ApiService.GetLogSummaryAsync();
            
            // Ensure we have valid objects even if API returns null
            if (logs == null)
            {
                logs = new PagedLogsDto { Logs = new List<LogEntryDto>(), TotalCount = 0 };
            }
            
            if (logSummary == null)
            {
                logSummary = new LogSummaryDto 
                { 
                    TotalLogs = 0, 
                    ErrorCount = 0, 
                    WarningCount = 0, 
                    InfoCount = 0,
                    CriticalCount = 0,
                    LogsBySource = new Dictionary<string, int>(),
                    LogsByCategory = new Dictionary<string, int>()
                };
            }
            
            // Clear any previous error messages on successful load
            statusMessage = null;
        }
        catch (HttpRequestException httpEx)
        {
            // Show specific HTTP error details
            statusMessage = httpEx.Message;
            isSuccess = false;
            
            // Set safe defaults on error
            logs = new PagedLogsDto { Logs = new List<LogEntryDto>(), TotalCount = 0 };
            logSummary = new LogSummaryDto 
            { 
                TotalLogs = 0, 
                ErrorCount = 0, 
                WarningCount = 0, 
                InfoCount = 0,
                CriticalCount = 0,
                LogsBySource = new Dictionary<string, int>(),
                LogsByCategory = new Dictionary<string, int>()
            };
        }
        catch (Exception ex)
        {
            statusMessage = $"Error loading logs: {ex.Message}";
            isSuccess = false;
            
            // Set safe defaults on error
            logs = new PagedLogsDto { Logs = new List<LogEntryDto>(), TotalCount = 0 };
            logSummary = new LogSummaryDto 
            { 
                TotalLogs = 0, 
                ErrorCount = 0, 
                WarningCount = 0, 
                InfoCount = 0,
                CriticalCount = 0,
                LogsBySource = new Dictionary<string, int>(),
                LogsByCategory = new Dictionary<string, int>()
            };
        }
        
        isLoading = false;
        StateHasChanged();
    }

    private async Task RefreshLogs()
    {
        await LoadData();
        await ApiService.LogUserActionAsync("RefreshLogs", "UserAction", "Admin refreshed system logs");
        
        statusMessage = "Logs refreshed successfully!";
        isSuccess = true;
        StateHasChanged();
        
        await Task.Delay(3000);
        statusMessage = null;
        StateHasChanged();
    }

    private async Task FilterByLevel(string level)
    {
        selectedLevel = level;
        await LoadData();
        await ApiService.LogUserActionAsync("FilterLogs", "UserAction", $"Filtered logs by level: {level}");
    }

    private string GetRowClass(string level)
    {
        return level switch
        {
            "Error" => "table-danger",
            "Warning" => "table-warning",
            "Critical" => "table-danger",
            _ => ""
        };
    }

    private string GetLevelBadgeClass(string level)
    {
        return level switch
        {
            "Info" => "bg-info",
            "Warning" => "bg-warning text-dark",
            "Error" => "bg-danger",
            "Critical" => "bg-dark",
            _ => "bg-secondary"
        };
    }

    private async Task ClearAllLogs()
    {
        var confirmResult = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to clear ALL logs? This action cannot be undone.");
        
        if (!confirmResult)
            return;

        try
        {
            var success = await ApiService.ClearAllLogsAsync();
            
            if (success)
            {
                // Reset local state immediately to prevent null reference issues
                logs = new PagedLogsDto { Logs = new List<LogEntryDto>(), TotalCount = 0 };
                logSummary = new LogSummaryDto 
                { 
                    TotalLogs = 0, 
                    ErrorCount = 0, 
                    WarningCount = 0, 
                    InfoCount = 0,
                    CriticalCount = 0,
                    LogsBySource = new Dictionary<string, int>(),
                    LogsByCategory = new Dictionary<string, int>()
                };
                
                statusMessage = "All logs cleared successfully!";
                isSuccess = true;
                
                // Force state update
                StateHasChanged();
                
                // Wait a moment for database consistency
                await Task.Delay(1000);
                
                // Refresh the data
                await LoadData();
                await ApiService.LogUserActionAsync("ClearAllLogs", "UserAction", "Admin cleared all logs");
            }
            else
            {
                statusMessage = "Failed to clear all logs. Please try again.";
                isSuccess = false;
            }
        }
        catch (Exception ex)
        {
            statusMessage = $"Error clearing logs: {ex.Message}";
            isSuccess = false;
            
            // Reset to safe state on error
            logs = new PagedLogsDto { Logs = new List<LogEntryDto>(), TotalCount = 0 };
            logSummary = new LogSummaryDto 
            { 
                TotalLogs = 0, 
                ErrorCount = 0, 
                WarningCount = 0, 
                InfoCount = 0,
                CriticalCount = 0,
                LogsBySource = new Dictionary<string, int>(),
                LogsByCategory = new Dictionary<string, int>()
            };
        }

        StateHasChanged();
        
        // Clear message after 3 seconds
        await Task.Delay(3000);
        statusMessage = null;
        StateHasChanged();
    }

    private async Task ToggleConsoleToSystemLogging(ChangeEventArgs e)
    {
        isConsoleToSystemLoggingEnabled = (bool)e.Value!;

        try
        {
            // Use toggle service to persist and update
            await ToggleService.SetIsEnabledAsync(isConsoleToSystemLoggingEnabled);
            
            // Log this action
            await ApiService.LogUserActionAsync(
                "ToggleConsoleToSystemLogging", 
                "UserAction", 
                $"Console-to-System logging: {(isConsoleToSystemLoggingEnabled ? "Enabled" : "Disabled")}"
            );

            statusMessage = $"Console-to-System logging {(isConsoleToSystemLoggingEnabled ? "enabled" : "disabled")} and saved";
            isSuccess = true;
        }
        catch (Exception ex)
        {
            statusMessage = $"Error toggling console logging: {ex.Message}";
            isSuccess = false;
        }

        StateHasChanged();
        
        // Clear message after 2 seconds
        await Task.Delay(2000);
        statusMessage = null;
        StateHasChanged();
    }

    private void ToggleMessage(int logId)
    {
        if (expandedMessages.Contains(logId))
        {
            expandedMessages.Remove(logId);
        }
        else
        {
            expandedMessages.Add(logId);
        }
        StateHasChanged();
    }
    
    private async Task SetDateRange(string range)
    {
        selectedDateRange = range;
        showCustomDateRange = false;
        
        var now = DateTime.UtcNow;
        
        switch (range)
        {
            case "last24hours":
                fromDate = now.AddDays(-1);
                toDate = now;
                currentFilterDescription = "Last 24 Hours";
                break;
            case "today":
                fromDate = now.Date;
                toDate = now.Date.AddDays(1).AddSeconds(-1);
                currentFilterDescription = "Today";
                break;
            case "last7days":
                fromDate = now.AddDays(-7);
                toDate = now;
                currentFilterDescription = "Last 7 Days";
                break;
            case "last30days":
                fromDate = now.AddDays(-30);
                toDate = now;
                currentFilterDescription = "Last 30 Days";
                break;
            case "all":
                fromDate = null;
                toDate = null;
                currentFilterDescription = "All Time";
                break;
            case "custom":
                // Don't change dates, just show the custom range UI
                selectedDateRange = "custom";
                currentFilterDescription = "Custom Range";
                return;
        }
        
        await SaveFilterPreferences();
        await LoadData();
        await ApiService.LogUserActionAsync("FilterLogsByDate", "UserAction", $"Filtered logs by: {currentFilterDescription}");
    }
    
    private void ToggleCustomDateRange()
    {
        showCustomDateRange = !showCustomDateRange;
        if (showCustomDateRange)
        {
            selectedDateRange = "custom";
        }
    }
    
    private async Task ApplyCustomDateRange()
    {
        // Convert to UTC if not already
        fromDate = customFromDate.Kind == DateTimeKind.Utc ? customFromDate : customFromDate.ToUniversalTime();
        toDate = customToDate.Kind == DateTimeKind.Utc ? customToDate : customToDate.ToUniversalTime();
        selectedDateRange = "custom";
        
        // Display in local time for user readability
        var fromStr = fromDate?.ToLocalTime().ToString("MMM dd, yyyy HH:mm") ?? "Beginning";
        var toStr = toDate?.ToLocalTime().ToString("MMM dd, yyyy HH:mm") ?? "Now";
        currentFilterDescription = $"Custom: {fromStr} to {toStr}";
        
        showCustomDateRange = false;
        
        await SaveFilterPreferences();
        await LoadData();
        await ApiService.LogUserActionAsync("FilterLogsByCustomDate", "UserAction", currentFilterDescription);
    }
    
    private async Task LoadFilterPreferences()
    {
        try
        {
            // Try to load saved filter preferences from localStorage
            var savedRange = await JS.InvokeAsync<string?>("localStorage.getItem", "logsFilterDateRange");
            if (!string.IsNullOrEmpty(savedRange))
            {
                selectedDateRange = savedRange;
            }
            
            var savedFromDate = await JS.InvokeAsync<string?>("localStorage.getItem", "logsFilterFromDate");
            if (!string.IsNullOrEmpty(savedFromDate) && DateTime.TryParse(savedFromDate, out var parsedFrom))
            {
                customFromDate = parsedFrom;
            }
            
            var savedToDate = await JS.InvokeAsync<string?>("localStorage.getItem", "logsFilterToDate");
            if (!string.IsNullOrEmpty(savedToDate) && DateTime.TryParse(savedToDate, out var parsedTo))
            {
                customToDate = parsedTo;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading filter preferences: {ex.Message}");
        }
    }
    
    private async Task SaveFilterPreferences()
    {
        try
        {
            // Save current filter preferences to localStorage
            await JS.InvokeVoidAsync("localStorage.setItem", "logsFilterDateRange", selectedDateRange);
            
            if (selectedDateRange == "custom" && fromDate.HasValue && toDate.HasValue)
            {
                await JS.InvokeVoidAsync("localStorage.setItem", "logsFilterFromDate", fromDate.Value.ToString("yyyy-MM-ddTHH:mm"));
                await JS.InvokeVoidAsync("localStorage.setItem", "logsFilterToDate", toDate.Value.ToString("yyyy-MM-ddTHH:mm"));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving filter preferences: {ex.Message}");
        }
    }
}