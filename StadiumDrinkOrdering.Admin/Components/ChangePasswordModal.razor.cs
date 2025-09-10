using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class ChangePasswordModalBase : ComponentBase
{
    [Inject] protected IAdminApiService ApiService { get; set; } = default!;

    [Parameter] public UserDto? User { get; set; }
    [Parameter] public EventCallback OnPasswordChanged { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    protected ChangePasswordDto changePasswordDto = new ChangePasswordDto();
    protected bool isSubmitting = false;
    protected string errorMessage = "";

    protected override void OnParametersSet()
    {
        // Reset form when user changes
        changePasswordDto = new ChangePasswordDto();
        errorMessage = "";
    }

    protected async Task HandleValidSubmit()
    {
        if (User == null) return;

        isSubmitting = true;
        errorMessage = "";
        StateHasChanged();

        try
        {
            var result = await ApiService.ChangeUserPasswordAsync(User.Id, changePasswordDto);
            
            if (result)
            {
                await OnPasswordChanged.InvokeAsync();
            }
            else
            {
                errorMessage = "Failed to change password. Please try again.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error changing password: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
            StateHasChanged();
        }
    }
}