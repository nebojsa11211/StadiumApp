using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// First-run setup readiness for the Admin app. Reports which minimum-configuration prerequisites a
/// fresh installation still lacks, and lets an admin dismiss the setup nudge. All endpoints require
/// an Admin — the answers are only meaningful (and only shown) inside the admin shell.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class SetupController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SetupController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>Cheap DB checks for each setup prerequisite plus the dismissed flag.</summary>
    [HttpGet("status")]
    public async Task<ActionResult<SetupStatusDto>> GetStatus()
    {
        // A layout can come from JSON import (tribunes) or the drawing tool (overlays); either counts.
        var hasStructure = await _context.Tribunes.AnyAsync()
                           || await _context.StadiumSectorOverlays.AnyAsync();

        // Cheap existence check — never pulls the (potentially multi-MB) image blob into memory.
        var hasStadiumImage = await _context.Venues.AnyAsync(v => v.StadiumImage != null);

        var venue = await _context.Venues.AsNoTracking().FirstOrDefaultAsync();

        var dto = new SetupStatusDto
        {
            HasStadiumStructure = hasStructure,
            HasStadiumImage = hasStadiumImage,
            HasCategories = await _context.Categories.AnyAsync(),
            HasDrinks = await _context.Drinks.AnyAsync(),
            HasStaff = await _context.Users.AnyAsync(u =>
                u.IsActive && (u.Role == UserRole.Bartender || u.Role == UserRole.Waiter)),
            HasVenueIdentity = venue != null
                               && !string.IsNullOrWhiteSpace(venue.ClubName)
                               && (!string.IsNullOrWhiteSpace(venue.City)
                                   || !string.IsNullOrWhiteSpace(venue.AddressLine1)),
            Dismissed = venue?.SetupDismissed ?? false
        };

        return Ok(dto);
    }

    /// <summary>Hide the setup banner. Persists on the singleton venue row (created if missing).</summary>
    [HttpPost("dismiss")]
    public async Task<IActionResult> Dismiss()
    {
        var venue = await _context.Venues.FirstOrDefaultAsync();
        if (venue == null)
        {
            venue = new Venue { Name = "Stadium", CreatedAt = DateTime.UtcNow };
            _context.Venues.Add(venue);
        }

        venue.SetupDismissed = true;
        venue.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
