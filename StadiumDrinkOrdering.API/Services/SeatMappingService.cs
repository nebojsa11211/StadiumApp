using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public class SeatMappingService : ISeatMappingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SeatMappingService> _logger;

    public SeatMappingService(ApplicationDbContext context, ILogger<SeatMappingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Seat?> GetSeatFromStadiumSeatNewAsync(int stadiumSeatNewId)
    {
        try
        {
            var stadiumSeat = await _context.StadiumSeatsNew
                .Include(s => s.Sector)
                .ThenInclude(sec => sec.Ring)
                .ThenInclude(r => r.Tribune)
                .FirstOrDefaultAsync(s => s.Id == stadiumSeatNewId);

            if (stadiumSeat == null) return null;

            var mappingCode = GenerateSeatMappingCode(
                stadiumSeat.Sector.Ring.Tribune.Code,
                stadiumSeat.Sector.Ring.Number,
                stadiumSeat.Sector.Code,
                stadiumSeat.RowNumber,
                stadiumSeat.SeatNumber);

            return await _context.Seats
                .FirstOrDefaultAsync(s => s.SeatCode == mappingCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error mapping StadiumSeatNew {Id} to Seat", stadiumSeatNewId);
            return null;
        }
    }

    public async Task<StadiumSeatNew?> GetStadiumSeatNewFromSeatAsync(int seatId)
    {
        try
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == seatId);
            if (seat == null) return null;

            // Parse the seat code to get components (format: T1-R1-S1-R1-S1)
            var parts = seat.SeatCode.Split('-');
            if (parts.Length != 5) return null;

            var tribuneCode = parts[0];
            var ringNumber = int.Parse(parts[1].Substring(1)); // Remove 'R' prefix
            var sectorCode = parts[2];
            var rowNumber = int.Parse(parts[3].Substring(1)); // Remove 'R' prefix
            var seatNumber = int.Parse(parts[4].Substring(1)); // Remove 'S' prefix

            return await _context.StadiumSeatsNew
                .Include(s => s.Sector)
                .ThenInclude(sec => sec.Ring)
                .ThenInclude(r => r.Tribune)
                .FirstOrDefaultAsync(s => 
                    s.Sector.Ring.Tribune.Code == tribuneCode &&
                    s.Sector.Ring.Number == ringNumber &&
                    s.Sector.Code == sectorCode &&
                    s.RowNumber == rowNumber &&
                    s.SeatNumber == seatNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error mapping Seat {Id} to StadiumSeatNew", seatId);
            return null;
        }
    }

    public async Task<List<Ticket>> GetSoldTicketsForSectorAsync(int sectorId, int eventId)
    {
        try
        {
            // SIMPLIFIED: Get stadium seats for sector, then generate seat codes in memory
            var stadiumSeats = await _context.StadiumSeatsNew
                .Where(s => s.SectorId == sectorId)
                .Include(s => s.Sector)
                .ThenInclude(sec => sec.Ring)
                .ThenInclude(r => r.Tribune)
                .ToListAsync();

            var seatCodes = stadiumSeats.Select(s => GenerateSeatMappingCode(
                s.Sector.Ring.Tribune.Code,
                s.Sector.Ring.Number,
                s.Sector.Code,
                s.RowNumber,
                s.SeatNumber)).ToList();

            // Single query to get tickets by seat codes
            return await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Seat)
                .Where(t => t.EventId == eventId && seatCodes.Contains(t.Seat.SeatCode))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sold tickets for sector {SectorId} and event {EventId}", sectorId, eventId);
            return new List<Ticket>();
        }
    }

    public async Task<List<StadiumSeatNew>> GetSoldStadiumSeatsForEventAsync(int eventId)
    {
        try
        {
            // OPTIMIZED: Single query with joins to get all sold stadium seats
            var ticketSeatCodes = await _context.Tickets
                .Where(t => t.EventId == eventId)
                .Include(t => t.Seat)
                .Select(t => t.Seat.SeatCode)
                .ToListAsync();

            if (!ticketSeatCodes.Any())
                return new List<StadiumSeatNew>();

            // Parse seat codes and match to StadiumSeatNew in batch
            var soldStadiumSeats = new List<StadiumSeatNew>();
            
            foreach (var seatCode in ticketSeatCodes)
            {
                var parts = seatCode.Split('-');
                if (parts.Length == 5)
                {
                    var tribuneCode = parts[0].Substring(0, 1);  // Extract tribune code
                    var ringNumber = int.Parse(parts[0].Substring(1)); // Extract ring number
                    var sectorCode = parts[1];
                    var rowNumber = int.Parse(parts[2].Substring(1)); // Remove 'R' prefix
                    var seatNumber = int.Parse(parts[3].Substring(1)); // Remove 'S' prefix

                    var stadiumSeat = await _context.StadiumSeatsNew
                        .Include(s => s.Sector)
                        .ThenInclude(sec => sec.Ring)
                        .ThenInclude(r => r.Tribune)
                        .FirstOrDefaultAsync(s => 
                            s.Sector.Ring.Tribune.Code == tribuneCode &&
                            s.Sector.Ring.Number == ringNumber &&
                            s.Sector.Code == sectorCode &&
                            s.RowNumber == rowNumber &&
                            s.SeatNumber == seatNumber);
                    
                    if (stadiumSeat != null)
                    {
                        soldStadiumSeats.Add(stadiumSeat);
                    }
                }
            }

            return soldStadiumSeats;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sold stadium seats for event {EventId}", eventId);
            return new List<StadiumSeatNew>();
        }
    }

    public async Task<bool> GenerateSeatsForStadiumStructureAsync()
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Starting generation of Seat records for StadiumSeatNew records");

            // Get all StadiumSeatNew records with their related data
            var stadiumSeats = await _context.StadiumSeatsNew
                .Include(s => s.Sector)
                .ThenInclude(sec => sec.Ring)
                .ThenInclude(r => r.Tribune)
                .ToListAsync();

            var seatsToAdd = new List<Seat>();

            foreach (var stadiumSeat in stadiumSeats)
            {
                var mappingCode = GenerateSeatMappingCode(
                    stadiumSeat.Sector.Ring.Tribune.Code,
                    stadiumSeat.Sector.Ring.Number,
                    stadiumSeat.Sector.Code,
                    stadiumSeat.RowNumber,
                    stadiumSeat.SeatNumber);

                // Check if Seat already exists
                var existingSeat = await _context.Seats
                    .FirstOrDefaultAsync(s => s.SeatCode == mappingCode);

                if (existingSeat == null)
                {
                    // Create a new StadiumSection for this tribune/ring/sector combination if it doesn't exist
                    var sectionCode = $"{stadiumSeat.Sector.Ring.Tribune.Code}{stadiumSeat.Sector.Ring.Number}-{stadiumSeat.Sector.Code}";
                    var sectionName = $"{stadiumSeat.Sector.Ring.Tribune.Name} {stadiumSeat.Sector.Ring.Name} - {stadiumSeat.Sector.Name}";

                    var section = await _context.StadiumSections
                        .FirstOrDefaultAsync(s => s.SectionCode == sectionCode);

                    if (section == null)
                    {
                        section = new StadiumSection
                        {
                            SectionCode = sectionCode,
                            SectionName = sectionName,
                            TotalRows = stadiumSeat.Sector.TotalRows,
                            SeatsPerRow = stadiumSeat.Sector.SeatsPerRow,
                            PriceMultiplier = 1.0m,
                            Color = "#007bff"
                        };
                        _context.StadiumSections.Add(section);
                        await _context.SaveChangesAsync();
                    }

                    var newSeat = new Seat
                    {
                        SectionId = section.Id,
                        RowNumber = stadiumSeat.RowNumber,
                        SeatNumber = stadiumSeat.SeatNumber,
                        SeatCode = mappingCode,
                        XCoordinate = 0, // Default coordinates
                        YCoordinate = 0,
                        IsAccessible = false
                    };

                    seatsToAdd.Add(newSeat);
                }
            }

            if (seatsToAdd.Any())
            {
                _context.Seats.AddRange(seatsToAdd);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Generated {Count} new Seat records", seatsToAdd.Count);
            }
            else
            {
                _logger.LogInformation("No new Seat records needed to be generated");
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error generating Seat records for stadium structure");
            return false;
        }
    }

    public string GenerateSeatMappingCode(string tribuneCode, int ringNumber, string sectorCode, int rowNumber, int seatNumber)
    {
        return GenerateSeatMappingCodeStatic(tribuneCode, ringNumber, sectorCode, rowNumber, seatNumber);
    }

    public static string GenerateSeatMappingCodeStatic(string tribuneCode, int ringNumber, string sectorCode, int rowNumber, int seatNumber)
    {
        return $"{tribuneCode}{ringNumber}-{sectorCode}-R{rowNumber}-S{seatNumber}";
    }

    /// <summary>
    /// OPTIMIZED: Get seat availability for multiple sectors in a single operation
    /// </summary>
    public async Task<Dictionary<int, List<Ticket>>> GetSoldTicketsForMultipleSectorsAsync(List<int> sectorIds, int eventId)
    {
        try
        {
            var result = new Dictionary<int, List<Ticket>>();
            
            // SIMPLE FALLBACK: Use the existing single-sector method for each sector
            foreach (var sectorId in sectorIds)
            {
                var sectorTickets = await GetSoldTicketsForSectorAsync(sectorId, eventId);
                result[sectorId] = sectorTickets;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sold tickets for multiple sectors and event {EventId}", eventId);
            return new Dictionary<int, List<Ticket>>();
        }
    }

    /// <summary>
    /// OPTIMIZED: Get seat availability summary for all sections without loading detailed seat data
    /// </summary>
    public async Task<Dictionary<int, SeatAvailabilitySummary>> GetSectionAvailabilitySummaryAsync(int eventId)
    {
        try
        {
            // Get all sectors with their seat counts
            var sectorsWithCounts = await _context.Sectors
                .Include(s => s.Ring)
                .ThenInclude(r => r.Tribune)
                .Select(s => new
                {
                    SectorId = s.Id,
                    SectorCode = s.Code,
                    SectorName = s.Name,
                    TribuneName = s.Ring.Tribune.Name,
                    RingName = s.Ring.Name,
                    TotalSeats = s.TotalRows * s.SeatsPerRow,
                    TribuneCode = s.Ring.Tribune.Code,
                    RingNumber = s.Ring.Number
                })
                .ToListAsync();

            // Get sold ticket counts per sector in bulk
            var soldTicketsPerSector = await GetSoldTicketsForMultipleSectorsAsync(
                sectorsWithCounts.Select(s => s.SectorId).ToList(), 
                eventId);

            var result = new Dictionary<int, SeatAvailabilitySummary>();
            
            foreach (var sector in sectorsWithCounts)
            {
                var soldCount = soldTicketsPerSector.ContainsKey(sector.SectorId) 
                    ? soldTicketsPerSector[sector.SectorId].Count 
                    : 0;
                    
                result[sector.SectorId] = new SeatAvailabilitySummary
                {
                    SectorId = sector.SectorId,
                    SectorName = sector.SectorName,
                    TribuneName = sector.TribuneName,
                    RingName = sector.RingName,
                    TotalSeats = sector.TotalSeats,
                    SoldSeats = soldCount,
                    AvailableSeats = sector.TotalSeats - soldCount
                };
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting section availability summary for event {EventId}", eventId);
            return new Dictionary<int, SeatAvailabilitySummary>();
        }
    }
}

/// <summary>
/// Summary information for seat availability without loading individual seat details
/// </summary>
public class SeatAvailabilitySummary
{
    public int SectorId { get; set; }
    public string SectorName { get; set; } = string.Empty;
    public string TribuneName { get; set; } = string.Empty;
    public string RingName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int SoldSeats { get; set; }
    public int AvailableSeats { get; set; }
}