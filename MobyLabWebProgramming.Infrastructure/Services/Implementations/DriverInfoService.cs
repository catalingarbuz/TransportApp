using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class DriverInfoService : IDriverInfoService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;


    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public DriverInfoService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<DriverInfoDTO>> GetDriverInfo(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new DriverInfoProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<DriverInfoDTO>.ForSuccess(result) :
            ServiceResponse<DriverInfoDTO>.FromError(CommonErrors.DriverInfoNotFound);
    }

    public async Task<ServiceResponse> AddDriverInfo(DriverInfoAddDTO driverInfo, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role == UserRoleEnum.Client) // Verify who can add the driver info
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin and driver can add driver info!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new DriverInfoProjectionSpec(driverInfo.DriverId, true), cancellationToken);
        var driver = await _repository.GetAsync(new DriverProjectionSpec(driverInfo.DriverId), cancellationToken);

        if (result != null)
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The driver info already exists it can be updated!", ErrorCodes.DriverInfoAlreadyExists));


        if (driver == null)
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, $"The driver with Id: {driverInfo.DriverId} doesn't exist!", ErrorCodes.EntityNotFound));


        await _repository.AddAsync(new DriverInfo
        {
            CompletedTrips = driverInfo.CompletedTrips,
            DriverId = driverInfo.DriverId,
            YearExperience = driverInfo.YearExperience,
            BirthDate = driverInfo.BirthDate
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateDriverInfo(DriverInfoUpdateDTO driverInfo, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can update the driver info, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the driver info!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new DriverInfoSpec(driverInfo.Id), cancellationToken);

        if (entity != null)
        {
            entity.CompletedTrips = driverInfo.CompletedTrips ?? entity.CompletedTrips;
            entity.YearExperience = driverInfo.YearExperience ?? entity.YearExperience;

            await _repository.UpdateAsync(entity, cancellationToken);
        } else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The driver info was not found !", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteDriverInfo(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the driver info!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<DriverInfo>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
