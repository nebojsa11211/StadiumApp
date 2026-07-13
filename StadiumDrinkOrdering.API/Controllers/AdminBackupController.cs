using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Admin-only logical database backup and restore. Produces/consumes a single JSON snapshot of the
/// selected tables. Restore is a transactional "replace" of the selected tables. Used by the Settings
/// page's Backup &amp; Restore card. See <see cref="DatabaseBackupService"/> for the mechanics.
/// </summary>
[Route("api/admin/backup")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class AdminBackupController : ControllerBase
{
    // Backups/restores of a whole database can be large; allow generous request bodies here only.
    private const long MaxUploadBytes = 512L * 1024 * 1024; // 512 MB

    private readonly DatabaseBackupService _backup;
    private readonly ILogger<AdminBackupController> _logger;

    public AdminBackupController(DatabaseBackupService backup, ILogger<AdminBackupController> logger)
    {
        _backup = backup;
        _logger = logger;
    }

    /// <summary>Lists every backup-able table with grouping, defaults and live row counts.</summary>
    [HttpGet("tables")]
    public async Task<ActionResult<List<BackupTableInfoDto>>> GetTables(CancellationToken ct)
        => Ok(await _backup.GetTablesAsync(ct));

    /// <summary>Exports the selected tables (empty/absent = all) as a downloadable JSON backup file.</summary>
    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] List<string>? selectedKeys, CancellationToken ct)
    {
        var createdBy = User?.Identity?.Name ?? User?.FindFirst("email")?.Value;
        var bytes = await _backup.ExportAsync(selectedKeys, createdBy, ct);
        var fileName = $"stadium-backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";
        _logger.LogInformation("Admin exported a database backup ({Bytes} bytes) covering {Count} table(s).",
            bytes.Length, selectedKeys?.Count ?? -1);
        return File(bytes, "application/json", fileName);
    }

    /// <summary>Inspects an uploaded backup file and reports the tables/rows it contains (no changes).</summary>
    [HttpPost("import/preview")]
    [RequestSizeLimit(MaxUploadBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    public async Task<ActionResult<BackupPreviewDto>> Preview(IFormFile file, CancellationToken ct)
    {
        var parsed = await ReadFileAsync(file, ct);
        if (parsed is null)
            return BadRequest(new { message = "The uploaded file is not a valid Stadium backup." });
        return Ok(_backup.Preview(parsed));
    }

    /// <summary>Restores the selected tables from the uploaded backup file (transactional replace).</summary>
    [HttpPost("import")]
    [RequestSizeLimit(MaxUploadBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    public async Task<ActionResult<RestoreResultDto>> Import(
        IFormFile file, [FromForm] string selectedKeys, CancellationToken ct)
    {
        var parsed = await ReadFileAsync(file, ct);
        if (parsed is null)
            return BadRequest(new { message = "The uploaded file is not a valid Stadium backup." });

        List<string> keys;
        try
        {
            keys = JsonSerializer.Deserialize<List<string>>(selectedKeys ?? "[]") ?? new();
        }
        catch (JsonException)
        {
            return BadRequest(new { message = "Invalid table selection." });
        }

        var result = await _backup.RestoreAsync(parsed, keys, ct);
        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    private static async Task<BackupFileDto?> ReadFileAsync(IFormFile? file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return null;
        try
        {
            await using var stream = file.OpenReadStream();
            var parsed = await JsonSerializer.DeserializeAsync<BackupFileDto>(stream, cancellationToken: ct);
            // A real backup always has the metadata + tables envelope.
            if (parsed?.Tables is null || parsed.Meta is null)
                return null;
            return parsed;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
