using TransportApp.Core.Entities;
using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a booking within the application and to client application.
/// </summary>
public class BookingAddDTO
{
    public Guid DriverId { get; set; }

    public string RouteName { get; set; } = default!;
    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;
    public string DeparturePlace { get; set; } = default!;
    public string ArrivalPlace { get; set; } = default!;
}

