using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// An admin-customized override of a transactional email template. Only <em>edited</em> templates are
/// stored here — the built-in default subject/body for each template lives in code
/// (<c>EmailTemplateCatalog</c>), so a missing row means "use the default" and resetting a template
/// simply deletes its row. The human-facing name, description and available placeholders come from the
/// catalog and are not stored.
/// </summary>
public class EmailTemplate
{
    public int Id { get; set; }

    /// <summary>Stable identifier matching a catalog entry (e.g. <c>account-activation</c>). Unique.</summary>
    [Required]
    [StringLength(100)]
    public string TemplateKey { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Subject { get; set; } = string.Empty;

    /// <summary>HTML body with <c>{{Placeholder}}</c> tokens. Unbounded text.</summary>
    [Required]
    public string HtmlBody { get; set; } = string.Empty;

    /// <summary>Optional plain-text alternative with the same tokens.</summary>
    public string? TextBody { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [StringLength(200)]
    public string? UpdatedBy { get; set; }
}
