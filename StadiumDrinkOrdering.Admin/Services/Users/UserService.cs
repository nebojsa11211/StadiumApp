using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Users
{
    public class UserService : BaseApiService, IUserService
    {
        public UserService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, IErrorNotificationService errorNotificationService, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, errorNotificationService, tokenStorage)
        {
        }

        // Fetch a large page so the admin grids show the full set rather than the
        // default 20-row API page.
        private const int AdminPageSize = 1000;

        public Task<IEnumerable<UserDto>?> GetUsersAsync()
            => GetUsersAsync(new UserFilterDto());

        public async Task<IEnumerable<UserDto>?> GetUsersAsync(UserFilterDto filter)
        {
            try
            {
                // Set authorization header before making the request
                SetAuthorizationHeader();

                if (filter.PageSize <= 0 || filter.PageSize == 20)
                {
                    filter.PageSize = AdminPageSize;
                }

                // Use POST /users/search with the supplied filter
                var content = CreateJsonContent(filter);
                var response = await HttpClient.PostAsync("users/search", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = DeserializeResponse<UserListDto>(json);
                    return result?.Users ?? new List<UserDto>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await ErrorNotificationService.ShowAuthenticationErrorAsync();
                    throw new UnauthorizedAccessException("Authentication required. Please log in again.");
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetUsers", "Failed to retrieve users list");
            }
            return new List<UserDto>();
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync($"users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<UserDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetUser", $"Failed to retrieve user {id}");
            }
            return null;
        }

        public async Task<StaffMemberStatsDto?> GetUserStatsAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync($"users/{id}/stats");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<StaffMemberStatsDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetUserStats", $"Failed to retrieve stats for user {id}");
            }
            return null;
        }

        public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                SetAuthorizationHeader();
                var content = CreateJsonContent(createUserDto);
                var response = await HttpClient.PostAsync("users", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<UserDto>(responseJson);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "CreateUser", $"Failed to create user {createUserDto.Email}");
            }
            return null;
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                SetAuthorizationHeader();
                var content = CreateJsonContent(updateUserDto);
                var response = await HttpClient.PutAsync($"users/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<UserDto>(responseJson);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "UpdateUser", $"Failed to update user {id}");
            }
            return null;
        }

        public async Task<bool> ChangeUserPasswordAsync(int id, ChangePasswordDto changePasswordDto)
        {
            try
            {
                SetAuthorizationHeader();
                var content = CreateJsonContent(changePasswordDto);
                var response = await HttpClient.PutAsync($"users/{id}/password", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ChangeUserPassword", $"Failed to change password for user {id}");
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.DeleteAsync($"users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "DeleteUser", $"Failed to delete user {id}");
            }
            return false;
        }
    }
}