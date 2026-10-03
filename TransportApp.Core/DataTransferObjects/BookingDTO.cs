using TransportApp.Core.Entities;
using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a booking within the application and to client application.
/// </summary>
public class BookingDTO
{
    public Guid Id { get; set; }
    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;
    public string StartingLocationCity { get; set; } = default!;
    public string StartingLocationCountry { get; set; } = default!;
    public string FinalLocationCity { get; set; } = default!;
    public string FinalLocationCountry { get; set; } = default!;

    public Guid UserId { get; set; }
    public Guid DriverId { get; set; }
    public Guid CarId { get; set; }
    public Guid RouteId { get; set; }
}

