using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Bar-counter reusable-cup returns. A staff member scans (or types) the customer's ticket, sees how many
/// cups are still out with a held deposit, and refunds them — crediting the ticket's wallet (the default
/// refund path). Honor-system cups can be returned in bulk (no money). Staff-scoped
/// (Bartender/Waiter/Admin). See docs/reusable-cups-design.md.
/// </summary>
[Route("api/bar/cups")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
public class CupReturnsController : ControllerBase
{
    private readonly ICupService _cups;

    public CupReturnsController(ICupService cups) => _cups = cups;

    private int StaffUserId => int.TryParse(User.GetUserIdFromClaims(), out var id) ? id : 0;
    private string? StaffEmail => User.FindFirst(ClaimTypes.Email)?.Value;

    /// <summary>Resolve a scanned/typed code to a ticket and report its outstanding cup deposits.</summary>
    [HttpGet("lookup")]
    public async Task<ActionResult<CupReturnLookupDto>> Lookup([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new CupReturnLookupDto { Found = false, Error = "Missing code." });
        return Ok(await _cups.LookupAsync(query));
    }

    /// <summary>Refund up to <c>Count</c> of the ticket's held cup deposits (wallet credit).</summary>
    [HttpPost("return")]
    public async Task<ActionResult<CupReturnResultDto>> Return([FromBody] CupReturnRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Query))
            return BadRequest(new CupReturnResultDto { Success = false, Error = "Missing ticket." });

        return Ok(await _cups.ReturnDepositsAsync(request.Query, request.Count, request.IdempotencyKey, StaffUserId));
    }

    /// <summary>Record a bulk return of honor-system cups (no money moves).</summary>
    [HttpPost("return-honor")]
    public async Task<ActionResult<int>> ReturnHonor([FromBody] HonorCupReturnRequestDto request)
    {
        if (request is null || request.Count < 1)
            return BadRequest("Nothing to return.");
        return Ok(await _cups.ReturnHonorCupsAsync(request.Count, StaffUserId, StaffEmail));
    }

    /// <summary>Bind a scanned physical cup's QR to the ticket's next unassigned held deposit (cup-QR binding).</summary>
    [HttpPost("assign-cup")]
    public async Task<ActionResult<CupAssignResultDto>> AssignCup([FromBody] CupAssignRequestDto request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Query) || string.IsNullOrWhiteSpace(request.CupQrToken))
            return BadRequest(new CupAssignResultDto { Success = false, Error = "Ticket and cup are both required." });
        return Ok(await _cups.AssignCupAsync(request.Query, request.CupQrToken, StaffUserId));
    }
}

/// <summary>Admin reporting for reusable cups (outstanding liability, return rate, shrinkage).</summary>
[Route("api/admin/cups")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class CupsAdminController : ControllerBase
{
    private readonly ICupService _cups;

    public CupsAdminController(ICupService cups) => _cups = cups;

    [HttpGet("dashboard")]
    public async Task<ActionResult<CupDashboardDto>> Dashboard() => Ok(await _cups.GetDashboardAsync());

    [HttpGet("by-event")]
    public async Task<ActionResult<List<CupEventReportDto>>> ByEvent() => Ok(await _cups.GetEventReportAsync());
}
