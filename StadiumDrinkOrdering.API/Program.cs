using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Middleware;
using StadiumDrinkOrdering.API.Models;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Handlers;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.Shared.Services;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using System.Text;
using System.Diagnostics;
using Npgsql;

// Load variables from the gitignored .env file (if present) BEFORE the configuration builder is
// created, so values like ConnectionStrings__Supabase become available to configuration. Existing
// process/launch-profile environment variables are NOT overwritten (NoClobber semantics).
StadiumDrinkOrdering.Shared.Configuration.AppConfiguration.LoadDotEnvFile();

// QuestPDF is used to render printable ticket cards (GET /tickets/{id}/card.pdf).
// Community license is free for individuals / small companies.
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// ================================================================================================
// ENVIRONMENT VARIABLE CONFIGURATION
// ================================================================================================
// This application uses environment variables for secure configuration management.
// See ENVIRONMENT_VARIABLES.md for complete documentation.
//
// Required Environment Variables:
// - JWT_SECRET_KEY: JWT signing secret (minimum 32 characters)
// - Database: Either ConnectionStrings__DefaultConnection OR individual DB_* variables
// - STRIPE_SECRET_KEY: Stripe payment secret key
// - SUPABASE_API_KEY: Supabase project API key
//
// Optional Environment Variables:
// - CORS_ALLOWED_ORIGINS: Comma-separated list of allowed origins
// - JWT_ISSUER, JWT_AUDIENCE: JWT token issuer and audience
//
// For development, copy .env.example to .env and configure values.
// ================================================================================================

// Configure logging to include database logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// TEMPORARILY COMMENTED OUT - Database logging causing middleware deadlock
// Configure database logging from appsettings
// var databaseLoggingConfig = builder.Configuration.GetSection("Logging:Database");
// builder.Logging.AddDatabaseLogger(config =>
// {
//     config.IsEnabled = databaseLoggingConfig.GetValue<bool>("Enabled", true);
//     config.MinimumLevel = Enum.Parse<LogLevel>(databaseLoggingConfig.GetValue<string>("MinLevel") ?? "Information");
//     config.BatchingEnabled = databaseLoggingConfig.GetValue<bool>("BatchingEnabled", true);
//     
//     // Load excluded categories
//     var excludeCategories = databaseLoggingConfig.GetSection("ExcludeCategories").Get<List<string>>();
//     if (excludeCategories != null)
//     {
//         config.ExcludeCategories = new HashSet<string>(excludeCategories);
//     }
//     
//     // Load critical categories
//     var criticalCategories = databaseLoggingConfig.GetSection("CriticalCategories").Get<List<string>>();
//     if (criticalCategories != null)
//     {
//         config.CriticalCategories = new HashSet<string>(criticalCategories);
//     }
// });

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.MaxDepth = 64; // Increase max depth to handle complex object graphs
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ================================================================================================
// DATABASE PROVIDER SELECTION
// ================================================================================================
// Two databases are supported, selected by the Database:Provider configuration value:
//   "LocalPostgres" (default) -> ConnectionStrings:LocalPostgres (appsettings.json, no secret)
//   "Supabase"                -> ConnectionStrings:Supabase (the password lives in the gitignored
//                                .env file as ConnectionStrings__Supabase, loaded at startup)
//
// Precedence:
//   1. A full ConnectionStrings:DefaultConnection (e.g. set by docker-compose / production / CI)
//      always wins. This keeps container and deployment behaviour unchanged.
//   2. Otherwise the connection string named by Database:Provider is used.
// This is the single place that decides which database the API talks to.

string connectionString;
var explicitConnection = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrWhiteSpace(explicitConnection) && !explicitConnection.Contains("{DB_"))
{
    connectionString = explicitConnection;
    Console.WriteLine("Database selection: explicit ConnectionStrings:DefaultConnection (Docker/production override)");
}
else
{
    var dbProvider = (builder.Configuration["Database:Provider"] ?? "LocalPostgres").Trim();

    connectionString = dbProvider.ToLowerInvariant() switch
    {
        "supabase" => builder.Configuration.GetConnectionString("Supabase"),
        "localpostgres" or "local" or "postgres" => builder.Configuration.GetConnectionString("LocalPostgres"),
        _ => throw new InvalidOperationException(
            $"Unknown Database:Provider '{dbProvider}'. Valid values are 'LocalPostgres' or 'Supabase'.")
    };

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        var hint = dbProvider.ToLowerInvariant().Contains("supabase")
            ? "Set ConnectionStrings__Supabase in the gitignored .env file."
            : "Set ConnectionStrings:LocalPostgres in appsettings.json.";
        throw new InvalidOperationException(
            $"Database:Provider is '{dbProvider}' but no matching connection string was found. {hint}");
    }

    Console.WriteLine($"Database selection: Database:Provider = '{dbProvider}'");
}

// Masked connection string for diagnostics (never log the password)
var maskedConnection = System.Text.RegularExpressions.Regex.Replace(
    connectionString, @"(?i)(Password\s*=)[^;]*", "$1***");
