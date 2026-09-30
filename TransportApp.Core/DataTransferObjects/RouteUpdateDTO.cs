using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="RouteName"></param>
/// <param name="Description"></param>
/// <param name="RouteLength"></param>
public record RouteUpdateDTO(Guid Id, Guid? StartingLocationId, Guid? FinalLocationId, DateTime? DepartureTime, DateTime? ArrivalTime);

