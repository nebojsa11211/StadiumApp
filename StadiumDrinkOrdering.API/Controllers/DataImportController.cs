using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using System.IO;
using System.Threading.Tasks;

namespace StadiumDrinkOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataImportController : ControllerBase
    {
        private readonly TicketingDataImportService _importService;
        private readonly ILogger<DataImportController> _logger;
        private readonly IWebHostEnvironment _environment;

        public DataImportController(
            TicketingDataImportService importService,
            ILogger<DataImportController> logger,
            IWebHostEnvironment environment)
        {
            _importService = importService;
            _logger = logger;
            _environment = environment;
        }

        [HttpPost("import-ticketing-data")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportTicketingData()
        {
            try
            {
                // Look for the JSON file in the project root
                var jsonFilePath = Path.Combine(_environment.ContentRootPath, "..", "ticketing-test-data.json");
                
                if (!System.IO.File.Exists(jsonFilePath))
                {
                    // Try alternative path
                    jsonFilePath = Path.Combine(_environment.ContentRootPath, "ticketing-test-data.json");
                    
                    if (!System.IO.File.Exists(jsonFilePath))
                    {
                        _logger.LogError($"Ticketing data file not found at {jsonFilePath}");
                        return NotFound(new { message = "Ticketing data file not found. Please ensure ticketing-test-data.json exists in the project root." });
                    }
                }

                _logger.LogInformation($"Starting import from {jsonFilePath}");
                var success = await _importService.ImportFromJsonFileAsync(jsonFilePath);
                
                if (success)
                {
                    return Ok(new { 
                        message = "Ticketing data imported successfully",
                        filePath = jsonFilePath
                    });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to import ticketing data. Check logs for details." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing ticketing data");
                return StatusCode(500, new { 
                    message = "An error occurred while importing ticketing data",
                    error = ex.Message 
                });
            }
        }

        [HttpPost("upload-and-import")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAndImportTicketingData(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = "File must be a JSON file" });
                }

                // Save uploaded file to temp location
                var tempPath = Path.GetTempFileName();
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"Starting import from uploaded file: {file.FileName}");
                var success = await _importService.ImportFromJsonFileAsync(tempPath);
                
                // Clean up temp file
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
                
                if (success)
                {
                    return Ok(new { 
                        message = "Ticketing data imported successfully",
                        fileName = file.FileName
                    });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to import ticketing data. Check logs for details." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing uploaded ticketing data");
                return StatusCode(500, new { 
                    message = "An error occurred while importing ticketing data",
                    error = ex.Message 
                });
            }
        }
    }
}