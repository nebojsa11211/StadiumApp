using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>Request to claim a shell account by setting its first password, using an emailed token.</summary>
public class ActivateAccountDto
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>What the set-password page shows the fan after validating their activation token — who the
/// account belongs to, so they know they're activating the right one.</summary>
public class ActivationInfoDto
{
    public bool Valid { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }

    /// <summary>Set when <see cref="Valid"/> is false: "NotFound", "Expired", "AlreadyUsed", "AlreadyActive".</summary>
    public string? Reason { get; set; }
}
