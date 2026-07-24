namespace StadiumDrinkOrdering.UI;

/// <summary>
/// Lightweight, anonymous diagnostic info about the running API. Surfaced by the frontend apps
/// (Admin/Bar/Simulator/Runner) as a small "which database am I talking to" badge so it's obvious
/// whether the API is pointed at the local Postgres container or the Supabase project.
///
/// Lives in the WASM-safe UI library (not Shared) so the Blazor WebAssembly Runner can use it too.
/// The API returns the same shape as an anonymous object; matching happens by JSON property name.
/// </summary>
public class SystemInfoDto
{
    /// <summary>Friendly database label: "Local", "Supabase", or the raw host for anything else.</summary>
    public string Database { get; set; } = "Unknown";

    /// <summary>ASP.NET Core environment name (Development/Production/...).</summary>
    public string Environment { get; set; } = "Unknown";
}
