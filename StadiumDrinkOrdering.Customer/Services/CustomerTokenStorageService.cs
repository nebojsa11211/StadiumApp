namespace StadiumDrinkOrdering.Customer.Services;

public interface ICustomerTokenStorageService
{
    string? Token { get; set; }
    void ClearToken();
}

public class CustomerTokenStorageService : ICustomerTokenStorageService
{
    private readonly object _lock = new object();
    private string? _token;

    public string? Token 
    { 
        get 
        {
            lock (_lock)
            {
                return _token;
            }
        }
        set 
        {
            lock (_lock)
            {
                _token = value;
            }
        }
    }
    
    public void ClearToken()
    {
        lock (_lock)
        {
            _token = null;
        }
    }
}