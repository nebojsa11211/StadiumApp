using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Customers : ComponentBase
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool isLoading = false;
    private List<UserDto> allCustomers = new();
    private List<UserDto> filteredCustomers = new();

    // Summary data
    private int totalCustomers = 0;
    private int activeCustomers = 0;
    private int newCustomersThisMonth = 0;

    // Filters
    private string selectedStatus = "";
    private string searchTerm = "";

    // Sorting
    private readonly TableSortState sortState = new();
    private static readonly Dictionary<string, Func<UserDto, object?>> SortSelectors = new()
    {
        ["id"] = u => u.Id,
        ["name"] = u => u.Name,
        ["username"] = u => u.Username,
        ["email"] = u => u.Email,
        ["status"] = u => u.IsActive,
        ["created"] = u => u.CreatedAt,
    };

    // Edit modal
    private bool showEditModal = false;
    private UpdateUserDto editUser = new();
    private int editUserId = 0;
    private bool isUpdating = false;
    private bool isActiveChecked = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadCustomers();
    }

    private async Task LoadCustomers()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            var customers = await AdminApiService.GetUsersAsync(new UserFilterDto
            {
                Role = UserRole.Customer
            });
            if (customers != null)
            {
                allCustomers = customers.ToList();
                CalculateSummaryData();
                FilterCustomers();
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to load customers:", ex.Message);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void CalculateSummaryData()
    {
        totalCustomers = allCustomers.Count;
        activeCustomers = allCustomers.Count(u => u.IsActive);
        newCustomersThisMonth = allCustomers.Count(u => u.CreatedAt.Month == DateTime.Now.Month && u.CreatedAt.Year == DateTime.Now.Year);
    }

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        FilterCustomers();
    }

    private void FilterCustomers()
    {
        var query = allCustomers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(selectedStatus))
        {
            if (bool.TryParse(selectedStatus, out var isActive))
            {
                query = query.Where(u => u.IsActive == isActive);
            }
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                (u.Name?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var ordered = sortState.Column is null
            ? query.OrderByDescending(u => u.CreatedAt)
            : sortState.Apply(query, SortSelectors);
        filteredCustomers = ordered.ToList();
        StateHasChanged();
    }

    private async Task RefreshData()
    {
        await LoadCustomers();
        await JSRuntime.InvokeVoidAsync("showToast", "Customers refreshed successfully", "success");
    }

    private void ClearFilters()
    {
        selectedStatus = "";
        searchTerm = "";
        FilterCustomers();
    }

    private static UpdateUserDto BuildUpdateDto(UserDto user) => new()
    {
        Username = user.Username,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber,
        Role = UserRole.Customer,
        IsActive = user.IsActive
    };

    private void EditCustomer(UserDto customer)
    {
        editUserId = customer.Id;
        isActiveChecked = customer.IsActive;
        editUser = BuildUpdateDto(customer);
        showEditModal = true;
    }

    private void CloseEditModal()
    {
        showEditModal = false;
        editUser = new UpdateUserDto();
        editUserId = 0;
    }

    private async Task UpdateCustomer()
    {
        isUpdating = true;
        try
        {
            editUser.IsActive = isActiveChecked;
            editUser.Role = UserRole.Customer;

            var result = await AdminApiService.UpdateUserAsync(editUserId, editUser);
            if (result != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Customer updated successfully", "success");
                CloseEditModal();
                await LoadCustomers();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to update customer", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update customer", "error");
        }
        finally
        {
            isUpdating = false;
        }
    }

    private async Task SetActive(UserDto customer, bool active)
    {
        try
        {
            var dto = BuildUpdateDto(customer);
            dto.IsActive = active;

            var result = await AdminApiService.UpdateUserAsync(customer.Id, dto);
            if (result != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast",
                    active ? "Customer activated successfully" : "Customer deactivated successfully",
                    active ? "success" : "warning");
                await LoadCustomers();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to update customer status", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update customer status", "error");
        }
    }

    private async Task DeleteCustomer(int customerId)
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this customer? This action cannot be undone."))
        {
            return;
        }

        try
        {
            var success = await AdminApiService.DeleteUserAsync(customerId);
            if (success)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Customer deleted successfully", "success");
                await LoadCustomers();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to delete customer", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to delete customer", "error");
        }
    }
}
