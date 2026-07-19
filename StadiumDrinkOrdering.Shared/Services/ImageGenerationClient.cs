using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Shared.Services;

/// <summary>
/// Client for the external image-generation API (a separate service, not part of this solution).
///
/// Contract notes:
///  - The endpoint consumes a FORM POST (application/x-www-form-urlencoded), NOT a JSON body.
///  - We always send <c>ReturnAsHtml=false</c> so the API returns JSON we can deserialize.
///  - On generation failure the API never throws: it returns HTTP 200 with a placeholder 1x1 PNG.
///    Callers should check <see cref="ImageGenerationClient.IsPlaceholder"/> before trusting a result.
///
/// The base URL is configured once (see <c>AddImageGeneration</c>); do not hardcode it elsewhere.
/// </summary>
public interface IImageGenerationClient
{
    /// <summary>
    /// Generates a single image from a text prompt. Returns null only on transport/HTTP failure;
    /// a successful call may still be a placeholder (check <see cref="IImageGenerationClient.IsPlaceholder"/>).
    /// </summary>
    Task<ImageGenerationResult?> GenerateAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default);

    /// <summary>Convenience overload for the common prompt-only / prompt+size case.</summary>
    Task<ImageGenerationResult?> GenerateAsync(string prompt, int? width = null, int? height = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// True when the result is the API's 1x1 placeholder PNG returned on generation failure.
    /// Decoded from the actual PNG header rather than the echoed request width/height.
    /// </summary>
    bool IsPlaceholder(ImageGenerationResult result);

    /// <summary>Decodes the base64 PNG to raw bytes.</summary>
    byte[] GetPngBytes(ImageGenerationResult result);

    /// <summary>
    /// The image's true pixel dimensions, read from the PNG header. Prefer this over
    /// <see cref="ImageGenerationResult.Width"/>/<see cref="ImageGenerationResult.Height"/>, which
    /// only echo the requested size — the model may return a different aspect (e.g. 1344x768 for a
    /// requested 1920x1080). Returns (null, null) if the bytes can't be parsed as a PNG.
    /// </summary>
    (int? Width, int? Height) GetActualDimensions(ImageGenerationResult result);

    /// <summary>Saves the generated PNG to disk, returning the full path written.</summary>
    Task<string> SaveToFileAsync(ImageGenerationResult result, string filePath, CancellationToken cancellationToken = default);
}

public class ImageGenerationClient : IImageGenerationClient
{
    // Named HttpClient key; the base address + dev cert handling are configured in AddImageGeneration.
    public const string HttpClientName = "ImageGeneration";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ImageGenerationClient> _logger;

    public ImageGenerationClient(IHttpClientFactory httpClientFactory, ILogger<ImageGenerationClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task<ImageGenerationResult?> GenerateAsync(string prompt, int? width = null, int? height = null, CancellationToken cancellationToken = default)
        => GenerateAsync(new ImageGenerationRequest { Prompt = prompt, Width = width, Height = height }, cancellationToken);

    public async Task<ImageGenerationResult?> GenerateAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
            throw new ArgumentException("Prompt is required.", nameof(request));

        // Build the form-urlencoded body. ReturnAsHtml is omitted on purpose so we get JSON back.
        var fields = new List<KeyValuePair<string, string>>
        {
            new("Prompt", request.Prompt)
        };
        if (request.Width is int w)
            fields.Add(new("Width", w.ToString(CultureInfo.InvariantCulture)));
        if (request.Height is int h)
            fields.Add(new("Height", h.ToString(CultureInfo.InvariantCulture)));
        if (request.ReferenceImages is { Count: > 0 })
        {
            // ASP.NET model-binds a repeated field name into a string[].
            foreach (var img in request.ReferenceImages)
                fields.Add(new("ReferenceImages", img));
        }

        try
        {
            var client = _httpClientFactory.CreateClient(HttpClientName);
            using var content = new FormUrlEncodedContent(fields);
            using var response = await client.PostAsync("api/image/generate", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var body = await SafeReadAsync(response, cancellationToken);
                _logger.LogWarning("Image generation returned {StatusCode}: {Body}", (int)response.StatusCode, body);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<ImageGenerationResult>(cancellationToken: cancellationToken);
            if (result is null || string.IsNullOrEmpty(result.Base64Image))
            {
                _logger.LogWarning("Image generation returned an empty/unparseable body.");
                return null;
            }

            if (IsPlaceholder(result))
                _logger.LogWarning("Image generation returned a placeholder image (generation likely failed upstream).");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call image generation API for prompt '{Prompt}'", request.Prompt);
            return null;
        }
    }

    public bool IsPlaceholder(ImageGenerationResult result)
    {
        try
        {
            var (width, height) = ReadPngDimensions(GetPngBytes(result));
            return width <= 1 && height <= 1;
        }
        catch
        {
            // If we can't decode it, treat it as suspect rather than crashing the caller.
            return true;
        }
    }

    public byte[] GetPngBytes(ImageGenerationResult result)
        => Convert.FromBase64String(result.Base64Image);

    public (int? Width, int? Height) GetActualDimensions(ImageGenerationResult result)
    {
        try
        {
            var (width, height) = ReadPngDimensions(GetPngBytes(result));
            return (width, height);
        }
        catch
        {
            return (null, null);
        }
    }

    public async Task<string> SaveToFileAsync(ImageGenerationResult result, string filePath, CancellationToken cancellationToken = default)
    {
        var bytes = GetPngBytes(result);
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);
        return Path.GetFullPath(filePath);
    }

    private static async Task<string> SafeReadAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try { return await response.Content.ReadAsStringAsync(ct); }
        catch { return "<unreadable>"; }
    }

    /// <summary>
    /// Reads the pixel dimensions straight from the PNG IHDR chunk. A valid PNG is an 8-byte
    /// signature, a 4-byte chunk length, the "IHDR" tag, then width (4 bytes) and height (4 bytes),
    /// all big-endian.
    /// </summary>
    private static (int Width, int Height) ReadPngDimensions(byte[] png)
    {
        if (png.Length < 24)
            throw new ArgumentException("Not a valid PNG (too short).");

        int width = (png[16] << 24) | (png[17] << 16) | (png[18] << 8) | png[19];
        int height = (png[20] << 24) | (png[21] << 16) | (png[22] << 8) | png[23];
        return (width, height);
    }
}

public static class ImageGenerationServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IImageGenerationClient"/> and its named HttpClient.
    /// </summary>
    /// <param name="baseUrl">
    /// Base URL of the external image API (e.g. https://localhost:7285). Resolve this from
    /// configuration (ImageGeneration:BaseUrl) so it is not hardcoded in multiple places.
    /// </param>
    /// <param name="bypassDevCertValidation">
    /// When true, TLS certificate validation is disabled for this client only. Enable ONLY for
    /// local development against the API's self-signed dev cert; never in production. The clean
    /// alternative is to trust the cert once with <c>dotnet dev-certs https --trust</c>.
    /// </param>
    public static IServiceCollection AddImageGeneration(this IServiceCollection services, string baseUrl, bool bypassDevCertValidation)
    {
        var normalized = baseUrl.TrimEnd('/') + "/";

        var httpBuilder = services.AddHttpClient(ImageGenerationClient.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(normalized);
            // Image generation can take a while; give it room but still bound it.
            client.Timeout = TimeSpan.FromMinutes(3);
        });

        if (bypassDevCertValidation)
        {
            httpBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            });
        }

        services.AddScoped<IImageGenerationClient, ImageGenerationClient>();
        return services;
    }
}
