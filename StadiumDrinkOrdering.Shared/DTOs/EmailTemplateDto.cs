using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>An email template as shown on the Admin email-templates page: the effective subject/body
/// (admin override if present, otherwise the built-in default) plus catalog metadata describing what
/// the template is and which <c>{{Placeholder}}</c> tokens it supports.</summary>
public class EmailTemplateDto
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>Placeholder tokens available in this template, without braces (e.g. "ActivationLink").</summary>
    public List<string> Placeholders { get; set; } = new();

    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; }

    /// <summary>True when an admin override is stored (i.e. it differs from the built-in default).</summary>
    public bool IsCustomized { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>Admin edit of a template's subject/body. Sent to <c>PUT api/email-templates/{key}</c>.</summary>
public class UpdateEmailTemplateDto
{
    [Required]
    [StringLength(300)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string HtmlBody { get; set; } = string.Empty;

    public string? TextBody { get; set; }
}

/// <summary>Renders the supplied (possibly unsaved) template content with sample placeholder values so
/// the admin can preview it before saving.</summary>
public class EmailTemplatePreviewResultDto
{
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; }
}
