using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}

public class RegisterDto
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Identity fields captured at sign-up so a fan can be verified at the bar (full name + OIB) before a
    // cash top-up. Optional server-side to stay backward-compatible with existing callers; the customer
    // registration form marks them required. Existing accounts fill these via the profile page.
    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    /// <summary>Croatian OIB — exactly 11 digits when supplied.</summary>
    [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB must be exactly 11 digits.")]
    public string? Oib { get; set; }
}

/// <summary>Fields a signed-in customer may edit on their own profile. All optional; a null field
/// leaves the stored value unchanged is NOT assumed — the profile page always submits the full set.</summary>
public class UpdateProfileDto
{
    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [Phone]
    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    /// <summary>Croatian OIB — exactly 11 digits when supplied.</summary>
    [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB must be exactly 11 digits.")]
    public string? Oib { get; set; }
}



