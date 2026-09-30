using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a route within the application and to client application.
/// </summary>
public class RouteDTO
{
    public Guid Id { get; set; }
    public string StartingLocationCity { get; set; } = default!;
    public string StartingLocationCountry { get; set; } = default!;
    public string FinalLocationCity { get; set; } = default!;
    public string FinalLocationCountry { get; set; } = default!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}

