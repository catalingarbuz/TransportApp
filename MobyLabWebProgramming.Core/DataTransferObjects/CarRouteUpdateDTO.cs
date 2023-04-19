using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// this DTO is used to update a carroute
/// </summary>
public record CarRouteUpdateDTO(Guid Id, Guid? RouteId, Guid? CarId);
