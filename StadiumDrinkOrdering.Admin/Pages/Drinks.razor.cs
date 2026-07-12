using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Drinks : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<DrinkDto>? drinks;
    private List<CategoryDto> categories = new();
    private DrinkDto? editingDrink;
    private bool showDrinkModal = false;
    private bool isSaving = false;
    private string selectedCategory = "";
    private string availabilityFilter = "";
    private string searchTerm = "";
    private string alertMessage = "";
    private string alertType = "";
    private bool loadingFailed = false;
    private string loadingError = "";
    private bool isUploadingImage = false;
    private string imageError = "";

    // Upload guardrails: reject anything over 5 MB before we touch it, then downscale the picked
    // image to a thumbnail so the stored data URL stays small (ImageUrl is a text column, so the
    // image travels inline with the drink — no separate storage or migration needed).
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private const int ImageMaxDimension = 400;

    private DrinkFormModel drinkForm = new();

    // Restock ("Add stock") modal state
    private bool showRestockModal = false;
    private DrinkDto? restockDrink;
    private int restockQuantity = 1;
    private string? restockNote;
    private bool isRestocking = false;

    // Stock-history modal state
    private bool showHistoryModal = false;
    private DrinkDto? historyDrink;
    private List<StockMovementDto>? stockMovements;
    private bool loadingHistory = false;

    // Below this level a drink is flagged as low stock (badge, filter, metric).
    private const int LowStockThreshold = 10;

    // Sorting
    private readonly TableSortState sortState = new();
    private readonly PagedView<DrinkDto> pager = new();
    private static readonly Dictionary<string, Func<DrinkDto, object?>> SortSelectors = new()
    {
        ["name"] = d => d.Name,
        ["category"] = d => d.CategoryName,
        ["description"] = d => d.Description,
        ["price"] = d => d.Price,
        ["stock"] = d => d.StockQuantity,
        ["status"] = d => d.IsAvailable,
    };

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
        await LoadDrinks();
    }

    private async Task LoadCategories()
    {
        var result = await ApiService.GetCategoriesAsync();
        categories = result?.OrderBy(c => c.SortOrder).ThenBy(c => c.Name).ToList() ?? new List<CategoryDto>();
    }

    // Only active categories are offered when creating/editing a drink, but an existing drink
    // may already point at a now-inactive category, so keep that one selectable while editing.
    private IEnumerable<CategoryDto> SelectableCategories =>
        categories.Where(c => c.IsActive || c.Id == drinkForm.CategoryId);

    private async Task LoadDrinks()
    {
        try
        {
            loadingFailed = false;
            loadingError = "";
            var result = await ApiService.GetDrinksAsync();

            if (result == null)
            {
                // API returned an error (Bad Request, etc.)
                loadingFailed = true;
                loadingError = "Failed to load drinks. The server returned an error.";
                drinks = new List<DrinkDto>(); // Set empty list to show error state
            }
            else
            {
                drinks = result.ToList();
            }
        }
        catch (Exception ex)
        {
            loadingFailed = true;
            loadingError = $"An error occurred while loading drinks: {ex.Message}";
            drinks = new List<DrinkDto>();
        }
    }

    private IEnumerable<DrinkDto> FilteredDrinks
    {
        get
        {
            if (drinks == null) return Enumerable.Empty<DrinkDto>();

            var filtered = drinks.AsEnumerable();

            // Category filter
            if (!string.IsNullOrEmpty(selectedCategory) && int.TryParse(selectedCategory, out var categoryId))
            {
                filtered = filtered.Where(d => d.CategoryId == categoryId);
            }

            // Availability filter
            filtered = availabilityFilter switch
            {
                "available" => filtered.Where(d => d.IsAvailable && d.StockQuantity > 0),
                "unavailable" => filtered.Where(d => !d.IsAvailable || d.StockQuantity == 0),
                "lowstock" => filtered.Where(d => d.StockQuantity < LowStockThreshold && d.IsAvailable),
                _ => filtered
            };

            // Search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var search = searchTerm.ToLower();
                filtered = filtered.Where(d => 
                    d.Name.ToLower().Contains(search) ||
                    (d.Description?.ToLower().Contains(search) ?? false));
            }

            var ordered = sortState.Column is null
                ? filtered.OrderBy(d => d.CategoryName).ThenBy(d => d.Name)
                : sortState.Apply(filtered, SortSelectors);
            return ordered;
        }
    }

    private void ShowCreateDrinkModal()
    {
        editingDrink = null;
        var defaultCategory = categories.FirstOrDefault(c => c.IsActive) ?? categories.FirstOrDefault();
        drinkForm = new DrinkFormModel { IsAvailable = true, CategoryId = defaultCategory?.Id ?? 0 };
        imageError = "";
        showDrinkModal = true;
    }

    private void ShowEditDrinkModal(DrinkDto drink)
    {
        editingDrink = drink;
        drinkForm = new DrinkFormModel
        {
            Name = drink.Name,
            Description = drink.Description,
            Price = drink.Price,
            StockQuantity = drink.StockQuantity,
            ImageUrl = drink.ImageUrl,
            CategoryId = drink.CategoryId,
            IsAvailable = drink.IsAvailable
        };
        imageError = "";
        showDrinkModal = true;
    }

    private async Task HideDrinkModal()
    {
        showDrinkModal = false;
        editingDrink = null;
        drinkForm = new();

        // Clean up any orphaned modal backdrops
        await JSRuntime.InvokeVoidAsync("eval", @"
            setTimeout(() => {
                document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());
                document.body.classList.remove('modal-open');
                document.body.style.overflow = '';
                document.body.style.paddingRight = '';
            }, 100);
        ");
    }

    private async Task SaveDrink()
    {
        if (string.IsNullOrWhiteSpace(drinkForm.Name) || drinkForm.Price <= 0 || drinkForm.CategoryId <= 0)
        {
            ShowAlert("Please fill in all required fields", "danger");
            return;
        }

        isSaving = true;
        try
        {
            if (editingDrink == null)
            {
                // Create new drink
                var createDto = new CreateDrinkDto
                {
                    Name = drinkForm.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(drinkForm.Description) ? null : drinkForm.Description.Trim(),
                    Price = drinkForm.Price,
                    StockQuantity = drinkForm.StockQuantity,
                    ImageUrl = string.IsNullOrWhiteSpace(drinkForm.ImageUrl) ? null : drinkForm.ImageUrl.Trim(),
                    CategoryId = drinkForm.CategoryId,
                    IsAvailable = drinkForm.IsAvailable
                };

                var created = await ApiService.CreateDrinkAsync(createDto);
                if (created != null)
                {
                    await LoadDrinks();
                    await HideDrinkModal();
                    ShowAlert($"Drink '{created.Name}' created successfully", "success");
                }
                else
                {
                    ShowAlert("Failed to create drink", "danger");
                }
            }
            else
            {
                // Update existing drink
                var updateDto = new UpdateDrinkDto
                {
                    Name = drinkForm.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(drinkForm.Description) ? null : drinkForm.Description.Trim(),
                    Price = drinkForm.Price,
                    StockQuantity = drinkForm.StockQuantity,
                    ImageUrl = string.IsNullOrWhiteSpace(drinkForm.ImageUrl) ? null : drinkForm.ImageUrl.Trim(),
                    CategoryId = drinkForm.CategoryId,
                    IsAvailable = drinkForm.IsAvailable
                };

                var result = await ApiService.UpdateDrinkAsync(editingDrink.Id, updateDto);
                if (result != null)
                {
                    await LoadDrinks();
                    await HideDrinkModal();
                    ShowAlert($"Drink '{drinkForm.Name}' updated successfully", "success");
                }
                else
                {
                    ShowAlert("Failed to update drink", "danger");
                }
            }
        }
        finally
        {
            isSaving = false;
        }
    }

    private async Task OnImageSelected(InputFileChangeEventArgs e)
    {
        imageError = "";
        var file = e.File;
        if (file == null)
            return;

        if (file.Size > MaxImageBytes)
        {
            imageError = "Image is too large (max 5 MB).";
            return;
        }

        isUploadingImage = true;
        try
        {
            // Blazor resizes the image for us (no JS needed); we then inline it as a data URL.
            var resized = await file.RequestImageFileAsync("image/jpeg", ImageMaxDimension, ImageMaxDimension);
            await using var stream = resized.OpenReadStream(MaxImageBytes);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            drinkForm.ImageUrl = $"data:image/jpeg;base64,{base64}";
        }
        catch (Exception ex)
        {
            imageError = $"Could not read image: {ex.Message}";
        }
        finally
        {
            isUploadingImage = false;
        }
    }

    private void ClearImage()
    {
        drinkForm.ImageUrl = null;
        imageError = "";
    }

    private async Task ToggleAvailability(DrinkDto drink)
    {
        var updateDto = new UpdateDrinkDto { IsAvailable = !drink.IsAvailable };
        var result = await ApiService.UpdateDrinkAsync(drink.Id, updateDto);

        if (result != null)
        {
            await LoadDrinks();
            ShowAlert($"Drink '{drink.Name}' {(drink.IsAvailable ? "disabled" : "enabled")}", "success");
        }
        else
        {
            ShowAlert("Failed to update drink availability", "danger");
        }
    }

    private async Task DeleteDrink(DrinkDto drink)
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete '{drink.Name}'?"))
        {
            var success = await ApiService.DeleteDrinkAsync(drink.Id);
            if (success)
            {
                await LoadDrinks();
                ShowAlert($"Drink '{drink.Name}' deleted successfully", "success");
            }
            else
            {
                ShowAlert("Failed to delete drink", "danger");
            }
        }
    }

    private void ShowRestockModal(DrinkDto drink)
    {
        restockDrink = drink;
        restockQuantity = 1;
        restockNote = null;
        showRestockModal = true;
    }

    private void HideRestockModal()
    {
        showRestockModal = false;
        restockDrink = null;
    }

    private async Task SubmitRestock()
    {
        if (restockDrink == null)
            return;

        if (restockQuantity <= 0)
        {
            ShowAlert(L["Drinks_RestockInvalidQuantity"], "danger");
            return;
        }

        isRestocking = true;
        try
        {
            var dto = new RestockDrinkDto
            {
                Quantity = restockQuantity,
                Note = string.IsNullOrWhiteSpace(restockNote) ? null : restockNote.Trim()
            };

            var updated = await ApiService.RestockDrinkAsync(restockDrink.Id, dto);
            if (updated != null)
            {
                var name = restockDrink.Name;
                var added = restockQuantity;
                HideRestockModal();
                await LoadDrinks();
                ShowAlert(L["Drinks_RestockSuccess", added, name, updated.StockQuantity], "success");
            }
            else
            {
                ShowAlert(L["Drinks_RestockFailed"], "danger");
            }
        }
        finally
        {
            isRestocking = false;
        }
    }

    private async Task ShowHistoryModal(DrinkDto drink)
    {
        historyDrink = drink;
        stockMovements = null;
        showHistoryModal = true;
        loadingHistory = true;
        try
        {
            var result = await ApiService.GetStockMovementsAsync(drink.Id);
            stockMovements = result?.ToList() ?? new List<StockMovementDto>();
        }
        finally
        {
            loadingHistory = false;
        }
    }

    private void HideHistoryModal()
    {
        showHistoryModal = false;
        historyDrink = null;
        stockMovements = null;
    }

    // Human label + badge style for a ledger entry's movement type.
    private string MovementTypeLabel(string type) => type switch
    {
        "Restock" => L["Drinks_MovementRestock"],
        "Sale" => L["Drinks_MovementSale"],
        "OrderCancelled" => L["Drinks_MovementOrderCancelled"],
        "ManualAdjustment" => L["Drinks_MovementManualAdjustment"],
        _ => type
    };

    private static string MovementBadgeClass(string type) => type switch
    {
        "Restock" => "is-active",
        "Sale" => "is-cancelled",
        "OrderCancelled" => "is-pending",
        _ => ""
    };

    private void ShowAlert(string message, string type)
    {
        alertMessage = message;
        alertType = type;
        StateHasChanged();
        
        // Auto-hide after 5 seconds
        _ = Task.Delay(5000).ContinueWith(_ => 
        {
            alertMessage = "";
            InvokeAsync(StateHasChanged);
        });
    }

    private void ClearAlert()
    {
        alertMessage = "";
    }

    private class DrinkFormModel
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}