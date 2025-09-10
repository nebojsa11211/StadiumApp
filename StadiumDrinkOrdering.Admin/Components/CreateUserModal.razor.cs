using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class CreateUserModalBase : ComponentBase
{
    [Inject] protected IAdminApiService ApiService { get; set; } = default!;

    [Parameter] public EventCallback OnUserCreated { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    protected CreateUserDto createUserDto = new CreateUserDto();
    protected bool isSubmitting = false;
    protected string errorMessage = "";

    protected async Task HandleValidSubmit()
    {
        isSubmitting = true;
        errorMessage = "";
        StateHasChanged();

        try
        {
            var result = await ApiService.CreateUserAsync(createUserDto);
            
            if (result != null)
            {
                await OnUserCreated.InvokeAsync();
            }
            else
            {
                errorMessage = "Failed to create user. User with this email or username may already exist.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error creating user: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
            StateHasChanged();
        }
    }
}