using StadiumDrinkOrdering.Shared.DTOs.Integration;

namespace StadiumDrinkOrdering.TicketingSimulator.Services;

/// <summary>
/// Sells simulated season passes into a season through the ordinary <c>SeasonTicketSold</c> webhook —
/// the same path a real ticketing feed uses, so a generated pass base behaves exactly like a sold one
/// (priced from the sector, provisioned a claimable account, extended to the season's fixtures).
///
/// Shared by the manual controls on the simulator home page and by the season generator, so the two
/// cannot drift apart. Deliberately not a bulk server-side insert: the point of the simulator is to
/// exercise the real ingestion path, and passes sold before any fixture exists are cheap anyway —
/// the receiving side's "materialize for every event already in the season" loop has nothing to do.
/// </summary>
public class SeasonPassSeller
{
    private readonly SimulatorApiClient _api;
    private readonly Random _rng = new();

    public SeasonPassSeller(SimulatorApiClient api) => _api = api;

    /// <summary>
    /// How many distinct people passes are drawn from. A pass holder needs an email as much as a
    /// single-match buyer does: the receiving side provisions a claimable account from it, and the
    /// per-match access tickets it materialises copy it — which is what lets a pass holder's drink
    /// orders be attributed to them rather than to an arbitrary account. Capped so a season's pass
    /// base doesn't mint an account per pass.
    /// </summary>
    private const int FanPoolSize = 200;

    /// <summary>One simulated pass holder: a stable identity, so the same people hold passes across runs.</summary>
    private sealed record Fan(string Name, string Email, string Oib);

    private static readonly string[] FirstNames =
    {
        "Ivan", "Marko", "Ana", "Petra", "Luka", "Sara", "Tomislav", "Maja", "Josip", "Ivana",
        "Filip", "Nikola", "Lucija", "Stjepan", "Marija", "Antonio", "Katarina", "Domagoj", "Nina", "Mislav"
    };

    private static readonly string[] LastNames =
    {
        "Horvat", "Kovac", "Babic", "Novak", "Maric", "Juric", "Knezevic", "Vukovic", "Peric", "Simic"
    };

    /// <summary>
    /// Built once and deterministically, so repeated runs sell to the same people and reuse their
    /// accounts instead of growing the customer list on every generation.
    /// </summary>
    private static readonly IReadOnlyList<Fan> FanPool = BuildFanPool();

    private static List<Fan> BuildFanPool()
    {
        var fans = new List<Fan>(FanPoolSize);
        foreach (var last in LastNames)
        {
            foreach (var first in FirstNames)
            {
                if (fans.Count >= FanPoolSize)
                    return fans;

                // Stable 11-digit OIB from the pool position, so a fan's pass and their account agree.
                // Offset away from the single-match crowd's range so the two pools are distinct people.
                var oib = (20_000_000_000L + fans.Count).ToString();
                fans.Add(new Fan(
                    $"{first} {last}",
                    $"{first}.{last}@pass.example.com".ToLowerInvariant(),
                    oib));
            }
        }
        return fans;
    }

    /// <summary>
    /// Outcome of one attempted pass sale, including what the receiving side did with it: how many
    /// per-match access tickets it materialised (one per fixture already in the season) and the id of
    /// the pass it created.
    /// </summary>
    public sealed record Sale(
        bool Accepted,
        bool Duplicate,
        string ExternalId,
        string? Message,
        int DerivedTicketsAffected = 0,
        int? SeasonTicketId = null)
    {
        /// <summary>A pass that actually came into existence (accepted and not a replayed duplicate).</summary>
        public bool IsNew => Accepted && !Duplicate;
    }

    /// <summary>Outcome of a batch: how many landed, how many didn't, and the ids of those that did.</summary>
    public sealed record Batch(int Sold, int Failed, List<string> ExternalIds, string? FirstError);

