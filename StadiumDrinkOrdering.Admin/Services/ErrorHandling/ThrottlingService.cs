using System.Collections.Concurrent;

namespace StadiumDrinkOrdering.Admin.Services.ErrorHandling;

public class ThrottlingService : IThrottlingService
{
    private readonly ConcurrentDictionary<string, DateTime> _throttledItems = new();
    private readonly TimeSpan _defaultThrottleWindow = TimeSpan.FromSeconds(3);

    public bool IsThrottled(string key, TimeSpan throttleWindow = default)
    {
        if (throttleWindow == default)
            throttleWindow = _defaultThrottleWindow;

        if (_throttledItems.TryGetValue(key, out var lastTime))
        {
            if (DateTime.UtcNow - lastTime < throttleWindow)
            {
                return true; // Still throttled
            }
            else
            {
                // Throttle period expired, remove the entry
                _throttledItems.TryRemove(key, out _);
                return false;
            }
        }

        return false; // Not throttled
    }

    public void SetThrottled(string key, TimeSpan throttleWindow = default)
    {
        if (throttleWindow == default)
            throttleWindow = _defaultThrottleWindow;

        _throttledItems[key] = DateTime.UtcNow;
    }

    public void ClearThrottled(string key)
    {
        _throttledItems.TryRemove(key, out _);
    }

    public void ClearAllThrottled()
    {
        _throttledItems.Clear();
    }
}