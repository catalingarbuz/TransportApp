using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class CarRoute : BaseEntity
{
    public Guid CarId { get; set; }
    public Car Car { get; set; }

    public Guid RouteId { get; set; }
    public Route Route { get; set; }
}
