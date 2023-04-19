using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

//<summary>
//This DTO is used to transfer information about a booking within the application and to client application.
//</summary>
public record BookingUpdateDTO(Guid Id,
    DateTime? BookingDate = default,
    DateTime? DepartureDate = default,
    string? DeparturePlace = default,
    string? ArrivalPlace = default,
    Guid? DriverId = default,
    Guid? CarId = default,
    Guid? RouteId = default);

