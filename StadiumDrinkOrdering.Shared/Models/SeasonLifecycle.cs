namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Centralised rules for the season window, mirroring <see cref="EventLifecycle"/>. A season is
/// "closed" once its <see cref="Season.EndDate"/> has passed; from that point its schedule is frozen
/// and no event may be added to, edited in, or removed from it.
///
/// This is deliberately a *separate* freeze from <see cref="EventLifecycle.CanEdit"/>: that one asks
/// "has this individual event finished?", this one asks "has the whole season it belongs to finished?".
/// An event can be blocked by either.
/// </summary>
public static class SeasonLifecycle
{
    /// <summary>
    /// True once the season's end date has passed. Compared at whole-day UTC granularity so a season
    /// stays open for the entirety of its final day — matching the existing "finished season cannot be
    /// current" rule in SeasonsController.
    /// </summary>
    public static bool IsClosed(DateTime endDateUtc, DateTime? nowUtc = null) =>
        endDateUtc.Date < (nowUtc ?? DateTime.UtcNow).Date;

    /// <summary>Convenience overload for a loaded season. A null season is never closed (unscheduled events stay editable).</summary>
    public static bool IsClosed(Season? season, DateTime? nowUtc = null) =>
        season != null && IsClosed(season.EndDate, nowUtc);

    /// <summary>
    /// Reason surfaced in API responses when an event change is blocked by a closed season. English,
    /// like <see cref="EventLifecycle.EditBlockedReason"/>; the Admin UI localizes its own wording.
    /// The date is formatted invariantly so the server's ambient culture can't render an English
    /// sentence with a translated month name.
    /// </summary>
    public static string ChangeBlockedReason(string seasonName, DateTime endDateUtc) =>
        $"Season \"{seasonName}\" ended on " +
        endDateUtc.ToString("d MMM yyyy", System.Globalization.CultureInfo.InvariantCulture) +
        " and is closed. Events in a closed season can no longer be added, changed or removed. " +
        "To make changes, extend the season's end date first.";
}
