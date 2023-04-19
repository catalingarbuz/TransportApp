using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

public class CarRoute : BaseEntity
{
    public Guid CarId { get; set; }
    public Car Car { get; set; }

    public Guid RouteId { get; set; }
    public Route Route { get; set; }
}
