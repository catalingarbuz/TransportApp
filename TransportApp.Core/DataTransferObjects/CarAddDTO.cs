using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a car, note that it doesn't have an id property because the id for the user entity should be added by the application.
/// </summary>
public class CarAddDTO
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;
    public int NumberOfSeats { get; set; } = default!;
    public Guid? DriverId { get; set; } = default!;
}

