using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// </summary>
/// 
public record BookingUpdateDTO(Guid Id,
    DateTime? BookingDate = default,
    DateTime? DepartureDate = default,
    string? DeparturePlace = default,
    string? ArrivalPlace = default,
    Guid? DriverId = default,
    Guid? CarId = default,
    Guid? RouteId = default);

