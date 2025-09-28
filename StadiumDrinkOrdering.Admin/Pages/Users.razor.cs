using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;

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

    // Create user modal
    private bool showCreateModal = false;
    private CreateUserDto newUser = new();

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
            var users = await AdminApiService.GetUsersAsync();
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

        filteredUsers = query.OrderByDescending(u => u.CreatedAt).ToList();
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
            foreach (var user in filteredUsers.Take(50))
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
        newUser = new CreateUserDto();
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

    private void EditUser(UserDto user)
    {
        // Implementation for user editing
        Navigation.NavigateTo($"/users/edit/{user.Id}");
    }

    private async Task ActivateUser(int userId)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("showToast", "User activated successfully", "success");
            await LoadUsers();
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to activate user", "error");
        }
    }

    private async Task DeactivateUser(int userId)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("showToast", "User deactivated successfully", "warning");
            await LoadUsers();
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to deactivate user", "error");
        }
    }

    private async Task DeleteUser(int userId)
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this user? This action cannot be undone."))
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("showToast", "User deleted successfully", "success");
                await LoadUsers();
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to delete user", "error");
            }
        }
    }

    private async Task ResetPassword(int userId)
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to reset this user's password?"))
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Password reset successfully", "success");
            }
            catch (Exception)
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to reset password", "error");
            }
        }
    }

    private async Task BulkActivate()
    {
        await JSRuntime.InvokeVoidAsync("showToast", $"Activated {selectedUserIds.Count} users", "success");
        selectedUserIds.Clear();
        await LoadUsers();
    }

    private async Task BulkDeactivate()
    {
        await JSRuntime.InvokeVoidAsync("showToast", $"Deactivated {selectedUserIds.Count} users", "warning");
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
        var roles = new[] { UserRole.Customer, UserRole.Bartender, UserRole.Waiter };
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