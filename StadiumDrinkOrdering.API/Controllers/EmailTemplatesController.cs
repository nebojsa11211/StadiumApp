using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Manages the editable transactional email templates surfaced on the Admin settings area. All
/// endpoints are Admin-only. Effective content is the admin override when present, otherwise the
/// built-in default from <see cref="EmailTemplateCatalog"/>.
/// </summary>
[Route("api/email-templates")]
[ApiController]
[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public class EmailTemplatesController : ControllerBase
{
    private readonly IEmailTemplateService _service;

    public EmailTemplatesController(IEmailTemplateService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<EmailTemplateDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{key}")]
    public async Task<ActionResult<EmailTemplateDto>> Get(string key)
    {
        var dto = await _service.GetAsync(key);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("{key}")]
    public async Task<ActionResult<EmailTemplateDto>> Update(string key, [FromBody] UpdateEmailTemplateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedBy = User.FindFirst(ClaimTypes.Email)?.Value;
        var result = await _service.UpdateAsync(key, dto, updatedBy);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Reset a template to its built-in default (removes the admin override).</summary>
    [HttpPost("{key}/reset")]
    public async Task<ActionResult<EmailTemplateDto>> Reset(string key)
    {
        var result = await _service.ResetAsync(key);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Render the supplied (possibly unsaved) content with sample placeholder values.</summary>
    [HttpPost("{key}/preview")]
    public ActionResult<EmailTemplatePreviewResultDto> Preview(string key, [FromBody] UpdateEmailTemplateDto dto)
    {
        var result = _service.Preview(key, dto.Subject, dto.HtmlBody, dto.TextBody);
        return result is null ? NotFound() : Ok(result);
    }
}
