using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services
{
    public interface IConsoleLoggingToggleService
    {
        Task<bool> GetIsEnabledAsync();
        Task SetIsEnabledAsync(bool enabled);
        bool GetIsEnabledFromConfig();
    }

    public class ConsoleLoggingToggleService : IConsoleLoggingToggleService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IConfiguration _configuration;
        private readonly ICentralizedLoggingClient _loggingClient;

        public ConsoleLoggingToggleService(IJSRuntime jsRuntime, IConfiguration configuration, ICentralizedLoggingClient loggingClient)
        {
            _jsRuntime = jsRuntime;
            _configuration = configuration;
            _loggingClient = loggingClient;
        }

        public async Task<bool> GetIsEnabledAsync()
        {
            try
            {
                // Primary: Check localStorage (client-side persistence)
                var storedValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "consoleToSystemLogging");
                if (!string.IsNullOrEmpty(storedValue))
                {
                    return storedValue.Equals("true", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                await _loggingClient.LogWarningAsync(
                    message: $"Failed to read localStorage for consoleToSystemLogging: {ex.Message}",
                    action: "GetConsoleLoggingState",
                    category: "AdminSystem",
                    source: "Admin"
                );
            }

            // Fallback: Use configuration file default
            return GetIsEnabledFromConfig();
        }

        public async Task SetIsEnabledAsync(bool enabled)
        {
            try
            {
                // Persist to localStorage (client-side)
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "consoleToSystemLogging", enabled.ToString().ToLower());
                
                // Update JavaScript console interceptor
                await _jsRuntime.InvokeVoidAsync("toggleConsoleToSystemLogging", enabled);

                await _loggingClient.LogInfoAsync(
                    message: $"Console-to-System logging toggled to: {enabled}",
                    action: "SetConsoleLoggingState",
                    category: "AdminSystem",
                    source: "Admin"
                );
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(
                    exception: ex,
                    action: "SetConsoleLoggingState",
                    category: "AdminSystem",
                    source: "Admin"
                );
                throw;
            }
        }

        public bool GetIsEnabledFromConfig()
        {
            return _configuration.GetValue<bool>("ConsoleToSystemLogging:Enabled", false);
        }
    }
}