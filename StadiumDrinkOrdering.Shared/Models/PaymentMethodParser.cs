namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Canonical translation from the legacy free-form payment-method strings to the
/// <see cref="PaymentMethod"/> / <see cref="PaymentDirection"/> pair.
///
/// Before these columns were real enums the method was stored as free text, and several writers
/// disagreed on spelling ("CreditCard" vs "Credit Card"), stored a <i>provider</i> rather than a rail
/// ("Stripe"), or encoded the direction into the method ("CardRefund", "CashPayout"). Anything that
/// still hands us a string — the wallet gateway, an inbound deposit request, a restored backup — is
/// normalised here so the mapping lives in exactly one place and matches the data migration.
/// </summary>
public static class PaymentMethodParser
{
    /// <summary>
    /// Resolves a legacy/external method string. Returns false for values we don't recognise, so
    /// callers can reject rather than silently mis-record money — the old behaviour quietly booked
    /// unknown values as CreditCard, which is how refunds ended up displayed as card purchases.
    /// </summary>
    public static bool TryParse(string? raw, out PaymentMethod method, out PaymentDirection direction)
    {
        method = PaymentMethod.CreditCard;
        direction = PaymentDirection.In;

        if (string.IsNullOrWhiteSpace(raw))
            return false;

        // Fold away spacing/underscores/casing so "Credit Card", "credit_card" and "CreditCard" agree.
        var key = new string(raw.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();

        switch (key)
        {
            case "creditcard":
            case "card":
            case "stripe":      // a provider, not a rail — Stripe charges are card payments
                method = PaymentMethod.CreditCard;
                return true;

            case "debitcard":
                method = PaymentMethod.DebitCard;
                return true;

            case "digitalwallet":
            case "wallet":
                method = PaymentMethod.DigitalWallet;
                return true;

            case "banktransfer":
                method = PaymentMethod.BankTransfer;
                return true;

            case "cash":
                method = PaymentMethod.Cash;
                return true;

            case "ticketwallet":
                method = PaymentMethod.TicketWallet;
                return true;

            // Legacy payout rails: the direction used to be baked into the method name.
            case "cardrefund":
                method = PaymentMethod.CreditCard;
                direction = PaymentDirection.Out;
                return true;

            case "cashpayout":
                method = PaymentMethod.Cash;
                direction = PaymentDirection.Out;
                return true;

            default:
                return false;
        }
    }
}
