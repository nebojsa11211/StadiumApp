using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UpdateUserDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50, MinimumLength = 2)]
    public string? FirstName { get; set; }

    [StringLength(50, MinimumLength = 2)]
    public string? LastName { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public UserRole? Role { get; set; }

    public bool? IsActive { get; set; }
}