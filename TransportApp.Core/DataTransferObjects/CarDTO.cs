using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a car within the application and to client application.
/// </summary>
public class CarDTO
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;
    public int NumberOfSeats { get; set; } = default!;
}