Console.WriteLine($"Using PostgreSQL connection: {maskedConnection}");

// ================================================================================================
// LOCAL DEV DATABASE CONTAINER AUTO-START  (the "F5 always brings up the DB" guarantee)
// ================================================================================================
// The MSBuild EnsureLocalDbBeforeBuild target only runs when Visual Studio actually BUILDS. When
// the project is already up to date, pressing F5 SKIPS the build, the target never fires, and the
// local Postgres container stays down -> the API dies with the NpgsqlRetryingExecutionStrategy
// "maximum number of retries (3) was exceeded" error. Doing it here in Program.cs runs on EVERY
// launch (F5 or dotnet run), so the container (stadium-postgres-local) is always started/recreated
// before we try to connect. Skipped inside Docker and when not targeting the local DB.
var runningInContainer = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true",
    StringComparison.OrdinalIgnoreCase);
var usesLocalDb = connectionString.Contains("localhost", StringComparison.OrdinalIgnoreCase)
    || connectionString.Contains("127.0.0.1");
if (builder.Environment.IsDevelopment() && !runningInContainer && usesLocalDb)
{
    EnsureLocalDbContainerRunning(builder.Environment.ContentRootPath, connectionString);
}

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", failureStatus: HealthStatus.Unhealthy, tags: new[] { "database" })
    .AddNpgSql(connectionString, name: "postgresql-connection", failureStatus: HealthStatus.Unhealthy, tags: new[] { "database", "postgresql" });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null);
        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });

    // Performance optimizations
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
    options.EnableServiceProviderCaching();
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

    // Configure query behavior
    options.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.MultipleCollectionIncludeWarning));

    // Add logging for SQL queries in development.
    // Kept at Warning (not Information): at Information EF writes EVERY executed SQL command to the
    // console synchronously (and it double-prints alongside the default logger), which adds real I/O
    // to both startup and every request while burying the useful lines. Warning still surfaces
    // problems (e.g. client-eval / multiple-collection-include). Set to Information ad hoc when
    // actively debugging a specific query.
    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Warning);
    }
});

// Authentication - Use environment variables first, then configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Get JWT secret from environment variable first, then configuration
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? jwtSettings["SecretKey"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException(
        "JWT Secret Key is not configured. Set the JWT_SECRET_KEY environment variable or JwtSettings:SecretKey in configuration.");
}

if (secretKey.Length < 32)
{
    throw new InvalidOperationException(
        "JWT Secret Key must be at least 32 characters long for security.");
}

var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
             ?? jwtSettings["Issuer"]
             ?? "StadiumDrinkOrdering";

var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
               ?? jwtSettings["Audience"]
               ?? "StadiumDrinkOrdering";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Basic validation
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Issuer and audience settings
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

            // Security enhancements
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }, // Restrict to HMAC SHA-256 only
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            RequireAudience = true,

            // Clock skew tolerance (reduced for better security)
            ClockSkew = TimeSpan.FromMinutes(1), // Allow only 1 minute clock skew

            // Additional security settings
            ValidateActor = false, // We don't use actor tokens
            ValidateTokenReplay = false, // Enable if replay protection is needed

            // Lifetime validation settings
            LifetimeValidator = (notBefore, expires, token, parameters) =>
            {
                // Custom lifetime validation
                var now = DateTime.UtcNow;

                // Check if token is not yet valid
                if (notBefore.HasValue && now < notBefore.Value.AddMinutes(-1))
                    return false;

                // Check if token is expired (with minimal tolerance)
                if (expires.HasValue && now > expires.Value.AddMinutes(1))
                    return false;

                return true;
            }
        };

        // SECURITY: Configure JWT authentication for SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Allow SignalR to receive JWT tokens from query string or headers
                var accessToken = context.Request.Query["access_token"];

                // If the request is for SignalR hubs
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/bartenderHub") || path.StartsWithSegments("/customerHub")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// ================================================================================================
// COMPREHENSIVE AUTHORIZATION CONFIGURATION
// ================================================================================================
// Policy-based authorization with custom requirements and handlers for enterprise-grade security

builder.Services.AddAuthorization(options =>
{
    // Configure all authorization policies using centralized policy definitions
    AuthorizationPolicies.ConfigurePolicies(options);
});

// Register custom authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, OrderOwnershipHandler>();
builder.Services.AddScoped<IAuthorizationHandler, UserOwnershipHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TicketOwnershipHandler>();

// Register authorization service for helper methods
builder.Services.AddScoped<IStadiumAuthorizationService, StadiumAuthorizationService>();

// Services
builder.Services.AddScoped<DatabaseHealthCheck>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWalletService, WalletService>();
// Wallet deposit gateway: Mock (synchronous, default for dev) or Stripe (async intent + webhook).
// Selected via WalletGateway:Provider so switching to real payments is config-only.
var walletGatewayProvider = builder.Configuration.GetValue<string>("WalletGateway:Provider") ?? "Mock";
if (string.Equals(walletGatewayProvider, "Stripe", StringComparison.OrdinalIgnoreCase))
    builder.Services.AddScoped<IWalletPaymentGateway, StripeWalletPaymentGateway>();
