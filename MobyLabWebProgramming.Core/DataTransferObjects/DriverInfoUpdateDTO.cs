using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// </summary>

public record DriverInfoUpdateDTO(Guid Id, int? CompletedTrips = default, int? YearExperience = default);

