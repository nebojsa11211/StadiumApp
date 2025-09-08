using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        /// <summary>
        /// Get paginated logs with filtering
        /// </summary>
        [HttpPost("search")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<PagedLogsDto>> GetLogs([FromBody] LogFilterDto filter)
        {
            try
            {
                // Log this action
                await _loggingService.LogUserActionAsync(
                    action: "ViewLogs",
                    category: "UserAction",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: $"Filter: Page={filter.Page}, PageSize={filter.PageSize}, Level={filter.Level}",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                var result = await _loggingService.GetLogsAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(
                    exception: ex,
                    action: "ViewLogs",
                    category: "SystemError",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: "Failed to retrieve logs",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                return StatusCode(500, new { message = "Failed to retrieve logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Get logs summary statistics
        /// </summary>
        [HttpGet("summary")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<LogSummaryDto>> GetLogSummary()
        {
            try
            {
                // Log this action
                await _loggingService.LogUserActionAsync(
                    action: "ViewLogSummary",
                    category: "UserAction",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                var summary = await _loggingService.GetLogSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(
                    exception: ex,
                    action: "ViewLogSummary",
                    category: "SystemError",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: "Failed to retrieve log summary",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                return StatusCode(500, new { message = "Failed to retrieve log summary", error = ex.Message });
            }
        }

        /// <summary>
        /// Clear old logs (Admin only)
        /// </summary>
        [HttpDelete("clear-old")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ClearOldLogs([FromQuery] int daysToKeep = 30)
        {
            try
            {
                // Log this action
                await _loggingService.LogUserActionAsync(
                    action: "ClearOldLogs",
                    category: "UserAction",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: $"Days to keep: {daysToKeep}",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                var success = await _loggingService.ClearOldLogsAsync(daysToKeep);
                
                if (success)
                {
                    return Ok(new { message = $"Successfully cleared logs older than {daysToKeep} days" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to clear old logs" });
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(
                    exception: ex,
                    action: "ClearOldLogs",
                    category: "SystemError",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: $"Failed to clear logs older than {daysToKeep} days",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                return StatusCode(500, new { message = "Failed to clear old logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Clear ALL logs (Admin only)
        /// </summary>
        [HttpDelete("clear-all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ClearAllLogs()
        {
            try
            {
                // Log this action
                await _loggingService.LogUserActionAsync(
                    action: "ClearAllLogs",
                    category: "UserAction",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    details: "Admin cleared ALL logs",
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                var success = await _loggingService.ClearAllLogsAsync();
                
                if (success)
                {
                    return Ok(new { message = "Successfully cleared all logs" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to clear all logs" });
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(
                    exception: ex,
                    action: "ClearAllLogs",
                    category: "SystemError",
                    userId: User.GetUserIdFromClaims(),
                    userEmail: User.GetUserEmailFromClaims(),
                    userRole: User.GetUserRoleFromClaims(),
                    requestPath: Request.Path,
                    httpMethod: Request.Method,
                    ipAddress: GetClientIpAddress(),
                    userAgent: Request.Headers.UserAgent.ToString(),
                    source: "API"
                );

                return StatusCode(500, new { message = "Failed to clear all logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Log a custom user action (for frontend applications)
        /// </summary>
        [HttpPost("log-action")]
        [AllowAnonymous]
        public async Task<ActionResult> LogUserAction([FromBody] LogUserActionRequest request)
        {
            try
            {
                // Enrich request with server-side information
                request.UserId = User.GetUserIdFromClaims() ?? request.UserId;
                request.UserEmail = User.GetUserEmailFromClaims() ?? request.UserEmail;
                request.UserRole = User.GetUserRoleFromClaims() ?? request.UserRole;
                request.Source = request.Source ?? "Unknown";

                // Use the new business event logging method if any business event fields are present
                if (!string.IsNullOrEmpty(request.BusinessEntityType) || 
                    !string.IsNullOrEmpty(request.BusinessEntityId) ||
                    request.MonetaryAmount.HasValue)
                {
                    await _loggingService.LogBusinessEventAsync(request);
                }
                else
                {
                    await _loggingService.LogUserActionAsync(
                        action: request.Action,
                        category: request.Category,
                        userId: request.UserId,
                        userEmail: request.UserEmail,
                        userRole: request.UserRole,
                        details: request.Details,
                        requestPath: request.RequestPath,
                        httpMethod: request.HttpMethod,
                        ipAddress: GetClientIpAddress(),
                        userAgent: Request.Headers.UserAgent.ToString(),
                        source: request.Source
                    );
                }

                return Ok(new { message = "Action logged successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to log action", error = ex.Message });
            }
        }

        /// <summary>
        /// Log multiple user actions in batch (for high-volume scenarios)
        /// </summary>
        [HttpPost("log-batch")]
        public async Task<ActionResult> LogBatch([FromBody] List<LogUserActionRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest(new { message = "No log entries provided" });
            }

            var successCount = 0;
            var failureCount = 0;
            var errors = new List<string>();

            foreach (var request in requests.Take(100)) // Limit batch size
            {
                try
                {
                    // Enrich request with server-side information
                    request.UserId = User.GetUserIdFromClaims() ?? request.UserId;
                    request.UserEmail = User.GetUserEmailFromClaims() ?? request.UserEmail;
                    request.UserRole = User.GetUserRoleFromClaims() ?? request.UserRole;
                    request.Source = request.Source ?? "Unknown";

                    // Use the new business event logging method if any business event fields are present
                    if (!string.IsNullOrEmpty(request.BusinessEntityType) || 
                        !string.IsNullOrEmpty(request.BusinessEntityId) ||
                        request.MonetaryAmount.HasValue)
                    {
                        await _loggingService.LogBusinessEventAsync(request);
                    }
                    else
                    {
                        await _loggingService.LogUserActionAsync(
                            action: request.Action,
                            category: request.Category,
                            userId: request.UserId,
                            userEmail: request.UserEmail,
                            userRole: request.UserRole,
                            details: request.Details,
                            requestPath: request.RequestPath,
                            httpMethod: request.HttpMethod,
                            ipAddress: GetClientIpAddress(),
                            userAgent: Request.Headers.UserAgent.ToString(),
                            source: request.Source
                        );
                    }
                    successCount++;
                }
                catch (Exception ex)
                {
                    failureCount++;
                    errors.Add($"Failed to log action '{request.Action}': {ex.Message}");
                }
            }

            return Ok(new { 
                message = $"Batch processing completed", 
                successCount,
                failureCount,
                errors = errors.Take(10) // Limit error details
            });
        }

        private string? GetClientIpAddress()
        {
            // Try to get the real IP address
            var xForwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0].Trim();
            }

            var xRealIp = Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIp))
            {
                return xRealIp;
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }

}