namespace TransportApp.Core.Entities;

public class Route : BaseEntity
{
    public Guid StartingLocationId { get; set; }
    public Guid FinalLocationId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public Location StartingLocation { get; set; } = default!;
    public Location FinalLocation { get; set; } = default!;
    public ICollection<CarRoute> CarRoutes { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = default!;
}

