namespace StadiumDrinkOrdering.Shared.DTOs;

public class StadiumSeatDto
{
    public int Id { get; set; }
    public string Section { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public bool IsActive { get; set; }
    public bool HasActiveOrder { get; set; }
    public OrderDto? ActiveOrder { get; set; }
    public string SeatLabel => $"{Section}{RowNumber}-{SeatNumber}";
}

public class StadiumSectionDto
{
    public string SectionName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Color { get; set; } = "#007bff";
    public int TotalSeats { get; set; }
    public int ActiveOrders { get; set; }
    public List<StadiumSeatDto> Seats { get; set; } = new();
}

public class StadiumLayoutDto
{
    public List<StadiumSectionDto> Sections { get; set; } = new();
    public int TotalSeats { get; set; }
    public int ActiveOrders { get; set; }
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 600;
}

public class SeatOrderDto
{
    public StadiumSeatDto Seat { get; set; } = new();
    public OrderDto Order { get; set; } = new();
}
