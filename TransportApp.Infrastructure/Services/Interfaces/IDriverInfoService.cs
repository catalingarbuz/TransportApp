using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;

namespace TransportApp.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IDriverInfoService
{
    /// <summary>
    /// AddDriverInfo adds driver info
    /// </summary>
    public Task<ServiceResponse> AddDriverInfo(DriverInfoAddDTO driverInfo, UserDTO requestingUser, CancellationToken cancellationToken = default);
    /// <summary>
    /// GetDriverInfo gets a driver info from database.
    /// </summary>
    public Task<ServiceResponse<DriverInfoDTO>> GetDriverInfo(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// UpdateDriverInfo updates driver info for a specific driver
    /// </summary>
    public Task<ServiceResponse> UpdateDriverInfo(DriverInfoUpdateDTO driverInfo, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// DeleteDriverInfo deletes driver info for a specific driver
    /// </summary>
    public Task<ServiceResponse> DeleteDriverInfo(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}

