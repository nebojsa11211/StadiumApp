using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>?> GetUsersAsync();
        Task<IEnumerable<UserDto>?> GetUsersAsync(UserFilterDto filter);
        Task<UserDto?> GetUserAsync(int id);
        Task<StaffMemberStatsDto?> GetUserStatsAsync(int id);
        Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> ChangeUserPasswordAsync(int id, ChangePasswordDto changePasswordDto);
        Task<bool> DeleteUserAsync(int id);
    }
}