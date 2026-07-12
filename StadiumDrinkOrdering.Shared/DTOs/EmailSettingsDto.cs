using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Admin-only view of the installation's outgoing-email (SMTP) configuration, surfaced on the
/// Admin settings page and persisted on the singleton <c>Venue</c> row. The SMTP password is
/// never returned to the client — reads expose only <see cref="HasPassword"/>, and a write leaves
/// the stored password untouched unless a new non-empty <see cref="Password"/> is supplied.
/// </summary>
public class EmailSettingsDto
{
    /// <summary>Master switch: actually deliver emails over SMTP. When false the app logs them instead.</summary>
    public bool EmailEnabled { get; set; }

    [StringLength(200)]
    public string? SmtpHost { get; set; }

    [Range(1, 65535)]
    public int SmtpPort { get; set; } = 587;

    [StringLength(200)]
    public string? SmtpUsername { get; set; }

    /// <summary>New password to store. Leave null/empty to keep the existing one. Never populated on read.</summary>
    [StringLength(500)]
    public string? Password { get; set; }

    /// <summary>Read-only: whether a password is currently stored. Ignored on write.</summary>
    public bool HasPassword { get; set; }

    public bool SmtpUseSsl { get; set; } = true;

    [StringLength(200)]
    public string? FromAddress { get; set; }

    [StringLength(150)]
    public string? FromName { get; set; }
}

/// <summary>Request to send a one-off test email using the (optionally unsaved) settings the admin is editing.</summary>
public class SendTestEmailDto
{
    /// <summary>The SMTP settings to test with. If <see cref="EmailSettingsDto.Password"/> is empty the
    /// currently-stored password is used, so the admin can test without re-entering it.</summary>
    public EmailSettingsDto Settings { get; set; } = new();

    /// <summary>Recipient of the test message.</summary>
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string ToEmail { get; set; } = string.Empty;
}

/// <summary>Result of a test-email attempt: whether delivery succeeded and, if not, why.</summary>
public class SendTestEmailResultDto
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}
