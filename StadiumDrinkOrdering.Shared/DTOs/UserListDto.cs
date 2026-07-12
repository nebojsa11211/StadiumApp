using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class UserListDto
{
    public List<UserDto> Users { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class UserFilterDto
{
    public string? SearchTerm { get; set; }
    public UserRole? Role { get; set; }
    public UserRole? ExcludeRole { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// Restrict to customers linked to this event: they placed a drink order at it or bought a
    /// ticket for it (matched by email). Takes precedence over <see cref="SeasonId"/> when both are set.
    /// </summary>
    public int? EventId { get; set; }

    /// <summary>
    /// Restrict to customers linked to this season: they ordered/bought at any of its events, or
    /// hold a season pass for it.
    /// </summary>
    public int? SeasonId { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}