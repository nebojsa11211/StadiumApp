using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ISeasonService
{
    Task<List<SeasonDto>> GetSeasonsAsync(CancellationToken ct = default);
    Task<SeasonDto?> GetSeasonAsync(int id, CancellationToken ct = default);
    Task<SeasonDto> CreateSeasonAsync(CreateSeasonDto dto, CancellationToken ct = default);
    Task<SeasonDto?> UpdateSeasonAsync(int id, UpdateSeasonDto dto, CancellationToken ct = default);

    /// <summary>Returns null when not found, false when blocked (has season tickets), true when deleted.</summary>
    Task<(bool found, bool deleted)> DeleteSeasonAsync(int id, CancellationToken ct = default);
    Task<SeasonDto?> SetCurrentAsync(int id, CancellationToken ct = default);
    Task<Dictionary<int, string>> GetSeasonNamesAsync(IEnumerable<int> seasonIds, CancellationToken ct = default);

    /// <summary>
    /// The current season (or the most recent if none is flagged current) plus its next upcoming
    /// or live fixture, for the public mobile landing. Null when no season exists.
    /// </summary>
    Task<CurrentSeasonDto?> GetCurrentSeasonAsync(CancellationToken ct = default);

    /// <summary>Active season passes for a season with their fixed seat, ordered by sector/row/seat.</summary>
    Task<List<SeasonTicketDto>> GetSeasonTicketsAsync(int seasonId, CancellationToken ct = default);

    /// <summary>The scannable QR for a season pass (ensures a stable PassToken on first request).</summary>
    Task<SeasonPassQrDto?> GetPassQrAsync(int seasonTicketId, CancellationToken ct = default);
}

public class SeasonService : ISeasonService
{
    private readonly ApplicationDbContext _context;
    private readonly IQRCodeService _qrCode;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SeasonService> _logger;

    public SeasonService(ApplicationDbContext context, IQRCodeService qrCode, IConfiguration configuration, ILogger<SeasonService> logger)
    {
        _context = context;
        _qrCode = qrCode;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<SeasonDto>> GetSeasonsAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var seasons = await _context.Seasons
            .AsNoTracking()
            .OrderByDescending(s => s.StartDate)
            .Select(Project(now))
            .ToListAsync(ct);

        var stadiumSeatCount = await _context.Seats.CountAsync(ct);
        foreach (var s in seasons)
            s.StadiumSeatCount = stadiumSeatCount;

        await PopulateOrderDrinkStatsAsync(seasons, ct);
        return seasons;
    }

    public async Task<SeasonDto?> GetSeasonAsync(int id, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var season = await _context.Seasons
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(Project(now))
            .FirstOrDefaultAsync(ct);

        if (season != null)
        {
            season.StadiumSeatCount = await _context.Seats.CountAsync(ct);
            await PopulateOrderDrinkStatsAsync(new[] { season }, ct);
        }

        return season;
    }

    /// <summary>
    /// Fills in the drink-ordering activity (orders, revenue, drinks sold, best-seller) for each
    /// season by aggregating the orders placed at that season's events. Runs as a few grouped queries
    /// scoped to the given seasons rather than inflating the main projection with nested navigations.
    /// Cancelled orders are excluded so the figures reflect real (fulfilled/in-flight) sales.
    /// </summary>
    private async Task PopulateOrderDrinkStatsAsync(IReadOnlyCollection<SeasonDto> seasons, CancellationToken ct)
    {
        var ids = seasons.Select(s => s.Id).ToList();
        if (ids.Count == 0)
            return;

        // Order-level totals per season (count + gross value).
        var orderStats = await _context.Orders
            .Where(o => o.EventId != null && o.Event!.SeasonId != null
                        && ids.Contains(o.Event.SeasonId!.Value)
                        && o.Status != OrderStatus.Cancelled)
            .GroupBy(o => o.Event!.SeasonId!.Value)
            .Select(g => new { SeasonId = g.Key, OrderCount = g.Count(), Revenue = g.Sum(o => o.TotalAmount) })
            .ToDictionaryAsync(x => x.SeasonId, ct);

        // Units sold per season (sum of order-item quantities).
        var drinkQty = await _context.OrderItems
            .Where(oi => oi.Order.EventId != null && oi.Order.Event!.SeasonId != null
                         && ids.Contains(oi.Order.Event.SeasonId!.Value)
                         && oi.Order.Status != OrderStatus.Cancelled)
            .GroupBy(oi => oi.Order.Event!.SeasonId!.Value)
            .Select(g => new { SeasonId = g.Key, Units = g.Sum(oi => oi.Quantity) })
            .ToDictionaryAsync(x => x.SeasonId, x => x.Units, ct);

        // Best-selling drink per season: aggregate by (season, drink), then pick the top per season in memory.
        var perDrink = await _context.OrderItems
            .Where(oi => oi.Order.EventId != null && oi.Order.Event!.SeasonId != null
                         && ids.Contains(oi.Order.Event.SeasonId!.Value)
                         && oi.Order.Status != OrderStatus.Cancelled)
            .GroupBy(oi => new { SeasonId = oi.Order.Event!.SeasonId!.Value, oi.DrinkId, oi.Drink.Name })
            .Select(g => new { g.Key.SeasonId, g.Key.Name, Units = g.Sum(oi => oi.Quantity) })
            .ToListAsync(ct);

        var topDrinkBySeason = perDrink
            .GroupBy(x => x.SeasonId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Units).First());

