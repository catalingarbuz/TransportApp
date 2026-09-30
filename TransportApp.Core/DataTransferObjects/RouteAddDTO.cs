using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a route, note that it doesn't have an id property because the id for the user entity should be added by the application.
/// </summary>
public class RouteAddDTO
{
    public string StartingLocationCity { get; set; } = default!;
    public string StartingLocationCountry { get; set; } = default!;
    public string FinalLocationCity { get; set; } = default!;
    public string FinalLocationCountry { get; set; } = default!;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}

