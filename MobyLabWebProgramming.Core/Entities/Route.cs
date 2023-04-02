using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Route : BaseEntity
{
    public string RouteName { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<CarRoute> CarRoutes { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = default!;
}
