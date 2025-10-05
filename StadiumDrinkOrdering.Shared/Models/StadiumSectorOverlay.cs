using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Represents a visual sector overlay on the stadium blueprint for the drawing tool.
/// This defines the visual boundaries and metadata for stadium sectors.
/// </summary>
public class StadiumSectorOverlay
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique sector code (e.g., "A1", "VIP1", "NORTH-SEC1")
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string SectorCode { get; set; } = string.Empty;

    /// <summary>
    /// Display name for the sector
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Position & Size (percentage-based for responsive display)
    /// <summary>
    /// Top position as percentage of canvas height (0-100)
    /// </summary>
    [Required]
    public double TopPercent { get; set; }

    /// <summary>
    /// Left position as percentage of canvas width (0-100)
    /// </summary>
    [Required]
    public double LeftPercent { get; set; }

    /// <summary>
    /// Width as percentage of canvas width (0-100)
    /// </summary>
    [Required]
    public double WidthPercent { get; set; }

    /// <summary>
    /// Height as percentage of canvas height (0-100)
    /// </summary>
    [Required]
    public double HeightPercent { get; set; }

    // Shape Definition
    /// <summary>
    /// Shape type: Rectangle, Triangle, Rhombus, CircularSector, CustomPolygon
    /// </summary>
    [Required]
    public SectorShapeType ShapeTypeEnum { get; set; } = SectorShapeType.Rectangle;

    /// <summary>
    /// Legacy string shape type for backward compatibility
    /// </summary>
    [MaxLength(50)]
    [NotMapped]
    public string ShapeType
    {
        get => ShapeTypeEnum.ToString().ToLower();
        set
        {
            if (Enum.TryParse<SectorShapeType>(value, true, out var parsedType))
            {
                ShapeTypeEnum = parsedType;
            }
        }
    }

    /// <summary>
    /// JSON array of vertex coordinates for polygon-based shapes
    /// Format: [{"x": 10.5, "y": 20.3}, {"x": 30.1, "y": 40.2}, ...]
    /// </summary>
    [MaxLength(4000)]
    public string? VertexCoordinatesJson { get; set; }

    /// <summary>
    /// Deserialized vertex coordinates (not mapped to database)
    /// </summary>
    [NotMapped]
    public List<VertexCoordinate>? VertexCoordinates
    {
        get
        {
            if (string.IsNullOrEmpty(VertexCoordinatesJson))
                return null;

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<VertexCoordinate>>(VertexCoordinatesJson);
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value == null)
            {
                VertexCoordinatesJson = null;
            }
            else
            {
                VertexCoordinatesJson = System.Text.Json.JsonSerializer.Serialize(value);
            }
        }
    }

    /// <summary>
    /// JSON data defining the shape (for complex shapes like polygons)
    /// Legacy field - use VertexCoordinatesJson instead
    /// </summary>
    [MaxLength(4000)]
    public string? ShapeData { get; set; }

    // Seating Capacity
    /// <summary>
    /// Number of rows in this sector
    /// </summary>
    [Required]
    [Range(1, 100)]
    public int Rows { get; set; } = 10;

    /// <summary>
    /// Number of seats per row (used in uniform mode)
    /// </summary>
    [Required]
    [Range(1, 200)]
    public int SeatsPerRow { get; set; } = 20;

    /// <summary>
    /// Enable variable seating mode (different seats per row)
    /// </summary>
    public bool UseVariableSeating { get; set; } = false;

    /// <summary>
    /// JSON array of row patterns for variable seating
    /// Format: [{"fromRow": 1, "toRow": 5, "seatsPerRow": 15}, ...]
    /// </summary>
    [MaxLength(4000)]
    public string? VariableSeatingData { get; set; }

    /// <summary>
    /// Total seats in this sector (calculated based on mode)
    /// </summary>
    [NotMapped]
    public int TotalSeats
    {
        get
        {
            if (UseVariableSeating && !string.IsNullOrEmpty(VariableSeatingData))
            {
                try
                {
                    var patterns = System.Text.Json.JsonSerializer.Deserialize<List<RowPattern>>(VariableSeatingData);
                    return patterns?.Sum(p => (p.ToRow - p.FromRow + 1) * p.SeatsPerRow) ?? (Rows * SeatsPerRow);
                }
                catch
                {
                    return Rows * SeatsPerRow; // Fallback to uniform
                }
            }
            return Rows * SeatsPerRow;
        }
    }

    // Classification
    /// <summary>
    /// Sector type: standard, vip, wheelchair, premium, family
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = "standard";

    /// <summary>
    /// Display color for the sector on the canvas (hex color code)
    /// </summary>
    [Required]
    [MaxLength(7)]
    public string Color { get; set; } = "#007bff";

    // Metadata
    /// <summary>
    /// Date and time when the sector was created
    /// </summary>
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the sector was last modified
    /// </summary>
    [Required]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Soft delete flag - allows "deleting" sectors without removing them from the database
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Optional: Link to stadium section/tribune if integrated with main stadium structure
    /// </summary>
    public int? StadiumSectionId { get; set; }

    [ForeignKey(nameof(StadiumSectionId))]
    public virtual StadiumSection? StadiumSection { get; set; }
}

/// <summary>
/// Represents a row pattern for variable seating configuration
/// </summary>
public class RowPattern
{
    /// <summary>
    /// Starting row number (inclusive)
    /// </summary>
    public int FromRow { get; set; }

    /// <summary>
    /// Ending row number (inclusive)
    /// </summary>
    public int ToRow { get; set; }

    /// <summary>
    /// Number of seats in each row within this range
    /// </summary>
    public int SeatsPerRow { get; set; }
}
