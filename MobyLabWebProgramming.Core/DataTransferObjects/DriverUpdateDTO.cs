using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to update information about a driver.
/// </summary>
public record DriverUpdateDTO(Guid Id, string? Name = default, string? Password = default, string ? PhoneNumber = default);
