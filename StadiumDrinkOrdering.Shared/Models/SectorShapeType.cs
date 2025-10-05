namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Defines the geometric shape types supported for stadium sectors
/// </summary>
public enum SectorShapeType
{
    /// <summary>
    /// Standard rectangular sector (default)
    /// </summary>
    Rectangle,

    /// <summary>
    /// Three-sided polygon (triangle)
    /// </summary>
    Triangle,

    /// <summary>
    /// Four-sided parallelogram (rhombus/diamond)
    /// </summary>
    Rhombus,

    /// <summary>
    /// Circular arc sector (pizza slice shape)
    /// </summary>
    CircularSector,

    /// <summary>
    /// Custom polygon with user-defined vertices (3+ points)
    /// </summary>
    CustomPolygon
}
