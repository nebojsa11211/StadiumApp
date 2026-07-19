namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Thrown when an event mutation is rejected because the season it belongs to (or is being moved
/// into) has ended and is therefore closed to schedule changes. Carries a user-facing message that
/// controllers surface verbatim as a 400 — see <see cref="Shared.Models.SeasonLifecycle"/>.
/// </summary>
public class SeasonClosedException : Exception
{
    public SeasonClosedException(string message) : base(message) { }
}
