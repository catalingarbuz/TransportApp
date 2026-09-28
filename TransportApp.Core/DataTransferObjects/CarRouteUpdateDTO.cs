using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// this DTO is used to update a carroute
/// </summary>
public record CarRouteUpdateDTO(Guid Id, Guid? RouteId, Guid? CarId);

