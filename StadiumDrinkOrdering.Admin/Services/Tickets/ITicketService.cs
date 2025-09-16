using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Tickets
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>?> GetTicketsAsync();
        Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId);
        Task<bool> ValidateTicketAsync(string ticketCode);
        Task<bool> UpdateTicketStatusAsync(int ticketId, bool isActive);
    }
}