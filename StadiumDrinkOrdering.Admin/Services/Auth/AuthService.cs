using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Auth
{
    public class AuthService : BaseApiService, IAuthService
    {
        public string? Token { get; set; }

        public AuthService(HttpClient httpClient, ICentralizedLoggingClient loggingClient)
            : base(httpClient, loggingClient)
        {
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var content = CreateJsonContent(loginDto);
                var response = await HttpClient.PostAsync("auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = DeserializeResponse<LoginResponseDto>(responseJson);
                    Token = result?.Token;
                    return Token;
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "Login", $"Failed to login user {loginDto.Email}", "Security");
            }
            return null;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await HttpClient.PostAsync("auth/logout", null);
                if (response.IsSuccessStatusCode)
                {
                    Token = null;
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "Logout", "Failed to logout", "Security");
            }
            return false;
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("auth/current");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<UserDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetCurrentUser", "Failed to get current user", "Security");
            }
            return null;
        }
    }
}