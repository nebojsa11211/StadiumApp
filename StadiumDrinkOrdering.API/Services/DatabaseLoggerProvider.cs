using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace StadiumDrinkOrdering.API.Services
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseLoggerConfiguration _configuration;
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new();

        public DatabaseLoggerProvider(IServiceProvider serviceProvider, DatabaseLoggerConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DatabaseLogger(name, _serviceProvider, _configuration));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }

    public class DatabaseLoggerConfiguration
    {
        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;
        public bool IsEnabled { get; set; } = true;
        public bool BatchingEnabled { get; set; } = true;
        public HashSet<string> ExcludeCategories { get; set; } = new()
        {
            "Microsoft.EntityFrameworkCore",
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Routing",
            "Microsoft.AspNetCore.Mvc",
            "Microsoft.AspNetCore.Authorization",
            "Microsoft.AspNetCore.Authentication",
            "Microsoft.AspNetCore.Server",
            "Microsoft.AspNetCore.StaticFiles",
            "Microsoft.AspNetCore.Cors",
            "Microsoft.AspNetCore.DataProtection",
            "System.Net.Http.HttpClient",
            "Microsoft.Extensions.Http",
            "StadiumDrinkOrdering.API.Services.DatabaseLogger" // Prevent recursive logging
        };
        public HashSet<string> CriticalCategories { get; set; } = new()
        {
            "Security",
            "Authentication",
            "Authorization",
            "Payment"
        };
    }

    public static class DatabaseLoggerExtensions
    {
        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, 
            Action<DatabaseLoggerConfiguration>? configure = null)
        {
            var configuration = new DatabaseLoggerConfiguration();
            configure?.Invoke(configuration);

            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton<ILoggerProvider, DatabaseLoggerProvider>();

            return builder;
        }
    }
}