using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Car : BaseEntity
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;
    public int NumberOfSeats { get; set; } = default!; 

    public ICollection<Booking> Bookings { get; set; } = default!;
    public ICollection<CarRoute> CarRoutes { get; set; } = default!;
}
