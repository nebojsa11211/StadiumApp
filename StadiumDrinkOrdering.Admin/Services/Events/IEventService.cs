using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>?> GetEventsAsync();
        Task<object?> GetSeatStatusForEventAsync(int eventId);
        Task<bool> SimulateTicketSalesAsync(int eventId, int ticketCount);
    }
}