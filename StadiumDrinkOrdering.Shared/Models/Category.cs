using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StadiumDrinkOrdering.Shared.Models;

public class Category
{
    public int Id { get; set; }

    // Canonical/internal name (unique). Used for style mapping and as a fallback label.
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    // Optional friendly label shown to customers (e.g. Croatian "Pivo"). Falls back to Name.
    [StringLength(100)]
    public string? DisplayName { get; set; }

    // Emoji/icon shown next to the category (e.g. "🍺").
    [StringLength(16)]
    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    [JsonIgnore] // Prevent circular references in JSON serialization
    public virtual ICollection<Drink> Drinks { get; set; } = new List<Drink>();
}
