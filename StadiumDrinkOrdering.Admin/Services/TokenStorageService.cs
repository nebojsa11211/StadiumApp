namespace StadiumDrinkOrdering.Admin.Services;

public interface ITokenStorageService
{
    string? Token { get; set; }
    void ClearToken();
}

public class TokenStorageService : ITokenStorageService
{
    public string? Token { get; set; }
    
    public void ClearToken()
    {
        Token = null;
    }
}