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
    /// </summary>
    public bool IsHNKRijekaLayout(List<Tribune> tribunes)
    {
        if (!tribunes.Any())
        {
            _logger.LogInformation("No tribunes provided, defaulting to HNK Rijeka layout");
            return true;
        }

        // Check if we have sectors that match HNK Rijeka codes
        var allSectorCodes = tribunes
            .SelectMany(t => t.Rings)
            .SelectMany(r => r.Sectors)
            .Select(s => s.Code)
            .ToHashSet();

        var hnkRijekaCodes = HNKRijekaStadiumConstants.SECTOR_COORDINATES.Keys.ToHashSet();
        var matchingCodes = allSectorCodes.Intersect(hnkRijekaCodes).Count();

        // If more than 50% of codes match HNK Rijeka, consider it HNK Rijeka layout
        bool isHnkRijeka = matchingCodes > (allSectorCodes.Count * 0.5);
        
        _logger.LogInformation("Stadium layout analysis: {MatchingCodes}/{TotalCodes} codes match HNK Rijeka, IsHNKRijeka: {IsHNKRijeka}", 
            matchingCodes, allSectorCodes.Count, isHnkRijeka);

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
        // For now, return null - future enhancement
        // This would implement generic coordinate calculation algorithms
        _logger.LogWarning("Generic sector layout generation not implemented for sector {SectorCode}", sector.Code);
        return null;
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