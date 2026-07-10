using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// A single-use secret emailed to the holder of an auto-created <b>shell account</b> so they can set a
/// password and "claim" it (see <see cref="User.IsShellAccount"/>). Distinct from <see cref="RefreshToken"/>
/// (JWT renewal) and from <c>SeasonTicket.PassToken</c> (a stable QR resolver token) — this one expires and
/// is consumed on use.
/// </summary>
[Table("AccountActivationTokens")]
public class AccountActivationToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(512)]
    public string Token { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    public bool IsValid => !IsUsed && ExpiresAt > DateTime.UtcNow;

    /// <summary>Creates an activation token with a cryptographically-secure value (default 14-day lifetime).</summary>
    public static AccountActivationToken Create(int userId, TimeSpan? lifetime = null)
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var bytes = new byte[48];
        rng.GetBytes(bytes);
        // URL-safe base64 so it can ride in a query string without encoding.
        var token = Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

        return new AccountActivationToken
        {
            Token = token,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromDays(14))
        };
    }
}
