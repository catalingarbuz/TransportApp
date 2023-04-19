using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a carRoute, note that it doesn't have an id property because the id for the user entity should be added by the application.
/// </summary>
public class CarRouteAddDTO
{
    public Guid CarId { get; set; }
    public Guid RouteId { get; set; }
}
