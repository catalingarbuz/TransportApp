namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a booking within the application and to client application.
/// </summary>
public class BookingAddDTO
{
    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;
    public Guid RouteId { get; set; }
}

