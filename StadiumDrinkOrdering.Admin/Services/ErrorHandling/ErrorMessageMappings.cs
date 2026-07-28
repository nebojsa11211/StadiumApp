using System.Net;
using Microsoft.Extensions.Localization;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling
{
    /// <summary>
    /// Maps HTTP failures to the message an admin actually sees. The tables below hold SharedResources
    /// <em>keys</em>, not literal text, and are resolved through <see cref="ErrorLocalizer"/> at lookup
    /// time so the same failure reads Croatian or English according to the current request culture.
    /// </summary>
    public static class ErrorMessageMappings
    {
        /// <summary>
        /// <see cref="ApiResponse{T}.Failure"/> and friends are static factories with no DI, so the
        /// localizer is handed over once at startup instead of injected. Safe to share: the localizer
        /// holds no culture itself, resolving against CurrentUICulture on every lookup.
        /// </summary>
        public static class ErrorLocalizer
        {
            private static IStringLocalizer? _localizer;

            public static void Configure(IStringLocalizer localizer) => _localizer = localizer;

            /// <summary>Falls back to the key when startup wiring was skipped, so an admin still sees
            /// something identifiable rather than an empty toast.</summary>
            public static string Get(string key) => _localizer is null ? key : _localizer[key];
        }

        private static string L(string key) => ErrorLocalizer.Get(key);

        /// <summary>Status code -> (title key, message key) plus the non-textual response metadata.</summary>
        public static readonly Dictionary<HttpStatusCode, ErrorMessageInfo> StatusCodeMessages = new()
        {
            // 4xx Client Errors
            {
                HttpStatusCode.BadRequest,
                new ErrorMessageInfo("ApiErr_BadRequest_Title", "ApiErr_BadRequest_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.Unauthorized,
                new ErrorMessageInfo("ApiErr_Unauthorized_Title", "ApiErr_Unauthorized_Msg",
                    severity: ErrorSeverity.Critical, isRetryable: false, requiresAuth: true)
            },
            {
                HttpStatusCode.Forbidden,
                new ErrorMessageInfo("ApiErr_Forbidden_Title", "ApiErr_Forbidden_Msg",
                    severity: ErrorSeverity.Error, isRetryable: false)
            },
            {
                HttpStatusCode.NotFound,
                new ErrorMessageInfo("ApiErr_NotFound_Title", "ApiErr_NotFound_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.Conflict,
                new ErrorMessageInfo("ApiErr_Conflict_Title", "ApiErr_Conflict_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: true)
            },
            {
                HttpStatusCode.UnprocessableEntity,
                new ErrorMessageInfo("ApiErr_Validation_Title", "ApiErr_Validation_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: false)
            },
            {
                HttpStatusCode.TooManyRequests,
                new ErrorMessageInfo("ApiErr_TooManyRequests_Title", "ApiErr_TooManyRequests_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: true, suggestedRetryDelay: TimeSpan.FromSeconds(30))
            },

            // 5xx Server Errors
            {
                HttpStatusCode.InternalServerError,
                new ErrorMessageInfo("ApiErr_ServerError_Title", "ApiErr_ServerError_Msg",
                    severity: ErrorSeverity.Error, isRetryable: true)
            },
            {
                HttpStatusCode.BadGateway,
                new ErrorMessageInfo("ApiErr_BadGateway_Title", "ApiErr_BadGateway_Msg",
                    severity: ErrorSeverity.Error, isRetryable: true)
            },
            {
                HttpStatusCode.ServiceUnavailable,
                new ErrorMessageInfo("ApiErr_ServiceUnavailable_Title", "ApiErr_ServiceUnavailable_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: true, suggestedRetryDelay: TimeSpan.FromMinutes(2))
            },
            {
                HttpStatusCode.GatewayTimeout,
                new ErrorMessageInfo("ApiErr_GatewayTimeout_Title", "ApiErr_GatewayTimeout_Msg",
                    severity: ErrorSeverity.Warning, isRetryable: true)
            }
        };

        /// <summary>Endpoint fragment -> status code -> message key, for wording that beats the generic text.</summary>
        public static readonly Dictionary<string, Dictionary<HttpStatusCode, string>> EndpointSpecificMessages = new()
        {
            {
                "auth/login",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.Unauthorized, "ApiErr_Login_Unauthorized" },
                    { HttpStatusCode.TooManyRequests, "ApiErr_Login_TooMany" },
                    { HttpStatusCode.BadRequest, "ApiErr_Login_BadRequest" }
                }
            },
            {
                "orders",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.NotFound, "ApiErr_Orders_NotFound" },
                    { HttpStatusCode.Conflict, "ApiErr_Orders_Conflict" },
                    { HttpStatusCode.UnprocessableEntity, "ApiErr_Orders_Validation" }
                }
            },
            {
                "users",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.Conflict, "ApiErr_Users_Conflict" },
                    { HttpStatusCode.Forbidden, "ApiErr_Users_Forbidden" },
                    { HttpStatusCode.UnprocessableEntity, "ApiErr_Users_Validation" }
                }
            },
            {
                "stadium-structure",
                new Dictionary<HttpStatusCode, string>
                {
                    { HttpStatusCode.BadRequest, "ApiErr_Stadium_BadRequest" },
                    { HttpStatusCode.RequestEntityTooLarge, "ApiErr_Stadium_TooLarge" },
                    { HttpStatusCode.UnsupportedMediaType, "ApiErr_Stadium_MediaType" }
                }
            }
        };

        /// <summary>Resource key of the endpoint-specific message, or null when none applies.</summary>
        private static string? GetContextSpecificMessageKey(string endpoint, HttpStatusCode statusCode)
        {
            foreach (var (pattern, messages) in EndpointSpecificMessages)
            {
                if (endpoint.Contains(pattern, StringComparison.OrdinalIgnoreCase)
                    && messages.TryGetValue(statusCode, out var messageKey))
                {
                    return messageKey;
                }
            }

            return null;
        }

        /// <summary>Localized message for an endpoint + status, falling back to the generic status text.</summary>
        public static string GetContextSpecificMessage(string endpoint, HttpStatusCode statusCode)
        {
            var key = GetContextSpecificMessageKey(endpoint, statusCode);
            if (key is not null)
            {
                return L(key);
            }

            return StatusCodeMessages.TryGetValue(statusCode, out var defaultInfo)
                ? L(defaultInfo.Message)
                : L("ApiErr_Unexpected");
        }

        /// <summary>
        /// Returns an <see cref="ErrorMessageInfo"/> whose Title and Message are already localized, so
        /// callers can surface them directly.
        /// </summary>
        public static ErrorMessageInfo GetErrorInfo(HttpStatusCode statusCode, string? endpoint = null)
        {
            var defaultInfo = StatusCodeMessages.TryGetValue(statusCode, out var info)
                ? info
                : new ErrorMessageInfo("ApiErr_Generic_Title", "ApiErr_Generic_Msg", ErrorSeverity.Error);

            // Endpoint-specific wording wins over the generic status text when one is defined.
            var messageKey = endpoint is { Length: > 0 }
                ? GetContextSpecificMessageKey(endpoint, statusCode) ?? defaultInfo.Message
                : defaultInfo.Message;

            return new ErrorMessageInfo(L(defaultInfo.Title), L(messageKey), defaultInfo.Severity,
                defaultInfo.IsRetryable, defaultInfo.RequiresAuth, defaultInfo.SuggestedRetryDelay);
        }
    }
}
