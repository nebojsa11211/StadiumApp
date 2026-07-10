using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Categories : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<CategoryDto>? categories;
    private bool showModal = false;
    private bool isSaving = false;
    private int? editingId;
    private CategoryFormModel form = new();

    private string alertMessage = "";
    private string alertType = "";
    private bool loadingFailed = false;
    private string loadingError = "";

    protected override async Task OnInitializedAsync() => await LoadCategories();

    private async Task LoadCategories()
    {
        try
        {
            loadingFailed = false;
            loadingError = "";
            var result = await ApiService.GetCategoriesAsync();
            if (result == null)
            {
                loadingFailed = true;
                loadingError = "Failed to load categories. The server returned an error.";
                categories = new List<CategoryDto>();
            }
            else
            {
                categories = result.OrderBy(c => c.SortOrder).ThenBy(c => c.Name).ToList();
            }
        }
        catch (Exception ex)
        {
            loadingFailed = true;
            loadingError = $"An error occurred while loading categories: {ex.Message}";
            categories = new List<CategoryDto>();
        }
    }

    private void ShowCreateModal()
    {
        editingId = null;
        var nextSort = (categories?.Count > 0 ? categories.Max(c => c.SortOrder) : 0) + 1;
        form = new CategoryFormModel { IsActive = true, SortOrder = nextSort };
        showModal = true;
    }

    private void ShowEditModal(CategoryDto category)
    {
        editingId = category.Id;
        form = new CategoryFormModel
        {
            Name = category.Name,
            DisplayName = category.DisplayName,
            Icon = category.Icon,
            IsActive = category.IsActive,
            SortOrder = category.SortOrder
        };
        showModal = true;
    }

    private async Task HideModal()
    {
        showModal = false;
        editingId = null;
        form = new();
        await JSRuntime.InvokeVoidAsync("eval", @"
            setTimeout(() => {
                document.querySelectorAll('.modal-backdrop').forEach(b => b.remove());
                document.body.classList.remove('modal-open');
                document.body.style.overflow = '';
                document.body.style.paddingRight = '';
            }, 100);
        ");
    }

    private async Task SaveCategory()
    {
        if (string.IsNullOrWhiteSpace(form.Name))
        {
            ShowAlert("Category name is required", "danger");
            return;
        }

        isSaving = true;
        try
        {
            if (editingId == null)
            {
                var dto = new CreateCategoryDto
                {
                    Name = form.Name.Trim(),
                    DisplayName = string.IsNullOrWhiteSpace(form.DisplayName) ? null : form.DisplayName.Trim(),
                    Icon = string.IsNullOrWhiteSpace(form.Icon) ? null : form.Icon.Trim(),
                    IsActive = form.IsActive,
                    SortOrder = form.SortOrder
                };
                var created = await ApiService.CreateCategoryAsync(dto);
                if (created != null)
                {
                    await LoadCategories();
                    await HideModal();
                    ShowAlert($"Category '{created.Name}' created", "success");
                }
                else
                {
                    ShowAlert("Failed to create category. The name may already be in use.", "danger");
                }
            }
            else
            {
                var dto = new UpdateCategoryDto
                {
                    Name = form.Name.Trim(),
                    DisplayName = string.IsNullOrWhiteSpace(form.DisplayName) ? null : form.DisplayName.Trim(),
                    Icon = string.IsNullOrWhiteSpace(form.Icon) ? null : form.Icon.Trim(),
                    IsActive = form.IsActive,
                    SortOrder = form.SortOrder
                };
                var updated = await ApiService.UpdateCategoryAsync(editingId.Value, dto);
                if (updated != null)
                {
                    await LoadCategories();
                    await HideModal();
                    ShowAlert($"Category '{form.Name}' updated", "success");
                }
                else
                {
                    ShowAlert("Failed to update category. The name may already be in use.", "danger");
                }
            }
        }
        finally
        {
            isSaving = false;
        }
    }

    private async Task DeleteCategory(CategoryDto category)
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", $"Delete category '{category.Name}'?"))
            return;

        var (success, error) = await ApiService.DeleteCategoryAsync(category.Id);
        if (success)
        {
            await LoadCategories();
            ShowAlert($"Category '{category.Name}' deleted", "success");
        }
        else
        {
            ShowAlert(string.IsNullOrWhiteSpace(error)
                ? $"Cannot delete '{category.Name}'. It may still be in use."
                : error, "danger");
        }
    }

    private void ShowAlert(string message, string type)
    {
        alertMessage = message;
        alertType = type;
        StateHasChanged();
        _ = Task.Delay(5000).ContinueWith(_ =>
        {
            alertMessage = "";
            InvokeAsync(StateHasChanged);
        });
    }

    private void ClearAlert() => alertMessage = "";

    private class CategoryFormModel
    {
        public string Name { get; set; } = "";
        public string? DisplayName { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }
    }
}
