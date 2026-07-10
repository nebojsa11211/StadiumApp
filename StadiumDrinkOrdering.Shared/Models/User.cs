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

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [Phone]
    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    /// <summary>Croatian personal identification number (OIB) — 11 digits. Optional; captured at
    /// registration or via the customer profile page. Shown to bar staff to confirm the fan's identity
    /// before loading cash onto their wallet at the counter.</summary>
    [StringLength(11)]
    public string? Oib { get; set; }

    /// <summary>Whether the account is enabled. Deactivated accounts remain in the system
    /// (for history) but are flagged inactive. Defaults to true.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// True for an auto-created <b>shell account</b>: one provisioned from a ticket's email so the fan can
    /// hold a wallet (and be topped up at the bar) before they've signed up. It has a random, unusable
    /// <see cref="PasswordHash"/> — nobody can log in — until the fan "claims" it by setting a password
    /// (via the emailed activation link or by registering with the same email), which flips this to false.
    /// </summary>
    public bool IsShellAccount { get; set; } = false;

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


