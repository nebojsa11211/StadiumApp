namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Seam between the wallet and a money-movement provider. Two settlement models are supported:
/// <list type="bullet">
/// <item><b>Synchronous</b> (mock/dev): <see cref="AuthorizeDepositAsync"/> captures immediately and the
/// wallet is credited inline.</item>
/// <item><b>Asynchronous</b> (real gateway, e.g. Stripe): <see cref="CreateDepositIntentAsync"/> returns a
/// client secret for the browser to confirm the card; settlement arrives later via a signed webhook that
/// <see cref="HandleWebhookAsync"/> parses, and the wallet is credited then.</item>
/// </list>
/// Swapping providers is a registration change in Program.cs — nothing in the ledger/balance logic moves.
/// </summary>
public interface IWalletPaymentGateway
{
    /// <summary>True when deposits settle asynchronously via webhook; false for synchronous capture.</summary>
    bool SettlesAsynchronously { get; }

    /// <summary>Publishable (public) provider key the browser needs to confirm a card, or null for a
    /// gateway that requires no browser-side card entry (e.g. the synchronous mock). Safe to expose.</summary>
    string? PublishableKey { get; }

    /// <summary>Synchronous authorize + capture. Only called when <see cref="SettlesAsynchronously"/> is false.</summary>
    Task<WalletGatewayResult> AuthorizeDepositAsync(decimal amount, string currency, string method, string reference);

    /// <summary>Create a provider payment intent for an async deposit. Only called when
    /// <see cref="SettlesAsynchronously"/> is true. Returns the client secret the browser uses to confirm.</summary>
    Task<WalletDepositIntent> CreateDepositIntentAsync(decimal amount, string currency, WalletDepositContext context);

    /// <summary>Verify + parse a provider webhook into a settlement, or null if it isn't a relevant wallet
    /// deposit event. Throws if the signature is invalid (caller maps that to HTTP 400).</summary>
    Task<WalletDepositSettlement?> HandleWebhookAsync(string payload, string signatureHeader);
}

/// <param name="Success">Whether funds were captured (synchronous path).</param>
/// <param name="GatewayTransactionId">Provider-side id — also the idempotency key for confirmation.</param>
/// <param name="FailureReason">Populated when <paramref name="Success"/> is false.</param>
/// <param name="RawResponse">Provider payload, stored on the Payment for audit.</param>
public record WalletGatewayResult(bool Success, string GatewayTransactionId, string? FailureReason, string RawResponse);

/// <summary>Context carried into an async deposit so the webhook can correlate the settlement back to a fan.</summary>
public record WalletDepositContext(int UserId, string IdempotencyKey, string Method);

/// <summary>A created payment intent: the browser confirms the card with <see cref="ClientSecret"/> +
/// <see cref="PublishableKey"/>.</summary>
public record WalletDepositIntent(string ProviderIntentId, string ClientSecret, string PublishableKey);

/// <summary>A settled (or failed) async deposit parsed from a provider webhook.</summary>
public record WalletDepositSettlement(
    bool Succeeded, int UserId, decimal Amount, string Currency, string ProviderIntentId, string Method, string RawResponse);

/// <summary>
/// Development stand-in that captures synchronously and always succeeds — mirrors the system's existing
/// simulated-payment approach. Deterministic id derived from the reference so retries are stable.
/// </summary>
public class MockWalletPaymentGateway : IWalletPaymentGateway
{
    public bool SettlesAsynchronously => false;

    public string? PublishableKey => null; // synchronous capture — no browser-side card entry.

    public Task<WalletGatewayResult> AuthorizeDepositAsync(decimal amount, string currency, string method, string reference)
    {
        var gatewayTxnId = $"mock_{reference}";
        var raw = $"{{\"provider\":\"mock\",\"amount\":{amount},\"currency\":\"{currency}\",\"method\":\"{method}\",\"status\":\"captured\"}}";
        return Task.FromResult(new WalletGatewayResult(true, gatewayTxnId, null, raw));
    }

    // Not used for the synchronous mock — present only to satisfy the seam.
    public Task<WalletDepositIntent> CreateDepositIntentAsync(decimal amount, string currency, WalletDepositContext context)
        => throw new NotSupportedException("MockWalletPaymentGateway settles synchronously.");

    public Task<WalletDepositSettlement?> HandleWebhookAsync(string payload, string signatureHeader)
        => Task.FromResult<WalletDepositSettlement?>(null);
}
