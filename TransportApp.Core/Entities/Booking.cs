namespace TransportApp.Core.Entities;

public class Booking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RouteId { get; set; }

    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;

    public User User { get; set; } = default!;
    public Route Route { get; set; } = default!;
}

