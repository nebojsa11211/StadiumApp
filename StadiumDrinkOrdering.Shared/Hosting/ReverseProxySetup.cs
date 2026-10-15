using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StadiumDrinkOrdering.Shared.Hosting;

/// <summary>
/// Lets a server app run behind the path-based ngrok proxy (tools/proxy/proxy.js, see Ngrok.md).
///
/// Driven purely by configuration, which only the <c>ngrok</c> launch profiles set:
/// <list type="bullet">
///   <item><c>Proxy:Enabled=true</c> — trust <c>X-Forwarded-Proto/-Host/-For</c> from the proxy, so the
///   request's scheme/host are the public ngrok ones and every absolute URL the app composes (redirects,
///   cookie challenges, QR codes) points at the tunnel instead of localhost.</item>
///   <item><c>Proxy:Prefix=/admin</c> — <c>UsePathBase</c>, so <c>/admin/login</c> becomes path <c>/login</c>
///   with <c>PathBase=/admin</c> and every generated link, <c>&lt;base href="~/"&gt;</c> and Blazor circuit
///   URL keeps the prefix. Unprefixed requests pass through unchanged.</item>
/// </list>
///
/// Security: forwarded headers are trusted from any caller, which is safe only because the ngrok
/// profiles bind localhost and the proxy binds 127.0.0.1. Never combine these settings with a
/// LAN/0.0.0.0 binding.
/// </summary>
public static class ReverseProxySetup
{
    /// <summary>
    /// <c>Proxy:Enabled</c> parsed with <see cref="bool.TryParse(string?, out bool)"/>, so an empty value
    /// (<c>Proxy__Enabled=</c>) means "off" instead of throwing at startup like <c>GetValue&lt;bool&gt;</c>.
    /// </summary>
    public static bool IsEnabled(IConfiguration configuration) =>
        bool.TryParse(configuration["Proxy:Enabled"], out var enabled) && enabled;

    /// <summary><c>Proxy:Prefix</c> normalised to <c>/name</c> (leading slash, no trailing slash), or null.</summary>
    public static string? GetPrefix(IConfiguration configuration)
    {
        var prefix = configuration["Proxy:Prefix"]?.Trim().Trim('/');
        return string.IsNullOrEmpty(prefix) ? null : "/" + prefix;
    }

    /// <summary>
    /// Call right after <c>WebApplication.CreateBuilder</c>. Starts the ngrok tunnel on F5 when needed
    /// (<see cref="TunnelLauncher"/>) and configures forwarded-header trust when <c>Proxy:Enabled</c>.
    /// </summary>
    public static WebApplicationBuilder AddReverseProxySupport(this WebApplicationBuilder builder)
    {
        TunnelLauncher.EnsureRunning(builder.Configuration, builder.Environment);

        if (IsEnabled(builder.Configuration))
        {
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                         | ForwardedHeaders.XForwardedProto
                                         | ForwardedHeaders.XForwardedHost;
                // The proxy's address cannot be known in advance (IPv4 or IPv6 loopback).
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        return builder;
    }

    /// <summary>
    /// Call first in the pipeline, right after <c>builder.Build()</c> — before anything that reads the
    /// scheme, host or path (HTTPS redirection, static files, routing, authentication).
    /// </summary>
    public static WebApplication UseReverseProxySupport(this WebApplication app)
    {
        if (!IsEnabled(app.Configuration))
        {
            return app;
        }

        app.UseForwardedHeaders();

        var prefix = GetPrefix(app.Configuration);
        if (prefix is not null)
        {
            app.UsePathBase(prefix);
        }

        app.Logger.LogInformation("Reverse proxy mode: forwarded headers trusted, path base {Prefix}", prefix ?? "(none)");
        return app;
    }
}
