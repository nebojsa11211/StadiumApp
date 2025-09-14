using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StadiumDrinkOrdering.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestLoggingController : ControllerBase
    {
        private readonly ILogger<TestLoggingController> _logger;

        public TestLoggingController(ILogger<TestLoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test-all-levels")]
        public IActionResult TestAllLogLevels()
        {
            _logger.LogTrace("This is a TRACE level log - most detailed");
            _logger.LogDebug("This is a DEBUG level log - debugging information");
            _logger.LogInformation("This is an INFORMATION level log - general flow");
            _logger.LogWarning("This is a WARNING level log - something unexpected");
            _logger.LogError("This is an ERROR level log - something failed");
            _logger.LogCritical("This is a CRITICAL level log - system failure");

            return Ok(new
            {
                message = "All log levels have been tested. Check console and database for logs.",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("test-with-exception")]
        public IActionResult TestWithException()
        {
            try
            {
                _logger.LogInformation("Starting exception test");
                throw new InvalidOperationException("This is a test exception for logging");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during testing");
                return StatusCode(500, new
                {
                    message = "Exception was logged to console and database",
                    error = ex.Message
                });
            }
        }

        [HttpGet("test-business-event")]
        public IActionResult TestBusinessEvent()
        {
            var eventId = new EventId(1001, "OrderCreated");
            _logger.Log(LogLevel.Information, eventId, "New order created with ID: {OrderId}", 12345);

            eventId = new EventId(2001, "PaymentProcessed");
            _logger.Log(LogLevel.Information, eventId, "Payment processed for amount: {Amount}", 99.99);

            return Ok(new
            {
                message = "Business events logged with custom EventIds",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("test-performance")]
        public async Task<IActionResult> TestPerformanceLogging()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            _logger.LogInformation("Starting performance test");

            // Simulate some work
            await Task.Delay(100);

            stopwatch.Stop();
            _logger.LogInformation("Performance test completed in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            // Test batch logging
            for (int i = 0; i < 20; i++)
            {
                _logger.LogInformation("Batch log entry {Index} of 20", i + 1);
            }

            return Ok(new
            {
                message = "Performance logging test completed",
                elapsedMs = stopwatch.ElapsedMilliseconds,
                batchLogsCreated = 20
            });
        }
    }
}