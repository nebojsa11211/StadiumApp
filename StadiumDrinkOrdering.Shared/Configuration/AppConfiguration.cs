using Microsoft.Extensions.Configuration;

namespace StadiumDrinkOrdering.Shared.Configuration;

/// <summary>
/// Centralized application configuration helpers shared by all apps (API, Admin, Customer, Staff).
/// Provides a single source of truth for loading the gitignored .env file and for resolving the
/// API base URL, so the "which backend am I talking to" decision is identical everywhere.
/// </summary>
public static class AppConfiguration
{
    /// <summary>
    /// Loads KEY=VALUE pairs from the nearest .env file (searching up from the working directory)
    /// into environment variables. Variables that are already set are preserved (NoClobber), so
    /// launch profiles / Docker / the OS always win over .env. Best-effort: any failure is ignored,
    /// so a missing or malformed .env never breaks startup.
    ///
    /// Call this BEFORE <c>WebApplication.CreateBuilder(args)</c> so the values are picked up by the
    /// environment-variable configuration provider.
    /// </summary>
    public static void LoadDotEnvFile()
    {
        try
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            string? envPath = null;
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, ".env");
                if (File.Exists(candidate)) { envPath = candidate; break; }
                dir = dir.Parent;
            }

            if (envPath == null)
                return;

            foreach (var rawLine in File.ReadAllLines(envPath))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                    continue;

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();

                // Strip a single pair of surrounding quotes, if present.
                if (value.Length >= 2 &&
                    ((value[0] == '"' && value[^1] == '"') || (value[0] == '\'' && value[^1] == '\'')))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                // NoClobber: never override a variable already set in the process environment.
                if (Environment.GetEnvironmentVariable(key) == null)
                    Environment.SetEnvironmentVariable(key, value);
            }

            Console.WriteLine($"Loaded environment variables from {envPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: could not load .env file: {ex.Message}");
        }
    }

    /// <summary>
    /// Resolves the API base URL (without a trailing slash) that a frontend app should use.
    ///
    /// Precedence:
    ///   1. In a Docker container: an explicit <c>ApiSettings__BaseUrl</c> environment variable if
    ///      set, otherwise the in-network API service URL (<c>https://api:8443</c>). Container apps
    ///      must reach the API over the Docker network, never via a localhost appsettings value.
    ///   2. Locally: <c>ApiSettings:BaseUrl</c> from configuration (which includes values loaded from
    ///      .env and environment variables, then appsettings), otherwise <c>https://localhost:7010</c>.
    ///
    /// This makes configuration the single source of truth locally while keeping container behaviour
    /// unchanged. To point a frontend at a different API locally, set <c>ApiSettings__BaseUrl</c> in
    /// .env or change <c>ApiSettings:BaseUrl</c> in appsettings.
    /// </summary>
    public static string ResolveApiBaseUrl(IConfiguration configuration)
    {
        var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

        if (inContainer)
        {
            var containerOverride = Environment.GetEnvironmentVariable("ApiSettings__BaseUrl");
            var containerUrl = string.IsNullOrWhiteSpace(containerOverride)
                ? "https://api:8443"
                : containerOverride;
            return Normalize(containerUrl);
        }

        var configured = configuration["ApiSettings:BaseUrl"];
        var localUrl = string.IsNullOrWhiteSpace(configured)
            ? "https://localhost:7010"
            : configured;
        return Normalize(localUrl);
    }

    private static string Normalize(string url) => url.TrimEnd('/');
}
