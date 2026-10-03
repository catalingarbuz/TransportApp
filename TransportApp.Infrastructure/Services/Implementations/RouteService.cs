using System.Net;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ServiceResponse<Dictionary<string, List<RouteDTO>>>> GetRoutesWithLocationsDictionary(CancellationToken cancellationToken = default)
    {
        var allRoutes = await _repository.ListAsync(new RouteProjectionSpec(null, false), cancellationToken);

        var dictionary = new Dictionary<string, List<RouteDTO>>();

        foreach (var route in allRoutes)
        {
            var key = $"{route.StartingLocationCity}, {route.StartingLocationCountry}";

            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<RouteDTO>();
            }

            dictionary[key].Add(route);
        }

        return ServiceResponse<Dictionary<string, List<RouteDTO>>>.ForSuccess(dictionary);
    }

    public async Task<ServiceResponse<RouteDTO>> GetRoute(Guid id, CancellationToken cancellationToken = default)
    {
        RouteDTO? result = await _repository.GetAsync(new RouteProjectionSpec(id), cancellationToken);

        if (result != null)
        {
            var carRoutes = await _repository.DbContext.Set<CarRoute>()
                .Where(cr => cr.RouteId == result.Id)
                .ToListAsync(cancellationToken);
            var carIds = carRoutes.Select(cr => cr.CarId).ToList();
            var cars = new List<CarDTO>();
            foreach (var carId in carIds)
            {
                var car = await _repository.DbContext.Set<Car>()
                    .FirstOrDefaultAsync(c => c.Id == carId, cancellationToken);
                if (car != null)
                {
                    cars.Add(new CarDTO
                    {
                        Id = car.Id,
                        RegistrationNumber = car.RegistrationNumber,
                        Model = car.Model,
                        Brand = car.Brand,
                        NumberOfSeats = car.NumberOfSeats
                    });
                }
            }
            result.AssignedCars = cars;
        }

        return result != null ?
            ServiceResponse<RouteDTO>.ForSuccess(result) :
            ServiceResponse<RouteDTO>.FromError(CommonErrors.RouteNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<RouteDTO>>> GetRoutes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        PagedResponse<RouteDTO> result = await _repository.PageAsync(pagination, new RouteProjectionSpec(pagination.Search, false), cancellationToken);
        List<RouteDTO> routes = result.Data;

        // populate the AssignedCars property for each route
        foreach (var route in routes)
        {
            var carRoutes = await _repository.DbContext.Set<CarRoute>()
                .Where(cr => cr.RouteId == route.Id)
                .ToListAsync(cancellationToken);
            var carIds = carRoutes.Select(cr => cr.CarId).ToList();
            var cars = new List<CarDTO>();
            foreach (var carId in carIds)
            {
                var car = await _repository.DbContext.Set<Car>()
                    .FirstOrDefaultAsync(c => c.Id == carId, cancellationToken);
                if (car != null)
                {
                    cars.Add(new CarDTO
                    {
                        Id = car.Id,
                        RegistrationNumber = car.RegistrationNumber,
                        Model = car.Model,
                        Brand = car.Brand,
                        NumberOfSeats = car.NumberOfSeats
                    });
                }
            }
            route.AssignedCars = cars;
        }

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

        Route result = await _repository.AddAsync(new Route
        {
            StartingLocationId = startingLocation.Id,
            FinalLocationId = finalLocation.Id,
            DepartureTime = route.DepartureTime,
            ArrivalTime = route.ArrivalTime
        }, cancellationToken);

        if (route.CarIds != null && route.CarIds.Count > 0)
        {
            foreach (var carId in route.CarIds)
            {
                Car? carEntity = await _repository.GetAsync(new CarSpec(carId), cancellationToken);
                if (carEntity != null)
                {
                    await _repository.AddAsync(new CarRoute
                    {
                        CarId = carEntity.Id,
                        RouteId = result.Id
                    }, cancellationToken);
                }
            }
        }

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
            if (route.CarIds != null)
            {
                // Remove existing car routes
                var existingCarRoutes = _repository.DbContext.Set<CarRoute>().Where(cr => cr.RouteId == entity.Id);
                _repository.DbContext.Set<CarRoute>().RemoveRange(existingCarRoutes);
                // Add new car routes
                foreach (var carId in route.CarIds)
                {
                    Car? carEntity = await _repository.GetAsync(new CarSpec(carId.Value), cancellationToken);
                    if (carEntity != null)
                    {
                        await _repository.AddAsync(new CarRoute
                        {
                            CarId = carEntity.Id,
                            RouteId = entity.Id
                        }, cancellationToken);
                    }
                }

                await _repository.DbContext.SaveChangesAsync(cancellationToken);
            }
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

