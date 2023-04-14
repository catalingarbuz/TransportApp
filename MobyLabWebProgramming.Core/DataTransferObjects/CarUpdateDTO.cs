using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// this DTO is used to update a car
/// </summary>
/// <param name="Id"></param>
/// <param name="Brand"></param>
/// <param name="Model"></param>
/// <param name="RegistrationNumber"></param>
/// <param name="NumberOfSeats"></param>
public record CarUpdateDTO(Guid Id, string? Brand = default, string? Model = default, int? NumberOfSeats = default);
