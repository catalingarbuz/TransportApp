using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="RouteName"></param>
/// <param name="Description"></param>
/// <param name="RouteLength"></param>
/// <param name="CarIds"></param>
public record RouteUpdateDTO(Guid Id, Guid? StartingLocationId, Guid? FinalLocationId, DateTime? DepartureTime, DateTime? ArrivalTime, List<Guid?> CarIds);

