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

        await LoadData();
        await ApiService.LogUserActionAsync("ViewLogsPage", "UserAction", "Admin viewed System Logs page");
    }

    private async Task LoadData()
    {
        isLoading = true;
        
        var filter = new LogFilterDto 
        { 
            Page = 1, 
            PageSize = 100,
            Level = selectedLevel
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
}