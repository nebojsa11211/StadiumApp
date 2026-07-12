using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>The subject and rendered bodies of an email ready to hand to <see cref="IEmailSender"/>.</summary>
public sealed record RenderedEmail(string Subject, string HtmlBody, string? TextBody);

/// <summary>Reads/edits transactional email templates and renders them with placeholder values.
/// Effective content = admin override (DB) when present, otherwise the built-in default from
/// <see cref="EmailTemplateCatalog"/>.</summary>
public interface IEmailTemplateService
{
    Task<List<EmailTemplateDto>> GetAllAsync(CancellationToken ct = default);
    Task<EmailTemplateDto?> GetAsync(string key, CancellationToken ct = default);
    Task<EmailTemplateDto?> UpdateAsync(string key, UpdateEmailTemplateDto dto, string? updatedBy, CancellationToken ct = default);
    Task<EmailTemplateDto?> ResetAsync(string key, CancellationToken ct = default);

    /// <summary>Render arbitrary (possibly unsaved) content with the catalog's sample values, for preview.</summary>
    EmailTemplatePreviewResultDto? Preview(string key, string subject, string htmlBody, string? textBody);

    /// <summary>Render the effective template for <paramref name="key"/> with the given token values.
    /// Returns null only if the key is unknown.</summary>
    Task<RenderedEmail?> RenderAsync(string key, IReadOnlyDictionary<string, string?> tokens, CancellationToken ct = default);
}

public class EmailTemplateService : IEmailTemplateService
{
    // Matches {{Token}} with optional inner whitespace; token is letters/digits/underscore.
    private static readonly Regex TokenPattern = new(@"\{\{\s*(?<key>[A-Za-z0-9_]+)\s*\}\}", RegexOptions.Compiled);

    private readonly ApplicationDbContext _db;

    public EmailTemplateService(ApplicationDbContext db) => _db = db;

    public async Task<List<EmailTemplateDto>> GetAllAsync(CancellationToken ct = default)
    {
        var overrides = await _db.EmailTemplates.AsNoTracking().ToListAsync(ct);
        return EmailTemplateCatalog.All
            .Select(def => ToDto(def, overrides.FirstOrDefault(o =>
                string.Equals(o.TemplateKey, def.Key, StringComparison.OrdinalIgnoreCase))))
            .ToList();
    }

    public async Task<EmailTemplateDto?> GetAsync(string key, CancellationToken ct = default)
    {
        var def = EmailTemplateCatalog.Find(key);
        if (def is null) return null;
        var ovr = await FindOverrideAsync(def.Key, ct);
        return ToDto(def, ovr);
    }

    public async Task<EmailTemplateDto?> UpdateAsync(string key, UpdateEmailTemplateDto dto, string? updatedBy, CancellationToken ct = default)
    {
        var def = EmailTemplateCatalog.Find(key);
        if (def is null) return null;

        var ovr = await FindOverrideAsync(def.Key, ct, tracked: true);
        if (ovr is null)
        {
            ovr = new EmailTemplate { TemplateKey = def.Key };
            _db.EmailTemplates.Add(ovr);
        }

        ovr.Subject = dto.Subject.Trim();
        ovr.HtmlBody = dto.HtmlBody;
        ovr.TextBody = string.IsNullOrWhiteSpace(dto.TextBody) ? null : dto.TextBody;
        ovr.UpdatedAt = DateTime.UtcNow;
        ovr.UpdatedBy = updatedBy;

        await _db.SaveChangesAsync(ct);
        return ToDto(def, ovr);
    }

    public async Task<EmailTemplateDto?> ResetAsync(string key, CancellationToken ct = default)
    {
        var def = EmailTemplateCatalog.Find(key);
        if (def is null) return null;

        var ovr = await FindOverrideAsync(def.Key, ct, tracked: true);
        if (ovr is not null)
        {
            _db.EmailTemplates.Remove(ovr);
            await _db.SaveChangesAsync(ct);
        }
        return ToDto(def, null);
    }

    public EmailTemplatePreviewResultDto? Preview(string key, string subject, string htmlBody, string? textBody)
    {
        var def = EmailTemplateCatalog.Find(key);
        if (def is null) return null;

        var tokens = def.Placeholders.ToDictionary(p => p.Token, p => (string?)p.SampleValue, StringComparer.OrdinalIgnoreCase);
        return new EmailTemplatePreviewResultDto
        {
            Subject = Substitute(subject, tokens),
            HtmlBody = Substitute(htmlBody, tokens),
            TextBody = textBody is null ? null : Substitute(textBody, tokens)
        };
    }

    public async Task<RenderedEmail?> RenderAsync(string key, IReadOnlyDictionary<string, string?> tokens, CancellationToken ct = default)
    {
        var def = EmailTemplateCatalog.Find(key);
        if (def is null) return null;

        var ovr = await FindOverrideAsync(def.Key, ct);
        var subject = ovr?.Subject ?? def.Subject;
        var html = ovr?.HtmlBody ?? def.HtmlBody;
        var text = ovr?.TextBody ?? def.TextBody;

        var lookup = new Dictionary<string, string?>(tokens, StringComparer.OrdinalIgnoreCase);
        return new RenderedEmail(
            Substitute(subject, lookup),
            Substitute(html, lookup),
            text is null ? null : Substitute(text, lookup));
    }

    private Task<EmailTemplate?> FindOverrideAsync(string key, CancellationToken ct, bool tracked = false)
    {
        var query = tracked ? _db.EmailTemplates : _db.EmailTemplates.AsNoTracking();
        return query.FirstOrDefaultAsync(o => o.TemplateKey == key, ct);
    }

    /// <summary>Replace <c>{{Token}}</c> occurrences using <paramref name="tokens"/>. Unknown tokens are
    /// left intact so an author sees a typo verbatim rather than a silent blank.</summary>
    private static string Substitute(string template, IReadOnlyDictionary<string, string?> tokens)
    {
        if (string.IsNullOrEmpty(template)) return template ?? string.Empty;
        return TokenPattern.Replace(template, m =>
        {
            var name = m.Groups["key"].Value;
            return tokens.TryGetValue(name, out var value) ? value ?? string.Empty : m.Value;
        });
    }

    private static EmailTemplateDto ToDto(EmailTemplateDefinition def, EmailTemplate? ovr) => new()
    {
        Key = def.Key,
        Name = def.Name,
        Description = def.Description,
        Placeholders = def.Placeholders.Select(p => p.Token).ToList(),
        Subject = ovr?.Subject ?? def.Subject,
        HtmlBody = ovr?.HtmlBody ?? def.HtmlBody,
        TextBody = ovr?.TextBody ?? def.TextBody,
        IsCustomized = ovr is not null,
        UpdatedAt = ovr?.UpdatedAt,
        UpdatedBy = ovr?.UpdatedBy
    };
}
