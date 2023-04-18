using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to add a user file, it contains a IFormFile that has the stream to the file in a form and an additional property.
/// </summary>
public class DriverInfoAddDTO
{
    public int CompletedTrips { get; set; } = default!;
    public int YearExperience { get; set; } = default!;
    public DateTime BirthDate { get; set; } = default!;
    public Guid DriverId { get; set; }
}
