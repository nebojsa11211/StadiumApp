using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// The physical stadium / venue identity for the deployment. This is effectively a singleton
/// (one venue per installation) and gives meaning to the pre-existing <see cref="Event.VenueId"/>
/// column. It carries the branding surfaced to customers (stadium name, address, photo, contacts)
/// and owns the collection of <see cref="Club"/>s that are resident at the stadium.
/// </summary>
public class Venue
{
    public int Id { get; set; }

    /// <summary>Display name of the stadium (e.g. "Stadion Maksimir").</summary>
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Name of the club the stadium primarily belongs to, shown alongside the stadium identity.
    /// This is a single free-text label stored on the venue itself, distinct from the richer
    /// <see cref="Club"/> collection of resident clubs.
    /// </summary>
    [StringLength(150)]
    public string? ClubName { get; set; }

    /// <summary>Optional club logo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? ClubLogo { get; set; }

    [StringLength(100)]
    public string? ClubLogoContentType { get; set; }

    [StringLength(200)]
    public string? AddressLine1 { get; set; }

    [StringLength(200)]
    public string? AddressLine2 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    /// <summary>Optional geo coordinates for map links / directions.</summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>
    /// Officially stated capacity. Optional: the true seat count is computed from the stadium
    /// structure, but the official figure can legitimately differ (standing areas, closed sections),
    /// so it is stored separately rather than derived.
    /// </summary>
    public int? OfficialCapacity { get; set; }

    [StringLength(200)]
    public string? ContactEmail { get; set; }

    [StringLength(50)]
    public string? ContactPhone { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    /// <summary>Optional stadium photo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? Photo { get; set; }

    [StringLength(100)]
    public string? PhotoContentType { get; set; }

    /// <summary>
    /// Master switch for this installation's direct-to-customer ticket sales. When false the
    /// Customer app hides its buy flow and the cart/order API rejects new purchases, so tickets
    /// enter the system only via the external ticketing integration. Defaults to true.
    /// </summary>
    public bool TicketSalesEnabled { get; set; } = true;

    // ---- Accepted payment methods -----------------------------------------------------------
    // Which ways a fan may pay for drinks in the Customer app. Operators turn these off per
    // installation (e.g. a cashless stadium disables Cash, a venue without a card terminal at the
    // bar disables Card). The Customer cart hides a disabled option and the order API rejects it,
    // so a stale client cannot slip one through. At least one must stay enabled.

    /// <summary>Allow paying from the fan's HALFTIME wallet (personal or anonymous ticket wallet).</summary>
    public bool WalletPaymentEnabled { get; set; } = true;

    /// <summary>Allow card as the declared settle-at-the-bar method.</summary>
    public bool CardPaymentEnabled { get; set; } = true;

    /// <summary>Allow cash on delivery as the declared settle-on-delivery method.</summary>
    public bool CashPaymentEnabled { get; set; } = true;

    // ---- Outgoing email (SMTP) --------------------------------------------------------------
    // Configured at runtime from the Admin settings page rather than appsettings, so an operator
    // can point the installation at their own mail server without a redeploy. When
    // <see cref="EmailEnabled"/> is false (or the host is blank) the app logs emails instead of
    // sending them.

    /// <summary>Master switch: actually deliver transactional emails over SMTP. When false the
    /// activation/notification flow logs the message instead of sending it.</summary>
    public bool EmailEnabled { get; set; }

    [StringLength(200)]
    public string? SmtpHost { get; set; }

    public int SmtpPort { get; set; } = 587;

    [StringLength(200)]
    public string? SmtpUsername { get; set; }

    /// <summary>SMTP password. Stored as-is on the singleton row; never returned to the client
    /// (the settings API exposes only a <c>HasPassword</c> flag).</summary>
    [StringLength(500)]
    public string? SmtpPassword { get; set; }

    /// <summary>Use SSL/TLS for the SMTP connection. Defaults to true (STARTTLS on 587 / SSL on 465).</summary>
    public bool SmtpUseSsl { get; set; } = true;

    [StringLength(200)]
    public string? EmailFromAddress { get; set; }

    [StringLength(150)]
    public string? EmailFromName { get; set; }

    /// <summary>
    /// True once an admin has dismissed the first-run setup banner. Stored on the singleton venue
    /// row (rather than a separate settings table) since the venue already is this installation's
    /// singleton. The banner also stops showing on its own once setup is actually complete; this
    /// flag lets the admin silence it earlier.
    /// </summary>
    public bool SetupDismissed { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Email of the admin who last saved the profile (audit).</summary>
    [StringLength(200)]
    public string? UpdatedBy { get; set; }

    // Navigation
    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
}

/// <summary>
/// A club/team resident at the <see cref="Venue"/>. A stadium can host more than one (e.g. two
/// teams sharing a ground), hence a collection rather than fields on the venue. This is the club
/// <em>identity</em> (logo, colours, founding) and is distinct from the free-text
/// <see cref="Event.HomeTeam"/>/<see cref="Event.AwayTeam"/> labels used per fixture.
/// </summary>
public class Club
{
    public int Id { get; set; }

    public int VenueId { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Abbreviation shown where space is tight (e.g. "GNK").</summary>
    [StringLength(50)]
    public string? ShortName { get; set; }

    /// <summary>Club crest/logo, stored in-DB (PostgreSQL bytea) alongside its content type.</summary>
    public byte[]? Logo { get; set; }

    [StringLength(100)]
    public string? LogoContentType { get; set; }

    /// <summary>Brand colours as hex (e.g. "#1d4ed8"). Stored now; app theming is a later phase.</summary>
    [StringLength(16)]
    public string? PrimaryColor { get; set; }

    [StringLength(16)]
    public string? SecondaryColor { get; set; }

    public int? FoundedYear { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    /// <summary>Marks the principal home club when several share the venue.</summary>
    public bool IsPrimary { get; set; }

    /// <summary>Sort order for display in lists / headers.</summary>
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public virtual Venue? Venue { get; set; }
}
