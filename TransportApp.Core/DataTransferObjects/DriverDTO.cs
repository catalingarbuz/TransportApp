using TransportApp.Core.Entities;
using TransportApp.Core.Enums;

namespace TransportApp.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a driver within the application and to client application.
/// </summary>
public class DriverDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public UserRoleEnum Role { get; set; } = default!;

    public DriverDTO(Driver driver)
    {
        Id = driver.Id;
        Name = driver.Name;
        Email = driver.Email;
        PhoneNumber = driver.PhoneNumber;
        Role = driver.Role;
    }
}

