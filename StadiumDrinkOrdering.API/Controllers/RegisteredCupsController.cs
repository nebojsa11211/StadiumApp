using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Admin management of registered (BYOC / premium) cups — the approved-cup registry that gates
/// bring-your-own-cup service. Each cup has a unique scannable QR; an approved, Active cup earns the
/// BYOC discount at order time. See docs/reusable-cups-design.md.
/// </summary>
[Route("api/admin/registered-cups")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class RegisteredCupsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RegisteredCupsController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<RegisteredCupListDto>> List(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 100) pageSize = 25;

        var q = _db.RegisteredCups
            .Include(c => c.CupType).Include(c => c.User).Include(c => c.Ticket)
            .AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(c => c.QrToken.Contains(s));
        }

        var total = await q.CountAsync();
        var cups = (await q.OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync())
            .Select(ToDto).ToList();

        return Ok(new RegisteredCupListDto { Cups = cups, TotalCount = total, Page = page, PageSize = pageSize });
    }

    [HttpPost]
    public async Task<ActionResult<RegisteredCupDto>> Create([FromBody] RegisteredCupUpsertDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tok = dto.QrToken.Trim();
        if (string.IsNullOrWhiteSpace(tok))
            return BadRequest("A QR code is required.");
        if (await _db.RegisteredCups.AnyAsync(c => c.QrToken == tok))
            return Conflict("A cup with that QR code is already registered.");

        var cup = new RegisteredCup
        {
            QrToken = tok,
            CupTypeId = dto.CupTypeId,
            IsApproved = dto.IsApproved,
            Status = ParseStatus(dto.Status),
            OwnerType = WalletOwnerType.User, // unassigned club stock; can be linked to an owner later
            RegisteredAt = DateTime.UtcNow
        };
        _db.RegisteredCups.Add(cup);
        await _db.SaveChangesAsync();

        cup = await _db.RegisteredCups.Include(c => c.CupType).FirstAsync(c => c.Id == cup.Id);
        return Ok(ToDto(cup));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RegisteredCupDto>> Update(int id, [FromBody] RegisteredCupUpsertDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cup = await _db.RegisteredCups
            .Include(c => c.CupType).Include(c => c.User).Include(c => c.Ticket)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cup is null) return NotFound();

        var tok = dto.QrToken.Trim();
        if (tok != cup.QrToken && await _db.RegisteredCups.AnyAsync(c => c.QrToken == tok && c.Id != id))
            return Conflict("A cup with that QR code is already registered.");

        cup.QrToken = tok;
        cup.CupTypeId = dto.CupTypeId;
        cup.IsApproved = dto.IsApproved;
        cup.Status = ParseStatus(dto.Status);
        cup.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(ToDto(cup));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cup = await _db.RegisteredCups.FirstOrDefaultAsync(c => c.Id == id);
        if (cup is null) return NotFound();

        // OrderItem.RegisteredCupId is SetNull on delete, so past orders keep their line, just unlinked.
        _db.RegisteredCups.Remove(cup);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static RegisteredCupStatus ParseStatus(string? s) =>
        Enum.TryParse<RegisteredCupStatus>(s, ignoreCase: true, out var st) ? st : RegisteredCupStatus.Active;

    private static RegisteredCupDto ToDto(RegisteredCup c) => new()
    {
        Id = c.Id,
        QrToken = c.QrToken,
        CupTypeId = c.CupTypeId,
        CupTypeName = c.CupType?.Name,
        VolumeMl = c.CupType?.VolumeMl,
        IsApproved = c.IsApproved,
        Status = c.Status.ToString(),
        OwnerLabel = c.User != null ? c.User.Email : c.Ticket?.TicketNumber,
        RegisteredAt = c.RegisteredAt
    };
}

/// <summary>Public cup lookups used by the customer app when scanning a personal cup for BYOC.</summary>
[Route("api/cups")]
[ApiController]
public class CupsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CupsController(ApplicationDbContext db) => _db = db;

    /// <summary>Active cup designs (models) — for the admin registered-cup form and any client that needs
    /// the catalog. Public read.</summary>
    [HttpGet("types")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CupTypeDto>>> Types()
    {
        var types = await _db.CupTypes.Where(c => c.IsActive).OrderBy(c => c.Name)
            .Select(c => new CupTypeDto { Id = c.Id, Name = c.Name, VolumeMl = c.VolumeMl, UnitCost = c.UnitCost })
            .ToListAsync();
        return Ok(types);
    }

    /// <summary>
    /// Resolve a scanned cup QR so the customer app can validate it and show the discount before adding a
    /// BYOC drink. The order endpoint re-validates authoritatively, so this is a UX convenience only.
    /// </summary>
    [HttpGet("resolve")]
    [AllowAnonymous]
    public async Task<ActionResult<CupResolveDto>> Resolve([FromQuery] string token)
    {
        var venue = await _db.Venues
            .Select(v => new { v.CupByocEnabled, v.CupByocDiscountAmount })
            .FirstOrDefaultAsync();
        var discount = venue?.CupByocDiscountAmount ?? 0m;

        if (string.IsNullOrWhiteSpace(token))
            return Ok(new CupResolveDto { Found = false, Usable = false, Discount = discount, Reason = "No code scanned." });

        var tok = token.Trim();
        var cup = await _db.RegisteredCups.Include(c => c.CupType).AsNoTracking()
            .FirstOrDefaultAsync(c => c.QrToken == tok);

        if (cup is null)
            return Ok(new CupResolveDto { Found = false, Usable = false, Discount = discount, Reason = "This cup isn't registered." });

        var usable = (venue?.CupByocEnabled ?? false)
            && cup.Status == RegisteredCupStatus.Active && cup.IsApproved;

        return Ok(new CupResolveDto
        {
            Found = true,
            Usable = usable,
            CupTypeName = cup.CupType?.Name,
            VolumeMl = cup.CupType?.VolumeMl,
            Discount = discount,
            Reason = usable ? null : "This cup isn't approved for use."
        });
    }
}
