namespace StadiumDrinkOrdering.Shared.DTOs;

/// <summary>
/// Request for a runner to claim several Ready orders from the shared delivery pool in one action
/// (a runner grabbing multiple prepared drinks for a single trip).
/// </summary>
public class BatchClaimRequestDto
{
    public List<int> OrderIds { get; set; } = new();
}

/// <summary>
/// Per-order outcome of a batch claim. An order lands in exactly one list: successfully claimed
/// (now OutForDelivery and assigned to this runner), already taken by another runner / no longer
/// Ready, or not found.
/// </summary>
public class BatchClaimResultDto
{
    public List<int> Claimed { get; set; } = new();
    public List<int> Taken { get; set; } = new();
    public List<int> NotFound { get; set; } = new();
}
