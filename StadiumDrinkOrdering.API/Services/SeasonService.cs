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

    /// <summary>Active season passes for a season with their fixed seat, ordered by sector/row/seat.</summary>
    Task<List<SeasonTicketDto>> GetSeasonTicketsAsync(int seasonId, CancellationToken ct = default);
}

public class SeasonService : ISeasonService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SeasonService> _logger;

    public SeasonService(ApplicationDbContext context, ILogger<SeasonService> logger)
    {
        _context = context;
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
