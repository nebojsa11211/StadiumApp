using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ISeatMappingService
{
    /// <summary>
    /// Maps a StadiumSeatNew to its corresponding Seat record
    /// </summary>
    Task<Seat?> GetSeatFromStadiumSeatNewAsync(int stadiumSeatNewId);
    
    /// <summary>
    /// Maps a Seat to its corresponding StadiumSeatNew record
    /// </summary>
    Task<StadiumSeatNew?> GetStadiumSeatNewFromSeatAsync(int seatId);
    
    /// <summary>
    /// Gets all tickets sold for seats within a specific sector
    /// </summary>
    Task<List<Ticket>> GetSoldTicketsForSectorAsync(int sectorId, int eventId);
    
    /// <summary>
    /// Gets all sold seats for a specific event as StadiumSeatNew records
    /// </summary>
    Task<List<StadiumSeatNew>> GetSoldStadiumSeatsForEventAsync(int eventId);
    
    /// <summary>
    /// Generates Seat records for all existing StadiumSeatNew records
    /// </summary>
    Task<bool> GenerateSeatsForStadiumStructureAsync();
    
    /// <summary>
    /// Creates a unique seat code for mapping purposes
    /// </summary>
    string GenerateSeatMappingCode(string tribuneCode, int ringNumber, string sectorCode, int rowNumber, int seatNumber);
    
    /// <summary>
    /// OPTIMIZED: Gets sold tickets for multiple sectors in a single operation
    /// </summary>
    Task<Dictionary<int, List<Ticket>>> GetSoldTicketsForMultipleSectorsAsync(List<int> sectorIds, int eventId);
    
    /// <summary>
    /// OPTIMIZED: Gets seat availability summary for all sections without loading detailed seat data
    /// </summary>
    Task<Dictionary<int, SeatAvailabilitySummary>> GetSectionAvailabilitySummaryAsync(int eventId);
}