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
        return await _context.Seasons
            .AsNoTracking()
            .OrderByDescending(s => s.StartDate)
            .Select(Project)
            .ToListAsync(ct);
    }

    public async Task<SeasonDto?> GetSeasonAsync(int id, CancellationToken ct = default)
    {
        return await _context.Seasons
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(Project)
            .FirstOrDefaultAsync(ct);
    }

    // Projection expression EF Core can translate to SQL (inline navigation counts).
    private static readonly System.Linq.Expressions.Expression<Func<Season, SeasonDto>> Project = s => new SeasonDto
    {
        Id = s.Id,
        Name = s.Name,
        StartDate = s.StartDate,
        EndDate = s.EndDate,
        IsCurrent = s.IsCurrent,
        CreatedAt = s.CreatedAt,
        SourceSystem = s.SourceSystem,
        EventCount = s.Events.Count,
        SeasonTicketCount = s.SeasonTickets.Count(st => st.Status != TicketStatuses.Cancelled)
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
            EventDate = e.EventDate,
            EventEndDate = e.EventEndDate,
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
        DateTime EventDate, DateTime? EventEndDate, int TotalSeats, EventStatus Status);

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
                            || ((e.Status == EventStatus.Active || e.Status == EventStatus.InProgress)
                                && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date))))
            .OrderBy(e => e.EventDate)
            .Take(MaxUpcomingFixtures)
            .Select(e => new NextEventRow(
                e.Id, e.EventName, e.EventType, e.HomeTeam, e.AwayTeam,
                e.EventDate, e.EventEndDate, e.TotalSeats, e.Status))
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
