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

public class CarService : ICarService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public CarService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<CarDTO>> GetCar(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new CarProjectionSpec(id), cancellationToken);
        
        return result != null ?
            ServiceResponse<CarDTO>.ForSuccess(result) :
            ServiceResponse<CarDTO>.FromError(CommonErrors.CarNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<CarDTO>>> GetCars(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new CarProjectionSpec(pagination.Search, false), cancellationToken);  

        return ServiceResponse<PagedResponse<CarDTO>>.ForSuccess(result);     
    }

    public async Task<ServiceResponse> AddCar(CarAddDTO car, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role == UserRoleEnum.Client) // Verify who can add the car
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin and drivers can add cars!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new CarProjectionSpec(car.RegistrationNumber), cancellationToken);
        
        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The car already exists!", ErrorCodes.CarAlreadyExists));
        }

        await _repository.AddAsync(new Car
        {
            Brand = car.Brand,
            Model = car.Model,
            RegistrationNumber = car.RegistrationNumber,
            NumberOfSeats = car.NumberOfSeats,
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateCar(CarUpdateDTO car, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can update the car, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the car!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new CarSpec(car.Id), cancellationToken);

        if (entity != null)
        {
            entity.Brand = car.Brand ?? entity.Brand;
            entity.Model = car.Model ?? entity.Model;
            entity.NumberOfSeats = car.NumberOfSeats ?? entity.NumberOfSeats;

            await _repository.UpdateAsync(entity, cancellationToken);
        } else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Car not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteCar(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the car!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Car>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}
