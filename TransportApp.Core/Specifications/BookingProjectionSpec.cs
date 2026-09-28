using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Specifications;

/// <summary>
/// This is a specification to filter the booking entities and map it to and BookingDTO object via the constructors.
/// Note how the constructors call the base class's constructors. Also, this is a sealed class, meaning it cannot be further derived.
/// </summary>
public sealed class BookingProjectionSpec : BaseSpec<BookingProjectionSpec, Booking, BookingDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get BookingDTO object from the database.
    /// </summary>
    protected override Expression<Func<Booking, BookingDTO>> Spec => e => new()
    {
        Id = e.Id,
        BookingDate = e.BookingDate,
        DeparturePlace = e.DeparturePlace,
        DepartureDate = e.DepartureDate,
        ArrivalPlace = e.ArrivalPlace,
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

        Query.Where(e => e.DeparturePlace == search); // This is an example on who database specific expressions can be used via C# expressions.                                                                  // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

}

