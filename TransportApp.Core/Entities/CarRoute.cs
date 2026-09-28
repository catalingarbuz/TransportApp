using TransportApp.Core.Enums;

namespace TransportApp.Core.Entities;

public class CarRoute : BaseEntity
{
    public Guid CarId { get; set; }
    public Car Car { get; set; }

    public Guid RouteId { get; set; }
    public Route Route { get; set; }
}

