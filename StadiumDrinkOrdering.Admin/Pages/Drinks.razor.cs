using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Drinks : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<DrinkDto>? drinks;
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

    private DrinkFormModel drinkForm = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDrinks();
    }

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
            if (!string.IsNullOrEmpty(selectedCategory) && Enum.TryParse<DrinkCategory>(selectedCategory, out var category))
            {
                filtered = filtered.Where(d => d.Category == category);
            }

            // Availability filter
            filtered = availabilityFilter switch
            {
                "available" => filtered.Where(d => d.IsAvailable && d.StockQuantity > 0),
                "unavailable" => filtered.Where(d => !d.IsAvailable || d.StockQuantity == 0),
                "lowstock" => filtered.Where(d => d.StockQuantity < 10 && d.IsAvailable),
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

            return filtered.OrderBy(d => d.Category).ThenBy(d => d.Name);
        }
    }

    private void ShowCreateDrinkModal()
    {
        editingDrink = null;
        drinkForm = new DrinkFormModel { IsAvailable = true };
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
            Category = drink.Category,
            IsAvailable = drink.IsAvailable
        };
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
        if (string.IsNullOrWhiteSpace(drinkForm.Name) || drinkForm.Price <= 0)
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
                    Category = drinkForm.Category,
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
                    Category = drinkForm.Category,
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
        public DrinkCategory Category { get; set; } = DrinkCategory.SoftDrink;
        public bool IsAvailable { get; set; } = true;
    }
}