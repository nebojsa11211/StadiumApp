using Microsoft.JSInterop;
using StadiumDrinkOrdering.Runner.Models;

namespace StadiumDrinkOrdering.Runner.Services;

/// <summary>
/// Lean, self-contained auth for the Runner PWA. The token lives in localStorage (survives reloads
/// / app relaunch). No refresh-token machinery: the Waiter token is fixed-lifetime (~4h) and the
/// runner re-logs in per shift — refresh can't happen offline anyway. Token expiry is enforced by
/// the API returning 401 (handled in <see cref="RunnerApiService"/>), so we don't gate on it here.
/// </summary>
public class RunnerAuthService
{
    private const string TokenKey = "runner.token";
    private const string RoleKey = "runner.role";
    private const string EmailKey = "runner.email";
    private const string ExpiresKey = "runner.expiresAt";

    private readonly IJSRuntime _js;

    public string? Token { get; private set; }
    public string? Role { get; private set; }
    public string? Email { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    // The Runner is a waiter tool; admins are allowed for supervision/testing.
    public bool IsAllowed =>
        IsAuthenticated &&
        (string.Equals(Role, "Waiter", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase));

    public event Action? OnChange;

    public RunnerAuthService(IJSRuntime js) => _js = js;

    public async Task InitializeAsync()
    {
        try
        {
            Token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
            Role = await _js.InvokeAsync<string?>("localStorage.getItem", RoleKey);
            Email = await _js.InvokeAsync<string?>("localStorage.getItem", EmailKey);
            var exp = await _js.InvokeAsync<string?>("localStorage.getItem", ExpiresKey);
            ExpiresAt = DateTime.TryParse(exp, out var dt) ? dt : null;
        }
        catch
        {
            // localStorage not available yet — treat as logged out.
        }
        OnChange?.Invoke();
    }

    public async Task SetSessionAsync(LoginResponseDto login)
    {
        Token = login.Token;
        Role = login.User?.Role.ToString();
        Email = login.User?.Email;
        ExpiresAt = login.ExpiresAt == default ? null : login.ExpiresAt;

        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, Token);
        await _js.InvokeVoidAsync("localStorage.setItem", RoleKey, Role ?? "");
        await _js.InvokeVoidAsync("localStorage.setItem", EmailKey, Email ?? "");
        await _js.InvokeVoidAsync("localStorage.setItem", ExpiresKey, ExpiresAt?.ToString("O") ?? "");
        OnChange?.Invoke();
    }

    public async Task ClearAsync()
    {
        Token = Role = Email = null;
        ExpiresAt = null;
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", RoleKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", EmailKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", ExpiresKey);
        }
        catch
        {
            // ignore
        }
        OnChange?.Invoke();
    }
}
