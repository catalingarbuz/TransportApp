using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a route, note that it doesn't have an id property because the id for the user entity should be added by the application.
/// </summary>
public class RouteAddDTO
{
    public string RouteName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? RouteLength { get; set; } = default!;
}
