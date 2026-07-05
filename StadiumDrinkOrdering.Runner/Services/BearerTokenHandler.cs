using System.Net.Http.Headers;

namespace StadiumDrinkOrdering.Runner.Services;

/// <summary>Injects the current bearer token onto every API request.</summary>
public class BearerTokenHandler : DelegatingHandler
{
    private readonly RunnerAuthService _auth;

    public BearerTokenHandler(RunnerAuthService auth) => _auth = auth;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_auth.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.Token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
