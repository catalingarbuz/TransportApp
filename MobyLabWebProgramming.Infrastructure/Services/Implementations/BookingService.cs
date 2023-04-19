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
            .Where(b => b.CarId == carId && b.RouteId == routeId && b.DepartureDate == departureDate)
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
            .FirstOrDefault(d => d.Id == booking.RouteId);

        if (route == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Route not found", ErrorCodes.EntityNotFound));
        }

        var car = _repository.DbContext.Set<Car>()
            .FirstOrDefault(c => c.Id == booking.CarId);

        if (car == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Car not found", ErrorCodes.EntityNotFound));
        } 
        else
        {
            var bookedSeats = GetBookingsCountForCarAndRouteAndDepartureDate(booking.CarId, booking.RouteId, booking.DepartureDate, cancellationToken);

            if (car.NumberOfSeats - bookedSeats <= 0)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "No available free seats for the current selection!", ErrorCodes.NoAvailableSeats));
            }
        } 

        await _repository.AddAsync(new Booking
        {
            UserId = booking.UserId,
            DriverId = booking.DriverId,
            CarId = booking.CarId,
            RouteId = booking.RouteId,
            BookingDate = booking.BookingDate,
            DepartureDate = booking.DepartureDate,
            DeparturePlace = booking.DeparturePlace,
            ArrivalPlace = booking.ArrivalPlace     
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
            entity.DepartureDate = booking.DepartureDate ?? entity.DepartureDate;
            entity.DeparturePlace = booking.DeparturePlace ?? entity.DeparturePlace;
            entity.ArrivalPlace = booking.ArrivalPlace ?? entity.ArrivalPlace;
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
