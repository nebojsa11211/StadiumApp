using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StadiumViewerController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StadiumViewerController> _logger;

    public StadiumViewerController(ApplicationDbContext context, ILogger<StadiumViewerController> logger)
    {
        _context = context;
        _logger = logger;
    }

[AllowAnonymous]
    [HttpGet("overview")]
    public async Task<ActionResult<StadiumViewerDto>> GetStadiumOverview()
    {
        try
        {
            _logger.LogInformation("Stadium overview API endpoint called");

            // Load the full stadium structure with proper relationships
            var tribunes = await _context.Tribunes
                .Include(t => t.Rings)
                    .ThenInclude(r => r.Sectors)
                        .ThenInclude(s => s.Seats)
                .OrderBy(t => t.Code)
                .ToListAsync();

            _logger.LogInformation($"Found {tribunes.Count} active tribunes");

            if (!tribunes.Any())
            {
                _logger.LogWarning("No tribunes found in database");
                return NotFound(new { message = "No stadium structure found" });
            }

            var stadiumViewer = GenerateStadiumLayoutFromTribunes(tribunes);
            _logger.LogInformation($"Generated stadium layout: {stadiumViewer.Name} with {stadiumViewer.Stands.Count} stands");
            return Ok(stadiumViewer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting stadium overview");
            return StatusCode(500, new { message = "Error loading stadium overview" });
        }
    }

    [HttpGet("sector/{sectorId}/seats")]
    public async Task<ActionResult<StadiumSectorDto>> GetSectorSeats(string sectorId)
    {
        try
        {
            var section = await _context.StadiumSections
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(s => s.SectionCode == sectorId && s.IsActive);

            if (section == null)
            {
                return NotFound(new { message = $"Sector {sectorId} not found" });
            }

            var sectorDto = ConvertSectionToSectorDto(section, includeSeats: true);
            return Ok(sectorDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sector seats for {SectorId}", sectorId);
            return StatusCode(500, new { message = "Error loading sector seats" });
        }
    }

    [HttpGet("event/{eventId}/seat-status")]
    public async Task<ActionResult<EventSeatStatusDto>> GetEventSeatStatus(int eventId)
    {
        try
        {
            var eventEntity = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.IsActive);

            if (eventEntity == null)
            {
                return NotFound(new { message = $"Event {eventId} not found" });
            }

            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId)
                .Select(t => new { t.SeatId, t.Status })
                .ToListAsync();

            var reservations = await _context.SeatReservations
                .Where(r => r.EventId == eventId && r.ReservedUntil > DateTime.UtcNow)
                .Select(r => new { r.SeatCode, r.ReservedUntil })
                .ToListAsync();

            var sections = await _context.StadiumSections
                .Include(s => s.Seats)
                .Where(s => s.IsActive)
                .ToListAsync();

            var seatStatusDto = new EventSeatStatusDto
            {
                EventId = eventId,
                SectorSummaries = new Dictionary<string, SeatStatusSummaryDto>()
            };

            foreach (var section in sections)
            {
                var sectorSummary = new SeatStatusSummaryDto
                {
                    SectorId = section.SectionCode,
                    TotalSeats = section.Seats.Count,
                    SoldSeats = tickets.Count(t => section.Seats.Any(s => s.Id == t.SeatId) && t.Status == "sold"),
                    HeldSeats = reservations.Count(r => section.Seats.Any(s => s.SeatCode == r.SeatCode)),
                    BlockedSeats = section.Seats.Count(s => !s.IsAccessible),
                    FreeSeats = 0
                };

                sectorSummary.FreeSeats = sectorSummary.TotalSeats - sectorSummary.SoldSeats - 
                                         sectorSummary.HeldSeats - sectorSummary.BlockedSeats;
                sectorSummary.OccupancyPercentage = sectorSummary.TotalSeats > 0 
                    ? Math.Round((decimal)(sectorSummary.SoldSeats + sectorSummary.HeldSeats) / sectorSummary.TotalSeats * 100, 2)
                    : 0;

                seatStatusDto.SectorSummaries[section.SectionCode] = sectorSummary;
            }

            return Ok(seatStatusDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event seat status for {EventId}", eventId);
            return StatusCode(500, new { message = "Error loading event seat status" });
        }
    }

    [HttpGet("event/{eventId}/sector/{sectorId}/seat-details")]
    public async Task<ActionResult<List<ViewerSeatStatusDto>>> GetSectorSeatDetails(int eventId, string sectorId)
    {
        try
        {
            var section = await _context.StadiumSections
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(s => s.SectionCode == sectorId && s.IsActive);

            if (section == null)
            {
                return NotFound(new { message = $"Sector {sectorId} not found" });
            }

            var seatIds = section.Seats.Select(s => s.Id).ToList();

            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId && t.SeatId.HasValue && seatIds.Contains(t.SeatId.Value))
                .ToDictionaryAsync(t => t.SeatId!.Value, t => t.Status ?? "unknown");

            var reservations = await _context.SeatReservations
                .Where(r => r.EventId == eventId && r.ReservedUntil > DateTime.UtcNow)
                .Where(r => section.Seats.Any(s => s.SeatCode == r.SeatCode))
                .ToDictionaryAsync(r => r.SeatCode, r => r.ReservedUntil);

            var seatStatuses = section.Seats.Select(seat =>
            {
                string status = "free";
                DateTime? reservedUntil = null;

                if (tickets.TryGetValue(seat.Id, out var ticketStatus))
                {
                    status = ticketStatus == "Paid" ? "sold" : "held";
                }
                else if (reservations.TryGetValue(seat.SeatCode, out var expiresAt))
                {
                    status = "held";
                    reservedUntil = expiresAt;
                }
                else if (!seat.IsAccessible)
                {
                    status = "blocked";
                }

                return new ViewerSeatStatusDto
                {
                    SeatId = seat.SeatCode,
                    Status = status,
                    ReservedUntil = reservedUntil
                };
            }).ToList();

            return Ok(seatStatuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sector seat details for Event {EventId}, Sector {SectorId}", eventId, sectorId);
            return StatusCode(500, new { message = "Error loading seat details" });
        }
    }

    [HttpPost("search-seat")]
    public async Task<ActionResult<SearchSeatResultDto>> SearchSeat([FromBody] SearchSeatRequestDto request)
    {
        try
        {
            var seat = await _context.Seats
                .Include(s => s.Section)
                .FirstOrDefaultAsync(s => s.SeatCode.ToUpper() == request.SeatCode.ToUpper());

            if (seat == null)
            {
                return NotFound(new { message = $"Seat {request.SeatCode} not found" });
            }

            var result = new SearchSeatResultDto
            {
                SeatId = seat.SeatCode,
                SectorId = seat.Section.SectionCode,
                SectorName = seat.Section.SectionName,
                RowNumber = seat.RowNumber,
                SeatNumber = seat.SeatNumber,
                X = seat.XCoordinate,
                Y = seat.YCoordinate,
                IsAccessible = seat.IsAccessible
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for seat {SeatCode}", request.SeatCode);
            return StatusCode(500, new { message = "Error searching for seat" });
        }
    }

    private StadiumViewerDto GenerateStadiumLayout(List<StadiumSection> sections)
    {
        var stadiumViewer = new StadiumViewerDto
        {
            StadiumId = "main-stadium",
            Name = "Main Stadium",
            CoordinateSystem = new StadiumCoordinateSystem
            {
                Width = 1200,
                Height = 900,
                OriginX = 600,
                OriginY = 450,
                Unit = "normalized"
            }
        };

        // Generate professional oval field
        var centerX = stadiumViewer.CoordinateSystem.Width / 2;
        var centerY = stadiumViewer.CoordinateSystem.Height / 2;
        var fieldRadiusX = 180;
        var fieldRadiusY = 120;

        // Create oval field using multiple points for smooth curves
        var fieldPoints = new List<PointDto>();
        for (int i = 0; i < 32; i++)
        {
            var angle = (2 * Math.PI * i) / 32;
            var x = centerX + fieldRadiusX * Math.Cos(angle);
            var y = centerY + fieldRadiusY * Math.Sin(angle);
            fieldPoints.Add(new PointDto(x, y));
        }

        stadiumViewer.Field = new StadiumFieldDto
        {
            Polygon = fieldPoints,
            FillColor = "#2d5a2d",
            StrokeColor = "#ffffff"
        };

        // Group sections by tribune for professional stadium layout
        var tribunes = new Dictionary<string, List<StadiumSection>>();
        foreach (var section in sections)
        {
            var tribuneCode = DetermineTribuneCode(section.SectionCode);
            if (!tribunes.ContainsKey(tribuneCode))
            {
                tribunes[tribuneCode] = new List<StadiumSection>();
            }
            tribunes[tribuneCode].Add(section);
        }

        // Generate curved stadium stands with professional appearance
        foreach (var tribune in tribunes)
        {
            var stand = new StadiumStandDto
            {
                Id = tribune.Key.ToLower(),
                Name = GetTribuneName(tribune.Key),
                TribuneCode = tribune.Key,
                Sectors = new List<StadiumSectorDto>()
            };

            // Calculate curved stand polygon
            stand.Polygon = GenerateCurvedStandPolygon(tribune.Key, stadiumViewer.CoordinateSystem, centerX, centerY, fieldRadiusX, fieldRadiusY);

            // Add curved sectors to stand
            var sectionCount = tribune.Value.Count;
            for (int i = 0; i < sectionCount; i++)
            {
                var section = tribune.Value[i];
                var sector = ConvertSectionToSectorDto(section, includeSeats: false);
                sector.StandId = stand.Id;
                
                // Generate curved sector polygon for professional appearance
                sector.Polygon = GenerateCurvedSectorPolygon(section, tribune.Key, sectionCount, i, 
                    stadiumViewer.CoordinateSystem, centerX, centerY, fieldRadiusX, fieldRadiusY);
                
                stand.Sectors.Add(sector);
            }

            stadiumViewer.Stands.Add(stand);
        }

        return stadiumViewer;
    }

    private StadiumViewerDto GenerateStadiumLayoutFromTribunes(List<Tribune> tribunes)
    {
        var stadiumViewer = new StadiumViewerDto
        {
            StadiumId = "main-stadium",
            Name = "Main Stadium",
            CoordinateSystem = new StadiumCoordinateSystem
            {
                Width = 1200,
                Height = 900,
                OriginX = 600,
                OriginY = 450,
                Unit = "normalized"
            }
        };

        // Generate professional oval field
        var centerX = stadiumViewer.CoordinateSystem.Width / 2;
        var centerY = stadiumViewer.CoordinateSystem.Height / 2;
        var fieldRadiusX = 180;
        var fieldRadiusY = 120;

        // Create oval field using multiple points for smooth curves
        var fieldPoints = new List<PointDto>();
        for (int i = 0; i < 32; i++)
        {
            var angle = (2 * Math.PI * i) / 32;
            var x = centerX + fieldRadiusX * Math.Cos(angle);
            var y = centerY + fieldRadiusY * Math.Sin(angle);
            fieldPoints.Add(new PointDto(x, y));
        }

        stadiumViewer.Field = new StadiumFieldDto
        {
            Polygon = fieldPoints,
            FillColor = "#2d5a2d",
            StrokeColor = "#ffffff"
        };

        // Process each tribune properly
        foreach (var tribune in tribunes)
        {
            var stand = new StadiumStandDto
            {
                Id = tribune.Code.ToLower(),
                Name = tribune.Name,
                TribuneCode = tribune.Code,
                Sectors = new List<StadiumSectorDto>()
            };

            // Calculate curved stand polygon
            stand.Polygon = GenerateCurvedStandPolygon(tribune.Code, stadiumViewer.CoordinateSystem,
                centerX, centerY, fieldRadiusX, fieldRadiusY);

            // Process all sectors from all rings in this tribune
            var allSectors = tribune.Rings.SelectMany(r => r.Sectors).ToList();

            for (int i = 0; i < allSectors.Count; i++)
            {
                var sector = allSectors[i];
                var sectorDto = ConvertSectorToSectorDto(sector, includeSeats: false);
                sectorDto.StandId = stand.Id;

                // Generate curved sector polygon for professional appearance
                sectorDto.Polygon = GenerateCurvedSectorPolygonFromSector(sector, tribune.Code,
                    allSectors.Count, i, stadiumViewer.CoordinateSystem, centerX, centerY, fieldRadiusX, fieldRadiusY);

                stand.Sectors.Add(sectorDto);
            }

            stadiumViewer.Stands.Add(stand);
        }

        _logger.LogInformation($"Generated stadium layout from tribunes: {stadiumViewer.Stands.Count} stands, " +
            $"{stadiumViewer.Stands.Sum(s => s.Sectors.Count)} total sectors");

        return stadiumViewer;
    }

    private StadiumSectorDto ConvertSectionToSectorDto(StadiumSection section, bool includeSeats)
    {
        var sectorDto = new StadiumSectorDto
        {
            Id = section.SectionCode,
            Name = section.SectionName,
            TotalSeats = section.Seats.Count,
            AvailableSeats = section.Seats.Count(s => s.IsAccessible),
            PriceMultiplier = section.PriceMultiplier,
            FillColor = section.Color ?? "#e3e3e3"
        };

        sectorDto.OccupancyPercentage = 0; // Will be updated with event data

        if (includeSeats && section.Seats.Any())
        {
            sectorDto.Rows = section.Seats
                .GroupBy(s => s.RowNumber)
                .OrderBy(g => g.Key)
                .Select(g => new StadiumRowDto
                {
                    Index = g.Key - 1,
                    RowNumber = g.Key,
                    Seats = g.OrderBy(s => s.SeatNumber)
                        .Select(s => new ViewerSeatDto
                        {
                            Id = s.SeatCode,
                            Code = s.SeatCode,
                            X = s.XCoordinate,
                            Y = s.YCoordinate,
                            RowNumber = s.RowNumber,
                            SeatNumber = s.SeatNumber,
                            IsAccessible = s.IsAccessible,
                            Status = "free"
                        }).ToList()
                }).ToList();
        }

        return sectorDto;
    }

    private StadiumSectorDto ConvertSectorToSectorDto(Sector sector, bool includeSeats)
    {
        var sectorDto = new StadiumSectorDto
        {
            Id = sector.Code,
            Name = sector.Name,
            TotalSeats = sector.Seats.Count,
            AvailableSeats = sector.Seats.Count(s => s.IsAvailable),
            PriceMultiplier = 1.0m, // Default value since new structure doesn't have this field
            FillColor = "#e3e3e3" // Default color since new structure doesn't have this field
        };

        sectorDto.OccupancyPercentage = 0; // Will be updated with event data

        if (includeSeats && sector.Seats.Any())
        {
            sectorDto.Rows = sector.Seats
                .GroupBy(s => s.RowNumber)
                .OrderBy(g => g.Key)
                .Select(g => new StadiumRowDto
                {
                    Index = g.Key - 1,
                    RowNumber = g.Key,
                    Seats = g.OrderBy(s => s.SeatNumber)
                        .Select(s => new ViewerSeatDto
                        {
                            Id = s.UniqueCode,
                            Code = s.UniqueCode,
                            X = 0, // Default coordinates since new structure doesn't have them
                            Y = 0, // Default coordinates since new structure doesn't have them
                            RowNumber = s.RowNumber,
                            SeatNumber = s.SeatNumber,
                            IsAccessible = s.IsAvailable,
                            Status = "free"
                        }).ToList()
                }).ToList();
        }

        return sectorDto;
    }

    private List<PointDto> GenerateCurvedSectorPolygonFromSector(Sector sector, string tribuneCode, int totalSectors,
        int sectorIndex, StadiumCoordinateSystem coords, double centerX, double centerY, double fieldRadiusX, double fieldRadiusY)
    {
        var innerRadiusX = fieldRadiusX + 60;
        var innerRadiusY = fieldRadiusY + 40;
        var outerRadiusX = fieldRadiusX + 160;
        var outerRadiusY = fieldRadiusY + 120;

        var points = new List<PointDto>();

        // Get tribune angle range and calculate sector portion
        var (tribuneStartAngle, tribuneEndAngle) = GetTribuneAngles(tribuneCode);
        var sectorAngleRange = (tribuneEndAngle - tribuneStartAngle) / totalSectors;
        var sectorStartAngle = tribuneStartAngle + sectorAngleRange * sectorIndex;
        var sectorEndAngle = sectorStartAngle + sectorAngleRange;

        var angleStep = sectorAngleRange / 8; // 8 points per sector for smooth curves

        // Inner arc (closer to field)
        for (int i = 0; i <= 8; i++)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + innerRadiusX * Math.Cos(angle);
            var y = centerY + innerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }

        // Outer arc (back of sector) - reverse order
        for (int i = 8; i >= 0; i--)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + outerRadiusX * Math.Cos(angle);
            var y = centerY + outerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }

        return points;
    }

    private string DetermineTribuneCode(string sectionCode)
    {
        // Extract tribune from section code (e.g., "N1A" -> "N")
        if (string.IsNullOrEmpty(sectionCode)) return "N";
        
        var firstChar = sectionCode[0].ToString().ToUpper();
        if (new[] { "N", "S", "E", "W" }.Contains(firstChar))
            return firstChar;
        
        // Default tribune assignment based on section code
        return "N";
    }

    private string GetTribuneName(string tribuneCode)
    {
        return tribuneCode switch
        {
            "N" => "North Tribune",
            "S" => "South Tribune",
            "E" => "East Tribune",
            "W" => "West Tribune",
            _ => "Tribune"
        };
    }

    private List<PointDto> GenerateCurvedStandPolygon(string tribuneCode, StadiumCoordinateSystem coords, 
        double centerX, double centerY, double fieldRadiusX, double fieldRadiusY)
    {
        var innerRadiusX = fieldRadiusX + 60; // Distance from field
        var innerRadiusY = fieldRadiusY + 40;
        var outerRadiusX = fieldRadiusX + 160; // Stand depth
        var outerRadiusY = fieldRadiusY + 120;
        
        var points = new List<PointDto>();
        
        // Calculate arc angles for each tribune
        var (startAngle, endAngle) = GetTribuneAngles(tribuneCode);
        var angleStep = (endAngle - startAngle) / 20; // 20 points per arc for smooth curves
        
        // Inner arc (closer to field)
        for (int i = 0; i <= 20; i++)
        {
            var angle = startAngle + angleStep * i;
            var x = centerX + innerRadiusX * Math.Cos(angle);
            var y = centerY + innerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        // Outer arc (back of stand) - reverse order for proper polygon
        for (int i = 20; i >= 0; i--)
        {
            var angle = startAngle + angleStep * i;
            var x = centerX + outerRadiusX * Math.Cos(angle);
            var y = centerY + outerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        return points;
    }

    private List<PointDto> GenerateCurvedSectorPolygon(StadiumSection section, string tribuneCode, int totalSectors, 
        int sectorIndex, StadiumCoordinateSystem coords, double centerX, double centerY, double fieldRadiusX, double fieldRadiusY)
    {
        var innerRadiusX = fieldRadiusX + 60;
        var innerRadiusY = fieldRadiusY + 40;
        var outerRadiusX = fieldRadiusX + 160;
        var outerRadiusY = fieldRadiusY + 120;
        
        var points = new List<PointDto>();
        
        // Get tribune angle range and calculate sector portion
        var (tribuneStartAngle, tribuneEndAngle) = GetTribuneAngles(tribuneCode);
        var sectorAngleRange = (tribuneEndAngle - tribuneStartAngle) / totalSectors;
        var sectorStartAngle = tribuneStartAngle + sectorAngleRange * sectorIndex;
        var sectorEndAngle = sectorStartAngle + sectorAngleRange;
        
        var angleStep = sectorAngleRange / 8; // 8 points per sector for smooth curves
        
        // Inner arc (closer to field)
        for (int i = 0; i <= 8; i++)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + innerRadiusX * Math.Cos(angle);
            var y = centerY + innerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        // Outer arc (back of sector) - reverse order
        for (int i = 8; i >= 0; i--)
        {
            var angle = sectorStartAngle + angleStep * i;
            var x = centerX + outerRadiusX * Math.Cos(angle);
            var y = centerY + outerRadiusY * Math.Sin(angle);
            points.Add(new PointDto(x, y));
        }
        
        return points;
    }
    
    private (double startAngle, double endAngle) GetTribuneAngles(string tribuneCode)
    {
        // Professional stadium layout with realistic tribune angles
        return tribuneCode switch
        {
            "N" => (-2.8, -0.35), // North stand (top)
            "E" => (-0.35, 2.45), // East stand (right) 
            "S" => (0.35, 2.8),   // South stand (bottom)
            "W" => (2.45, -2.8),  // West stand (left) - wraps around
            _ => (-Math.PI / 2, Math.PI / 2)
        };
    }
}

