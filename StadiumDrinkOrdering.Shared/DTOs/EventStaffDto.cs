using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// One row of the per-event staff-assignment editor: a staff member (Bartender/Waiter) together with
/// whether they are currently assigned to work the event. Used to render the "Staff" picker in the
/// Admin event modal, mirroring the sector-pricing editor.
/// </summary>
public class EventStaffMemberDto
{
    /// <summary>The staff user's id (User.Id).</summary>
    public int StaffId { get; set; }

    public string Username { get; set; } = string.Empty;

    /// <summary>First + last name if available; falls back to the username on the client.</summary>
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    /// <summary>The staff member's system role (Bartender or Waiter).</summary>
    public UserRole Role { get; set; }

    /// <summary>Display name of <see cref="Role"/> (e.g. "Bartender").</summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>Whether the underlying user account is enabled. Disabled accounts are shown muted.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>True when this staff member is assigned to work the event. Bound to the row's checkbox.</summary>
    public bool IsAssigned { get; set; }

    /// <summary>
    /// This member's function for the event — <see cref="EventStaffRoles.Runner"/> or
    /// <see cref="EventStaffRoles.Bartender"/>. Prefilled from the stored assignment when editing,
    /// otherwise defaulted from the system <see cref="Role"/> (Bartender→bar, Waiter→runner).
    /// </summary>
    public string EventRole { get; set; } = EventStaffRoles.Runner;

    /// <summary>Overlay-sector ids (StadiumSectorOverlay.Id) this member covers for the event.</summary>
    public List<int> SectorOverlayIds { get; set; } = new();
}

/// <summary>
/// A single staff assignment supplied by the Admin form when saving an event: the staff member, the
/// function they'll perform (Runner/Barman), and the overlay sectors they cover. Non-staff ids and
/// unknown sector ids are ignored server-side.
/// </summary>
public class EventStaffInputDto
{
    public int StaffId { get; set; }

    /// <summary>"Runner" or "Bartender". Normalized server-side; defaults from the member's system role.</summary>
    public string? EventRole { get; set; }

    /// <summary>Overlay-sector ids this member covers. Null/empty means no specific sectors.</summary>
    public List<int>? SectorOverlayIds { get; set; }
}
