using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a route within the application and to client application.
/// </summary>
public class RouteDTO
{
    public Guid Id { get; set; }
    public string RouteName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? RouteLength { get; set; } = default!;
}
