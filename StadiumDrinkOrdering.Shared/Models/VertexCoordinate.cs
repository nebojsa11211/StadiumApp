using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Represents a single vertex point in a polygon-based stadium sector shape.
/// Coordinates are stored as percentages (0-100) for responsive scaling.
/// </summary>
public class VertexCoordinate
{
    /// <summary>
    /// X coordinate as percentage of canvas width (0-100)
    /// </summary>
    [Required]
    [Range(0, 100)]
    public double X { get; set; }

    /// <summary>
    /// Y coordinate as percentage of canvas height (0-100)
    /// </summary>
    [Required]
    [Range(0, 100)]
    public double Y { get; set; }

    /// <summary>
    /// Optional: Sequence order for ordered vertex lists
    /// </summary>
    public int? Order { get; set; }

    public VertexCoordinate() { }

    public VertexCoordinate(double x, double y)
    {
        X = x;
        Y = y;
    }

    public VertexCoordinate(double x, double y, int order)
    {
        X = x;
        Y = y;
        Order = order;
    }

    public override string ToString()
    {
        return $"({X:F2}, {Y:F2})";
    }
}
