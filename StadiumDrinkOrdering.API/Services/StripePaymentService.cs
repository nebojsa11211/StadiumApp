using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Options;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

public class StripeSettings
{
    public string PublishableKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string Currency { get; set; } = "eur";
}

public interface IPaymentService
{
    Task<PaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request);
    Task<PaymentConfirmationResponse> ConfirmPaymentAsync(string paymentIntentId);
    Task<RefundResponse> ProcessRefundAsync(int paymentId, decimal? amount, string reason);
    Task<Payment?> GetPaymentByIdAsync(int paymentId);
    Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
    Task<bool> ProcessWebhookAsync(string payload, string signature);
}

public class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StripePaymentService> _logger;

    public StripePaymentService(IOptions<StripeSettings> stripeSettings, ApplicationDbContext context, ILogger<StripePaymentService> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _context = context;
        _logger = logger;
        
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<PaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request)
    {
        try
        {
            // Get the order to verify amount and details
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
                throw new ArgumentException("Order not found");

            var paymentIntentService = new PaymentIntentService();
            
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100), // Convert to cents
                Currency = _stripeSettings.Currency,
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", request.OrderId.ToString() },
                    { "customer_id", order.CustomerId.ToString() },
                    { "event_id", order.EventId?.ToString() ?? "0" },
                    { "seat_id", order.SeatId?.ToString() ?? "0" }
                }
            };

            var paymentIntent = await paymentIntentService.CreateAsync(options);

            // Create Payment record in database
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = order.TotalAmount,
                Currency = _stripeSettings.Currency.ToUpper(),
                PaymentMethod = "Stripe",
                Status = "Pending",
                TransactionId = paymentIntent.Id,
                PaymentDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentIntentResponse
            {
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                PublishableKey = _stripeSettings.PublishableKey,
                Amount = order.TotalAmount,
                Currency = _stripeSettings.Currency.ToUpper(),
                Status = paymentIntent.Status
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment intent for order {OrderId}", request.OrderId);
            throw;
        }
    }

    public async Task<PaymentConfirmationResponse> ConfirmPaymentAsync(string paymentIntentId)
    {
        try
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.TransactionId == paymentIntentId);

            if (payment == null)
                throw new ArgumentException("Payment not found");

            switch (paymentIntent.Status)
            {
                case "succeeded":
                    payment.Status = "Completed";
                    payment.ProcessedAt = DateTime.UtcNow;
                    payment.PaymentGatewayResponse = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = paymentIntent.Status,
                        amount = paymentIntent.Amount,
                        currency = paymentIntent.Currency,
                        payment_method = paymentIntent.PaymentMethodId,
                        confirmed_at = DateTime.UtcNow
                    });

                    // Update order status
                    if (payment.Order != null)
                    {
                        // Don't automatically move to Accepted - let staff accept orders
                        // payment.Order.Status = OrderStatus.Accepted;
                        // payment.Order.AcceptedAt = DateTime.UtcNow;
                    }
                    break;

                case "requires_payment_method":
                case "requires_confirmation":
                    payment.Status = "Pending";
                    break;

                case "canceled":
                    payment.Status = "Cancelled";
                    payment.FailedAt = DateTime.UtcNow;
                    payment.FailureReason = "Payment cancelled";
                    break;

                default:
                    payment.Status = "Failed";
                    payment.FailedAt = DateTime.UtcNow;
                    payment.FailureReason = $"Unexpected status: {paymentIntent.Status}";
                    break;
            }

            await _context.SaveChangesAsync();

            return new PaymentConfirmationResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status,
                Amount = payment.Amount,
                TransactionId = paymentIntentId,
                ProcessedAt = payment.ProcessedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment {PaymentIntentId}", paymentIntentId);
            throw;
        }
    }

    public async Task<RefundResponse> ProcessRefundAsync(int paymentId, decimal? amount, string reason)
    {
        try
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId && p.Status == "Completed");

            if (payment == null)
                throw new ArgumentException("Payment not found or not eligible for refund");

            var refundService = new RefundService();
            var refundAmount = amount ?? payment.Amount;

            var options = new RefundCreateOptions
            {
                PaymentIntent = payment.TransactionId,
                Amount = (long)(refundAmount * 100), // Convert to cents
                Reason = RefundReasons.RequestedByCustomer,
                Metadata = new Dictionary<string, string>
                {
                    { "payment_id", paymentId.ToString() },
                    { "reason", reason }
                }
            };

            var refund = await refundService.CreateAsync(options);

            // Update payment record
            payment.Status = refundAmount >= payment.Amount ? "Refunded" : "Completed";
            payment.RefundAmount = (payment.RefundAmount ?? 0) + refundAmount;
            payment.RefundDate = DateTime.UtcNow;
            payment.RefundReason = reason;

            await _context.SaveChangesAsync();

            return new RefundResponse
            {
                RefundId = refund.Id,
                Amount = refundAmount,
                Status = refund.Status,
                ProcessedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for payment {PaymentId}", paymentId);
            throw;
        }
    }

    public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == paymentId);
    }

    public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
    {
        return await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public async Task<bool> ProcessWebhookAsync(string payload, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ParseEvent(payload);
            var signatureEvent = EventUtility.ConstructEvent(payload, signature, _stripeSettings.WebhookSecret);

            _logger.LogInformation("Stripe webhook received: {EventType}", stripeEvent.Type);

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    await HandlePaymentIntentSucceeded(stripeEvent);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    await HandlePaymentIntentFailed(stripeEvent);
                    break;
                default:
                    _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                    break;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Stripe webhook");
            return false;
        }
    }

    private async Task HandlePaymentIntentSucceeded(Stripe.Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent != null)
        {
            await ConfirmPaymentAsync(paymentIntent.Id);
        }
    }

    private async Task HandlePaymentIntentFailed(Stripe.Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent != null)
        {
            var payment = await GetPaymentByTransactionIdAsync(paymentIntent.Id);
            if (payment != null)
            {
                payment.Status = "Failed";
                payment.FailedAt = DateTime.UtcNow;
                payment.FailureReason = paymentIntent.LastPaymentError?.Message ?? "Payment failed";
                await _context.SaveChangesAsync();
            }
        }
    }
}

// DTOs for Payment Service
public class CreatePaymentIntentRequest
{
    public int OrderId { get; set; }
}

public class PaymentIntentResponse
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class PaymentConfirmationResponse
{
    public int PaymentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
}

public class RefundResponse
{
    public string RefundId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}