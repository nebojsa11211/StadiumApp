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
using Npgsql;

// Load variables from the gitignored .env file (if present) BEFORE the configuration builder is
// created, so values like ConnectionStrings__Supabase become available to configuration. Existing
// process/launch-profile environment variables are NOT overwritten (NoClobber semantics).
StadiumDrinkOrdering.Shared.Configuration.AppConfiguration.LoadDotEnvFile();

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

    // Add logging for SQL queries in development
    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
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
builder.Services.AddScoped<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<IPaymentService, StripePaymentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderSessionService, OrderSessionService>();
builder.Services.AddScoped<IDemoDataService, DemoDataService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IStadiumStructureService, StadiumStructureService>();
builder.Services.AddScoped<ISeatMappingService, SeatMappingService>();
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
            // Default origins for development (should be overridden in production)
            corsBuilder.WithOrigins(
                "https://localhost:7010", "https://localhost:7020", "https://localhost:7030", "https://localhost:7040",
                "https://admin:9030", "https://customer:9020", "https://staff:9040"
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

// Enhanced database migration with circuit breaker pattern.
// Run in the BACKGROUND so Kestrel starts listening immediately instead of blocking the
// port until migrations + BCrypt seeding finish. On a warm DB the init is a no-op anyway;
// on a cold/first boot the only exposure is a tiny window where the seeded admin/customer
// users aren't present yet (login may 401 for ~1-2s until init completes).
Console.WriteLine("Scheduling database initialization (non-blocking)...");
_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

    try
    {
        await InitializeDatabaseAsync(context, logger, environment);
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Background database initialization failed");
    }
});


// Database initialization method - FIXED VERSION
static async Task InitializeDatabaseAsync(ApplicationDbContext context, ILogger logger, IWebHostEnvironment environment)
{
    const int maxRetries = 5;
    const int baseDelayMs = 1000;
    
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
                        else
                        {
                            // Verify the hash actually works with the configured admin password
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

                        // Verify password hash works with "customer123"
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
        
        // Exponential backoff with jitter
        var delay = TimeSpan.FromMilliseconds(baseDelayMs * Math.Pow(2, attempt - 1) + Random.Shared.Next(0, 1000));
        logger.LogInformation("Waiting {DelayMs:F0}ms before retry attempt {NextAttempt}...", 
            delay.TotalMilliseconds, attempt + 1);
        await Task.Delay(delay);
    }
}

app.Run();
 