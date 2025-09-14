using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Auth
{
    public interface IAuthService
    {
        string? Token { get; set; }
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync();
        Task<UserDto?> GetCurrentUserAsync();
    }
}