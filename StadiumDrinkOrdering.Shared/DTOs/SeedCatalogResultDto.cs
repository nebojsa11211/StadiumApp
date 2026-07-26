namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Outcome of generating the default drink catalog (common categories + drinks).
/// The operation is idempotent, so "skipped" counts what already existed.
/// </summary>
public class SeedCatalogResultDto
{
    public int CategoriesCreated { get; set; }
    public int CategoriesSkipped { get; set; }
    public int DrinksCreated { get; set; }
    public int DrinksSkipped { get; set; }

    /// <summary>Names of the drinks actually added, for the admin confirmation message.</summary>
    public List<string> CreatedDrinkNames { get; set; } = new();
}
