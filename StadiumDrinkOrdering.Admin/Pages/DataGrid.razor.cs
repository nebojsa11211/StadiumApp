using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class DataGrid : ComponentBase
{
    [Inject] public IAdminApiService ApiService { get; set; } = default!;
    [Inject] public ICentralizedLoggingClient LoggingClient { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;

    public class TableInfo
    {
        public string Name { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string Schema { get; set; } = string.Empty;
        public int ColumnCount { get; set; }
        public List<ColumnInfo> Columns { get; set; } = new List<ColumnInfo>();
    }

    public class ColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public int? MaxLength { get; set; }
    }

    private List<TableInfo> tables = new List<TableInfo>();
    private List<Dictionary<string, object?>> tableData = new List<Dictionary<string, object?>>();
    private List<ColumnInfo> columns = new List<ColumnInfo>();
    private Dictionary<string, string> filters = new Dictionary<string, string>();
    
    private string selectedTable = "";
    private string sortColumn = "";
    private string sortDirection = "asc";
    private int currentPage = 1;
    private int pageSize = 20;
    private int totalCount = 0;
    private int totalPages = 0;
    private bool showTableInfo = false;
    
    private bool isLoading = true;
    private bool isLoadingData = false;
    private string errorMessage = "";
    
    // Modal states
    private bool showGenerateModal = false;
    private bool showDeleteModal = false;
    private bool isGenerating = false;
    private bool isDeleting = false;
    
    // Generate data properties
    private int recordCount = 10;
    private string generateValidationMessage = "";
    private List<string> generateDependencies = new List<string>();
    
    // Delete confirmation properties
    private string deleteConfirmation = "";
    private string deleteValidationMessage = "";
    private List<string> deleteDependencies = new List<string>();
    
    // Toast notification properties
    private string toastMessage = "";
    private string toastTitle = "";
    private string toastIcon = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadTables();
    }

    private async Task LoadTables()
    {
        try
        {
            isLoading = true;
            errorMessage = "";
            
            var tables_result = await ApiService.GetAsync<List<TableInfo>>("datagrid/tables");
            if (tables_result != null)
            {
                tables = tables_result;
            }
            else
            {
                errorMessage = "Failed to connect to database. Please check your connection settings.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Connection failed: {ex.Message}";
            // Log error to centralized logging
            await LoggingClient.LogErrorAsync(ex, "DataGrid.LoadTables", "SystemError", 
                details: $"Failed to load database tables: {ex.Message}");
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task RetryConnection()
    {
        await LoadTables();
    }

    private async Task OnTableChanged(ChangeEventArgs e)
    {
        selectedTable = e.Value?.ToString() ?? "";
        showTableInfo = false;
        
        if (!string.IsNullOrEmpty(selectedTable))
        {
            // Reset state
            currentPage = 1;
            sortColumn = "";
            sortDirection = "asc";
            filters.Clear();
            
            // Get columns for the selected table
            var table = tables.FirstOrDefault(t => t.EntityName == selectedTable);
            
            if (table != null)
            {
                columns = table.Columns;
            }
            
            await LoadTableData();
        }
        else
        {
            tableData.Clear();
            columns.Clear();
        }
    }

    private async Task LoadTableData()
    {
        if (string.IsNullOrEmpty(selectedTable)) return;
        
        try
        {
            isLoadingData = true;
            errorMessage = "";
            
            var queryParams = new List<string>
            {
                $"page={currentPage}",
                $"pageSize={pageSize}"
            };
            
            if (!string.IsNullOrEmpty(sortColumn))
            {
                queryParams.Add($"sortColumn={HttpUtility.UrlEncode(sortColumn)}");
                queryParams.Add($"sortDirection={sortDirection}");
            }
            
            if (filters.Any())
            {
                var filterJson = JsonSerializer.Serialize(filters);
                queryParams.Add($"filters={HttpUtility.UrlEncode(filterJson)}");
            }
            
            var url = $"datagrid/table-data/{selectedTable}?{string.Join("&", queryParams)}";
            var response = await ApiService.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (result != null)
                {
                    if (result.ContainsKey("data"))
                    {
                        var dataJson = result["data"]?.ToString();
                        if (!string.IsNullOrEmpty(dataJson))
                        {
                            tableData = JsonSerializer.Deserialize<List<Dictionary<string, object?>>>(dataJson, 
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Dictionary<string, object?>>();
                        }
                    }
                    
                    if (result.ContainsKey("totalCount"))
                    {
                        var totalCountStr = result["totalCount"]?.ToString();
                        if (!string.IsNullOrEmpty(totalCountStr) && int.TryParse(totalCountStr, out var parsedTotalCount))
                        {
                            totalCount = parsedTotalCount;
                        }
                    }

                    if (result.ContainsKey("totalPages"))
                    {
                        var totalPagesStr = result["totalPages"]?.ToString();
                        if (!string.IsNullOrEmpty(totalPagesStr) && int.TryParse(totalPagesStr, out var parsedTotalPages))
                        {
                            totalPages = parsedTotalPages;
                        }
                    }
                }
            }
            else
            {
                errorMessage = $"Failed to load data for table {selectedTable}";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading table data: {ex.Message}";
            // Log error to centralized logging
            await LoggingClient.LogErrorAsync(ex, "DataGrid.LoadTableData", "SystemError", 
                details: $"Failed to load data for table {selectedTable}: {ex.Message}");
        }
        finally
        {
            isLoadingData = false;
        }
    }

    private async Task Sort(string columnName)
    {
        if (sortColumn == columnName)
        {
            sortDirection = sortDirection == "asc" ? "desc" : "asc";
        }
        else
        {
            sortColumn = columnName;
            sortDirection = "asc";
        }
        
        await LoadTableData();
    }

    private void OnFilterChanged(string columnName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            filters.Remove(columnName);
        }
        else
        {
            filters[columnName] = value;
        }
    }

    private async Task ApplyFilters()
    {
        currentPage = 1;
        await LoadTableData();
    }

    private async Task ClearFilters()
    {
        filters.Clear();
        currentPage = 1;
        await LoadTableData();
    }

    private void OnQuickSearchChanged(ChangeEventArgs e)
    {
        // This could be enhanced to search across all columns
        // For now, it's a placeholder for future enhancement
    }

    private string GetFilterValue(string columnName)
    {
        return filters.ContainsKey(columnName) ? filters[columnName] : "";
    }

    private async Task GoToPage(int page)
    {
        currentPage = page;
        await LoadTableData();
    }

    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newSize))
        {
            pageSize = newSize;
            currentPage = 1;
            await LoadTableData();
        }
    }

    private async Task RefreshData()
    {
        await LoadTableData();
    }

    private void ShowTableInfo()
    {
        showTableInfo = !showTableInfo;
    }

    private async Task ExportToCsv()
    {
        if (string.IsNullOrEmpty(selectedTable)) return;
        
        try
        {
            var queryParams = new List<string>();
            
            if (!string.IsNullOrEmpty(sortColumn))
            {
                queryParams.Add($"sortColumn={HttpUtility.UrlEncode(sortColumn)}");
                queryParams.Add($"sortDirection={sortDirection}");
            }
            
            if (filters.Any())
            {
                var filterJson = JsonSerializer.Serialize(filters);
                queryParams.Add($"filters={HttpUtility.UrlEncode(filterJson)}");
            }
            
            var url = $"datagrid/export/{selectedTable}";
            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }
            
            var response = await ApiService.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = $"{selectedTable}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                // Use JavaScript to download the file
                await JS.InvokeVoidAsync("downloadFileFromStream", fileName, bytes);
            }
            else
            {
                errorMessage = "Failed to export data";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error exporting data: {ex.Message}";
            // Log error to centralized logging
            await LoggingClient.LogErrorAsync(ex, "DataGrid.ExportToCsv", "SystemError", 
                details: $"Failed to export table {selectedTable}: {ex.Message}");
        }
    }

    #region Modal and Action Methods

    private async Task ShowGenerateModal()
    {
        generateValidationMessage = "";
        generateDependencies.Clear();
        recordCount = 10;
        
        try
        {
            var response = await ApiService.GetAsync($"datagrid/dependencies/{selectedTable}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<DependencyCheckResult>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (result != null && !result.CanProceed)
                {
                    generateValidationMessage = result.Message;
                    generateDependencies = result.Dependencies ?? new List<string>();
                }
            }
        }
        catch (Exception ex)
        {
            generateValidationMessage = $"Error checking dependencies: {ex.Message}";
        }
        
        showGenerateModal = true;
    }

    private void HideGenerateModal()
    {
        showGenerateModal = false;
        generateValidationMessage = "";
        generateDependencies.Clear();
        recordCount = 10;
    }

    private async Task GenerateData()
    {
        try
        {
            isGenerating = true;
            
            var request = new { Count = recordCount };
            var response = await ApiService.PostAsync($"datagrid/generate-data/{selectedTable}", request);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (result != null && result.ContainsKey("generatedCount"))
                {
                    var generatedCount = result["generatedCount"].ToString();
                    ShowToast("Success", $"Successfully generated {generatedCount} records", "oi-check", "success");
                    await LoadTableData();
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<Dictionary<string, object>>(errorContent, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                var message = errorResult?.ContainsKey("message") == true 
                    ? errorResult["message"].ToString() 
                    : "Failed to generate data";
                    
                ShowToast("Error", message, "oi-warning", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error", $"Error generating data: {ex.Message}", "oi-warning", "danger");
        }
        finally
        {
            isGenerating = false;
            HideGenerateModal();
        }
    }

    private async Task ShowDeleteModal()
    {
        deleteValidationMessage = "";
        deleteDependencies.Clear();
        deleteConfirmation = "";
        
        try
        {
            var response = await ApiService.GetAsync($"datagrid/dependencies/{selectedTable}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<DependencyCheckResult>(json, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (result != null && result.Dependencies?.Any() == true)
                {
                    deleteValidationMessage = result.Message;
                    deleteDependencies = result.Dependencies ?? new List<string>();
                }
            }
        }
        catch (Exception ex)
        {
            deleteValidationMessage = $"Error checking dependencies: {ex.Message}";
        }
        
        showDeleteModal = true;
    }

    private void HideDeleteModal()
    {
        showDeleteModal = false;
        deleteValidationMessage = "";
        deleteDependencies.Clear();
        deleteConfirmation = "";
    }

    private async Task DeleteAllData()
    {
        try
        {
            isDeleting = true;
            
            var (success, errorMessage) = await ApiService.DeleteAsync($"datagrid/clear-table/{selectedTable}");
            
            if (success)
            {
                ShowToast("Success", "Successfully deleted all records from the table", "oi-check", "success");
                await LoadTableData();
            }
            else
            {
                // Show the actual error message from the API
                ShowToast("Error", errorMessage ?? "Failed to delete data", "oi-warning", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error", $"Error deleting data: {ex.Message}", "oi-warning", "danger");
        }
        finally
        {
            isDeleting = false;
            HideDeleteModal();
        }
    }

    private void ShowToast(string title, string message, string icon, string type)
    {
        toastTitle = title;
        toastMessage = message;
        toastIcon = icon;
        
        // Auto-hide toast after 5 seconds
        Task.Run(async () =>
        {
            await Task.Delay(5000);
            await InvokeAsync(() =>
            {
                HideToast();
                StateHasChanged();
            });
        });
    }

    private void HideToast()
    {
        toastMessage = "";
        toastTitle = "";
        toastIcon = "";
    }

    #endregion

    public class DependencyCheckResult
    {
        public bool CanProceed { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Dependencies { get; set; } = new List<string>();
    }
}