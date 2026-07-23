namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// First-run readiness of the installation, surfaced to the Admin app so it can nudge an operator
/// through the minimum configuration a fresh deployment needs before the system is usable. Every
/// flag is a cheap DB check; <see cref="IsComplete"/> is derived, and <see cref="Dismissed"/> lets
/// an admin silence the setup banner even while items remain outstanding.
/// </summary>
public class SetupStatusDto
{
    /// <summary>A stadium layout exists (imported tribunes/sectors or a drawn overlay) — the seat
    /// map everything else depends on.</summary>
    public bool HasStadiumStructure { get; set; }

    /// <summary>At least one drink category exists.</summary>
    public bool HasCategories { get; set; }

    /// <summary>At least one drink exists.</summary>
    public bool HasDrinks { get; set; }

    /// <summary>At least one active staff account (Bartender or Waiter) exists.</summary>
    public bool HasStaff { get; set; }

    /// <summary>The venue identity is filled in: a home club name plus an address.</summary>
    public bool HasVenueIdentity { get; set; }

    /// <summary>The admin chose to hide the setup banner. Persisted on the singleton venue row.</summary>
    public bool Dismissed { get; set; }

    /// <summary>True once every prerequisite is met.</summary>
    public bool IsComplete =>
        HasStadiumStructure && HasCategories && HasDrinks && HasStaff && HasVenueIdentity;
}
