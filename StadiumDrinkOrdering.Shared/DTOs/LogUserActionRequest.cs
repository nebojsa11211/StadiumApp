namespace StadiumDrinkOrdering.Shared.DTOs
{
    public class LogUserActionRequest
    {
        public string Action { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }
        public string? Details { get; set; }
        public string? RequestPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? Source { get; set; }
        
        // Business Event Fields
        public string? BusinessEntityType { get; set; }
        public string? BusinessEntityId { get; set; }
        public string? BusinessEntityName { get; set; }
        public string? RelatedEntityType { get; set; }
        public string? RelatedEntityId { get; set; }
        public decimal? MonetaryAmount { get; set; }
        public string? Currency { get; set; }
        public int? Quantity { get; set; }
        public string? LocationInfo { get; set; }
        public string? StatusBefore { get; set; }
        public string? StatusAfter { get; set; }
        public string? MetadataJson { get; set; }
    }
}