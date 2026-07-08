using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
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

    public VenueController(ApplicationDbContext context)
    {
        _context = context;
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
