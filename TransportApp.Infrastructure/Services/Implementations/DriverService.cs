using System.Net;
using TransportApp.Core.Constants;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;
using TransportApp.Core.Enums;
using TransportApp.Core.Errors;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Core.Specifications;
using TransportApp.Infrastructure.Database;
using TransportApp.Infrastructure.Repositories.Interfaces;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Infrastructure.Services.Implementations;

public class DriverService : IDriverService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public DriverService(IRepository<WebAppDatabaseContext> repository, ILoginService loginService)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<DriverDTO>> GetDriver(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new DriverProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<DriverDTO>.ForSuccess(result) :
            ServiceResponse<DriverDTO>.FromError(CommonErrors.DriverNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<DriverDTO>>> GetDrivers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new DriverProjectionSpec(pagination.Search, false), cancellationToken);

        return ServiceResponse<PagedResponse<DriverDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddDriver(DriverAddDTO driver, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the driver
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add drivers!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new DriverProjectionSpec(driver.Email), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The driver already exists!", ErrorCodes.UserAlreadyExists));
        }

        await _repository.AddAsync(new Driver
        {
            Email = driver.Email,
            Name = driver.Name,
            Role = driver.Role,
            Password = driver.Password,
            PhoneNumber = driver.PhoneNumber,
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateDriver(DriverUpdateDTO driver, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can update the driver, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own user can update the user!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new DriverSpec(driver.Id), cancellationToken);

        if (entity != null)
        {
            entity.Name = driver.Name ?? entity.Name;
            entity.Password = driver.Password ?? entity.Password;
            entity.PhoneNumber = driver.PhoneNumber ?? entity.PhoneNumber;

            await _repository.UpdateAsync(entity, cancellationToken);
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteDriver(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin or the own driver can delete the driver!", ErrorCodes.CannotDelete));
        }

        var driver = await _repository.GetAsync(new DriverSpec(id), cancellationToken);
        if (driver != null)
        {
            var userEntity = await _repository.GetAsync(new UserSpec(driver.Email), cancellationToken);

            if (userEntity != null)
                await _repository.DeleteAsync<User>(userEntity.Id, cancellationToken);
        }

        await _repository.DeleteAsync<Driver>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}

