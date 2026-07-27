using System.Text;

namespace StadiumDrinkOrdering.Shared.Simulation;

/// <summary>One simulated person: a stable identity, so the same fan recurs across fixtures and
/// seasons instead of a fresh account being minted per ticket.</summary>
public sealed record SimulatedFanIdentity(string Name, string Email, string Oib);

/// <summary>
/// The people simulated ticket sales are drawn from, defined once for the whole solution so the
/// crowd the API seats (<c>MatchSimulationService</c>) and the pass holders the simulator sells to
/// (<c>SeasonPassSeller</c>) cannot drift apart on address format or collide on identity.
///
/// Every fan is a plus-address on one real mailbox: mail to <c>owner+ivan_horvat@gmail.com</c> is
/// deliverable and lands in <c>owner@gmail.com</c>, so a simulated fan can be mailed and their
/// account claimed for real — unlike the <c>@example.com</c> placeholders these replaced. That also
/// means generated data is *deliverable*: paths that would mail a simulated fan in bulk should pass
/// <c>sendActivation: false</c> (see <c>TicketingWebhookEnvelope.SuppressActivationEmail</c>).
/// </summary>
public static class SimulatedFans
{
    /// <summary>The mailbox every simulated fan is a plus-tagged alias of.</summary>
    public const string MailboxLocalPart = "nebojsa.medancic";
    public const string MailboxDomain = "gmail.com";

    /// <summary>How many distinct people each pool holds. A whole season is thousands of tickets;
    /// drawing them from a handful of names makes every drink order in the ground belong to the same
    /// few accounts, which is useless for testing anything per-customer. Capped rather than unbounded
    /// so a generated season doesn't bury Admin → Customers under a new fan per ticket.</summary>
    public const int PoolSize = 200;

    private static readonly string[] FirstNames =
    {
        "Ivan", "Marko", "Ana", "Petra", "Luka", "Josip", "Marija", "Tomislav", "Ivana", "Filip",
        "Sara", "Nikola", "Maja", "Stjepan", "Lucija", "Antonio", "Katarina", "Domagoj", "Nina", "Mislav"
    };

    /// <summary>Surnames for the single-match crowd. Disjoint from <see cref="PassHolderLastNames"/>:
    /// the two pools must never produce the same full name, because the name is what the address is
    /// built from — one shared address would merge two people carrying different OIBs onto one
    /// account, and the second OIB would then provision a duplicate shell at the bar.</summary>
    private static readonly string[] CrowdLastNames =
    {
        "Horvat", "Kovacevic", "Babic", "Novak", "Maric", "Juric", "Vukovic", "Peric", "Simic", "Barisic"
    };

    /// <summary>Surnames for season-pass holders — see <see cref="CrowdLastNames"/> on why disjoint.</summary>
    private static readonly string[] PassHolderLastNames =
    {
        "Kovac", "Knezevic", "Matic", "Pavlovic", "Bozic", "Radic", "Grgic", "Tomic", "Blazevic", "Lovric"
    };

    /// <summary>
    /// The single-match crowd, built once and deterministically so a rerun reuses the same people
    /// (and therefore the same provisioned accounts) rather than doubling the customer list.
    /// </summary>
    public static readonly IReadOnlyList<SimulatedFanIdentity> Crowd =
        BuildPool(CrowdLastNames, oibBase: 10_000_000_000L);

    /// <summary>Season-pass holders. Their OIB range is offset from <see cref="Crowd"/>'s so the two
    /// pools stay recognisably different people even where the data is inspected by OIB alone.</summary>
    public static readonly IReadOnlyList<SimulatedFanIdentity> PassHolders =
        BuildPool(PassHolderLastNames, oibBase: 20_000_000_000L);

    private static List<SimulatedFanIdentity> BuildPool(string[] lastNames, long oibBase)
    {
        var fans = new List<SimulatedFanIdentity>(PoolSize);
        foreach (var last in lastNames)
        {
            foreach (var first in FirstNames)
            {
                if (fans.Count >= PoolSize)
                    return fans;

                var name = $"{first} {last}";
                // OIB derived from the position in the pool: 11 digits, stable per fan, so the fan's
                // ticket and their provisioned account always agree on who they are.
                fans.Add(new SimulatedFanIdentity(name, EmailFor(name), (oibBase + fans.Count).ToString()));
            }
        }
        return fans;
    }

    /// <summary>
    /// Picks a fan from <paramref name="pool"/> from an arbitrary key — an external ticket id, say —
    /// so the same key always yields the same person. Lets a caller re-derive who bought a ticket
    /// without storing it: a replayed sale carries the buyer the original sale did.
    /// </summary>
    public static SimulatedFanIdentity PickStable(IReadOnlyList<SimulatedFanIdentity> pool, string key)
    {
        // FNV-1a. Deliberately not string.GetHashCode(), which is randomised per process — the same
        // ticket id would then pick a different buyer after a restart, which is the whole point here.
        var hash = 2166136261u;
        foreach (var ch in key)
        {
            hash ^= ch;
            hash *= 16777619u;
        }
        return pool[(int)(hash % (uint)pool.Count)];
    }

    /// <summary>
    /// The deliverable address for a simulated fan: the shared mailbox plus-tagged with their name,
    /// e.g. "Ivan Horvat" → <c>nebojsa.medancic+ivan_horvat@gmail.com</c>. Spaces become underscores;
    /// Croatian diacritics are transliterated rather than dropped, so "Marić" and "Mari" don't collapse
    /// onto one address if the name lists ever grow beyond plain ASCII.
    /// </summary>
    public static string EmailFor(string fullName)
    {
        var tag = new StringBuilder(fullName.Length);
        foreach (var ch in fullName.Trim().ToLowerInvariant())
        {
            var mapped = ch switch
            {
                'č' or 'ć' => "c",
                'š' => "s",
                'ž' => "z",
                'đ' => "d",
                ' ' or '_' or '-' or '.' => "_",
                _ => char.IsAsciiLetterOrDigit(ch) ? ch.ToString() : string.Empty
            };

            // Collapse runs of separators, so "O'Brien-Maric" doesn't yield a doubled underscore.
            if (mapped == "_" && (tag.Length == 0 || tag[^1] == '_'))
                continue;

            tag.Append(mapped);
        }

        var slug = tag.ToString().Trim('_');
        return slug.Length == 0
            ? $"{MailboxLocalPart}@{MailboxDomain}"
            : $"{MailboxLocalPart}+{slug}@{MailboxDomain}";
    }
}
