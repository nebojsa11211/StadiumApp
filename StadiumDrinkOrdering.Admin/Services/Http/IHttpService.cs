namespace StadiumDrinkOrdering.Admin.Services.Http
{
    public interface IHttpService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object? data = null);
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null);
        Task<(bool success, string errorMessage)> DeleteAsync(string endpoint);
    }
}