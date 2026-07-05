using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    [JsonIgnore] // Ignore to prevent circular references in JSON serialization
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    /// <summary>The fan's stored-value wallet, if one has been created (eligibility gated on holding a
    /// season ticket). Null until first created.</summary>
    [JsonIgnore]
    public virtual Wallet? Wallet { get; set; }

    /// <summary>Season tickets linked to this account by the email-match linker.</summary>
    [JsonIgnore]
    public virtual ICollection<SeasonTicket> SeasonTickets { get; set; } = new List<SeasonTicket>();
}

public enum UserRole
{
    Customer = 1,
    Admin = 2,
    Bartender = 3,
    Waiter = 4
}