        foreach (var s in seasons)
        {
            if (orderStats.TryGetValue(s.Id, out var os))
            {
                s.OrderCount = os.OrderCount;
                s.DrinksRevenue = os.Revenue;
            }
            s.DrinksSold = drinkQty.GetValueOrDefault(s.Id);
            if (topDrinkBySeason.TryGetValue(s.Id, out var top))
            {
                s.TopDrinkName = top.Name;
                s.TopDrinkQuantity = top.Units;
            }
        }
    }

    // Projection EF Core can translate to SQL (inline navigation counts/aggregates). Parameterised
    // by "now" so the event-phase splits (upcoming / live / completed) are evaluated in the query.
    private static System.Linq.Expressions.Expression<Func<Season, SeasonDto>> Project(DateTime now) => s => new SeasonDto
    {
        Id = s.Id,
        Name = s.Name,
        StartDate = s.StartDate,
        EndDate = s.EndDate,
        IsCurrent = s.IsCurrent,
        CreatedAt = s.CreatedAt,
        SourceSystem = s.SourceSystem,
        EventCount = s.Events.Count,
        SeasonTicketCount = s.SeasonTickets.Count(st => st.Status != TicketStatuses.Cancelled),

        // Revenue & holder mix — active passes only.
        // Cast to decimal? so an empty set yields SQL NULL → 0 rather than failing to map to decimal.
        PassRevenue = s.SeasonTickets.Where(st => st.Status != TicketStatuses.Cancelled).Sum(st => (decimal?)st.Price) ?? 0m,
        DistinctHolderCount = s.SeasonTickets
            .Where(st => st.Status != TicketStatuses.Cancelled && st.HolderEmail != null)
            .Select(st => st.HolderEmail).Distinct().Count(),
        LinkedHolderCount = s.SeasonTickets.Count(st => st.Status != TicketStatuses.Cancelled && st.UserId != null),
        ExternalPassCount = s.SeasonTickets.Count(st => st.Status != TicketStatuses.Cancelled && st.SourceSystem != null),

        // Event phase split. Completed is terminal; "live" mirrors Event.IsLiveAt (game-day status,
        // not yet ended); everything else non-cancelled is treated as upcoming.
        CompletedEventCount = s.Events.Count(e => e.Status == EventStatus.Completed),
        LiveEventCount = s.Events.Count(e =>
            e.Status == EventStatus.Active
            && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date)),
        UpcomingEventCount = s.Events.Count(e =>
            e.Status != EventStatus.Completed && e.Status != EventStatus.Cancelled
            && !(e.Status == EventStatus.Active
                 && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date)))
    };

    public async Task<SeasonDto> CreateSeasonAsync(CreateSeasonDto dto, CancellationToken ct = default)
    {
        var season = new Season
        {
            Name = dto.Name.Trim(),
            StartDate = EnsureUtc(dto.StartDate),
            EndDate = EnsureUtc(dto.EndDate),
            IsCurrent = dto.IsCurrent,
            CreatedAt = DateTime.UtcNow
        };
        _context.Seasons.Add(season);

        if (dto.IsCurrent)
            await ClearCurrentExceptAsync(season, ct);

        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Created season {Name} (ID: {Id})", season.Name, season.Id);
        return (await GetSeasonAsync(season.Id, ct))!;
    }

    public async Task<SeasonDto?> UpdateSeasonAsync(int id, UpdateSeasonDto dto, CancellationToken ct = default)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (season == null)
            return null;

        if (!string.IsNullOrWhiteSpace(dto.Name)) season.Name = dto.Name.Trim();
        if (dto.StartDate.HasValue) season.StartDate = EnsureUtc(dto.StartDate.Value);
        if (dto.EndDate.HasValue) season.EndDate = EnsureUtc(dto.EndDate.Value);
        if (dto.IsCurrent == true)
        {
            await ClearCurrentExceptAsync(season, ct);
            season.IsCurrent = true;
        }
        else if (dto.IsCurrent == false)
        {
            season.IsCurrent = false;
        }
        season.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return await GetSeasonAsync(id, ct);
    }

    public async Task<(bool found, bool deleted)> DeleteSeasonAsync(int id, CancellationToken ct = default)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (season == null)
            return (false, false);

        // Season tickets cascade-delete, but their derived match tickets are Restrict-linked, so a
        // delete would fail at the DB. Block deletion while any pass exists — the admin must refund
        // the passes first. Events simply unlink (FK is SET NULL).
        var hasPasses = await _context.SeasonTickets.AnyAsync(st => st.SeasonId == id, ct);
        if (hasPasses)
            return (true, false);

        _context.Seasons.Remove(season);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Deleted season {Name} (ID: {Id})", season.Name, id);
        return (true, true);
    }

    public async Task<SeasonDto?> SetCurrentAsync(int id, CancellationToken ct = default)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (season == null)
            return null;

        await ClearCurrentExceptAsync(season, ct);
        season.IsCurrent = true;
        season.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return await GetSeasonAsync(id, ct);
    }

    public async Task<CurrentSeasonDto?> GetCurrentSeasonAsync(CancellationToken ct = default)
    {
        // Prefer the flagged-current season; otherwise fall back to the most recently starting one.
        var season = await _context.Seasons
            .AsNoTracking()
            .OrderByDescending(s => s.IsCurrent)
            .ThenByDescending(s => s.StartDate)
            .FirstOrDefaultAsync(ct);

        var now = DateTime.UtcNow;

        // Upcoming fixtures: prefer the current season's own events; when the season has no upcoming/live
        // fixture of its own (e.g. events aren't linked to it yet, or it's between seasons) fall back
        // to the events across the whole stadium so the landing surfaces any published match instead of
        // sitting empty. Fixtures that aren't in the season are not badged with its name.
        var rows = season != null ? await QueryUpcomingEventsAsync(season.Id, now, ct) : new();
        var nextIsInSeason = rows.Count > 0;
        if (rows.Count == 0)
            rows = await QueryUpcomingEventsAsync(null, now, ct);

        // Nothing to show at all (no season configured and no events anywhere).
        if (season == null && rows.Count == 0)
            return null;

        // Sold counts for every listed fixture in a single grouped query (avoids N per-event counts).
        var soldByEvent = new Dictionary<int, int>();
        if (rows.Count > 0)
        {
            var ids = rows.Select(r => r.Id).ToList();
            soldByEvent = await _context.Tickets
                .Where(t => ids.Contains(t.EventId) && t.Status != TicketStatuses.Cancelled)
                .GroupBy(t => t.EventId)
                .Select(g => new { EventId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.EventId, x => x.Count, ct);
        }

        var upcoming = rows
            .Select(r => MapUpcoming(r, soldByEvent.GetValueOrDefault(r.Id), now))
            .ToList();

        return new CurrentSeasonDto
        {
            Id = season?.Id ?? 0,
            // Show the season name only when it's genuinely the context: the empty state, or a
            // fixture that belongs to the season. A cross-season fallback fixture must not be
            // mislabelled with the current season's name.
            Name = (rows.Count == 0 || nextIsInSeason) ? (season?.Name ?? string.Empty) : string.Empty,
            StartDate = season?.StartDate ?? default,
            EndDate = season?.EndDate ?? default,
            IsCurrent = season?.IsCurrent ?? false,
            EventCount = season != null
                ? await _context.Events.CountAsync(e => e.SeasonId == season.Id, ct)
                : 0,
            NextEvent = upcoming.FirstOrDefault(),
            UpcomingEvents = upcoming
        };
    }

    /// <summary>Maps a fixture row to its landing DTO, computing availability and live state.</summary>
    private static UpcomingEventDto MapUpcoming(NextEventRow e, int sold, DateTime now) =>
        new()
        {
            Id = e.Id,
            EventName = e.EventName,
            EventType = e.EventType,
            HomeTeam = e.HomeTeam,
            AwayTeam = e.AwayTeam,
            HomeClubId = e.HomeClubId,
            EventDate = e.EventDate,
            EventEndDate = e.EventEndDate,
            HasPoster = e.HasPoster,
            TotalSeats = e.TotalSeats,
            AvailableSeats = Math.Max(0, e.TotalSeats - sold),
            // "Live" only when the match is in a game-day state, has actually kicked off, AND has
            // not yet ended. The kick-off guard stops a fixture flipped Active ahead of its date
            // from showing as live days early; the end guard (closing at EventEndDate, or the end
            // of the start day when none is set) stops a finished match from staying "live" forever.
            IsLive = EventLifecycle.CanOrderDrinks(e.Status)
                     && e.EventDate <= now
                     && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date),
            Status = e.Status.ToString()
        };

    private sealed record NextEventRow(
        int Id, string EventName, string EventType, string? HomeTeam, string? AwayTeam,
        int? HomeClubId,
        DateTime EventDate, DateTime? EventEndDate, int TotalSeats, EventStatus Status,
        bool HasPoster);

    /// <summary>Cap on how many upcoming fixtures the landing strip will surface.</summary>
    private const int MaxUpcomingFixtures = 20;

    /// <summary>
    /// The upcoming (and currently live) events in chronological order, optionally scoped to a season.
    /// Pass <paramref name="seasonId"/> = null to search across every event. "Upcoming" is any non-terminal
    /// event whose start is still in the future; a started event qualifies only while it is genuinely
    /// live — in a game-day status and not yet ended (closing at EventEndDate, or the end of its start
    /// day when no end is set) — so a finished-but-stuck fixture never lingers in the list. The first
    /// entry is therefore the "next" fixture.
    /// </summary>
    private async Task<List<NextEventRow>> QueryUpcomingEventsAsync(int? seasonId, DateTime now, CancellationToken ct) =>
        await _context.Events
            .AsNoTracking()
            .Where(e => (seasonId == null || e.SeasonId == seasonId)
                        && e.Status != EventStatus.Cancelled
                        && e.Status != EventStatus.Completed
                        && (e.EventDate >= now
                            || (e.Status == EventStatus.Active
                                && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date))))
            .OrderBy(e => e.EventDate)
            .Take(MaxUpcomingFixtures)
            .Select(e => new NextEventRow(
                e.Id, e.EventName, e.EventType, e.HomeTeam, e.AwayTeam,
                e.HomeClubId,
                e.EventDate, e.EventEndDate, e.TotalSeats, e.Status,
                // Projected from metadata columns, so the poster blob is never loaded here.
                // Approved only: the artwork has the teams and kick-off rendered into it by the
                // image model, which occasionally misspells them, so unreviewed posters must not
                // reach fans — those fixtures fall back to the plain card.
                e.PosterContentType != null && e.PosterApprovedAt != null))
            .ToListAsync(ct);

    public async Task<Dictionary<int, string>> GetSeasonNamesAsync(IEnumerable<int> seasonIds, CancellationToken ct = default)
    {
        var ids = seasonIds.Distinct().ToList();
        if (ids.Count == 0)
            return new Dictionary<int, string>();

        return await _context.Seasons
            .AsNoTracking()
            .Where(s => ids.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.Name, ct);
    }

    public async Task<List<SeasonTicketDto>> GetSeasonTicketsAsync(int seasonId, CancellationToken ct = default)
    {
        return await _context.SeasonTickets
            .AsNoTracking()
            .Where(st => st.SeasonId == seasonId && st.Status != TicketStatuses.Cancelled)
            .OrderBy(st => st.Seat.Section.SectionCode)
            .ThenBy(st => st.Seat.RowNumber)
            .ThenBy(st => st.Seat.SeatNumber)
            .Select(st => new SeasonTicketDto
            {
                Id = st.Id,
                SeasonTicketNumber = st.SeasonTicketNumber,
                HolderName = st.HolderName,
                HolderEmail = st.HolderEmail,
                SectionCode = st.Seat.Section.SectionCode,
                RowNumber = st.Seat.RowNumber,
                SeatNumber = st.Seat.SeatNumber,
                SeatCode = st.Seat.SeatCode,
                Status = st.Status,
                PurchaseDate = st.PurchaseDate,
                SourceSystem = st.SourceSystem
            })
            .ToListAsync(ct);
    }

    public async Task<SeasonPassQrDto?> GetPassQrAsync(int seasonTicketId, CancellationToken ct = default)
    {
        var pass = await _context.SeasonTickets.FirstOrDefaultAsync(st => st.Id == seasonTicketId, ct);
        if (pass == null)
            return null;

        // Ensure a stable token exists (covers any pass created after the one-off backfill migration).
        if (string.IsNullOrEmpty(pass.PassToken))
        {
            pass.PassToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync(ct);
        }

        var baseUrl = (_configuration["CustomerApp:BaseUrl"] ?? "https://localhost:7020").TrimEnd('/');
        var url = $"{baseUrl}/t/{pass.PassToken}";
        var png = await _qrCode.GenerateQRCodeImageAsync(url);

        return new SeasonPassQrDto
        {
            SeasonTicketId = pass.Id,
            PassToken = pass.PassToken,
            Url = url,
            ImageDataUri = $"data:image/png;base64,{Convert.ToBase64String(png)}"
        };
    }

    private async Task ClearCurrentExceptAsync(Season season, CancellationToken ct)
    {
        var others = await _context.Seasons.Where(s => s.IsCurrent && s.Id != season.Id).ToListAsync(ct);
        foreach (var o in others)
            o.IsCurrent = false;
    }

    private static DateTime EnsureUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
