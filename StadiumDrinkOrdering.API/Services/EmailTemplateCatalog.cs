namespace StadiumDrinkOrdering.API.Services;

/// <summary>A placeholder token available inside a template, with a sample value used for previews.</summary>
public sealed record EmailTemplatePlaceholder(string Token, string SampleValue);

/// <summary>The built-in definition of a transactional email template: its stable key, human-facing
/// name/description, the placeholder tokens it supports, and the default subject/body used until an
/// admin overrides it.</summary>
public sealed record EmailTemplateDefinition(
    string Key,
    string Name,
    string Description,
    IReadOnlyList<EmailTemplatePlaceholder> Placeholders,
    string Subject,
    string HtmlBody,
    string TextBody);

/// <summary>
/// The catalog of known transactional emails. The default content lives here (not in the database), so
/// a template with no admin override renders from its default and "reset" just removes the override.
/// Add a new email by registering it here and rendering it via <see cref="IEmailTemplateService"/>.
/// </summary>
public static class EmailTemplateCatalog
{
    /// <summary>Sent when a shell account is provisioned after a ticket purchase, inviting the holder to
    /// activate the account and set a password.</summary>
    public const string AccountActivation = "account-activation";

    public static readonly IReadOnlyList<EmailTemplateDefinition> All = new[]
    {
        new EmailTemplateDefinition(
            Key: AccountActivation,
            Name: "Account activation",
            Description: "Sent to a ticket holder whose account was created automatically, inviting them to activate it and set a password.",
            Placeholders: new[]
            {
                new EmailTemplatePlaceholder("Greeting", "Pozdrav Ivan Horvat"),
                new EmailTemplatePlaceholder("Name", "Ivan Horvat"),
                new EmailTemplatePlaceholder("ActivationLink", "https://localhost:7020/set-password?token=SAMPLE-TOKEN"),
                new EmailTemplatePlaceholder("ExpiryDays", "14"),
            },
            Subject: "Aktivirajte svoj račun — Stadium",
            HtmlBody:
                "<p>{{Greeting}},</p>" +
                "<p>Kupili ste ulaznicu i pripremili smo vam korisnički račun kako biste mogli dodati sredstva u svoj novčanik i naručivati na stadionu.</p>" +
                "<p>Aktivirajte račun i postavite lozinku klikom na poveznicu: <a href=\"{{ActivationLink}}\">Aktiviraj račun</a></p>" +
                "<p>Ili otvorite: {{ActivationLink}}</p>" +
                "<p>Poveznica vrijedi {{ExpiryDays}} dana.</p>",
            TextBody:
                "{{Greeting}},\n\n" +
                "Pripremili smo vam korisnički račun. Aktivirajte ga i postavite lozinku:\n{{ActivationLink}}\n\n" +
                "Poveznica vrijedi {{ExpiryDays}} dana."),
    };

    public static EmailTemplateDefinition? Find(string key) =>
        All.FirstOrDefault(t => string.Equals(t.Key, key, StringComparison.OrdinalIgnoreCase));
}
