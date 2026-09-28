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
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the car route
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
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can update the car route, you can change this however you se fit.
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

