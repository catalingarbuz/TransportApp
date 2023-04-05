using System.Net;
using MobyLabWebProgramming.Core.Constants;
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

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

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
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != driver.Id) // Verify who can add the user, you can change this however you se fit.
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

        await _repository.DeleteAsync<Driver>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
