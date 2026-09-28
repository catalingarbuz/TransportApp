using Microsoft.AspNetCore.Http;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used for driver info
/// </summary>
public class DriverInfoDTO
{
    public int CompletedTrips { get; set; } = default!;
    public int YearExperience { get; set; } = default!;
    public DateTime BirthDate { get; set; } = default!;
    public DriverDTO Driver { get; set; } = default!;
}

