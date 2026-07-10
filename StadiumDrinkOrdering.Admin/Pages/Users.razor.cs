using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Users : ComponentBase
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool isLoading = false;
    private bool isCreatingUser = false;
    private bool isCreatingSampleUsers = false;
    private List<UserDto> allUsers = new();
    private List<UserDto> filteredUsers = new();
    private HashSet<int> selectedUserIds = new();

    // Summary data
    private int totalUsers = 0;
    private int activeUsers = 0;
    private int newUsersThisMonth = 0;
    private int adminUsers = 0;

    // Filters
    private string selectedRole = "";
    private string selectedStatus = "";
    private string searchTerm = "";

    private readonly PagedView<UserDto> pager = new();

    // Sorting
    private readonly TableSortState sortState = new();
    private static readonly Dictionary<string, Func<UserDto, object?>> SortSelectors = new()
    {
        ["id"] = u => u.Id,
        ["name"] = u => u.Name,
        ["username"] = u => u.Username,
        ["email"] = u => u.Email,
        ["role"] = u => u.Role,
        ["status"] = u => u.IsActive,
        ["created"] = u => u.CreatedAt,
    };

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        FilterUsers();
    }

    // Create user modal
    private bool showCreateModal = false;
    private CreateUserDto newUser = new();

    // Edit user modal
    private bool showEditModal = false;
    private UpdateUserDto editUser = new();
    private int editUserId = 0;
    private bool isUpdatingUser = false;
    private bool isActiveChecked = true;

    // Stats modal
    private bool showStatsModal = false;
    private bool isLoadingStats = false;
    private StaffMemberStatsDto? selectedStats;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            // This page manages internal team accounts only (Admin/Staff).
            // Customer accounts are managed on the dedicated /customers page.
            var users = await AdminApiService.GetUsersAsync(new UserFilterDto
            {
                ExcludeRole = UserRole.Customer
            });
            if (users != null)
            {
                allUsers = users.ToList();
                CalculateSummaryData();
                FilterUsers();
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to load users:", ex.Message);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void CalculateSummaryData()
    {
        totalUsers = allUsers.Count;
        activeUsers = allUsers.Count(u => u.IsActive);
        newUsersThisMonth = allUsers.Count(u => u.CreatedAt.Month == DateTime.Now.Month && u.CreatedAt.Year == DateTime.Now.Year);
        adminUsers = allUsers.Count(u => u.Role == UserRole.Admin);
    }

    private void FilterUsers()
    {
        var query = allUsers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(selectedRole))
        {
            if (Enum.TryParse<UserRole>(selectedRole, out var role))
            {
                query = query.Where(u => u.Role == role);
            }
        }

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

        // Default to newest-first until the user picks a sort column.
        var ordered = sortState.Column is null
            ? query.OrderByDescending(u => u.CreatedAt)
            : sortState.Apply(query, SortSelectors);
        filteredUsers = ordered.ToList();
        pager.Source = filteredUsers;
        pager.Reset();
        StateHasChanged();
    }

    private async Task RefreshData()
    {
        await LoadUsers();
        await JSRuntime.InvokeVoidAsync("showToast", "Users refreshed successfully", "success");
    }

    private void ClearFilters()
    {
        selectedRole = "";
        selectedStatus = "";
        searchTerm = "";
        FilterUsers();
    }

    private void ToggleSelectAll(ChangeEventArgs e)
    {
        var isSelected = (bool)e.Value!;
        selectedUserIds.Clear();
        if (isSelected)
        {
            foreach (var user in pager.PageItems)
            {
                selectedUserIds.Add(user.Id);
            }
        }
        StateHasChanged();
    }

    private void ToggleUserSelection(int userId, bool isSelected)
    {
        if (isSelected)
            selectedUserIds.Add(userId);
        else
            selectedUserIds.Remove(userId);
        StateHasChanged();
    }

    private void ShowCreateUserModal()
    {
        newUser = new CreateUserDto { Role = UserRole.Bartender };
        showCreateModal = true;
    }

    private void CloseCreateModal()
    {
        showCreateModal = false;
        newUser = new CreateUserDto();
    }

    private async Task CreateUser()
    {
        isCreatingUser = true;
        try
        {
            var result = await AdminApiService.CreateUserAsync(newUser);
            if (result != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "User created successfully", "success");
                CloseCreateModal();
                await LoadUsers();
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to create user", "error");
        }
        finally
        {
            isCreatingUser = false;
        }
    }

    private async Task ShowUserStats(UserDto user)
    {
        showStatsModal = true;
        isLoadingStats = true;
        selectedStats = null;
        StateHasChanged();

        try
        {
            selectedStats = await AdminApiService.GetUserStatsAsync(user.Id);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to load user stats:", ex.Message);
        }
        finally
        {
            isLoadingStats = false;
            StateHasChanged();
        }
    }

    private void CloseStatsModal()
    {
        showStatsModal = false;
        selectedStats = null;
    }

    private void EditUser(UserDto user)
    {
        editUserId = user.Id;
        isActiveChecked = user.IsActive;
        editUser = BuildUpdateDto(user);
        showEditModal = true;
    }

    private void CloseEditModal()
    {
        showEditModal = false;
        editUser = new UpdateUserDto();
        editUserId = 0;
    }

    private async Task UpdateUser()
    {
        isUpdatingUser = true;
        try
        {
            // Set the IsActive value from the checkbox
            editUser.IsActive = isActiveChecked;

            var result = await AdminApiService.UpdateUserAsync(editUserId, editUser);
            if (result != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "User updated successfully", "success");
                CloseEditModal();
                await LoadUsers();
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update user", "error");
        }
        finally
        {
            isUpdatingUser = false;
        }
    }

    private static UpdateUserDto BuildUpdateDto(UserDto user) => new()
    {
        Username = user.Username,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber,
        Role = user.Role,
        IsActive = user.IsActive
    };

    private Task ActivateUser(int userId) => SetUserActive(userId, true);

    private Task DeactivateUser(int userId) => SetUserActive(userId, false);

    private async Task SetUserActive(int userId, bool active)
    {
        var user = allUsers.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return;

        try
        {
            var dto = BuildUpdateDto(user);
            dto.IsActive = active;

            var result = await AdminApiService.UpdateUserAsync(userId, dto);
            if (result != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast",
                    active ? "User activated successfully" : "User deactivated successfully",
                    active ? "success" : "warning");
                await LoadUsers();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to update user status", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update user status", "error");
        }
    }

    private async Task DeleteUser(int userId)
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this user? This action cannot be undone."))
            return;

        try
        {
            var success = await AdminApiService.DeleteUserAsync(userId);
            if (success)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "User deleted successfully", "success");
                await LoadUsers();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to delete user", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to delete user", "error");
        }
    }

    private async Task ResetPassword(int userId)
    {
        var newPassword = await JSRuntime.InvokeAsync<string?>("prompt", "Enter a new password for this user (min 6 characters):");
        if (string.IsNullOrWhiteSpace(newPassword))
            return;

        if (newPassword.Length < 6)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Password must be at least 6 characters", "error");
            return;
        }

        try
        {
            var success = await AdminApiService.ChangeUserPasswordAsync(userId, new ChangePasswordDto
            {
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            });
            await JSRuntime.InvokeVoidAsync("showToast",
                success ? "Password reset successfully" : "Failed to reset password",
                success ? "success" : "error");
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to reset password", "error");
        }
    }

    private Task BulkActivate() => BulkSetActive(true);

    private Task BulkDeactivate() => BulkSetActive(false);

    private async Task BulkSetActive(bool active)
    {
        var ids = selectedUserIds.ToList();
        var successCount = 0;

        foreach (var id in ids)
        {
            var user = allUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                continue;

            try
            {
                var dto = BuildUpdateDto(user);
                dto.IsActive = active;
                var result = await AdminApiService.UpdateUserAsync(id, dto);
                if (result != null)
                    successCount++;
            }
            catch (Exception)
            {
                // Skip failures; summary toast reports the successful count.
            }
        }

        await JSRuntime.InvokeVoidAsync("showToast",
            $"{(active ? "Activated" : "Deactivated")} {successCount} of {ids.Count} users",
            active ? "success" : "warning");
        selectedUserIds.Clear();
        await LoadUsers();
    }

    private bool CanDeleteUser(UserDto user)
    {
        // Don't allow deleting the last admin user
        if (user.Role == UserRole.Admin && adminUsers <= 1)
            return false;
        return true;
    }

    private string GetRoleBadgeClass(UserRole role)
    {
        return role switch
        {
            UserRole.Admin => "bg-danger",
            UserRole.Bartender => "bg-primary",
            UserRole.Waiter => "bg-primary",
            UserRole.Customer => "bg-success",
            _ => "bg-secondary"
        };
    }

    private async Task CreateSampleUsers()
    {
        if (!await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to create 20 sample users? This will add test data to your database."))
        {
            return;
        }

        isCreatingSampleUsers = true;
        StateHasChanged();

        try
        {
            var sampleUsers = GenerateSampleUsers();
            var successCount = 0;
            var errorCount = 0;

            foreach (var user in sampleUsers)
            {
                try
                {
                    var result = await AdminApiService.CreateUserAsync(user);
                    if (result != null)
                    {
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
                catch (Exception)
                {
                    errorCount++;
                }

                // Small delay to prevent overwhelming the API
                await Task.Delay(100);
            }

            if (successCount > 0)
            {
                // Success: reload the users list to show new users
                await LoadUsers();
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to create sample users:", ex.Message);
        }
        finally
        {
            isCreatingSampleUsers = false;
            StateHasChanged();
        }
    }

    private List<CreateUserDto> GenerateSampleUsers()
    {
        var roles = new[] { UserRole.Bartender, UserRole.Waiter };
        var firstNames = new[] { "John", "Jane", "Mike", "Sarah", "David", "Emma", "Chris", "Lisa", "Tom", "Anna", "Mark", "Julia", "Steve", "Maria", "Paul", "Linda", "James", "Susan", "Daniel", "Nicole" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

        var sampleUsers = new List<CreateUserDto>();
        var random = new Random();

        for (int i = 1; i <= 20; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var role = roles[random.Next(roles.Length)];

            var user = new CreateUserDto
            {
                Username = $"user{i:D2}_{firstName.ToLower()}",
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@stadium.com",
                FirstName = firstName,
                LastName = lastName,
                Password = "Test123!",
                ConfirmPassword = "Test123!",
                Role = role
            };

            sampleUsers.Add(user);
        }

        return sampleUsers;
    }
}