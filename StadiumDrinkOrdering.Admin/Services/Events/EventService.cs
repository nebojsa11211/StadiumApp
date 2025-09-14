using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Events
{
    public class EventService : BaseApiService, IEventService
    {
        public EventService(HttpClient httpClient, ICentralizedLoggingClient loggingClient)
            : base(httpClient, loggingClient)
        {
        }

        public async Task<IEnumerable<EventDto>?> GetEventsAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("api/events");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<IEnumerable<EventDto>>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetEvents", "Failed to retrieve events");
            }
            return Array.Empty<EventDto>();
        }

        public async Task<object?> GetSeatStatusForEventAsync(int eventId)
        {
            try
            {
                var response = await HttpClient.GetAsync($"api/events/{eventId}/seat-status");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<object>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetSeatStatusForEvent",
                    $"Failed to retrieve seat status for event {eventId}");
            }
            return null;
        }

        public async Task<bool> SimulateTicketSalesAsync(int eventId, int ticketCount)
        {
            try
            {
                var simulationData = new { EventId = eventId, TicketCount = ticketCount };
                var content = CreateJsonContent(simulationData);
                var response = await HttpClient.PostAsync($"api/events/{eventId}/simulate-sales", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "SimulateTicketSales",
                    $"Failed to simulate ticket sales for event {eventId}");
            }
            return false;
        }
    }
}