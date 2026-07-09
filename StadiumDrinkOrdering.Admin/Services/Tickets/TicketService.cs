using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Tickets
{
    public class TicketService : BaseApiService, ITicketService
    {
        public TicketService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, tokenStorage: tokenStorage)
        {
        }

        public async Task<IEnumerable<TicketDto>?> GetTicketsAsync()
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync("tickets");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<IEnumerable<TicketDto>>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetTickets", "Failed to retrieve tickets list");
            }
            return Array.Empty<TicketDto>();
        }

        public async Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId)
        {
            try
            {
                SetAuthorizationHeader();
                var url = eventId.HasValue ? $"tickets?eventId={eventId.Value}" : "tickets";
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<IEnumerable<TicketDto>>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetTickets",
                    eventId.HasValue
                        ? $"Failed to retrieve tickets for event {eventId}"
                        : "Failed to retrieve tickets");
            }
            return null;
        }

        public async Task<bool> ValidateTicketAsync(string ticketCode)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.PostAsync($"tickets/validate/{ticketCode}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ValidateTicket", $"Failed to validate ticket {ticketCode}");
            }
            return false;
        }

        public async Task<bool> UpdateTicketStatusAsync(int ticketId, bool isActive)
        {
            try
            {
                SetAuthorizationHeader();
                var endpoint = isActive ? $"tickets/{ticketId}/activate" : $"tickets/{ticketId}/deactivate";
                var response = await HttpClient.PutAsync(endpoint, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "UpdateTicketStatus", $"Failed to update ticket {ticketId} status to {(isActive ? "active" : "inactive")}");
            }
            return false;
        }
    }
}