else
    builder.Services.AddScoped<IWalletPaymentGateway, MockWalletPaymentGateway>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();

// Email + shell-account provisioning. Real SMTP is used only when Email:Host is configured; otherwise a
// dev sender logs the activation link so the flow is testable without SMTP credentials.
var emailSettings = builder.Configuration.GetSection("Email").Get<EmailSettings>() ?? new EmailSettings();
builder.Services.AddSingleton(emailSettings);
if (!string.IsNullOrWhiteSpace(emailSettings.Host))
    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
else
    builder.Services.AddScoped<IEmailSender, LoggingEmailSender>();
builder.Services.AddScoped<IAccountProvisioningService, AccountProvisioningService>();

builder.Services.AddScoped<ITicketCardPdfService, TicketCardPdfService>();
builder.Services.AddScoped<ITicketDetailService, TicketDetailService>();
builder.Services.AddScoped<IPaymentService, StripePaymentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderSessionService, OrderSessionService>();
builder.Services.AddScoped<IDemoDataService, DemoDataService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<IStadiumStructureService, StadiumStructureService>();
builder.Services.AddScoped<ISeatMappingService, SeatMappingService>();
builder.Services.AddScoped<IOverlaySeatService, OverlaySeatService>();
builder.Services.AddScoped<ITicketIngestionService, TicketIngestionService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ITicketAuthService, TicketAuthService>();
builder.Services.AddScoped<ICustomerAnalyticsService, CustomerAnalyticsService>();
builder.Services.AddScoped<TicketingDataImportService>();

// Memory cache for stadium layout caching
builder.Services.AddMemoryCache();

// Stadium SVG Layout Services
builder.Services.AddScoped<IStadiumLayoutGenerator, HNKRijekaLayoutGenerator>();
builder.Services.AddScoped<IStadiumLayoutService, StadiumLayoutService>();

// Add centralized logging client (required by AuthController and other services)
builder.Services.AddCentralizedLogging("http://localhost", "API");

// Rate Limiting Configuration
builder.Services.Configure<RateLimitingConfig>(builder.Configuration.GetSection("RateLimiting"));
builder.Services.AddScoped<IBruteForceProtectionService>(serviceProvider =>
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var config = serviceProvider.GetRequiredService<IOptions<RateLimitingConfig>>();
    var logger = serviceProvider.GetRequiredService<ILogger<BruteForceProtectionService>>();
    // Try to get the logging client, but use null implementation if not available (for migrations)
    var loggingClient = serviceProvider.GetService<ICentralizedLoggingClient>();
    return new BruteForceProtectionService(context, config, loggingClient, logger);
});

// Rate limiting middleware configuration
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Security Headers Configuration
builder.Services.Configure<SecurityHeadersOptions>(builder.Configuration.GetSection("SecurityHeaders"));

// Add background services
// TEMPORARILY DISABLED: All background services causing database connection pool exhaustion and 182-second timeouts
// builder.Services.AddHostedService<LogRetentionBackgroundService>();
// builder.Services.AddHostedService<RateLimitCleanupService>();
// builder.Services.AddHostedService<TicketSessionCleanupService>();
// Wallet reconciliation is self-gated on WalletReconciliation:Enabled (default false), so registering
// it is safe: it no-ops unless explicitly enabled. Read-only (verifies Balance == Σ ledger).
builder.Services.AddHostedService<WalletReconciliationBackgroundService>();

// Event auto-completion: closes ended-but-still-live events to Completed on a schedule (default every
// 5 min). Gated on EventLifecycle:AutoCompleteEnabled (default true); scans only the few live events
// per pass, so it is far lighter than the log/rate cleanup services that were disabled above.
builder.Services.AddHostedService<EventStatusTransitionService>();

// Stripe Configuration
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// CORS - Use environment variables for allowed origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        // Get allowed origins from environment variable first, then use defaults
        var allowedOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS");

        if (!string.IsNullOrEmpty(allowedOrigins))
        {
            // Parse comma-separated origins from environment variable
            var origins = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(o => o.Trim())
                                      .ToArray();
            corsBuilder.WithOrigins(origins);
        }
        else
        {
            // Default origins for development (should be overridden in production).
            // 7060/9060 + host "runner" are the Runner WASM PWA (browser-origin, so it needs CORS
            // unlike the server-rendered apps). NOTE: 7050 is the TicketingSimulator (server-rendered,
            // no CORS needed) — the Runner moved off the shared 7050 to 7060. See docs/staff-app-split-plan.md.
            corsBuilder.WithOrigins(
                "https://localhost:7010", "https://localhost:7020", "https://localhost:7030", "https://localhost:7060",
                "https://admin:9030", "https://customer:9020", "https://runner:9060",
                "https://localhost:9060"
            );
        }

        corsBuilder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
});

