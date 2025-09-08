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
}

public enum UserRole
{
    Customer = 1,
    Admin = 2,
    Bartender = 3,
    Waiter = 4
}


