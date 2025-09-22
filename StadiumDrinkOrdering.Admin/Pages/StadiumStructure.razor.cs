using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumStructure : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private StadiumSummaryDto? summary;
    private IBrowserFile? selectedFile;
    private string? selectedFileName;
    private bool isImporting = false;
    private bool isExporting = false;
    private bool isClearing = false;
    private bool isRefreshingCache = false;
    private bool isLoadingSummary = true;
    private bool showClearModal = false;
    private bool importSuccess = false;
    private string importMessage = "";
    private string importError = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadSummary();
    }

    private async Task LoadSummary()
    {
        isLoadingSummary = true;
        try
        {
            summary = await ApiService.GetStadiumSummaryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading summary: {ex.Message}");
        }
        finally
        {
            isLoadingSummary = false;
            StateHasChanged();
        }
    }

    private void HandleFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
        selectedFileName = e.File.Name;
        importError = "";
        importSuccess = false;
    }

    private async Task ImportStructure()
    {
        if (selectedFile == null) return;

        isImporting = true;
        importError = "";
        importSuccess = false;

        try
        {
            using var stream = selectedFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB limit
            var success = await ApiService.ImportStadiumStructureAsync(stream);

            if (success)
            {
                importSuccess = true;
                importMessage = $"Successfully imported stadium structure from {selectedFile.Name}";
                selectedFile = null;
                selectedFileName = null;
                await LoadSummary();
            }
            else
            {
                importError = "Failed to import stadium structure. Please check the file format and try again.";
            }
        }
        catch (Exception ex)
        {
            importError = $"Error importing file: {ex.Message}";
        }
        finally
        {
            isImporting = false;
            StateHasChanged();
        }
    }

    private void ShowClearConfirmation() => showClearModal = true;
    private void HideClearConfirmation() => showClearModal = false;

    private async Task ClearStructure()
    {
        isClearing = true;
        try
        {
            var success = await ApiService.ClearStadiumStructureAsync();
            if (success)
            {
                importSuccess = true;
                importMessage = "Stadium structure cleared successfully";
                await LoadSummary();
            }
            else
            {
                importError = "Failed to clear stadium structure";
            }
        }
        catch (Exception ex)
        {
            importError = $"Error clearing structure: {ex.Message}";
        }
        finally
        {
            isClearing = false;
            showClearModal = false;
            StateHasChanged();
        }
    }

    private async Task ExportStructure()
    {
        isExporting = true;
        try
        {
            var stream = await ApiService.ExportStadiumStructureAsync();
            if (stream != null)
            {
                var fileName = $"stadium-structure-{DateTime.Now:yyyyMMdd-HHmmss}.json";

                var bytes = System.Text.Encoding.UTF8.GetBytes(stream);
                using var memoryStream = new MemoryStream(bytes);
                using var streamRef = new DotNetStreamReference(memoryStream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

                importSuccess = true;
                importMessage = $"Stadium structure exported as {fileName}";
            }
            else
            {
                importError = "Failed to export stadium structure";
            }
        }
        catch (Exception ex)
        {
            importError = $"Error exporting structure: {ex.Message}";
        }
        finally
        {
            isExporting = false;
            StateHasChanged();
        }
    }

    private async Task RefreshCache()
    {
        isRefreshingCache = true;
        try
        {
            var success = await ApiService.RefreshStadiumCacheAsync();
            if (success)
            {
                importSuccess = true;
                importMessage = "Stadium layout cache refreshed successfully. Stadium Overview will now display updated data.";
                importError = "";
            }
            else
            {
                importError = "Failed to refresh stadium layout cache";
                importMessage = "";
                importSuccess = false;
            }
        }
        catch (Exception ex)
        {
            importError = $"Error refreshing cache: {ex.Message}";
            importMessage = "";
            importSuccess = false;
        }
        finally
        {
            isRefreshingCache = false;
            StateHasChanged();
        }
    }
}