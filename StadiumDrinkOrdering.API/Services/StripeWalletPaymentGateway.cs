using Microsoft.Extensions.Options;
using Stripe;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Real (Stripe) wallet deposit gateway. Deposits settle asynchronously: a PaymentIntent is created for
/// the browser to confirm the card, and Stripe later posts a signed <c>payment_intent.succeeded</c>
/// webhook which credits the wallet. Card data never touches this server. Selected via
/// <c>WalletGateway:Provider = Stripe</c>; reuses the existing <see cref="StripeSettings"/>.
/// </summary>
public class StripeWalletPaymentGateway : IWalletPaymentGateway
{
    // Metadata keys carried on the PaymentIntent so the webhook can identify a wallet deposit and its fan.
    private const string MetaDepositFlag = "wallet_deposit";
    private const string MetaUserId = "wallet_user_id";
    private const string MetaKey = "wallet_idempotency_key";

    private readonly StripeSettings _settings;
    private readonly ILogger<StripeWalletPaymentGateway> _logger;

    public StripeWalletPaymentGateway(IOptions<StripeSettings> settings, ILogger<StripeWalletPaymentGateway> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public bool SettlesAsynchronously => true;

    public string? PublishableKey => _settings.PublishableKey;

    public Task<WalletGatewayResult> AuthorizeDepositAsync(decimal amount, string currency, string method, string reference)
        => throw new NotSupportedException("Stripe settles asynchronously via CreateDepositIntentAsync + webhook.");

    public async Task<WalletDepositIntent> CreateDepositIntentAsync(decimal amount, string currency, WalletDepositContext context)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)Math.Round(amount * 100m),          // minor units (cents)
            Currency = currency.ToLowerInvariant(),
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string>
            {
                { MetaDepositFlag, "1" },
                { MetaUserId, context.UserId.ToString() },
                { MetaKey, context.IdempotencyKey }
            }
        };

        // Stripe idempotency key: a client retry with the same key reuses the same PaymentIntent
        // instead of creating (and charging) a second one.
        var requestOptions = new RequestOptions { IdempotencyKey = $"wallet-deposit-{context.IdempotencyKey}" };

        var intent = await new PaymentIntentService().CreateAsync(options, requestOptions);
        return new WalletDepositIntent(intent.Id, intent.ClientSecret, _settings.PublishableKey);
    }

    public Task<WalletDepositSettlement?> HandleWebhookAsync(string payload, string signatureHeader)
    {
        // Throws StripeException on an invalid signature — the controller maps that to HTTP 400.
        var stripeEvent = EventUtility.ConstructEvent(payload, signatureHeader, _settings.WebhookSecret);

        if (stripeEvent.Type != Events.PaymentIntentSucceeded)
        {
            _logger.LogInformation("Ignoring Stripe wallet webhook event {Type}", stripeEvent.Type);
            return Task.FromResult<WalletDepositSettlement?>(null);
        }

        if (stripeEvent.Data.Object is not PaymentIntent pi ||
            pi.Metadata is null ||
            !pi.Metadata.TryGetValue(MetaDepositFlag, out var flag) || flag != "1")
        {
            // A succeeded intent that isn't one of our wallet deposits (e.g. an order payment) — not ours.
            return Task.FromResult<WalletDepositSettlement?>(null);
        }

        if (!pi.Metadata.TryGetValue(MetaUserId, out var userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            _logger.LogWarning("Wallet deposit webhook {IntentId} missing/invalid user metadata", pi.Id);
            return Task.FromResult<WalletDepositSettlement?>(null);
        }

        var amount = pi.AmountReceived / 100m; // what was actually captured
        var settlement = new WalletDepositSettlement(
            Succeeded: true,
            UserId: userId,
            Amount: amount,
            Currency: pi.Currency.ToUpperInvariant(),
            ProviderIntentId: pi.Id,
            Method: "Stripe",
            RawResponse: $"{{\"provider\":\"stripe\",\"intent\":\"{pi.Id}\",\"amount_received\":{pi.AmountReceived},\"currency\":\"{pi.Currency}\",\"status\":\"{pi.Status}\"}}");

        return Task.FromResult<WalletDepositSettlement?>(settlement);
    }
}
