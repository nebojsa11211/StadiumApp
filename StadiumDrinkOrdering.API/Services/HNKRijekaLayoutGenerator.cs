using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Constants;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// HNK Rijeka Stadium layout generator
/// Generates SVG layouts using exact coordinates from static SVG for pixel-perfect consistency
/// </summary>
public class HNKRijekaLayoutGenerator : IStadiumLayoutGenerator
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<HNKRijekaLayoutGenerator> _logger;

    public HNKRijekaLayoutGenerator(IMemoryCache cache, ILogger<HNKRijekaLayoutGenerator> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Generate SVG layout from Tribune/Ring/Sector structure
    /// Uses HNK Rijeka coordinates for known sectors, generates generic layout for others
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GenerateLayoutAsync(List<Tribune> tribunes)
    {
        try
        {
            _logger.LogInformation("Generating stadium layout for {TribuneCount} tribunes", tribunes.Count);

            // Check if this matches HNK Rijeka layout
            if (IsHNKRijekaLayout(tribunes))
            {
                _logger.LogInformation("Detected HNK Rijeka layout, using exact coordinates");
                return await GenerateHNKRijekaLayoutAsync(tribunes);
            }

            // For now, always use HNK Rijeka layout - future enhancement for generic layouts
            _logger.LogWarning("Non-HNK Rijeka layout detected, falling back to HNK Rijeka layout");
            return await GenerateHNKRijekaLayoutAsync(tribunes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating stadium layout");
            throw;
        }
    }

    /// <summary>
    /// Generate HNK Rijeka specific layout (exact static SVG replica)
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GenerateHNKRijekaLayoutAsync(List<Tribune> tribunes)
    {
        return await Task.FromResult(_cache.GetOrCreate("hnk-rijeka-layout", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            
            _logger.LogInformation("Generating HNK Rijeka layout from {TribuneCount} tribunes", tribunes.Count);

            var layout = new StadiumSvgLayoutDto
            {
                Name = HNKRijekaStadiumConstants.STADIUM_NAME,
                Description = HNKRijekaStadiumConstants.STADIUM_DESCRIPTION,
                Field = CreateField(),
                ViewBox = HNKRijekaStadiumConstants.VIEW_BOX,
                Stands = GenerateStands(tribunes),
                LastUpdated = DateTime.UtcNow
            };

            _logger.LogInformation("Generated layout with {StandCount} stands and {TotalSectors} total sectors", 
                layout.Stands.Count, layout.Stands.Sum(s => s.Sectors.Count));

            return layout;
        }));
    }

    /// <summary>
    /// Generate generic stadium layout for unknown structures (placeholder for future)
    /// </summary>
    public async Task<StadiumSvgLayoutDto> GenerateGenericLayoutAsync(List<Tribune> tribunes, LayoutType layoutType = LayoutType.Oval)
    {
        // For now, delegate to HNK Rijeka layout
        // Future enhancement: implement actual generic layout algorithms
        _logger.LogWarning("Generic layout generation not yet implemented, using HNK Rijeka layout");
        return await GenerateHNKRijekaLayoutAsync(tribunes);
    }

    /// <summary>
    /// Check if the provided tribunes match HNK Rijeka layout
    /// Updated to recognize imported stadium structure with N1, N2, S1, S2, E1, E2, W1, W2 codes
    /// </summary>
    public bool IsHNKRijekaLayout(List<Tribune> tribunes)
    {
        if (!tribunes.Any())
        {
            _logger.LogInformation("No tribunes provided, defaulting to HNK Rijeka layout");
            return true;
        }

        // Check if we have sectors that match HNK Rijeka codes (both legacy and new imported codes)
        var allSectorCodes = tribunes
            .SelectMany(t => t.Rings)
            .SelectMany(r => r.Sectors)
            .Select(s => s.Code)
            .ToHashSet();

        _logger.LogInformation("All sector codes found: {SectorCodes}", string.Join(", ", allSectorCodes));

        var hnkRijekaCodes = HNKRijekaStadiumConstants.SECTOR_COORDINATES.Keys.ToHashSet();
        var matchingCodes = allSectorCodes.Intersect(hnkRijekaCodes).Count();

        // Check if this is the imported stadium structure (N1, N2, S1, S2, E1, E2, W1, W2)
        var importedStadiumCodes = new HashSet<string> { "N1", "N2", "S1", "S2", "E1", "E2", "W1", "W2" };
        var importedMatches = allSectorCodes.Intersect(importedStadiumCodes).Count();

        _logger.LogInformation("Expected imported codes: {ExpectedCodes}", string.Join(", ", importedStadiumCodes));
        _logger.LogInformation("Matching imported codes: {MatchingImportedCodes}",
            string.Join(", ", allSectorCodes.Intersect(importedStadiumCodes)));

        // If we have the imported stadium codes (4+ matches), consider it HNK Rijeka compatible
        bool isImportedStadium = importedMatches >= 4;

        // If more than 50% of codes match HNK Rijeka, or if we have imported stadium codes, consider it HNK Rijeka layout
        bool isHnkRijeka = matchingCodes > (allSectorCodes.Count * 0.5) || isImportedStadium;

        _logger.LogInformation("Stadium layout analysis: {MatchingCodes}/{TotalCodes} codes match HNK Rijeka, {ImportedMatches} imported codes, IsHNKRijeka: {IsHNKRijeka}",
            matchingCodes, allSectorCodes.Count, importedMatches, isHnkRijeka);

        return isHnkRijeka;
    }

    /// <summary>
    /// Validate stadium structure for SVG generation
    /// </summary>
    public ValidationResult ValidateStructure(List<Tribune> tribunes)
    {
        var result = new ValidationResult { IsValid = true };

        if (!tribunes.Any())
        {
            result.Errors.Add("No tribunes provided");
            result.IsValid = false;
            return result;
        }

        foreach (var tribune in tribunes)
        {
            if (string.IsNullOrWhiteSpace(tribune.Code))
            {
                result.Errors.Add($"Tribune {tribune.Name} has empty code");
                result.IsValid = false;
            }

            if (!tribune.Rings.Any())
            {
                result.Errors.Add($"Tribune {tribune.Code} has no rings");
                result.IsValid = false;
            }

            foreach (var ring in tribune.Rings)
            {
                if (!ring.Sectors.Any())
                {
                    result.Errors.Add($"Ring {ring.Number} in tribune {tribune.Code} has no sectors");
                    result.IsValid = false;
                }

                foreach (var sector in ring.Sectors)
                {
                    if (string.IsNullOrWhiteSpace(sector.Code))
                    {
                        result.Errors.Add($"Sector in ring {ring.Number}, tribune {tribune.Code} has empty code");
                        result.IsValid = false;
                    }

                    if (sector.TotalRows <= 0 || sector.SeatsPerRow <= 0)
                    {
                        result.Errors.Add($"Sector {sector.Code} has invalid row/seat configuration");
                        result.IsValid = false;
                    }
                }
            }
        }

        return result;
    }

    #region Private Helper Methods

    /// <summary>
    /// Create football field DTO (exact same as static SVG)
    /// </summary>
    private FieldSvgDto CreateField()
    {
        return new FieldSvgDto
        {
            Bounds = HNKRijekaStadiumConstants.FIELD.ToRectangle(),
            GradientId = "field-gradient",
            FilterId = "sector-shadow",
            StrokeColor = "#ffffff",
            StrokeWidth = 3
        };
    }

    /// <summary>
    /// Generate stands from tribunes using HNK Rijeka coordinates
    /// </summary>
    private List<StandSvgDto> GenerateStands(List<Tribune> tribunes)
    {
        var stands = new List<StandSvgDto>();

        foreach (var tribune in tribunes)
        {
            var stand = new StandSvgDto
            {
                Code = tribune.Code,
                Name = tribune.Name,
                CssClass = GetStandCssClass(tribune.Code),
                DataAttribute = GetStandDataAttribute(tribune.Code),
                Sectors = GenerateSectors(tribune)
            };

            if (stand.Sectors.Any())
            {
                stands.Add(stand);
                _logger.LogDebug("Generated stand {StandCode} with {SectorCount} sectors", 
                    stand.Code, stand.Sectors.Count);
            }
        }

        // Add East Stand as unavailable (exactly like static version) if no East tribune exists
        if (!tribunes.Any(t => t.Code == "E"))
        {
            stands.Add(GenerateEastStandUnavailable());
        }

        return stands;
    }

    /// <summary>
    /// Generate sectors from tribune using exact HNK Rijeka coordinates
    /// </summary>
    private List<SectorSvgDto> GenerateSectors(Tribune tribune)
    {
        var sectors = new List<SectorSvgDto>();

        foreach (var ring in tribune.Rings)
        {
            foreach (var sector in ring.Sectors)
            {
                // Map to EXACT HNK Rijeka coordinates if available
                if (HNKRijekaStadiumConstants.SECTOR_COORDINATES.TryGetValue(sector.Code, out var coords))
                {
                    var sectorDto = new SectorSvgDto
                    {
                        Code = sector.Code,
                        Name = sector.Name,
                        Bounds = coords.ToRectangle(),
                        TextCenter = coords.TextCenter,
                        Style = coords.ToStyle(),
                        TotalSeats = CalculateTotalSeats(sector),
                        OccupiedSeats = 0 // Will be updated by real-time data
                    };

                    sectors.Add(sectorDto);
                    _logger.LogDebug("Mapped sector {SectorCode} to HNK Rijeka coordinates: {X},{Y} {Width}x{Height}", 
                        sector.Code, coords.X, coords.Y, coords.Width, coords.Height);
                }
                else
                {
                    // For sectors not in HNK Rijeka layout, generate generic coordinates
                    var genericSector = GenerateGenericSectorLayout(sector, tribune.Code);
                    if (genericSector != null)
                    {
                        sectors.Add(genericSector);
                        _logger.LogWarning("Generated generic coordinates for unknown sector {SectorCode}", sector.Code);
                    }
                }
            }
        }

        return sectors;
    }

    /// <summary>
    /// Calculate total seats for a sector
    /// </summary>
    private int CalculateTotalSeats(Sector sector)
    {
        // Use real seat count if available
        if (sector.Seats?.Any() == true)
        {
            return sector.Seats.Count;
        }

        // Calculate from rows and seats per row
        if (sector.TotalRows > 0 && sector.SeatsPerRow > 0)
        {
            return sector.TotalRows * sector.SeatsPerRow;
        }

        // Use HNK Rijeka default if available
        if (HNKRijekaStadiumConstants.DEFAULT_SEAT_COUNTS.TryGetValue(sector.Code, out var defaultCount))
        {
            return defaultCount;
        }

        // Generic fallback
        return 200;
    }

    /// <summary>
    /// Generate generic sector layout for unknown sectors
    /// </summary>
    private SectorSvgDto? GenerateGenericSectorLayout(Sector sector, string tribuneCode)
    {
        try
        {
            // Generate coordinates based on tribune and sector position
            var coords = CalculateGenericSectorCoordinates(sector.Code, tribuneCode);

            var sectorDto = new SectorSvgDto
            {
                Code = sector.Code,
                Name = sector.Name,
                Bounds = coords.ToRectangle(),
                TextCenter = coords.TextCenter,
                Style = coords.ToStyle(),
                TotalSeats = CalculateTotalSeats(sector),
                OccupiedSeats = 0 // Will be updated by real-time data
            };

            _logger.LogInformation("Generated generic layout for sector {SectorCode} in tribune {TribuneCode}: {X},{Y} {Width}x{Height}",
                sector.Code, tribuneCode, coords.X, coords.Y, coords.Width, coords.Height);

            return sectorDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating generic layout for sector {SectorCode}", sector.Code);
            return null;
        }
    }

    /// <summary>
    /// Calculate generic coordinates for sectors based on tribune and position
    /// </summary>
    private SectorCoordinates CalculateGenericSectorCoordinates(string sectorCode, string tribuneCode)
    {
        // Base dimensions for generic sectors
        const int sectorWidth = 120;
        const int sectorHeight = 60;
        const int sectorSpacing = 10;

        // Extract sector index from code (A=0, B=1, C=2, etc.)
        int sectorIndex = 0;
        if (!string.IsNullOrEmpty(sectorCode) && char.IsLetter(sectorCode[^1]))
        {
            sectorIndex = sectorCode[^1] - 'A';
        }

        // Calculate position based on tribune
        int x, y;

        switch (tribuneCode.ToUpper())
        {
            case "N": // North - top of stadium
                x = 200 + (sectorIndex * (sectorWidth + sectorSpacing));
                y = 50;
                break;

            case "S": // South - bottom of stadium
                x = 200 + (sectorIndex * (sectorWidth + sectorSpacing));
                y = 450;
                break;

            case "E": // East - right side of stadium
                x = 650;
                y = 150 + (sectorIndex * (sectorHeight + sectorSpacing));
                break;

            case "W": // West - left side of stadium
                x = 50;
                y = 150 + (sectorIndex * (sectorHeight + sectorSpacing));
                break;

            default:
                // Default positioning for unknown tribunes
                x = 300 + (sectorIndex * (sectorWidth + sectorSpacing));
                y = 250;
                break;
        }

        return new SectorCoordinates
        {
            X = x,
            Y = y,
            Width = sectorWidth,
            Height = sectorHeight,
            FillColor = GetTribuneFillColor(tribuneCode),
            StrokeColor = GetTribuneStrokeColor(tribuneCode)
        };
    }

    /// <summary>
    /// Get fill color for tribune-based sectors
    /// </summary>
    private string GetTribuneFillColor(string tribuneCode)
    {
        return tribuneCode.ToUpper() switch
        {
            "N" => "#4A90E2", // Blue for North
            "S" => "#E24A4A", // Red for South
            "E" => "#4AE24A", // Green for East
            "W" => "#E2A04A", // Orange for West
            _ => "#9E9E9E"     // Gray for unknown
        };
    }

    /// <summary>
    /// Get stroke color for tribune-based sectors
    /// </summary>
    private string GetTribuneStrokeColor(string tribuneCode)
    {
        return tribuneCode.ToUpper() switch
        {
            "N" => "#2563eb", // Darker blue for North
            "S" => "#dc2626", // Darker red for South
            "E" => "#16a34a", // Darker green for East
            "W" => "#ea580c", // Darker orange for West
            _ => "#6b7280"    // Gray for unknown
        };
    }

    /// <summary>
    /// Generate East Stand as unavailable (gray rectangles)
    /// </summary>
    private StandSvgDto GenerateEastStandUnavailable()
    {
        var eastStand = new StandSvgDto
        {
            Code = "E",
            Name = "East Stand (Unavailable)",
            CssClass = "east-stand",
            DataAttribute = "east",
            Sectors = new List<SectorSvgDto>()
        };

        // Add gray unavailable rectangles (exactly like static SVG)
        for (int i = 0; i < HNKRijekaStadiumConstants.EAST_UNAVAILABLE.Length; i++)
        {
            var rect = HNKRijekaStadiumConstants.EAST_UNAVAILABLE[i];
            var sector = new SectorSvgDto
            {
                Code = $"E{i + 1}",
                Name = "Unavailable",
                Bounds = rect,
                TextCenter = new SvgPointDto(rect.X + rect.Width / 2, rect.Y + rect.Height / 2),
                Style = new SectorStyleDto("#9E9E9E", "#757575"),
                TotalSeats = 0,
                OccupiedSeats = 0
            };

            eastStand.Sectors.Add(sector);
        }

        return eastStand;
    }

    /// <summary>
    /// Get CSS class for stand based on tribune code
    /// </summary>
    private string GetStandCssClass(string tribuneCode)
    {
        return HNKRijekaStadiumConstants.TRIBUNE_CSS_CLASSES.TryGetValue(tribuneCode, out var cssClass) 
            ? cssClass 
            : $"{tribuneCode.ToLower()}-stand";
    }

    /// <summary>
    /// Get data attribute for stand based on tribune code
    /// </summary>
    private string GetStandDataAttribute(string tribuneCode)
    {
        return HNKRijekaStadiumConstants.TRIBUNE_DATA_ATTRIBUTES.TryGetValue(tribuneCode, out var dataAttr) 
            ? dataAttr 
            : tribuneCode.ToLower();
    }

    #endregion
}