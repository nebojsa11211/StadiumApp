using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Options for a single call to the external image-generation API. Only <see cref="Prompt"/> is
/// required; width/height fall back to the API defaults (1024x1024) when left null.
/// </summary>
public class ImageGenerationRequest
{
    public string Prompt { get; set; } = string.Empty;

    /// <summary>Optional output width in pixels. Null lets the API apply its 1024 default.</summary>
    public int? Width { get; set; }

    /// <summary>Optional output height in pixels. Null lets the API apply its 1024 default.</summary>
    public int? Height { get; set; }

    /// <summary>
    /// Optional base64-encoded PNGs to edit/combine (the API's <c>ReferenceImages</c> field).
    /// Sent as raw base64 with no data-URI prefix.
    /// </summary>
    public List<string>? ReferenceImages { get; set; }
}

/// <summary>
/// The JSON shape returned by <c>POST /api/image/generate</c> when <c>ReturnAsHtml</c> is
/// false/omitted. Property names match the API's camelCase JSON.
/// </summary>
public class ImageGenerationResult
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    /// <summary>Raw base64 PNG, with no <c>data:image/png;base64,</c> prefix.</summary>
    [JsonPropertyName("base64Image")]
    public string Base64Image { get; set; } = string.Empty;

    /// <summary>Ready-to-use data URI for an <c>&lt;img src&gt;</c>.</summary>
    [JsonPropertyName("dataUri")]
    public string DataUri { get; set; } = string.Empty;

    /// <summary>Slugified filename suggestion, e.g. <c>a-red-fox-in-snow.png</c>.</summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;
}

/// <summary>A team crest reusable across fixtures, as returned by the crest lookup endpoint.</summary>
public class TeamCrestDto
{
    public string DisplayName { get; set; } = string.Empty;
    /// <summary>Raw base64 PNG, ready to pass straight to the image API as a reference image.</summary>
    public string ImageBase64 { get; set; } = string.Empty;
}

/// <summary>Request body for storing/updating a reusable team crest.</summary>
public class SaveTeamCrestDto
{
    public string TeamName { get; set; } = string.Empty;
    /// <summary>Raw base64 PNG (no data: prefix).</summary>
    public string ImageBase64 { get; set; } = string.Empty;
}