// SignalR with JWT authentication support
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stadium API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// TEMPORARILY DISABLE security headers middleware due to hanging issue investigation
// TODO: Re-enable after fixing hanging issue
// app.UseSecurityHeaders();

// TEMPORARILY DISABLE rate limiting middleware due to hanging issue
// TODO: Fix rate limiting middleware or implement alternative
// app.UseIpRateLimiting();

// TEMPORARILY DISABLE global exception middleware due to hanging issue investigation
// TODO: Re-enable after fixing hanging issue or implement alternative
// app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add SignalR hubs
app.MapHub<StadiumDrinkOrdering.API.Hubs.BartenderHub>("/bartenderHub");
app.MapHub<StadiumDrinkOrdering.API.Hubs.CustomerHub>("/customerHub");

// Add health check endpoint
app.MapHealthChecks("/health");

// ================================================================================================
// OPEN THE LISTENING SOCKET *FIRST*, then warm the database.
// ================================================================================================
// This ordering is the fix for: "No connection could be made because the target machine actively
// refused it (localhost:7010)". Previously the DB warm-up / migrations / seeding all ran BEFORE the
// server started, so Kestrel did not bind port 7010 until every bit of that finished. During a cold
// or slow DB start that window was many seconds (and if the DB was briefly unreachable, the retry
// loop stretched it to tens of seconds) - and for that ENTIRE window every app pointing at the API
// got a hard "connection refused", because nothing was listening yet.
//
// app.StartAsync() binds the socket and begins accepting connections immediately. We then warm the
// DB while the server is already up, so clients connect right away instead of being refused. The
// local DB container is guaranteed running by EnsureLocalDbContainerRunning (above), so an early
// request warms the pool in ~1s rather than hitting the old cold-connect penalty. If the DB is
// genuinely unreachable we log and stay up (degraded) rather than hard-crash.
await app.StartAsync();
Console.WriteLine("✓ API is now listening (port bound); warming database connection/pool...");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

    // Re-verifying every seed user's BCrypt hash costs ~0.9s on EVERY launch (4 users x ~200-300ms).
    // It only exists to self-heal a corrupted/mismatched password hash, which is rare - so it is OFF
    // by default. Set SeedData:VerifyPasswordsOnStartup=true (appsettings or env) to force the check
    // when you suspect a bad seed password. Missing users are still always created; empty/invalid-
    // format hashes are still always reset (those checks are free).
    var verifySeedPasswords = app.Configuration.GetValue<bool>("SeedData:VerifyPasswordsOnStartup", false);

    try
    {
        await InitializeDatabaseAsync(context, logger, environment, verifySeedPasswords);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database initialization failed - starting in a degraded state");
    }
}


// Runs ensure-localdb.ps1 to guarantee the local Postgres dev container is up before the API tries
// to connect. Invoked on every local (non-container) launch that targets the local DB. Failures are
// swallowed - the DB-init retry loop below surfaces a clear error if the DB is genuinely unreachable.
static void EnsureLocalDbContainerRunning(string contentRootPath, string connectionString)
{
    try
    {
        // FAST PATH: if the DB port is already accepting TCP connections, the container is up and
        // there is nothing to do. This cheap (~ms) probe avoids shelling out to PowerShell +
        // several docker CLI round-trips (each ~0.3-1s on Docker Desktop/Windows) on EVERY launch
        // when the DB is already running - which is the overwhelmingly common case. We only pay the
        // slow ensure-localdb.ps1 path when the port is genuinely dead (container stopped/removed).
        var (dbHost, dbPort) = ParseHostPort(connectionString);
        if (IsTcpPortOpen(dbHost, dbPort, TimeSpan.FromMilliseconds(500)))
        {
            Console.WriteLine($"Local dev database already reachable at {dbHost}:{dbPort}; skipping container auto-start.");
            return;
        }
        Console.WriteLine($"Local dev database not reachable at {dbHost}:{dbPort}; running ensure-localdb.ps1...");

        // Walk up from the content root (the API project dir under VS/dotnet run) to the repo root
        // that holds ensure-localdb.ps1.
        string? scriptPath = null;
        for (var dir = new DirectoryInfo(contentRootPath); dir != null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "ensure-localdb.ps1");
            if (File.Exists(candidate)) { scriptPath = candidate; break; }
        }

        if (scriptPath is null)
        {
            Console.WriteLine("⚠ ensure-localdb.ps1 not found; skipping local DB container auto-start.");
            return;
        }

        Console.WriteLine("Ensuring local dev database container (stadium-postgres-local) is running...");
        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        using var proc = Process.Start(psi);
        if (proc is null)
        {
            Console.WriteLine("⚠ Could not start powershell to run ensure-localdb.ps1.");
            return;
        }

        proc.OutputDataReceived += (_, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
        proc.ErrorDataReceived += (_, e) => { if (e.Data != null) Console.WriteLine(e.Data); };
        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        // ensure-localdb.ps1 caps its own health wait at ~30s, so waiting much longer than that
        // just adds dead time on a genuinely broken DB. Give it a small margin over its own budget.
        if (!proc.WaitForExit(35000))
        {
            Console.WriteLine("⚠ ensure-localdb.ps1 timed out after 35s; continuing startup.");
            try { proc.Kill(entireProcessTree: true); } catch { /* best effort */ }
        }
    }
    catch (Exception ex)
    {
        // Never block API startup because Docker/PowerShell is unavailable.
        Console.WriteLine($"⚠ Local DB container auto-start failed: {ex.Message}");
    }
}

