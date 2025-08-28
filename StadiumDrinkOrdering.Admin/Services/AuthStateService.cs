namespace StadiumDrinkOrdering.Admin.Services;

public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    event Action? OnAuthenticationStateChanged;
    void Login(string token);
    void Logout();
}

public class AuthStateService : IAuthStateService
{
    private readonly IAdminApiService _apiService;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_apiService.Token);

    public event Action? OnAuthenticationStateChanged;

    public AuthStateService(IAdminApiService apiService)
    {
        _apiService = apiService;
    }

    public void Login(string token)
    {
        _apiService.Token = token;
        OnAuthenticationStateChanged?.Invoke();
    }

    public void Logout()
    {
        _apiService.Token = null;
        OnAuthenticationStateChanged?.Invoke();
    }
}
