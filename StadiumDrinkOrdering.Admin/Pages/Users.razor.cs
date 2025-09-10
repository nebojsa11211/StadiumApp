using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Users : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private UserListDto? userList;
    private bool isLoading = true;
    private string searchTerm = "";
    private UserRole? selectedRole;
    private int currentPage = 1;
    private int pageSize = 20;

    // Modal states
    private bool showCreateModal = false;
    private bool showEditModal = false;
    private bool showChangePasswordModal = false;
    private bool showDeleteModal = false;
    private bool isDeleting = false;
    private UserDto? selectedUser;

    // Modal references
    private CreateUserModal? createUserModal;
    private EditUserModal? editUserModal;
    private ChangePasswordModal? changePasswordModal;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        isLoading = true;
        StateHasChanged();

        var filter = new UserFilterDto
        {
            SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm,
            Role = selectedRole,
            Page = currentPage,
            PageSize = pageSize
        };

        userList = await ApiService.GetUsersAsync(filter);
        isLoading = false;
        StateHasChanged();
    }

    private async Task OnFilterChanged()
    {
        currentPage = 1; // Reset to first page when filtering
        await LoadUsers();
    }

    private async Task OnPageSizeChanged()
    {
        currentPage = 1; // Reset to first page when changing page size
        await LoadUsers();
    }

    private async Task OnSearchKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await OnFilterChanged();
        }
    }

    private async Task GoToPage(int page)
    {
        if (page >= 1 && page <= (userList?.TotalPages ?? 1))
        {
            currentPage = page;
            await LoadUsers();
        }
    }

    private string GetRoleBadgeClass(UserRole role)
    {
        return role switch
        {
            UserRole.Admin => "bg-danger",
            UserRole.Bartender => "bg-warning text-dark",
            UserRole.Waiter => "bg-info",
            UserRole.Customer => "bg-success",
            _ => "bg-secondary"
        };
    }

    private int GetAdminCount()
    {
        return userList?.Users?.Count(u => u.Role == UserRole.Admin) ?? 0;
    }

    // Modal methods
    private void ShowCreateUserModal()
    {
        showCreateModal = true;
        StateHasChanged();
    }

    private void ShowEditUserModal(UserDto user)
    {
        selectedUser = user;
        showEditModal = true;
        StateHasChanged();
    }

    private void ShowChangePasswordModal(UserDto user)
    {
        selectedUser = user;
        showChangePasswordModal = true;
        StateHasChanged();
    }

    private void ShowDeleteUserModal(UserDto user)
    {
        selectedUser = user;
        showDeleteModal = true;
        StateHasChanged();
    }

    private async Task OnUserCreated()
    {
        showCreateModal = false;
        await LoadUsers();
        await JSRuntime.InvokeVoidAsync("alert", "User created successfully!");
    }

    private async Task OnUserUpdated()
    {
        showEditModal = false;
        await LoadUsers();
        await JSRuntime.InvokeVoidAsync("alert", "User updated successfully!");
    }

    private async Task OnPasswordChanged()
    {
        showChangePasswordModal = false;
        await JSRuntime.InvokeVoidAsync("alert", "Password changed successfully!");
    }

    private async Task ConfirmDeleteUser()
    {
        if (selectedUser == null) return;

        isDeleting = true;
        StateHasChanged();

        var success = await ApiService.DeleteUserAsync(selectedUser.Id);
        
        isDeleting = false;
        showDeleteModal = false;
        
        if (success)
        {
            await LoadUsers();
            await JSRuntime.InvokeVoidAsync("alert", "User deleted successfully!");
        }
        else
        {
            await JSRuntime.InvokeVoidAsync("alert", "Failed to delete user. Cannot delete the last admin user.");
        }
    }
}