    /// <summary>
    /// Sells <paramref name="count"/> passes spread across the drawn sectors, weighted by capacity so
    /// the pass base looks like a real one rather than filling one end of the ground. Never throws on
    /// an individual rejection — a full sector shouldn't abandon the rest of the batch.
    /// <paramref name="onProgress"/> is invoked with the running total so a caller can show progress.
    /// </summary>
    public async Task<Batch> SellBatchAsync(
        string externalSeasonId,
        IReadOnlyList<StadiumSectionInfoDto> sections,
        int count,
        Func<int, Task>? onProgress = null,
        CancellationToken ct = default)
    {
        var ids = new List<string>();
        var failed = 0;
        string? firstError = null;

        if (sections.Count == 0 || count <= 0)
            return new Batch(0, 0, ids, null);

        for (var i = 0; i < count; i++)
        {
            ct.ThrowIfCancellationRequested();

            var sale = await SellOneAsync(externalSeasonId, sections, PickWeightedSector(sections).SectionCode, ct);
            if (sale.IsNew)
            {
                ids.Add(sale.ExternalId);
            }
            else
            {
                failed++;
                firstError ??= sale.Message;
            }

            // Report every so often rather than per pass: a few hundred sales would otherwise spend
            // more time re-rendering than selling.
            if (onProgress != null && i % 10 == 0)
                await onProgress(ids.Count);
        }

        if (onProgress != null)
            await onProgress(ids.Count);

        return new Batch(ids.Count, failed, ids, firstError);
    }

    /// <summary>Sells a single pass into a named sector, priced from that sector's configuration.</summary>
    public async Task<Sale> SellOneAsync(
        string externalSeasonId,
        IReadOnlyList<StadiumSectionInfoDto> sections,
        string sectorCode,
        CancellationToken ct = default)
    {
        var extId = "STK-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
        var fan = FanPool[_rng.Next(FanPool.Count)];
        var envelope = new TicketingWebhookEnvelope
        {
            EventType = TicketingEventTypes.SeasonTicketSold,
            IdempotencyKey = Guid.NewGuid().ToString(),
            OccurredAt = DateTime.UtcNow,
            SourceSystem = _api.SourceSystem,
            SeasonTicket = new ExternalSeasonTicketDto
            {
                ExternalSeasonTicketId = extId,
                ExternalSeasonId = externalSeasonId,
                SectionCode = sectorCode,
                // The venue configures a season-pass price per sector; quote it so the pass carries
                // real money. It is amortized across the season's matches on the receiving side, so
                // a pass sold at 0 would make every match it covers report no season takings.
                Price = PriceFor(sections, sectorCode),
                HolderName = fan.Name,
                // Supplying the email is what gets the holder a claimable account on the receiving side,
                // and it is copied onto every per-match access ticket the pass materialises — so their
                // match-day drink orders resolve to them instead of to an arbitrary customer.
                HolderEmail = fan.Email,
                HolderOib = fan.Oib,
                SoldAt = DateTime.UtcNow
            }
        };

        try
        {
            var result = await _api.SendAsync(envelope);
            return new Sale(result.Accepted, result.Duplicate, extId, result.Message,
                result.DerivedTicketsAffected, result.SeasonTicketId);
        }
        catch (Exception ex)
        {
            return new Sale(false, false, extId, ex.Message);
        }
    }

    /// <summary>
    /// What a season pass for this sector sells for: the price the venue configured on the sector
    /// (stadium drawing tool). Falls back to the sector's single-event price × a nominal 10-match
    /// season when no season price is set, so a pass never goes out worth nothing — and to 0 only
    /// when the sector carries no price at all.
    /// </summary>
    public static decimal PriceFor(IReadOnlyList<StadiumSectionInfoDto> sections, string sectorCode)
    {
        var sector = sections.FirstOrDefault(s => s.SectionCode == sectorCode);
        if (sector?.SeasonTicketPrice is decimal configured && configured > 0)
            return configured;
        if (sector?.Price is decimal single && single > 0)
            return decimal.Round(single * 10m, 2);
        return 0m;
    }

    /// <summary>Picks a sector at random, weighted by capacity, so bulk passes spread realistically.</summary>
    public StadiumSectionInfoDto PickWeightedSector(IReadOnlyList<StadiumSectionInfoDto> sections)
    {
        var total = sections.Sum(s => s.Capacity);
        if (total <= 0) return sections[_rng.Next(sections.Count)];

        var pick = _rng.Next(total);
        var acc = 0;
        foreach (var s in sections)
        {
            acc += s.Capacity;
            if (pick < acc) return s;
        }
        return sections[^1];
    }

}
