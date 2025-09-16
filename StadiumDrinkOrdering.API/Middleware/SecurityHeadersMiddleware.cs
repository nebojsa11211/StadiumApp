using Microsoft.Extensions.Primitives;

namespace StadiumDrinkOrdering.API.Middleware;

/// <summary>
/// Middleware that adds security headers to all HTTP responses
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers before processing the request
        AddSecurityHeaders(context);

        // Call the next middleware in the pipeline
        await _next(context);

        // Log security header application (debug level)
        _logger.LogTrace("Security headers applied to response for {RequestPath}", context.Request.Path);
    }

    /// <summary>
    /// Adds comprehensive security headers to the HTTP response
    /// </summary>
    private static void AddSecurityHeaders(HttpContext context)
    {
        var headers = context.Response.Headers;

        // X-Content-Type-Options: Prevent MIME type sniffing
        headers.XContentTypeOptions = "nosniff";

        // X-Frame-Options: Prevent clickjacking attacks
        headers.XFrameOptions = "DENY";

        // X-XSS-Protection: Enable XSS filtering (legacy browsers)
        headers["X-XSS-Protection"] = "1; mode=block";

        // Referrer-Policy: Control referrer information
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Permissions-Policy: Control browser features (replaces Feature-Policy)
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

        // Content-Security-Policy: Comprehensive CSP for API
        var csp = "default-src 'none'; " +
                  "script-src 'none'; " +
                  "style-src 'none'; " +
                  "img-src 'none'; " +
                  "font-src 'none'; " +
                  "connect-src 'self'; " +
                  "frame-ancestors 'none'; " +
                  "base-uri 'none'; " +
                  "form-action 'none';";
        headers["Content-Security-Policy"] = csp;

        // Strict-Transport-Security: Enforce HTTPS (only if HTTPS)
        if (context.Request.IsHttps)
        {
            headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
        }

        // Custom security headers for API
        headers["X-API-Version"] = "1.0";
        headers["X-Security-Policy"] = "Authentication required for protected endpoints";

        // Remove sensitive server information
        headers.Remove("Server");
        headers.Remove("X-Powered-By");
        headers.Remove("X-AspNet-Version");
        headers.Remove("X-AspNetMvc-Version");

        // Cache control for API responses (prevent caching of sensitive data)
        if (IsApiEndpoint(context.Request.Path))
        {
            headers.CacheControl = "no-store, no-cache, must-revalidate, private";
            headers.Pragma = "no-cache";
            headers.Expires = "0";
        }

        // Cross-Origin-Embedder-Policy and Cross-Origin-Opener-Policy
        headers["Cross-Origin-Embedder-Policy"] = "require-corp";
        headers["Cross-Origin-Opener-Policy"] = "same-origin";

        // X-Permitted-Cross-Domain-Policies: Restrict cross-domain policies
        headers["X-Permitted-Cross-Domain-Policies"] = "none";

        // Custom headers for tracking and debugging (non-sensitive)
        headers["X-Request-ID"] = context.TraceIdentifier;
        headers["X-Response-Time"] = DateTimeOffset.UtcNow.ToString("O");
    }

    /// <summary>
    /// Determines if the request path is an API endpoint
    /// </summary>
    private static bool IsApiEndpoint(PathString path)
    {
        return path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
    }
}

/// <summary>
/// Extension methods for adding security headers middleware
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    /// <summary>
    /// Adds the security headers middleware to the application pipeline
    /// </summary>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }

    /// <summary>
    /// Adds security headers middleware with custom configuration
    /// </summary>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder, Action<SecurityHeadersOptions> configureOptions)
    {
        var options = new SecurityHeadersOptions();
        configureOptions(options);

        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}

/// <summary>
/// Configuration options for security headers middleware
/// </summary>
public class SecurityHeadersOptions
{
    /// <summary>
    /// Whether to add the Strict-Transport-Security header
    /// </summary>
    public bool AddStrictTransportSecurity { get; set; } = true;

    /// <summary>
    /// Maximum age for HSTS in seconds
    /// </summary>
    public int HstsMaxAge { get; set; } = 31536000; // 1 year

    /// <summary>
    /// Whether to include subdomains in HSTS
    /// </summary>
    public bool HstsIncludeSubdomains { get; set; } = true;

    /// <summary>
    /// Whether to preload HSTS
    /// </summary>
    public bool HstsPreload { get; set; } = true;

    /// <summary>
    /// Custom Content Security Policy
    /// </summary>
    public string? ContentSecurityPolicy { get; set; }

    /// <summary>
    /// Custom referrer policy
    /// </summary>
    public string ReferrerPolicy { get; set; } = "strict-origin-when-cross-origin";

    /// <summary>
    /// Custom permissions policy
    /// </summary>
    public string PermissionsPolicy { get; set; } = "camera=(), microphone=(), geolocation=(), payment=()";

    /// <summary>
    /// Whether to remove server identification headers
    /// </summary>
    public bool RemoveServerHeaders { get; set; } = true;

    /// <summary>
    /// Additional custom headers to add
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = new();
}