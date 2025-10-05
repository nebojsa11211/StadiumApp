using StadiumDrinkOrdering.Shared.Models;
using System.Text;

namespace StadiumDrinkOrdering.Shared.Services;

/// <summary>
/// Utility class for generating SVG path data from stadium sector shapes
/// </summary>
public static class SvgPathGenerator
{
    /// <summary>
    /// Generates an SVG path string for the given stadium sector overlay
    /// </summary>
    /// <param name="sector">The sector overlay with shape definition</param>
    /// <returns>SVG path data string (d attribute value)</returns>
    public static string GeneratePath(StadiumSectorOverlay sector)
    {
        if (sector == null)
            throw new ArgumentNullException(nameof(sector));

        return sector.ShapeTypeEnum switch
        {
            SectorShapeType.Rectangle => GenerateRectanglePath(sector),
            SectorShapeType.Triangle => GeneratePolygonPath(sector.VertexCoordinates),
            SectorShapeType.Rhombus => GeneratePolygonPath(sector.VertexCoordinates),
            SectorShapeType.CustomPolygon => GeneratePolygonPath(sector.VertexCoordinates),
            SectorShapeType.CircularSector => GenerateCircularSectorPath(sector),
            _ => GenerateRectanglePath(sector) // Fallback to rectangle
        };
    }

    /// <summary>
    /// Generates SVG path for a rectangular sector (backward compatible)
    /// </summary>
    private static string GenerateRectanglePath(StadiumSectorOverlay sector)
    {
        // Rectangle can be drawn with four points
        var vertices = new List<VertexCoordinate>
        {
            new(sector.LeftPercent, sector.TopPercent), // Top-left
            new(sector.LeftPercent + sector.WidthPercent, sector.TopPercent), // Top-right
            new(sector.LeftPercent + sector.WidthPercent, sector.TopPercent + sector.HeightPercent), // Bottom-right
            new(sector.LeftPercent, sector.TopPercent + sector.HeightPercent) // Bottom-left
        };

        return GeneratePolygonPath(vertices);
    }

    /// <summary>
    /// Generates SVG path for polygon-based shapes (triangle, rhombus, custom polygon)
    /// </summary>
    private static string GeneratePolygonPath(List<VertexCoordinate>? vertices)
    {
        if (vertices == null || vertices.Count < 3)
            return string.Empty;

        var sb = new StringBuilder();

        // Move to first point
        sb.Append($"M {vertices[0].X:F2} {vertices[0].Y:F2}");

        // Line to each subsequent point
        for (int i = 1; i < vertices.Count; i++)
        {
            sb.Append($" L {vertices[i].X:F2} {vertices[i].Y:F2}");
        }

        // Close the path (line back to first point)
        sb.Append(" Z");

        return sb.ToString();
    }

    /// <summary>
    /// Generates SVG path for circular sector (pizza slice shape)
    /// </summary>
    private static string GenerateCircularSectorPath(StadiumSectorOverlay sector)
    {
        // Parse metadata for circular sector parameters
        // Expected format: { "centerX": 50, "centerY": 50, "radius": 30, "startAngle": 0, "endAngle": 90 }

        if (string.IsNullOrEmpty(sector.ShapeData))
            return string.Empty;

        try
        {
            var metadata = System.Text.Json.JsonSerializer.Deserialize<CircularSectorMetadata>(sector.ShapeData);
            if (metadata == null)
                return string.Empty;

            return GenerateArcPath(
                metadata.CenterX,
                metadata.CenterY,
                metadata.Radius,
                metadata.StartAngle,
                metadata.EndAngle
            );
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Generates SVG arc path for circular sector
    /// </summary>
    private static string GenerateArcPath(double centerX, double centerY, double radius, double startAngle, double endAngle)
    {
        // Convert angles to radians
        double startRad = startAngle * Math.PI / 180.0;
        double endRad = endAngle * Math.PI / 180.0;

        // Calculate start and end points on the arc
        double x1 = centerX + radius * Math.Cos(startRad);
        double y1 = centerY + radius * Math.Sin(startRad);
        double x2 = centerX + radius * Math.Cos(endRad);
        double y2 = centerY + radius * Math.Sin(endRad);

        // Determine if arc should be drawn as large arc (> 180 degrees)
        int largeArc = (endAngle - startAngle) > 180 ? 1 : 0;

        // SVG path format: M centerX,centerY L x1,y1 A radius,radius 0 largeArc,1 x2,y2 Z
        var sb = new StringBuilder();
        sb.Append($"M {centerX:F2} {centerY:F2}"); // Move to center
        sb.Append($" L {x1:F2} {y1:F2}"); // Line to start of arc
        sb.Append($" A {radius:F2} {radius:F2} 0 {largeArc} 1 {x2:F2} {y2:F2}"); // Arc to end
        sb.Append(" Z"); // Close path (line back to center)

        return sb.ToString();
    }

    /// <summary>
    /// Calculates the bounding box (top, left, width, height) for a polygon
    /// </summary>
    public static (double Top, double Left, double Width, double Height) CalculateBoundingBox(List<VertexCoordinate> vertices)
    {
        if (vertices == null || vertices.Count == 0)
            return (0, 0, 0, 0);

        double minX = vertices.Min(v => v.X);
        double maxX = vertices.Max(v => v.X);
        double minY = vertices.Min(v => v.Y);
        double maxY = vertices.Max(v => v.Y);

        return (
            Top: minY,
            Left: minX,
            Width: maxX - minX,
            Height: maxY - minY
        );
    }
}

/// <summary>
/// Metadata for circular sector shapes
/// </summary>
public class CircularSectorMetadata
{
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double Radius { get; set; }
    public double StartAngle { get; set; }
    public double EndAngle { get; set; }
}
