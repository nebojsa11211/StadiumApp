using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        [Required]
        [StringLength(50)]
        public string Level { get; set; } = string.Empty; // Info, Warning, Error, Critical
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty; // UserAction, SystemError, Authentication, etc.
        
        [Required]
        [StringLength(500)]
        public string Action { get; set; } = string.Empty; // Login, Upload, ButtonClick, NavigateTo, Exception, etc.
        
        [StringLength(100)]
        public string? UserId { get; set; }
        
        [StringLength(100)]
        public string? UserEmail { get; set; }
        
        [StringLength(50)]
        public string? UserRole { get; set; }
        
        [StringLength(100)]
        public string? IPAddress { get; set; }
        
        [StringLength(500)]
        public string? UserAgent { get; set; }
        
        [StringLength(100)]
        public string? RequestPath { get; set; }
        
        [StringLength(20)]
        public string? HttpMethod { get; set; }
        
        [StringLength(1000)]
        public string? Message { get; set; }
        
        public string? Details { get; set; } // JSON for additional data
        
        [StringLength(200)]
        public string? ExceptionType { get; set; }
        
        public string? StackTrace { get; set; }
        
        [StringLength(200)]
        public string Source { get; set; } = string.Empty; // API, Admin, Customer, Staff
        
        // Business Event Fields
        [StringLength(100)]
        public string? BusinessEntityType { get; set; }
        
        [StringLength(100)]
        public string? BusinessEntityId { get; set; }
        
        [StringLength(200)]
        public string? BusinessEntityName { get; set; }
        
        [StringLength(100)]
        public string? RelatedEntityType { get; set; }
        
        [StringLength(100)]
        public string? RelatedEntityId { get; set; }
        
        public decimal? MonetaryAmount { get; set; }
        
        [StringLength(10)]
        public string? Currency { get; set; }
        
        public int? Quantity { get; set; }
        
        [StringLength(200)]
        public string? LocationInfo { get; set; }
        
        [StringLength(100)]
        public string? StatusBefore { get; set; }
        
        [StringLength(100)]
        public string? StatusAfter { get; set; }
        
        public string? MetadataJson { get; set; } // JSON string for flexible additional data
    }

    public enum CustomLogLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }

    public enum LogCategory
    {
        UserAction,
        SystemError,
        Authentication,
        Authorization,
        Database,
        Network,
        Performance,
        Security
    }
}