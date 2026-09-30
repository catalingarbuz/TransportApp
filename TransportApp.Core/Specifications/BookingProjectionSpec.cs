using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Specifications;

public sealed class BookingProjectionSpec : BaseSpec<BookingProjectionSpec, Booking, BookingDTO>
{
    protected override Expression<Func<Booking, BookingDTO>> Spec => e => new()
    {
        Id = e.Id,
        BookingDate = e.BookingDate,
        DepartureDate = e.Route != null ? e.Route.DepartureTime : default,
        UserId = e.UserId,
        DriverId = e.DriverId,
        CarId = e.CarId,
        RouteId = e.RouteId
    };

    public BookingProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public BookingProjectionSpec(Guid id) : base(id)
    {
    }

    public BookingProjectionSpec(string? search, bool fake = false)
    {
        if (search == null)
        {
            return;
        }

        // filter using the navigation property on Route instead of a non-existent Booking.DeparturePlace
        Query.Include(b => b.Route)
             .Where(e => e.Route != null && e.Route.StartingLocation != null && e.Route.StartingLocation.City == search);
    }

}

