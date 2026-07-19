using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Manages the directory of visiting teams and their crests, so an opponent's badge is uploaded
/// once rather than re-attached to every fixture. Reads are public (crests appear on customer
/// fixture cards); all writes require an Admin.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    /// <summary>Crests are small logos, not photographs — well below the poster limit.</summary>
    private const long MaxLogoBytes = 2 * 1024 * 1024; // 2 MB

    private static readonly string[] AllowedImageTypes =
        { "image/png", "image/jpeg", "image/webp", "image/svg+xml" };

    private readonly ApplicationDbContext _context;
    private readonly ILogger<TeamsController> _logger;

    public TeamsController(ApplicationDbContext context, ILogger<TeamsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lists teams alphabetically. <paramref name="search"/> filters on name/short name so the
    /// event form's picker stays usable once the directory grows past a season or two.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<TeamDto>>> GetTeams([FromQuery] string? search = null)
    {
        var query = _context.Teams.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLowerInvariant();
            query = query.Where(t =>
                t.NormalizedName.Contains(term)
                || (t.ShortName != null && t.ShortName.ToLower().Contains(term)));
        }

        var teams = await query.OrderBy(t => t.Name).ToListAsync();
        return Ok(teams.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeamDto>> GetTeam(int id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (team == null) return NotFound();
        return Ok(ToDto(team));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] TeamUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var key = Team.Normalize(dto.Name);
        if (key == null)
            return BadRequest(new { message = "A team name is required." });

        if (await _context.Teams.AnyAsync(t => t.NormalizedName == key))
            return Conflict(new { message = $"A team named \"{dto.Name.Trim()}\" already exists." });

        var team = new Team { NormalizedName = key, CreatedAt = DateTime.UtcNow };
        Apply(team, dto);

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Team {TeamId} \"{TeamName}\" created", team.Id, team.Name);
        return Ok(ToDto(team));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<TeamDto>> UpdateTeam(int id, [FromBody] TeamUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();

        var key = Team.Normalize(dto.Name);
        if (key == null)
            return BadRequest(new { message = "A team name is required." });

        if (await _context.Teams.AnyAsync(t => t.NormalizedName == key && t.Id != id))
            return Conflict(new { message = $"Another team is already named \"{dto.Name.Trim()}\"." });

        var previousName = team.Name;
        team.NormalizedName = key;
        Apply(team, dto);
        team.UpdatedAt = DateTime.UtcNow;

        // Fixtures store the team name as a snapshot, so a rename here would otherwise leave events
        // showing the old spelling forever. Events linked by FK follow the rename; unlinked ones
        // (external imports that never resolved to a directory entry) are deliberately left alone.
        if (!string.Equals(previousName, team.Name, StringComparison.Ordinal))
        {
            var affected = await _context.Events.Where(e => e.AwayTeamId == id).ToListAsync();
            foreach (var ev in affected)
                ev.AwayTeam = team.Name;

            if (affected.Count > 0)
                _logger.LogInformation(
                    "Team {TeamId} renamed \"{OldName}\" -> \"{NewName}\"; updated {Count} fixture(s)",
                    id, previousName, team.Name, affected.Count);
        }

        await _context.SaveChangesAsync();
        return Ok(ToDto(team));
    }

    /// <summary>
    /// Removes a team from the directory. Fixtures that referenced it keep their team-name snapshot
    /// (the FK is SET NULL), so match history survives; only the reusable crest link is lost.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Team {TeamId} \"{TeamName}\" deleted", id, team.Name);
        return NoContent();
    }

    // ---- Crest ---------------------------------------------------------------------------

    [HttpPost("{id:int}/logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> UploadLogo(int id, IFormFile file)
    {
        var validation = ValidateImage(file);
        if (validation != null) return validation;

        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        team.Logo = ms.ToArray();
        team.LogoContentType = file.ContentType;
        team.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}/logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeleteLogo(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();

        team.Logo = null;
        team.LogoContentType = null;
        team.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id:int}/logo")]
    public async Task<IActionResult> GetLogo(int id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (team?.Logo == null || team.Logo.Length == 0)
            return NotFound();

        // Crests change only when an admin replaces one, so let clients cache briefly.
        Response.Headers.CacheControl = "public, max-age=300";
        return File(team.Logo, team.LogoContentType ?? "application/octet-stream");
    }

    // ---- Helpers -------------------------------------------------------------------------

    private static void Apply(Team team, TeamUpsertDto dto)
    {
        team.Name = dto.Name.Trim();
        team.ShortName = Blank(dto.ShortName);
        team.PrimaryColor = Blank(dto.PrimaryColor);
        team.SecondaryColor = Blank(dto.SecondaryColor);
        team.FoundedYear = dto.FoundedYear;
        team.City = Blank(dto.City);
        team.Website = Blank(dto.Website);
    }

    private static string? Blank(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private BadRequestObjectResult? ValidateImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });
        if (file.Length > MaxLogoBytes)
            return BadRequest(new { message = $"Crest exceeds the {MaxLogoBytes / 1024 / 1024} MB size limit." });
        if (!AllowedImageTypes.Contains(file.ContentType))
            return BadRequest(new { message = "Unsupported image type. Use PNG, JPEG, WebP or SVG." });
        return null;
    }

    private static TeamDto ToDto(Team t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        ShortName = t.ShortName,
        PrimaryColor = t.PrimaryColor,
        SecondaryColor = t.SecondaryColor,
        FoundedYear = t.FoundedYear,
        City = t.City,
        Website = t.Website,
        HasLogo = t.Logo != null && t.Logo.Length > 0,
        UpdatedAt = t.UpdatedAt
    };
}
