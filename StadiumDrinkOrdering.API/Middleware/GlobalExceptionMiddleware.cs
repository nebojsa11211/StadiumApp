using System.Net;
using System.Text.Json;
using StadiumDrinkOrdering.API.Services;

namespace StadiumDrinkOrdering.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                // Log the exception to our centralized logging system
                using var scope = _serviceProvider.CreateScope();
                var loggingService = scope.ServiceProvider.GetService<ILoggingService>();
                
                if (loggingService != null)
                {
                    await loggingService.LogErrorAsync(
                        exception: exception,
                        action: "UnhandledException",
                        category: "SystemError",
                        userId: GetUserIdFromContext(context),
                        userEmail: GetUserEmailFromContext(context),
                        userRole: GetUserRoleFromContext(context),
                        details: $"Unhandled exception in {context.Request.Path}",
                        requestPath: context.Request.Path,
                        httpMethod: context.Request.Method,
                        ipAddress: GetClientIpAddress(context),
                        userAgent: context.Request.Headers.UserAgent.ToString(),
                        source: "API"
                    );
                }
            }
            catch (Exception loggingException)
            {
                _logger.LogError(loggingException, "Failed to log exception to centralized logging system");
            }

            // Return error response to client
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            
            var response = new
            {
                message = "An error occurred while processing your request",
                error = isDevelopment ? exception.Message : "Internal server error",
                stackTrace = isDevelopment ? exception.StackTrace : null,
                timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private string? GetUserIdFromContext(HttpContext context)
        {
            return context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        private string? GetUserEmailFromContext(HttpContext context)
        {
            return context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        }

        private string? GetUserRoleFromContext(HttpContext context)
        {
            return context.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        }

        private string? GetClientIpAddress(HttpContext context)
        {
            // Try to get the real IP address
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0].Trim();
            }

            var xRealIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIp))
            {
                return xRealIp;
            }

            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}