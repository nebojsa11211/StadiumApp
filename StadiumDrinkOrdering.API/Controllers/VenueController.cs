using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Manages the venue branding profile (stadium identity + resident clubs). Reads are public so the
/// customer/staff apps can surface branding; all writes require an Admin.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VenueController : ControllerBase
{
    private const long MaxImageBytes = 4 * 1024 * 1024; // 4 MB
    private static readonly string[] AllowedImageTypes =
        { "image/png", "image/jpeg", "image/webp", "image/svg+xml" };

    private readonly ApplicationDbContext _context;
    private readonly ILogger<VenueController> _logger;

    public VenueController(ApplicationDbContext context, ILogger<VenueController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ---- Venue profile -------------------------------------------------------------------

    [HttpGet]
    public async Task<ActionResult<VenueDto>> GetVenue()
    {
        var venue = await GetOrCreateVenueAsync();
        return Ok(ToDto(venue));
    }

    [HttpPut]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<VenueDto>> UpdateVenue([FromBody] UpdateVenueDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var venue = await GetOrCreateVenueAsync();

        venue.Name = dto.Name.Trim();
        venue.ClubName = dto.ClubName?.Trim();
        venue.AddressLine1 = dto.AddressLine1;
        venue.AddressLine2 = dto.AddressLine2;
        venue.City = dto.City;
        venue.PostalCode = dto.PostalCode;
        venue.Country = dto.Country;
        venue.Latitude = dto.Latitude;
        venue.Longitude = dto.Longitude;
        venue.OfficialCapacity = dto.OfficialCapacity;
        venue.ContactEmail = dto.ContactEmail;
        venue.ContactPhone = dto.ContactPhone;
        venue.Website = dto.Website;
        venue.UpdatedAt = DateTime.UtcNow;
        venue.UpdatedBy = User.FindFirst(ClaimTypes.Email)?.Value;

        await _context.SaveChangesAsync();
        return Ok(ToDto(venue));
    }

    // ---- App settings --------------------------------------------------------------------

    /// <summary>
    /// Public read of the installation settings so the Customer app can decide whether to show
    /// its ticket buy flow. Only the non-sensitive settings are exposed here.
    /// </summary>
    [HttpGet("settings")]
    public async Task<ActionResult<AppSettingsDto>> GetSettings()
    {
        var venue = await GetOrCreateVenueAsync();
        return Ok(new AppSettingsDto { TicketSalesEnabled = venue.TicketSalesEnabled });
    }

    [HttpPut("settings")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<AppSettingsDto>> UpdateSettings([FromBody] AppSettingsDto dto)
    {
        var venue = await GetOrCreateVenueAsync();
        venue.TicketSalesEnabled = dto.TicketSalesEnabled;
        venue.UpdatedAt = DateTime.UtcNow;
        venue.UpdatedBy = User.FindFirst(ClaimTypes.Email)?.Value;

        await _context.SaveChangesAsync();
        return Ok(new AppSettingsDto { TicketSalesEnabled = venue.TicketSalesEnabled });
    }

    // ---- Email (SMTP) settings -----------------------------------------------------------

    /// <summary>Read the installation's outgoing-email configuration. Admin-only; the SMTP password
    /// is never returned (only <see cref="EmailSettingsDto.HasPassword"/>).</summary>
    [HttpGet("email-settings")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<EmailSettingsDto>> GetEmailSettings()
    {
        var venue = await GetOrCreateVenueAsync();
        return Ok(ToEmailDto(venue));
    }

    [HttpPut("email-settings")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<EmailSettingsDto>> UpdateEmailSettings([FromBody] EmailSettingsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var venue = await GetOrCreateVenueAsync();
        ApplyEmailSettings(venue, dto);
        venue.UpdatedAt = DateTime.UtcNow;
        venue.UpdatedBy = User.FindFirst(ClaimTypes.Email)?.Value;

        await _context.SaveChangesAsync();
        return Ok(ToEmailDto(venue));
    }

    /// <summary>Send a one-off test email using the supplied (possibly unsaved) settings, so an admin
    /// can validate SMTP config before/without saving. If the password field is blank the currently
    /// stored password is used. Never throws — failures are returned as a structured result.</summary>
    [HttpPost("email-settings/test")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<SendTestEmailResultDto>> SendTestEmail([FromBody] SendTestEmailDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var s = request.Settings;
        if (string.IsNullOrWhiteSpace(s.SmtpHost))
            return Ok(new SendTestEmailResultDto { Success = false, Error = "SMTP host is not set." });

        // Fall back to the stored password when the admin didn't re-enter it.
        var password = s.Password;
        if (string.IsNullOrEmpty(password))
        {
            var venue = await GetOrCreateVenueAsync();
            password = venue.SmtpPassword;
        }

        // Explicit test → deliver even to demo domains (no skip list).
        var cfg = new ResolvedEmailConfig(
            Host: s.SmtpHost.Trim(),
            Port: s.SmtpPort,
            Username: s.SmtpUsername,
            Password: password,
            EnableSsl: s.SmtpUseSsl,
            FromAddress: string.IsNullOrWhiteSpace(s.FromAddress) ? "no-reply@stadium.local" : s.FromAddress.Trim(),
            FromName: string.IsNullOrWhiteSpace(s.FromName) ? "Stadium" : s.FromName.Trim(),
            SkipDomains: Array.Empty<string>());

        try
        {
            await SmtpMailer.SendAsync(
                cfg,
                request.ToEmail.Trim(),
                "Stadium — test email",
                "<p>This is a test email from the Stadium admin settings. If you received it, your SMTP configuration works.</p>",
                "This is a test email from the Stadium admin settings. If you received it, your SMTP configuration works.",
                _logger);

            return Ok(new SendTestEmailResultDto { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Test email to {To} failed", request.ToEmail);
            return Ok(new SendTestEmailResultDto { Success = false, Error = ex.Message });
        }
    }

    [HttpPost("photo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        var validation = ValidateImage(file);
        if (validation != null) return validation;

        var venue = await GetOrCreateVenueAsync();
        venue.Photo = await ReadFileAsync(file);
        venue.PhotoContentType = file.ContentType;
        venue.UpdatedAt = DateTime.UtcNow;
        venue.UpdatedBy = User.FindFirst(ClaimTypes.Email)?.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("photo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeletePhoto()
    {
        var venue = await GetOrCreateVenueAsync();
        venue.Photo = null;
        venue.PhotoContentType = null;
        venue.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("photo")]
    public async Task<IActionResult> GetPhoto()
    {
        var venue = await _context.Venues.AsNoTracking().FirstOrDefaultAsync();
        if (venue?.Photo == null || venue.Photo.Length == 0)
            return NotFound();
        return File(venue.Photo, venue.PhotoContentType ?? "application/octet-stream");
    }

    // ---- Club logo (venue-level) ---------------------------------------------------------

    [HttpPost("club-logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> UploadClubLogo(IFormFile file)
    {
        var validation = ValidateImage(file);
        if (validation != null) return validation;

        var venue = await GetOrCreateVenueAsync();
        venue.ClubLogo = await ReadFileAsync(file);
        venue.ClubLogoContentType = file.ContentType;
        venue.UpdatedAt = DateTime.UtcNow;
        venue.UpdatedBy = User.FindFirst(ClaimTypes.Email)?.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("club-logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeleteClubLogo()
    {
        var venue = await GetOrCreateVenueAsync();
        venue.ClubLogo = null;
        venue.ClubLogoContentType = null;
        venue.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("club-logo")]
    public async Task<IActionResult> GetClubLogo()
    {
        var venue = await _context.Venues.AsNoTracking().FirstOrDefaultAsync();
        if (venue?.ClubLogo == null || venue.ClubLogo.Length == 0)
            return NotFound();
        return File(venue.ClubLogo, venue.ClubLogoContentType ?? "application/octet-stream");
    }

    // ---- Resident clubs ------------------------------------------------------------------

    [HttpPost("clubs")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<ClubDto>> CreateClub([FromBody] ClubUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var venue = await GetOrCreateVenueAsync();
        var club = new Club { VenueId = venue.Id };
        ApplyClub(club, dto);
        club.CreatedAt = DateTime.UtcNow;

        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();
        return Ok(ToDto(club));
    }

    [HttpPut("clubs/{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<ActionResult<ClubDto>> UpdateClub(int id, [FromBody] ClubUpsertDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var club = await _context.Clubs.FindAsync(id);
        if (club == null) return NotFound();

        ApplyClub(club, dto);
        club.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(ToDto(club));
    }

    [HttpDelete("clubs/{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeleteClub(int id)
    {
        var club = await _context.Clubs.FindAsync(id);
        if (club == null) return NotFound();

        _context.Clubs.Remove(club);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("clubs/{id:int}/logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> UploadClubLogo(int id, IFormFile file)
    {
        var validation = ValidateImage(file);
        if (validation != null) return validation;

        var club = await _context.Clubs.FindAsync(id);
        if (club == null) return NotFound();

        club.Logo = await ReadFileAsync(file);
        club.LogoContentType = file.ContentType;
        club.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("clubs/{id:int}/logo")]
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
    public async Task<IActionResult> DeleteClubLogo(int id)
    {
        var club = await _context.Clubs.FindAsync(id);
        if (club == null) return NotFound();

        club.Logo = null;
        club.LogoContentType = null;
        club.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("clubs/{id:int}/logo")]
    public async Task<IActionResult> GetClubLogo(int id)
    {
        var club = await _context.Clubs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (club?.Logo == null || club.Logo.Length == 0)
            return NotFound();
        return File(club.Logo, club.LogoContentType ?? "application/octet-stream");
    }

    // ---- Helpers -------------------------------------------------------------------------

    /// <summary>
    /// Returns the singleton venue, creating it lazily if the row is somehow missing (e.g. a
    /// database predating the seed), so callers never have to null-check.
    /// </summary>
    private async Task<Venue> GetOrCreateVenueAsync()
    {
        var venue = await _context.Venues
            .Include(v => v.Clubs)
            .FirstOrDefaultAsync();

        if (venue == null)
        {
            venue = new Venue { Name = "Stadium", CreatedAt = DateTime.UtcNow };
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        return venue;
    }

    private static EmailSettingsDto ToEmailDto(Venue venue) => new()
    {
        EmailEnabled = venue.EmailEnabled,
        SmtpHost = venue.SmtpHost,
        SmtpPort = venue.SmtpPort,
        SmtpUsername = venue.SmtpUsername,
        SmtpUseSsl = venue.SmtpUseSsl,
        FromAddress = venue.EmailFromAddress,
        FromName = venue.EmailFromName,
        HasPassword = !string.IsNullOrEmpty(venue.SmtpPassword),
        Password = null // never leak the stored password
    };

    private static void ApplyEmailSettings(Venue venue, EmailSettingsDto dto)
    {
        venue.EmailEnabled = dto.EmailEnabled;
        venue.SmtpHost = string.IsNullOrWhiteSpace(dto.SmtpHost) ? null : dto.SmtpHost.Trim();
        venue.SmtpPort = dto.SmtpPort;
        venue.SmtpUsername = string.IsNullOrWhiteSpace(dto.SmtpUsername) ? null : dto.SmtpUsername.Trim();
        venue.SmtpUseSsl = dto.SmtpUseSsl;
        venue.EmailFromAddress = string.IsNullOrWhiteSpace(dto.FromAddress) ? null : dto.FromAddress.Trim();
        venue.EmailFromName = string.IsNullOrWhiteSpace(dto.FromName) ? null : dto.FromName.Trim();

        // Only overwrite the stored password when a new non-empty value is supplied, so leaving the
        // field blank keeps the existing password rather than wiping it.
        if (!string.IsNullOrEmpty(dto.Password))
            venue.SmtpPassword = dto.Password;
    }

    private static void ApplyClub(Club club, ClubUpsertDto dto)
    {
        club.Name = dto.Name.Trim();
        club.ShortName = dto.ShortName;
        club.PrimaryColor = dto.PrimaryColor;
        club.SecondaryColor = dto.SecondaryColor;
        club.FoundedYear = dto.FoundedYear;
        club.Website = dto.Website;
        club.IsPrimary = dto.IsPrimary;
        club.DisplayOrder = dto.DisplayOrder;
    }

    private BadRequestObjectResult? ValidateImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
        if (file.Length > MaxImageBytes)
            return BadRequest("Image exceeds the 4 MB size limit.");
        if (!AllowedImageTypes.Contains(file.ContentType))
            return BadRequest("Unsupported image type. Use PNG, JPEG, WebP or SVG.");
        return null;
    }

    private static async Task<byte[]> ReadFileAsync(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    private static VenueDto ToDto(Venue v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        ClubName = v.ClubName,
        AddressLine1 = v.AddressLine1,
        AddressLine2 = v.AddressLine2,
        City = v.City,
        PostalCode = v.PostalCode,
        Country = v.Country,
        Latitude = v.Latitude,
        Longitude = v.Longitude,
        OfficialCapacity = v.OfficialCapacity,
        ContactEmail = v.ContactEmail,
        ContactPhone = v.ContactPhone,
        Website = v.Website,
        HasPhoto = v.Photo != null && v.Photo.Length > 0,
        HasClubLogo = v.ClubLogo != null && v.ClubLogo.Length > 0,
        UpdatedAt = v.UpdatedAt,
        UpdatedBy = v.UpdatedBy,
        Clubs = v.Clubs
            .OrderByDescending(c => c.IsPrimary)
            .ThenBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .Select(ToDto)
            .ToList()
    };

    private static ClubDto ToDto(Club c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        ShortName = c.ShortName,
        PrimaryColor = c.PrimaryColor,
        SecondaryColor = c.SecondaryColor,
        FoundedYear = c.FoundedYear,
        Website = c.Website,
        IsPrimary = c.IsPrimary,
        DisplayOrder = c.DisplayOrder,
        HasLogo = c.Logo != null && c.Logo.Length > 0
    };
}
