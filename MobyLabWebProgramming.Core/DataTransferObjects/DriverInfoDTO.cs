using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// </summary>
public class DriverInfoDTO
{
    public int CompletedTrips { get; set; } = default!;
    public int YearExperience { get; set; } = default!;
    public DateTime BirthDate { get; set; } = default!;
    public DriverDTO Driver { get; set; } = default!;
}
