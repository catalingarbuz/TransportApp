using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Booking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid DriverId { get; set; }
    public Guid CarId { get; set; }
    public Guid RouteId { get; set; }

    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;
    public string DeparturePlace { get; set; } = default!;
    public string ArrivalPlace { get; set; } = default!;

    public User User { get; set; } = default!;
    public Driver Driver { get; set; } = default!;
    public Car Car { get; set; } = default!;
    public Route Route { get; set; } = default!;
}
