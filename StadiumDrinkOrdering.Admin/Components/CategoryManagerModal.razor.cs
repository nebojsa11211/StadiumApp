using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class CategoryManagerModal : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private IStringLocalizer<SharedResources> L { get; set; } = default!;

    /// <summary>Controls the overlay. Opening it (re)loads the category list.</summary>
    [Parameter] public bool Visible { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    /// <summary>Raised after a create/update/delete so the host page can refresh its own copy.</summary>
    [Parameter] public EventCallback OnCategoriesChanged { get; set; }

    private List<CategoryDto>? categories;
    private bool showForm;
    private bool isSaving;
    private int? editingId;
    private CategoryFormModel form = new();

    private string alertMessage = "";
    private string alertType = "";
    private bool loadingFailed;
    private string loadingError = "";

    private bool wasVisible;

    protected override async Task OnParametersSetAsync()
    {
        if (Visible && !wasVisible)
        {
            // Fresh open: reset back to the list and pull the current data.
            showForm = false;
            editingId = null;
            alertMessage = "";
            categories = null;
            await LoadCategories();
        }
        wasVisible = Visible;
    }

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
                loadingError = L["Categories_LoadServerError"];
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
            loadingError = L["Categories_LoadException", ex.Message];
            categories = new List<CategoryDto>();
        }
    }

    private void ShowCreateForm()
    {
        editingId = null;
        var nextSort = (categories?.Count > 0 ? categories.Max(c => c.SortOrder) : 0) + 1;
        form = new CategoryFormModel { IsActive = true, SortOrder = nextSort };
        showForm = true;
    }

    private void ShowEditForm(CategoryDto category)
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
        showForm = true;
    }

    private void BackToList()
    {
        showForm = false;
        editingId = null;
        form = new();
    }

    private async Task CloseAsync()
    {
        BackToList();
        alertMessage = "";
        await OnClose.InvokeAsync();
    }

    private async Task SaveCategory()
    {
        if (string.IsNullOrWhiteSpace(form.Name))
        {
            ShowAlert(L["Categories_NameRequired"], "danger");
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
                    BackToList();
                    ShowAlert(L["Categories_Created", created.Name], "success");
                    await OnCategoriesChanged.InvokeAsync();
                }
                else
                {
                    ShowAlert(L["Categories_CreateFailed"], "danger");
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
                var name = form.Name.Trim();
                var updated = await ApiService.UpdateCategoryAsync(editingId.Value, dto);
                if (updated != null)
                {
                    await LoadCategories();
                    BackToList();
                    ShowAlert(L["Categories_Updated", name], "success");
                    await OnCategoriesChanged.InvokeAsync();
                }
                else
                {
                    ShowAlert(L["Categories_UpdateFailed"], "danger");
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
        if (!await JSRuntime.InvokeAsync<bool>("confirm", L["Categories_DeleteConfirm", category.Name].Value))
            return;

        var (success, error) = await ApiService.DeleteCategoryAsync(category.Id);
        if (success)
        {
            await LoadCategories();
            ShowAlert(L["Categories_Deleted", category.Name], "success");
            await OnCategoriesChanged.InvokeAsync();
        }
        else
        {
            ShowAlert(string.IsNullOrWhiteSpace(error)
                ? L["Categories_DeleteFailed", category.Name].Value
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
