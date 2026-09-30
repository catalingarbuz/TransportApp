using System.Net;
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

public class RouteService : IRouteService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public RouteService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<RouteDTO>> GetRoute(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new RouteProjectionSpec(id), cancellationToken);
        
        return result != null ?
            ServiceResponse<RouteDTO>.ForSuccess(result) :
            ServiceResponse<RouteDTO>.FromError(CommonErrors.RouteNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<RouteDTO>>> GetRoutes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new RouteProjectionSpec(pagination.Search, false), cancellationToken);  

        return ServiceResponse<PagedResponse<RouteDTO>>.ForSuccess(result);     
    }

    public async Task<ServiceResponse> AddRoute(RouteAddDTO route, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the route
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add routes !", ErrorCodes.CannotAdd));
        }

        // Get the starting location
        var startingLocation = _repository.DbContext.Set<Location>()
            .FirstOrDefault(l => l.City == route.StartingLocationCity && l.Country == route.StartingLocationCountry);

        if (startingLocation == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Starting location not found", ErrorCodes.EntityNotFound));
        }

        // Get the final location
        var finalLocation = _repository.DbContext.Set<Location>()
            .FirstOrDefault(l => l.City == route.FinalLocationCity && l.Country == route.FinalLocationCountry);

        if (finalLocation == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Final location not found", ErrorCodes.EntityNotFound));
        }

        // Check if route already exists with these locations
        RouteDTO? existingRoute = await _repository.GetAsync(new RouteProjectionSpec(startingLocation.Id, finalLocation.Id), cancellationToken);

        if (existingRoute != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The route already exists!", ErrorCodes.RouteAlreadyExists));
        }

        await _repository.AddAsync(new Route
        {
            StartingLocationId = startingLocation.Id,
            FinalLocationId = finalLocation.Id,
            DepartureTime = route.DepartureTime,
            ArrivalTime = route.ArrivalTime
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateRoute(RouteUpdateDTO route, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can update the route, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the route!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new RouteSpec(route.Id), cancellationToken);

        if (entity != null)
        {
            entity.StartingLocationId = route.StartingLocationId ?? entity.StartingLocationId;
            entity.FinalLocationId = route.FinalLocationId ?? entity.FinalLocationId;
            entity.DepartureTime = route.DepartureTime ?? entity.DepartureTime;
            entity.ArrivalTime = route.ArrivalTime ?? entity.ArrivalTime;

            await _repository.UpdateAsync(entity, cancellationToken);
        } else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Route not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteRoute(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the route!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Route>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}

