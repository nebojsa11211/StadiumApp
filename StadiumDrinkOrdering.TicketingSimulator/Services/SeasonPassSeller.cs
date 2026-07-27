using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Simulation;

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
    /// The people passes are drawn from. A pass holder needs an email as much as a single-match buyer
    /// does: the receiving side provisions a claimable account from it, and the per-match access tickets
    /// it materialises copy it — which is what lets a pass holder's drink orders be attributed to them
    /// rather than to an arbitrary account. Defined in <see cref="SimulatedFans"/> alongside the crowd
    /// the API seats, so the two share one address format and are guaranteed to be different people.
    /// </summary>
    private static readonly IReadOnlyList<SimulatedFanIdentity> FanPool = SimulatedFans.PassHolders;

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
            // Bulk-generated holders share one real mailbox, so a season's pass base would land a few
            // hundred activation invitations in it. The accounts are still created and claimable.
            SuppressActivationEmail = true,
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
