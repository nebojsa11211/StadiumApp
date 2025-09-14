using System.Net;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    public static class ErrorMessageMappings
    {
        public static readonly Dictionary<HttpStatusCode, ErrorMessageInfo> StatusCodeMessages = new()
        {
            // 4xx Client Errors
            {
                HttpStatusCode.BadRequest,
                new ErrorMessageInfo("Invalid Request", "⚠️ The request contains invalid data. Please check your input and try again.",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.Unauthorized,
                new ErrorMessageInfo("Authentication Required", "🔐 Your session has expired. Please log in again to continue.",
                    severity: ErrorSeverity.Critical, isRetryable: false, requiresAuth: true)
            },
            {
                HttpStatusCode.Forbidden,
                new ErrorMessageInfo("Access Denied", "❌ You don't have permission to perform this action. Contact your administrator if you believe this is an error.",
                    severity: ErrorSeverity.Error, isRetryable: false)
            },
            {
                HttpStatusCode.NotFound,
                new ErrorMessageInfo("Not Found", "📭 The requested resource could not be found. It may have been deleted or moved.",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.Conflict,
                new ErrorMessageInfo("Conflict", "⚠️ This action conflicts with the current state. Please refresh and try again.",
                    severity: ErrorSeverity.Warning, isRetryable: true)
            },
            {
                HttpStatusCode.UnprocessableEntity,
                new ErrorMessageInfo("Validation Failed", "📋 Please correct the highlighted fields and try again.",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.TooManyRequests,
                new ErrorMessageInfo("Too Many Requests", "⏳ You're performing actions too quickly. Please wait a moment and try again.",
                    severity: ErrorSeverity.Warning, isRetryable: true, suggestedRetryDelay: TimeSpan.FromSeconds(30))
            },

            // 5xx Server Errors
            {
                HttpStatusCode.InternalServerError,
                new ErrorMessageInfo("Server Error", "⚠️ A server error occurred. Our technical team has been automatically notified.",
                    severity: ErrorSeverity.Error, isRetryable: true)
            },
            {
                HttpStatusCode.BadGateway,
                new ErrorMessageInfo("Service Unavailable", "🌐 Unable to connect to our servers. Please check your internet connection.",
                    severity: ErrorSeverity.Error, isRetryable: true)
            },
            {
                HttpStatusCode.ServiceUnavailable,
                new ErrorMessageInfo("Service Maintenance", "🔧 Our service is temporarily unavailable for maintenance. Please try again in a few minutes.",
                    severity: ErrorSeverity.Warning, isRetryable: true, suggestedRetryDelay: TimeSpan.FromMinutes(2))
            },
            {
                HttpStatusCode.GatewayTimeout,
                new ErrorMessageInfo("Request Timeout", "⏱️ The request is taking longer than expected. Please try again.",
                    severity: ErrorSeverity.Warning, isRetryable: true)
            }
        };

        public static readonly Dictionary<string, Dictionary<HttpStatusCode, string>> EndpointSpecificMessages = new()
        {
            {
                "auth/login",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.Unauthorized, "🔐 Invalid email or password. Please check your credentials and try again." },
                    { HttpStatusCode.TooManyRequests, "🛡️ Too many login attempts. Please wait 15 minutes before trying again." },
                    { HttpStatusCode.BadRequest, "📧 Please enter a valid email address and password." }
                }
            },
            {
                "orders",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.NotFound, "📦 This order could not be found. It may have been cancelled or deleted." },
                    { HttpStatusCode.Conflict, "⚠️ This order has been modified by another user. Please refresh to see the latest changes." },
                    { HttpStatusCode.UnprocessableEntity, "📋 Please check that all required order fields are filled correctly." }
                }
            },
            {
                "users",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.Conflict, "👤 A user with this email address already exists." },
                    { HttpStatusCode.Forbidden, "🔒 You don't have permission to manage user accounts." },
                    { HttpStatusCode.UnprocessableEntity, "📋 Please provide a valid email address and strong password." }
                }
            },
            {
                "stadium-structure",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.BadRequest, "🏟️ The stadium structure file is invalid. Please check the format and try again." },
                    { HttpStatusCode.RequestEntityTooLarge, "📁 The stadium structure file is too large. Maximum size is 10MB." },
                    { HttpStatusCode.UnsupportedMediaType, "📄 Please upload a valid JSON file." }
                }
            }
        };

        public static string GetContextSpecificMessage(string endpoint, HttpStatusCode statusCode)
        {
            foreach (var (pattern, messages) in EndpointSpecificMessages)
            {
                if (endpoint.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    if (messages.TryGetValue(statusCode, out var message))
                    {
                        return message;
                    }
                }
            }

            return StatusCodeMessages.TryGetValue(statusCode, out var defaultInfo)
                ? defaultInfo.Message
                : "An unexpected error occurred. Please try again.";
        }

        public static ErrorMessageInfo GetErrorInfo(HttpStatusCode statusCode, string? endpoint = null)
        {
            var defaultInfo = StatusCodeMessages.TryGetValue(statusCode, out var info)
                ? info
                : new ErrorMessageInfo("Error", "An unexpected error occurred.", ErrorSeverity.Error);

            if (!string.IsNullOrEmpty(endpoint))
            {
                var contextMessage = GetContextSpecificMessage(endpoint, statusCode);
                if (contextMessage != defaultInfo.Message)
                {
                    return new ErrorMessageInfo(defaultInfo.Title, contextMessage, defaultInfo.Severity,
                        defaultInfo.IsRetryable, defaultInfo.RequiresAuth, defaultInfo.SuggestedRetryDelay);
                }
            }

            return defaultInfo;
        }
    }
}