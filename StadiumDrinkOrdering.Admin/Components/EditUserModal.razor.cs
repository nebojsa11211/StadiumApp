using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class EditUserModalBase : ComponentBase
{
    [Inject] protected IAdminApiService ApiService { get; set; } = default!;

    [Parameter] public UserDto? User { get; set; }
    [Parameter] public EventCallback OnUserUpdated { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    protected UpdateUserDto updateUserDto = new UpdateUserDto();
    protected bool isSubmitting = false;
    protected string errorMessage = "";

    protected override void OnParametersSet()
    {
        if (User != null)
        {
            updateUserDto = new UpdateUserDto
            {
                Username = User.Username,
                Email = User.Email,
                Role = User.Role
            };
        }
    }

    protected async Task HandleValidSubmit()
    {
        if (User == null) return;

        isSubmitting = true;
        errorMessage = "";
        StateHasChanged();

        try
        {
            var result = await ApiService.UpdateUserAsync(User.Id, updateUserDto);
            
            if (result != null)
            {
                await OnUserUpdated.InvokeAsync();
            }
            else
            {
                errorMessage = "Failed to update user. Email or username may already be taken by another user.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error updating user: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
            StateHasChanged();
        }
    }
}