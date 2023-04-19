using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a booking within the application and to client application.
/// </summary>
public class BookingAddDTO
{
    public Guid UserId { get; set; }
    public Guid DriverId { get; set; }
    public Guid CarId { get; set; }
    public Guid RouteId { get; set; }

    public DateTime BookingDate { get; set; } = default;
    public DateTime DepartureDate { get; set; } = default;
    public string DeparturePlace { get; set; } = default!;
    public string ArrivalPlace { get; set; } = default!;
}
