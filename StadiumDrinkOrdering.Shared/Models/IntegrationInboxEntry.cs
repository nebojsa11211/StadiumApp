using System;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Models;

/// <summary>
/// Idempotency ledger for inbound integration webhooks (e.g. the external ticketing
/// system). Every processed webhook records its <see cref="IdempotencyKey"/> here so
/// retries/duplicates are recognised and skipped rather than double-applied.
/// </summary>
public class IntegrationInboxEntry
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string IdempotencyKey { get; set; } = string.Empty;

    [StringLength(50)]
    public string EventType { get; set; } = string.Empty;

    [StringLength(100)]
    public string SourceSystem { get; set; } = string.Empty;

    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Processed | Failed</summary>
    [StringLength(50)]
    public string Status { get; set; } = "Processed";

    [StringLength(500)]
    public string? Notes { get; set; }
}
