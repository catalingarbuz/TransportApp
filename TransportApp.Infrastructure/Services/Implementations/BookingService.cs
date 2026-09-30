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

public class BookingService : IBookingService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public BookingService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<BookingDTO>> GetBooking(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new BookingProjectionSpec(id), cancellationToken);
        
        return result != null ?
            ServiceResponse<BookingDTO>.ForSuccess(result) :
            ServiceResponse<BookingDTO>.FromError(CommonErrors.BookingNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<BookingDTO>>> GetBookings(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new BookingProjectionSpec(pagination.Search, false), cancellationToken);  

        return ServiceResponse<PagedResponse<BookingDTO>>.ForSuccess(result);     
    }

    public int GetBookingsCountForCarAndRouteAndDepartureDate(Guid carId, Guid routeId, DateTime departureDate, CancellationToken cancellationToken = default)
    {
        var count = _repository.DbContext.Set<Booking>()
            .Where(b => b.CarId == carId && b.RouteId == routeId && b.Route.DepartureTime == departureDate)
            .Count();

        return count;
    }

    public async Task<ServiceResponse> AddBooking(BookingAddDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Error at adding booking!", ErrorCodes.CannotAdd));
        }

        var route = _repository.DbContext.Set<Route>()
            .FirstOrDefault(d => d.StartingLocation.City == booking.DeparturePlace);

        if (route == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Route not found", ErrorCodes.EntityNotFound));
        }

        var carRoute = _repository.DbContext.Set<CarRoute>()
            .Where(cr => cr.RouteId == route.Id).ToList();

        Guid carId = Guid.Empty;
        foreach(var cr in carRoute)
        {
            var car = _repository.DbContext.Set<Car>()
                .FirstOrDefault(c => c.Id == cr.CarId);

            if (car == null)
                continue;

            var bookedSeats = GetBookingsCountForCarAndRouteAndDepartureDate(cr.CarId, route.Id, booking.DepartureDate, cancellationToken);

            if (car.NumberOfSeats - bookedSeats > 0)
            {
                carId = car.Id;
                break;
            }            
        }

        if (carId == Guid.Empty)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "No available free seats for the current selection!", ErrorCodes.NoAvailableSeats));
        }

        await _repository.AddAsync(new Booking
        {
            UserId = requestingUser.Id,
            DriverId = booking.DriverId,
            CarId = carId,
            RouteId = route.Id,
            BookingDate = DateTime.Now    
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateBooking(BookingUpdateDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Error at updating booking!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new BookingSpec(booking.Id), cancellationToken);

        if (entity != null)
        {
            entity.BookingDate = booking.BookingDate ?? entity.BookingDate;
            entity.DriverId = booking.DriverId ?? entity.DriverId;
            entity.CarId = booking.CarId ?? entity.CarId;
            entity.RouteId = booking.RouteId ?? entity.RouteId;

            await _repository.UpdateAsync(entity, cancellationToken);
        } else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Booking not found", ErrorCodes.EntityNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteBooking(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) 
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the booking!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Booking>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}

