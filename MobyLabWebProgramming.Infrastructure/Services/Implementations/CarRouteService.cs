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

public class CarRouteService : ICarRouteService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public CarRouteService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<CarRoute>> GetCarRoute(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync<CarRoute>(id, cancellationToken);
        
        return result != null ?
            ServiceResponse<CarRoute>.ForSuccess(result) :
            ServiceResponse<CarRoute>.FromError(CommonErrors.CarRouteNotFound);
    }

    public async Task<ServiceResponse> AddCarRoute(CarRouteAddDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the driver
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add car routes !", ErrorCodes.CannotAdd));
        }

        var result = _repository.DbContext.Set<CarRoute>()
            .Where(e => e.CarId == carRoute.CarId && e.RouteId == carRoute.RouteId)
            .Count();
        
        if (result != 0)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The car route already exists!", ErrorCodes.CarAlreadyExists));
        }

        await _repository.AddAsync(new CarRoute
        {
            CarId = carRoute.CarId,
            RouteId = carRoute.RouteId
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateCarRoute(CarRouteUpdateDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the car routes !", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync<CarRoute>(carRoute.Id, cancellationToken);

        if (entity != null)
        {
            entity.CarId = carRoute.CarId ?? entity.CarId;
            entity.RouteId = carRoute.RouteId ?? entity.RouteId;

            await _repository.UpdateAsync(entity, cancellationToken);
        } else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Car route not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteCarRoute(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete car routes !", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<CarRoute>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
