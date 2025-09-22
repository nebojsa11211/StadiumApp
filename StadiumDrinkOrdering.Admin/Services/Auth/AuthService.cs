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
                    var result = DeserializeResponse<EnhancedLoginResponseDto>(responseJson);
                    Token = result?.Token;
                    return Token;
                }
            }
            catch (HttpRequestException ex)
            {
                await LogErrorAsync(ex, "Login", $"Network error during login for {loginDto.Email}", "Security");
                throw;
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                await LogErrorAsync(ex, "Login", $"Failed to login user {loginDto.Email}", "Security");
                throw;
            }
            return null;
        }

        public async Task<EnhancedLoginResponseDto?> LoginFullAsync(LoginDto loginDto)
        {
            try
            {
                var content = CreateJsonContent(loginDto);
                var response = await HttpClient.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = DeserializeResponse<EnhancedLoginResponseDto>(responseJson);
                    Token = result?.Token;
                    return result;
                }
                else if ((int)response.StatusCode == 423) // Account locked
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var errorData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(errorJson);

                    var errorMessage = "Account is temporarily locked";
                    if (errorData.TryGetProperty("remainingMinutes", out var minutes))
                    {
                        errorMessage = $"Account is temporarily locked. Try again in {minutes} minutes.";
                    }

                    throw new InvalidOperationException(errorMessage);
                }
                else if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Try to parse the error message from the API response
                    string errorMessage = "Login failed";
                    try
                    {
                        var errorData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(errorContent);
                        if (errorData.TryGetProperty("error", out var errorProp))
                        {
                            errorMessage = errorProp.GetString() ?? "Invalid credentials";
                        }
                        else if (errorData.TryGetProperty("message", out var messageProp))
                        {
                            errorMessage = messageProp.GetString() ?? "Invalid credentials";
                        }
                    }
                    catch
                    {
                        // If we can't parse the JSON, fall back to the reason phrase
                        errorMessage = response.ReasonPhrase ?? "Login failed";
                    }

                    throw new InvalidOperationException(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                await LogErrorAsync(ex, "Login", $"Network error during login for {loginDto.Email}", "Security");
                throw;
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                await LogErrorAsync(ex, "Login", $"Failed to login user {loginDto.Email}", "Security");
                throw;
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