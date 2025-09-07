using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Middleware;
using System.Text;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add health checks
builder.Services.AddHealthChecks();

// Debug: Print connection information
Console.WriteLine("=== DATABASE CONFIGURATION ===");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
Console.WriteLine($"Database Type: PostgreSQL (Supabase)");

// Get connection strings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var supabaseConnectionString = builder.Configuration.GetConnectionString("SupabaseConnection") ?? connectionString;

Console.WriteLine($"Connection String: {connectionString?.Substring(0, Math.Min(50, connectionString.Length ?? 0))}...");

// Supabase Configuration
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];
var supabaseServiceKey = builder.Configuration["Supabase:ServiceKey"];

if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
{
    Console.WriteLine("⚠️ WARNING: Supabase configuration is incomplete. Using direct PostgreSQL connection only.");
}
else
{
    Console.WriteLine($"Supabase URL: {supabaseUrl}");
    Console.WriteLine($"Supabase Key: {supabaseKey.Substring(0, Math.Min(20, supabaseKey.Length))}...");
}

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    try 
    {
        Console.WriteLine("Configuring PostgreSQL with Npgsql...");
        
        // Use PostgreSQL instead of SQLite
        options.UseNpgsql(supabaseConnectionString, npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(60); // 60 second timeout for complex queries
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });
        
        // Enable detailed logging for development
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine, LogLevel.Information);
        }
        
        Console.WriteLine("✅ PostgreSQL configuration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error configuring PostgreSQL: {ex.Message}");
        throw;
    }
});

// Supabase Client Configuration (Optional - for direct Supabase operations)
if (!string.IsNullOrEmpty(supabaseUrl) && !string.IsNullOrEmpty(supabaseKey))
{
    builder.Services.AddScoped<Supabase.Client>(provider =>
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false, // Disable if not using realtime features
            SessionHandler = new SupabaseSessionHandler(),
        };
        
        return new Supabase.Client(supabaseUrl, supabaseKey, options);
    });
}

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
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
        
        // Enhanced event handling for debugging
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"JWT Token validated for user: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStadiumStructureService, StadiumStructureService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISeatMappingService, SeatMappingService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ITicketAuthService, TicketAuthService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();
builder.Services.AddScoped<OrderSessionService>();
builder.Services.AddScoped<DemoDataService>();

// Background services
builder.Services.AddHostedService<LogRetentionBackgroundService>();
builder.Services.AddHostedService<TicketSessionCleanupService>();

// HTTP Client for external API calls (if needed)
builder.Services.AddHttpClient();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    
    Console.WriteLine("=== DEVELOPMENT MODE ===");
    Console.WriteLine("Swagger UI available at: /swagger");
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    Console.WriteLine("=== PRODUCTION MODE ===");
}

// Custom middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Health checks
app.UseHealthChecks("/health");

// API endpoints
app.MapControllers();

// SignalR hubs
app.MapHub<BartenderHub>("/bartenderHub");
app.MapHub<CustomerHub>("/customerHub");

// Database initialization and migration
await InitializeDatabaseAsync(app);

Console.WriteLine("=== APPLICATION STARTUP COMPLETE ===");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"Application started at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

app.Run();

// Database initialization method
async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        Console.WriteLine("=== DATABASE INITIALIZATION ===");
        
        // Test database connection
        Console.WriteLine("Testing database connection...");
        await context.Database.CanConnectAsync();
        Console.WriteLine("✅ Database connection successful");
        
        // Apply pending migrations
        Console.WriteLine("Checking for pending migrations...");
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        
        if (pendingMigrations.Any())
        {
            Console.WriteLine($"Found {pendingMigrations.Count()} pending migrations:");
            foreach (var migration in pendingMigrations)
            {
                Console.WriteLine($"  - {migration}");
            }
            
            Console.WriteLine("Applying migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Migrations applied successfully");
        }
        else
        {
            Console.WriteLine("✅ Database is up to date");
        }
        
        // Verify critical tables exist
        Console.WriteLine("Verifying database schema...");
        var tableNames = new[] { "users", "drinks", "orders", "tribunes", "stadium_seats", "events", "log_entries" };
        
        foreach (var tableName in tableNames)
        {
            try
            {
                var query = $"SELECT 1 FROM {tableName} LIMIT 1";
                await context.Database.ExecuteSqlRawAsync(query);
                Console.WriteLine($"✅ Table '{tableName}' exists and accessible");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Warning: Table '{tableName}' check failed: {ex.Message}");
            }
        }
        
        // Optional: Seed initial data
        await SeedInitialDataIfNeeded(context);
        
        Console.WriteLine("=== DATABASE INITIALIZATION COMPLETE ===");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Database initialization failed");
        Console.WriteLine($"❌ Database initialization failed: {ex.Message}");
        
        // In production, you might want to fail fast
        if (!app.Environment.IsDevelopment())
        {
            throw;
        }
    }
}

async Task SeedInitialDataIfNeeded(ApplicationDbContext context)
{
    try 
    {
        // Check if basic data exists
        var hasUsers = await context.Users.AnyAsync();
        var hasDrinks = await context.Drinks.AnyAsync();
        
        if (!hasUsers)
        {
            Console.WriteLine("No users found, seeding admin user...");
            // Note: In production, use proper password hashing
            var adminUser = new StadiumDrinkOrdering.Shared.Models.User
            {
                Username = "admin",
                Email = "admin@stadium.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Use proper hashing
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(adminUser);
        }
        
        if (!hasDrinks)
        {
            Console.WriteLine("No drinks found, seeding basic drinks...");
            var drinks = new[]
            {
                new StladiumDrinkOrdering.Shared.Models.Drink { Name = "Beer", Price = 5.50m, Category = "Alcoholic", VolumeML = 500, IsAvailable = true },
                new StladiumDrinkOrdering.Shared.Models.Drink { Name = "Coca Cola", Price = 3.00m, Category = "Soft Drink", VolumeML = 330, IsAvailable = true },
                new StladiumDrinkOrdering.Shared.Models.Drink { Name = "Water", Price = 2.00m, Category = "Water", VolumeML = 500, IsAvailable = true },
                new StladiumDrinkOrdering.Shared.Models.Drink { Name = "Coffee", Price = 3.50m, Category = "Hot Beverage", VolumeML = 250, IsAvailable = true }
            };
            
            context.Drinks.AddRange(drinks);
        }
        
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Initial data seeded successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Warning: Initial data seeding failed: {ex.Message}");
    }
}

// Custom session handler for Supabase
public class SupabaseSessionHandler : IGoTrueSessionPersistence<Supabase.Gotrue.Session>
{
    private Supabase.Gotrue.Session? _session;

    public void SaveSession(Supabase.Gotrue.Session session)
    {
        _session = session;
    }

    public void DestroySession()
    {
        _session = null;
    }

    public Supabase.Gotrue.Session? LoadSession()
    {
        return _session;
    }
}