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

        var result = await _repository.GetAsync(new RouteProjectionSpec(route.RouteName), cancellationToken);
        
        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The route already exists!", ErrorCodes.RouteAlreadyExists));
        }

        await _repository.AddAsync(new Route
        {
            RouteName = route.RouteName,
            Description = route.Description
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
            entity.RouteName = route.RouteName ?? entity.RouteName;
            entity.Description = route.Description ?? entity.Description;

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
