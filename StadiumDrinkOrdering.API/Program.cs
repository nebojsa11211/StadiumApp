using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Middleware;
using System.Text;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// COMPREHENSIVE DEBUG: Print ALL connection string sources
Console.WriteLine("=== VISUAL STUDIO DEBUGGING - Configuration Sources ===");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
Console.WriteLine($"ConnectionStrings__DefaultConnection env var: {Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")}");

// Check all configuration sources
Console.WriteLine("\n=== Configuration Providers ===");
foreach (var provider in builder.Configuration.Sources)
{
    Console.WriteLine($"Provider: {provider.GetType().Name}");
}

// Check specific configuration values
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"\n=== Final Connection String ===");
Console.WriteLine($"Connection String: {connectionString}");

// Check if this is a SQLite connection string
if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Data Source="))
{
    Console.WriteLine("⚠️  WARNING: This appears to be a SQLite connection string!");
    Console.WriteLine("Expected: PostgreSQL connection string with Host=aws-1-eu-north-1.pooler.supabase.com");
    
    // Show all ConnectionStrings section
    Console.WriteLine("\n=== All ConnectionStrings Configuration ===");
    var connStringsSection = builder.Configuration.GetSection("ConnectionStrings");
    foreach (var item in connStringsSection.GetChildren())
    {
        Console.WriteLine($"{item.Key}: {item.Value}");
    }
}

// Check current working directory and appsettings files
Console.WriteLine($"\n=== File System Context ===");
Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"appsettings.json exists: {File.Exists("appsettings.json")}");
Console.WriteLine($"appsettings.Development.json exists: {File.Exists("appsettings.Development.json")}");

if (File.Exists("appsettings.json"))
{
    Console.WriteLine("\n=== appsettings.json Content ===");
    var content = File.ReadAllText("appsettings.json");
    if (content.Contains("Data Source="))
    {
        Console.WriteLine("⚠️  appsettings.json contains SQLite connection string!");
    }
    else
    {
        Console.WriteLine("✓ appsettings.json looks correct (no SQLite reference)");
    }
}

Console.WriteLine("=== END VISUAL STUDIO DEBUGGING ===\n");

// Add comprehensive health checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database", 
        failureStatus: HealthStatus.Unhealthy, 
        tags: new[] { "database", "postgresql", "supabase" })
    .AddNpgSql(connectionString!, 
        name: "postgresql-connection",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "database", "postgresql" });
Console.WriteLine($"Connection String: {connectionString}");

// Database Configuration with Enhanced PostgreSQL Support
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string is not configured. Please check your appsettings.json or environment variables.");
    }
    
    // Use PostgreSQL/Supabase - NO FALLBACK TO SQLITE
    Console.WriteLine("Configuring PostgreSQL/Supabase connection...");
    Console.WriteLine($"Using connection string: {connectionString.Replace("Password=d!hZ5A9@t+e!Nn2", "Password=***")}");
    
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        // Enhanced retry logic with exponential backoff
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(15),
            errorCodesToAdd: new[] { "57P01", "53300", "53400" } // Common Supabase connection errors
        );
        
        // Longer command timeout for complex operations
        npgsqlOptions.CommandTimeout(300);
        
        // PostgreSQL specific optimizations
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

// Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "StadiumDrinkOrdering",
            ValidAudience = jwtSettings["Audience"] ?? "StadiumDrinkOrdering",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

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
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ITicketAuthService, TicketAuthService>();
builder.Services.AddScoped<ICustomerAnalyticsService, CustomerAnalyticsService>();

// Add background services
builder.Services.AddHostedService<LogRetentionBackgroundService>();

// Background Services
builder.Services.AddHostedService<TicketSessionCleanupService>();

// Stripe Configuration
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:8080", "http://localhost:8081", "http://localhost:8082", "http://admin:8082", "http://customer:8081")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add SignalR hubs
app.MapHub<StadiumDrinkOrdering.API.Hubs.BartenderHub>("/bartenderHub");
app.MapHub<StadiumDrinkOrdering.API.Hubs.CustomerHub>("/customerHub");

// Add health check endpoint
app.MapHealthChecks("/health");

// Enhanced database migration with circuit breaker pattern
Console.WriteLine("Initializing database connection and migrations...");
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    await InitializeDatabaseAsync(context, logger);
}

// Database initialization method - FIXED VERSION
static async Task InitializeDatabaseAsync(ApplicationDbContext context, ILogger logger)
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
                
                // If no users exist, ensure admin user is created
                if (userCount == 0)
                {
                    logger.LogInformation("No users found, creating default admin user...");
                    var adminUser = new StadiumDrinkOrdering.Shared.Models.User
                    {
                        Username = "admin",
                        Email = "admin@stadium.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        Role = StadiumDrinkOrdering.Shared.Models.UserRole.Admin,
                        CreatedAt = DateTime.UtcNow
                    };
                    context.Users.Add(adminUser);
                    await context.SaveChangesAsync(cancellationToken);
                    logger.LogInformation("Default admin user created successfully");
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
 