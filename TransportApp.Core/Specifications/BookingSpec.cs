using TransportApp.Core.Entities;
using Ardalis.Specification;

namespace TransportApp.Core.Specifications;

/// <summary>
/// This is a simple specification to filter the Booking entities from the database via the constructors.
/// Note that this is a sealed class, meaning it cannot be further derived.
/// </summary>
public sealed class BookingSpec : BaseSpec<BookingSpec, Booking>
{
    public BookingSpec(Guid id) : base(id)
    {
    }

}
