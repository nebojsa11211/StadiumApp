namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling;

public interface IThrottlingService
{
    bool IsThrottled(string key, TimeSpan throttleWindow = default);
    void SetThrottled(string key, TimeSpan throttleWindow = default);
    void ClearThrottled(string key);
    void ClearAllThrottled();
}