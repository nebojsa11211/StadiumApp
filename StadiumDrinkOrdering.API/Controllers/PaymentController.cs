using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StadiumDrinkOrdering.API.Services;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost("create-intent")]
    [Authorize]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
    {
        try
        {
            var result = await _paymentService.CreatePaymentIntentAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment intent for order {OrderId}", request.OrderId);
            return StatusCode(500, new { message = "An error occurred while creating payment intent" });
        }
    }

    [HttpPost("confirm/{paymentIntentId}")]
    [Authorize]
    public async Task<IActionResult> ConfirmPayment(string paymentIntentId)
    {
        try
        {
            var result = await _paymentService.ConfirmPaymentAsync(paymentIntentId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment {PaymentIntentId}", paymentIntentId);
            return StatusCode(500, new { message = "An error occurred while confirming payment" });
        }
    }

    [HttpGet("{paymentId}")]
    [Authorize]
    public async Task<IActionResult> GetPayment(int paymentId)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found" });
            }

            return Ok(new
            {
                paymentId = payment.Id,
                orderId = payment.OrderId,
                amount = payment.Amount,
                currency = payment.Currency,
                status = payment.Status,
                paymentMethod = payment.PaymentMethod,
                transactionId = payment.TransactionId,
                createdAt = payment.CreatedAt,
                processedAt = payment.ProcessedAt,
                refundAmount = payment.RefundAmount,
                refundDate = payment.RefundDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment {PaymentId}", paymentId);
            return StatusCode(500, new { message = "An error occurred while retrieving payment" });
        }
    }

    [HttpPost("refund")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> ProcessRefund([FromBody] ProcessRefundRequest request)
    {
        try
        {
            var result = await _paymentService.ProcessRefundAsync(request.PaymentId, request.Amount, request.Reason);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for payment {PaymentId}", request.PaymentId);
            return StatusCode(500, new { message = "An error occurred while processing refund" });
        }
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook()
    {
        try
        {
            var payload = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest("Missing Stripe signature");
            }

            var success = await _paymentService.ProcessWebhookAsync(payload, signature);
            
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Stripe webhook");
            return BadRequest();
        }
    }
}

// Request DTOs
public class ProcessRefundRequest
{
    public int PaymentId { get; set; }
    public decimal? Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}