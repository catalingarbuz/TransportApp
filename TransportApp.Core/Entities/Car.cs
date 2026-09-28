using TransportApp.Core.Enums;

namespace TransportApp.Core.Entities;

public class Car : BaseEntity
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;
    public int NumberOfSeats { get; set; } = default!; 

    public ICollection<Booking> Bookings { get; set; } = default!;
    public ICollection<CarRoute> CarRoutes { get; set; } = default!;
}