// Parses the host and port the DB lives on from the connection string, so the fast-path TCP probe
// knows where to knock. Defaults to localhost:5432 if anything is missing or unparseable.
static (string Host, int Port) ParseHostPort(string connectionString)
{
    try
    {
        var b = new NpgsqlConnectionStringBuilder(connectionString);
        // Host may be a comma-separated list (multi-host failover); probe the first entry.
        var host = (b.Host ?? "localhost").Split(',')[0].Trim();
        if (string.IsNullOrEmpty(host)) host = "localhost";
        return (host, b.Port); // NpgsqlConnectionStringBuilder.Port defaults to 5432
    }
    catch
    {
        return ("localhost", 5432);
    }
}

// Cheap synchronous "is anything listening?" probe. Returns false (not open) on any failure or
// timeout, which sends us down the slow ensure-localdb.ps1 path - the safe direction.
static bool IsTcpPortOpen(string host, int port, TimeSpan timeout)
{
    try
    {
        using var client = new System.Net.Sockets.TcpClient();
        var connectTask = client.ConnectAsync(host, port);
        return connectTask.Wait(timeout) && client.Connected;
    }
    catch
    {
        return false;
    }
}

// Database initialization method - FIXED VERSION
static async Task InitializeDatabaseAsync(ApplicationDbContext context, ILogger logger, IWebHostEnvironment environment, bool verifySeedPasswords)
{
    // Docker Desktop's Windows port proxy typically refuses/times-out the first few connections to
    // localhost:5432 ("Timeout during reading attempt"), then starts working. That warm-up needs a
    // handful of QUICK retries - not long waits - so we use many attempts with a short, capped delay
    // (see below). The old 5-attempt exponential backoff slept ~15-20s before the attempt that
    // finally connected, which is what left the DB unavailable for so long after launch.
    const int maxRetries = 12;
    const int baseDelayMs = 400;
    
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            logger.LogInformation("Database initialization attempt {Attempt} of {MaxRetries}", attempt, maxRetries);
            
            // Create a fresh timeout for this attempt
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            var cancellationToken = cancellationTokenSource.Token;
            
            // Get connection string for logging (safe to get before opening)
            var connectionString = context.Database.GetConnectionString();
            logger.LogInformation("Attempting to connect to database...");
            
            // Step 1: Open the connection explicitly to ensure it's working
            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
                logger.LogInformation("Database connection opened successfully");
            }
            
            // Now safely get database name AFTER connection is open
            var databaseName = connection.Database;
            logger.LogInformation("Connected to database: {DatabaseName}", databaseName);
            
            // Step 2: Run a simple test query using the open connection
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT 1";
                command.CommandTimeout = 30;
                var result = await command.ExecuteScalarAsync(cancellationToken);
                logger.LogInformation("Connection test query executed successfully, result: {Result}", result);
            }
            
            // Step 3: Check and apply migrations
            try
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync(cancellationToken);
                
                logger.LogInformation("Migration status - Applied: {AppliedCount}, Pending: {PendingCount}", 
                    appliedMigrations.Count(), pendingMigrations.Count());
                
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying {Count} pending migrations:", pendingMigrations.Count());
                    foreach (var migration in pendingMigrations)
                    {
                        logger.LogInformation("  - {MigrationName}", migration);
                    }
                    
                    await context.Database.MigrateAsync(cancellationToken);
                    logger.LogInformation("All migrations applied successfully");
                }
                else
                {
                    logger.LogInformation("No pending migrations, database schema is up to date");
                }
            }
            catch (Exception migrationEx)
            {
                // If migrations fail, it might be because the database doesn't exist yet
                logger.LogWarning("Migration check failed: {Message}. Attempting to ensure database exists...", migrationEx.Message);
                
                try
                {
                    // Try to ensure database exists and apply all migrations
                    await context.Database.EnsureCreatedAsync(cancellationToken);
                    logger.LogInformation("Database created/verified successfully");
                }
                catch
                {
                    // If EnsureCreated fails, try Migrate which will create the database
                    await context.Database.MigrateAsync(cancellationToken);
                    logger.LogInformation("Database created and migrations applied");
                }
            }
            
            // Step 4: Verify we can query actual tables
            try
            {
                // Try to count users as a final verification
                var userCount = await context.Users.CountAsync(cancellationToken);
                logger.LogInformation("Database verification successful - found {UserCount} users", userCount);
                
                // Check if admin user exists or needs to be created/updated. Only in Development.
                if (environment.IsDevelopment())
                {
                    const string adminEmail = "nebojsa.medancic+adminStadion@gmail.com";
                    const string adminUsername = "nebojsa.medancic+adminStadion@gmail.com";
                    const string adminPassword = "Admin123!";

                    // Match either the new email or the legacy "admin@stadium.com" so an existing
                    // admin record is migrated in place instead of leaving a duplicate behind.
                    var adminUser = await context.Users.FirstOrDefaultAsync(
                        u => u.Email == adminEmail || u.Email == "admin@stadium.com", cancellationToken);

                    if (adminUser == null)
                    {
                        logger.LogInformation("No admin user found, creating default admin user...");
                        adminUser = new StadiumDrinkOrdering.Shared.Models.User
                        {
                            Username = adminUsername,
                            Email = adminEmail,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                            Role = StadiumDrinkOrdering.Shared.Models.UserRole.Admin,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Users.Add(adminUser);
                        await context.SaveChangesAsync(cancellationToken);
                        logger.LogInformation("Default admin user created successfully: {AdminEmail}", adminEmail);
                    }
                    else
                    {
                        // Admin user exists - only update if necessary (don't regenerate password hash every time)
                        logger.LogInformation("Admin user found, checking if updates are needed...");

                        var needsUpdate = false;

                        // Migrate the email (e.g. from the legacy admin@stadium.com) if different
                        if (adminUser.Email != adminEmail)
                        {
                            logger.LogInformation("Updating admin email from '{OldEmail}' to '{NewEmail}'", adminUser.Email, adminEmail);
                            adminUser.Email = adminEmail;
                            context.Entry(adminUser).Property(u => u.Email).IsModified = true;
                            needsUpdate = true;
                        }

                        // Only update username if it's different
                        if (adminUser.Username != adminUsername)
                        {
                            logger.LogInformation("Updating admin username from '{OldUsername}' to '{NewUsername}'", adminUser.Username, adminUsername);
                            adminUser.Username = adminUsername;
                            context.Entry(adminUser).Property(u => u.Username).IsModified = true;
                            needsUpdate = true;
                        }

                        // Only update role if it's different
                        if (adminUser.Role != StadiumDrinkOrdering.Shared.Models.UserRole.Admin)
                        {
                            logger.LogInformation("Updating admin role from '{OldRole}' to 'Admin'", adminUser.Role);
                            adminUser.Role = StadiumDrinkOrdering.Shared.Models.UserRole.Admin;
                            needsUpdate = true;
                        }

                        // Verify password hash is valid and works with the configured admin password
                        bool passwordNeedsReset = false;

                        if (string.IsNullOrEmpty(adminUser.PasswordHash))
                        {
                            logger.LogInformation("Admin password hash is empty, generating new hash...");
                            passwordNeedsReset = true;
                        }
                        else if (!adminUser.PasswordHash.StartsWith("$2a$") &&
                                 !adminUser.PasswordHash.StartsWith("$2b$") &&
                                 !adminUser.PasswordHash.StartsWith("$2y$"))
                        {
                            logger.LogInformation("Admin password hash has invalid format, regenerating...");
                            passwordNeedsReset = true;
                        }
                        else if (verifySeedPasswords)
                        {
                            // Verify the hash actually works with the configured admin password.
                            // Gated: this BCrypt.Verify is ~300ms and only guards against a corrupted
                            // hash (rare). Skipped on normal launches; see SeedData:VerifyPasswordsOnStartup.
                            try
                            {
                                bool passwordValid = BCrypt.Net.BCrypt.Verify(adminPassword, adminUser.PasswordHash);
                                if (!passwordValid)
                                {
                                    logger.LogWarning("Admin password hash exists but doesn't verify with the configured password, regenerating...");
                                    passwordNeedsReset = true;
                                }
                                else
                                {
                                    logger.LogInformation("Admin password hash is valid and verified successfully");
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning(ex, "Failed to verify admin password hash, regenerating...");
                                passwordNeedsReset = true;
                            }
                        }

                        if (passwordNeedsReset)
                        {
                            adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword);
                            needsUpdate = true;
                            logger.LogInformation("Admin password hash regenerated successfully");
                        }

                        if (needsUpdate)
                        {
                            await context.SaveChangesAsync(cancellationToken);
                            logger.LogInformation("Admin user updated successfully: {AdminEmail}", adminEmail);
                        }
                        else
                        {
                            logger.LogInformation("Admin user is already correctly configured, no updates needed");
                        }
                    }

                    // Check if customer user exists or needs to be created
                    var customerUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "customer@stadium.com", cancellationToken);

                    if (customerUser == null)
                    {
                        logger.LogInformation("No customer user found, creating default customer user...");
                        customerUser = new StadiumDrinkOrdering.Shared.Models.User
                        {
                            Username = "customer",
                            Email = "customer@stadium.com",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123"),
                            Role = StadiumDrinkOrdering.Shared.Models.UserRole.Customer,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Users.Add(customerUser);
                        await context.SaveChangesAsync(cancellationToken);
                        logger.LogInformation("Default customer user created successfully: customer@stadium.com / customer123");
                    }
                    else
                    {
                        logger.LogInformation("Customer user found: customer@stadium.com");

                        // Verify password hash works with "customer123". Gated behind
                        // SeedData:VerifyPasswordsOnStartup - skipped on normal launches to save ~200ms.
                        if (verifySeedPasswords)
                        {
                            bool passwordValid = false;
                            try
                            {
                                passwordValid = BCrypt.Net.BCrypt.Verify("customer123", customerUser.PasswordHash);
                            }
                            catch { }

                            if (!passwordValid)
                            {
                                logger.LogInformation("Customer password needs reset, updating...");
                                customerUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("customer123");
                                customerUser.Role = StadiumDrinkOrdering.Shared.Models.UserRole.Customer;
                                await context.SaveChangesAsync(cancellationToken);
                                logger.LogInformation("Customer user password reset successfully");
                            }
                        }
                    }

                    // Check if staff user exists or needs to be created
                    const string staffEmail = "nebojsa.medancic+staff1@gmail.com";
                    const string staffPassword = "Admin123!";
                    var staffUser = await context.Users.FirstOrDefaultAsync(u => u.Email == staffEmail, cancellationToken);

                    if (staffUser == null)
                    {
                        logger.LogInformation("No staff user found, creating default staff user...");
                        staffUser = new StadiumDrinkOrdering.Shared.Models.User
                        {
                            Username = staffEmail,
                            Email = staffEmail,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword(staffPassword),
                            Role = StadiumDrinkOrdering.Shared.Models.UserRole.Bartender,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Users.Add(staffUser);
                        await context.SaveChangesAsync(cancellationToken);
                        logger.LogInformation("Default staff user created successfully: {StaffEmail}", staffEmail);
                    }
                    else
                    {
                        logger.LogInformation("Staff user found: {StaffEmail}", staffEmail);

                        // Verify password hash works with the expected password. Gated behind
                        // SeedData:VerifyPasswordsOnStartup - skipped on normal launches to save ~200ms.
                        if (verifySeedPasswords)
                        {
                            bool passwordValid = false;
                            try
                            {
                                passwordValid = BCrypt.Net.BCrypt.Verify(staffPassword, staffUser.PasswordHash);
                            }
                            catch { }

                            if (!passwordValid)
                            {
                                logger.LogInformation("Staff password needs reset, updating...");
                                staffUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(staffPassword);
                                await context.SaveChangesAsync(cancellationToken);
                                logger.LogInformation("Staff user password reset successfully");
                            }
                        }
                    }

                    // Check if waiter (Runner app) user exists or needs to be created
                    const string waiterEmail = "nebojsa.medancic+waiter1@gmail.com";
                    const string waiterPassword = "Admin123!";
                    var waiterUser = await context.Users.FirstOrDefaultAsync(u => u.Email == waiterEmail, cancellationToken);

                    if (waiterUser == null)
                    {
                        logger.LogInformation("No waiter user found, creating default waiter user...");
                        waiterUser = new StadiumDrinkOrdering.Shared.Models.User
                        {
                            Username = waiterEmail,
                            Email = waiterEmail,
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword(waiterPassword),
                            Role = StadiumDrinkOrdering.Shared.Models.UserRole.Waiter,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Users.Add(waiterUser);
                        await context.SaveChangesAsync(cancellationToken);
                        logger.LogInformation("Default waiter user created successfully: {WaiterEmail}", waiterEmail);
                    }
                    else
                    {
                        logger.LogInformation("Waiter user found: {WaiterEmail}", waiterEmail);

                        // Verify password hash works with the expected password. Gated behind
                        // SeedData:VerifyPasswordsOnStartup - skipped on normal launches to save ~200ms.
                        if (verifySeedPasswords)
                        {
                            bool passwordValid = false;
                            try
                            {
                                passwordValid = BCrypt.Net.BCrypt.Verify(waiterPassword, waiterUser.PasswordHash);
                            }
                            catch { }

                            if (!passwordValid)
                            {
                                logger.LogInformation("Waiter password needs reset, updating...");
                                waiterUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(waiterPassword);
                                waiterUser.Role = StadiumDrinkOrdering.Shared.Models.UserRole.Waiter;
                                await context.SaveChangesAsync(cancellationToken);
                                logger.LogInformation("Waiter user password reset successfully");
                            }
                        }
                    }

                    // Check if stadium structure exists, create basic structure if not
                    var tribuneCount = await context.Tribunes.CountAsync(cancellationToken);
                    if (tribuneCount == 0)
                    {
                        logger.LogInformation("No stadium structure found, creating basic structure...");

                        var tribuneN = new StadiumDrinkOrdering.Shared.Models.Tribune
                        {
                            Code = "N",
                            Name = "North Tribune",
                            Description = "North tribune",
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Tribunes.Add(tribuneN);
                        await context.SaveChangesAsync(cancellationToken);

                        var ring1 = new StadiumDrinkOrdering.Shared.Models.Ring
                        {
                            TribuneId = tribuneN.Id,
                            Number = 1,
                            Name = "Lower Ring",
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Rings.Add(ring1);
                        await context.SaveChangesAsync(cancellationToken);

                        var sectorNA = new StadiumDrinkOrdering.Shared.Models.Sector
                        {
                            RingId = ring1.Id,
                            Code = "NA",
                            Name = "North A",
                            TotalRows = 25,
                            SeatsPerRow = 20,
                            StartRow = 1,
                            StartSeat = 1,
                            CreatedAt = DateTime.UtcNow
                        };
                        context.Sectors.Add(sectorNA);
                        await context.SaveChangesAsync(cancellationToken);

                        logger.LogInformation("Basic stadium structure created successfully (North Tribune with 500 seats)");
                    }
                    else
                    {
                        logger.LogInformation("Stadium structure found - {TribuneCount} tribunes exist", tribuneCount);
                    }
                }
            }
            catch (Exception tableEx)
            {
                logger.LogWarning("Table query failed: {Message}. This might be normal on first run.", tableEx.Message);
                // Don't fail here - the migrations might create the tables
            }
            
            // Close the connection properly
            if (connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
            
            // Success! Exit the retry loop
            logger.LogInformation("✓ Database initialization completed successfully!");
            return;
        }
        catch (OperationCanceledException)
        {
            logger.LogError("Database initialization timed out on attempt {Attempt}", attempt);
        }
        catch (Npgsql.NpgsqlException npgsqlEx) when (npgsqlEx.SqlState == "3D000") // database does not exist
        {
            logger.LogWarning("Database does not exist, attempting to create it...");
            try
            {
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database created successfully");
                continue; // Retry the initialization
            }
            catch (Exception createEx)
            {
                logger.LogError(createEx, "Failed to create database");
            }
        }
        catch (Npgsql.NpgsqlException npgsqlEx)
        {
            logger.LogError(npgsqlEx, "PostgreSQL error on attempt {Attempt}: SqlState={ErrorCode}, Message={Message}", 
                attempt, npgsqlEx.SqlState, npgsqlEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed on attempt {Attempt}: {ErrorType} - {Message}",
                attempt, ex.GetType().Name, ex.Message);
        }
        finally
        {
            // The connection is reused across retries via the shared DbContext.
            // If an attempt fails after the connection was opened (e.g. a transient
            // SSL/stream timeout mid-migration), it is left open, and the next
            // attempt's OpenAsync throws "Connection already open" - masking the
            // real error and guaranteeing total failure. Close it here so each
            // retry starts from a clean state.
            try
            {
                var conn = context.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Closed)
                    await conn.CloseAsync();
            }
            catch
            {
                // Best-effort cleanup; ignore errors closing a broken connection.
            }
        }

        if (attempt == maxRetries)
        {
            logger.LogCritical("Database initialization failed after {MaxRetries} attempts", maxRetries);
            logger.LogError("Connection string: {ConnectionString}", 
                context.Database.GetConnectionString()?.Replace("Password=", "Password=***"));
            logger.LogError("Troubleshooting steps:");
            logger.LogError("1. Verify the database server is accessible");
            logger.LogError("2. Check firewall/network settings");
            logger.LogError("3. Confirm credentials are correct");
            logger.LogError("4. Ensure SSL/TLS settings match server requirements");
            logger.LogError("5. Check if connection pool is exhausted");
            
            throw new InvalidOperationException(
                $"Failed to initialize database after {maxRetries} attempts. Application cannot start. Check logs for details.");
        }
        
        // Backoff with jitter, but CAPPED at 1.5s so we keep retrying briskly through the Docker
        // port-proxy warm-up instead of sleeping for many seconds between the late attempts. With
        // baseDelay 400ms this gives ~0.4s, 0.8s, 1.5s, 1.5s... - the proxy is usually accepting by
        // the 4th-5th attempt, so the DB warms in a few seconds rather than ~15-20s.
        var delayMs = Math.Min(baseDelayMs * Math.Pow(2, attempt - 1), 1500) + Random.Shared.Next(0, 250);
        var delay = TimeSpan.FromMilliseconds(delayMs);
        logger.LogInformation("Waiting {DelayMs:F0}ms before retry attempt {NextAttempt}...",
            delay.TotalMilliseconds, attempt + 1);
        await Task.Delay(delay);
    }
}

// The server was already started with app.StartAsync() above so the port opens before the DB
// warm-up. Block here until shutdown instead of app.Run() (which would try to start it a second time).
Console.WriteLine("✓ API startup complete (database warmed).");
await app.WaitForShutdownAsync();
 