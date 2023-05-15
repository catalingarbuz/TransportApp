using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

public class Route : BaseEntity
{
    public string RouteName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? RouteLength { get; set; } = default!;

    public ICollection<CarRoute> CarRoutes { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = default!;
}
