using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services
{
    public interface IConsoleLoggingService
    {
        Task WriteLineAsync(string message, string level = "Info", string action = "ConsoleLog");
        Task WriteLineAsync(object value, string level = "Info", string action = "ConsoleLog");
        void WriteLine(string message, string level = "Info", string action = "ConsoleLog");
        void WriteLine(object value, string level = "Info", string action = "ConsoleLog");
        Task<bool> IsConsoleToSystemLoggingEnabledAsync();
    }

    public class ConsoleLoggingService : IConsoleLoggingService
    {
        private readonly ICentralizedLoggingClient _loggingClient;
        private readonly IConsoleLoggingToggleService _toggleService;

        public ConsoleLoggingService(ICentralizedLoggingClient loggingClient, IConsoleLoggingToggleService toggleService)
        {
            _loggingClient = loggingClient;
            _toggleService = toggleService;
        }

        public async Task<bool> IsConsoleToSystemLoggingEnabledAsync()
        {
            return await _toggleService.GetIsEnabledAsync();
        }

        public async Task WriteLineAsync(string message, string level = "Info", string action = "ConsoleLog")
        {
            Console.WriteLine(message);

            if (await IsConsoleToSystemLoggingEnabledAsync())
            {
                try
                {
                    if (level == "Error")
                    {
                        await _loggingClient.LogErrorAsync(
                            new Exception(message),
                            action: action,
                            category: "AdminConsole",
                            source: "Admin"
                        );
                    }
                    else if (level == "Warning")
                    {
                        await _loggingClient.LogWarningAsync(
                            message: message,
                            action: action,
                            category: "AdminConsole",
                            source: "Admin"
                        );
                    }
                    else
                    {
                        await _loggingClient.LogInfoAsync(
                            message: message,
                            action: action,
                            category: "AdminConsole",
                            source: "Admin"
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConsoleLoggingService] Failed to log to system: {ex.Message}");
                }
            }
        }

        public async Task WriteLineAsync(object value, string level = "Info", string action = "ConsoleLog")
        {
            await WriteLineAsync(value?.ToString() ?? "null", level, action);
        }

        public void WriteLine(string message, string level = "Info", string action = "ConsoleLog")
        {
            Console.WriteLine(message);

            // Fire and forget - check toggle state asynchronously
            Task.Run(async () =>
            {
                try
                {
                    if (await IsConsoleToSystemLoggingEnabledAsync())
                    {
                        if (level == "Error")
                        {
                            await _loggingClient.LogErrorAsync(
                                new Exception(message),
                                action: action,
                                category: "AdminConsole",
                                source: "Admin"
                            );
                        }
                        else if (level == "Warning")
                        {
                            await _loggingClient.LogWarningAsync(
                                message: message,
                                action: action,
                                category: "AdminConsole",
                                source: "Admin"
                            );
                        }
                        else
                        {
                            await _loggingClient.LogInfoAsync(
                                message: message,
                                action: action,
                                category: "AdminConsole",
                                source: "Admin"
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConsoleLoggingService] Failed to log to system: {ex.Message}");
                }
            });
        }

        public void WriteLine(object value, string level = "Info", string action = "ConsoleLog")
        {
            WriteLine(value?.ToString() ?? "null", level, action);
        }
    }
}