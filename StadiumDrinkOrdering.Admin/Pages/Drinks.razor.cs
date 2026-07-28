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

    // Category management overlay (replaces the former standalone /categories page)
    private bool showCategoryManager = false;

    // "Generate common categories & drinks" starter-catalog action
    private bool isGeneratingCatalog = false;

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

    private void ShowCategoryManager() => showCategoryManager = true;

    private void HideCategoryManager() => showCategoryManager = false;

    // A category rename/delete changes the CategoryName shown on drink rows and the
    // category filter, so refresh both lists after any change made in the overlay.
    private async Task OnCategoriesChanged()
    {
        await LoadCategories();
        await LoadDrinks();

        // Drop a filter that points at a category that no longer exists.
        if (!string.IsNullOrEmpty(selectedCategory) &&
            !categories.Any(c => c.Id.ToString() == selectedCategory))
        {
            selectedCategory = "";
        }
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
                loadingError = L["Drinks_LoadFailedServer"];
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
            loadingError = L["Drinks_LoadError", ex.Message];
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
            ShowAlert(L["Drinks_FillRequiredFields"], "danger");
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
                    ShowAlert(L["Drinks_CreatedSuccess", created.Name], "success");
                }
                else
                {
                    ShowAlert(L["Drinks_CreateFailed"], "danger");
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
                    ShowAlert(L["Drinks_UpdatedSuccess", drinkForm.Name], "success");
                }
                else
                {
                    ShowAlert(L["Drinks_UpdateFailed"], "danger");
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
            imageError = L["Drinks_ImageTooLarge"];
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
            imageError = L["Drinks_ImageReadError", ex.Message];
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
            ShowAlert(drink.IsAvailable ? L["Drinks_DisabledSuccess", drink.Name] : L["Drinks_EnabledSuccess", drink.Name], "success");
        }
        else
        {
            ShowAlert(L["Drinks_AvailabilityUpdateFailed"], "danger");
        }
    }

    private async Task DeleteDrink(DrinkDto drink)
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", L["Drinks_DeleteConfirm", drink.Name].Value))
        {
            var success = await ApiService.DeleteDrinkAsync(drink.Id);
            if (success)
            {
                await LoadDrinks();
                ShowAlert(L["Drinks_DeletedSuccess", drink.Name], "success");
            }
            else
            {
                ShowAlert(L["Drinks_DeleteFailed"], "danger");
            }
        }
    }

    /// <summary>
    /// Fills an empty (or partial) catalog with the standard set of categories and drinks. The API
    /// side is idempotent — anything already present is skipped — so pressing this twice is harmless.
    /// </summary>
    private async Task GenerateCatalog()
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", L["Drinks_GenerateCatalogConfirm"].Value))
            return;

        isGeneratingCatalog = true;
        try
        {
            var result = await ApiService.SeedCatalogAsync();
            if (result == null)
            {
                ShowAlert(L["Drinks_GenerateCatalogFailed"], "danger");
                return;
            }

            await LoadCategories();
            await LoadDrinks();

            if (result.DrinksCreated == 0 && result.CategoriesCreated == 0)
            {
                ShowAlert(L["Drinks_GenerateCatalogNothingNew"], "success");
            }
            else
            {
                ShowAlert(L["Drinks_GenerateCatalogSuccess", result.DrinksCreated, result.CategoriesCreated, result.DrinksSkipped], "success");
            }
        }
        finally
        {
            isGeneratingCatalog = false;
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