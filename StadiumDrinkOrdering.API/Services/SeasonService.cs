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

        if (season == null)
            return null;

        var now = DateTime.UtcNow;

        // Next fixture straight from the season's own events (independent of the ticketing list's
        // IsActive filter): upcoming by date, or currently live even if its start time has passed.
        // Cancelled/completed matches are excluded.
        var next = await _context.Events
            .AsNoTracking()
            .Where(e => e.SeasonId == season.Id
                        && e.Status != EventStatus.Cancelled
                        && e.Status != EventStatus.Completed
                        && (e.EventDate >= now
                            || e.Status == EventStatus.Active
                            || e.Status == EventStatus.InProgress))
            .OrderBy(e => e.EventDate)
            .Select(e => new { e.Id, e.EventName, e.EventType, e.EventDate, e.TotalSeats, e.Status })
            .FirstOrDefaultAsync(ct);

        UpcomingEventDto? nextDto = null;
        if (next != null)
        {
            var sold = await _context.Tickets
                .CountAsync(t => t.EventId == next.Id && t.Status != TicketStatuses.Cancelled, ct);

            nextDto = new UpcomingEventDto
            {
                Id = next.Id,
                EventName = next.EventName,
                EventType = next.EventType,
                EventDate = next.EventDate,
                TotalSeats = next.TotalSeats,
                AvailableSeats = Math.Max(0, next.TotalSeats - sold),
                // "Live" only when the match is in a game-day state AND has actually kicked off —
                // guards against a fixture that was flipped Active ahead of its date being shown
                // as live while it is still days away.
                IsLive = EventLifecycle.CanOrderDrinks(next.Status) && next.EventDate <= now,
                Status = next.Status.ToString()
            };
        }

        return new CurrentSeasonDto
        {
            Id = season.Id,
            Name = season.Name,
            StartDate = season.StartDate,
            EndDate = season.EndDate,
            IsCurrent = season.IsCurrent,
            EventCount = await _context.Events.CountAsync(e => e.SeasonId == season.Id, ct),
            NextEvent = nextDto
        };
    }

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
