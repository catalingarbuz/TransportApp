using TransportApp.Core.Entities;
using Ardalis.Specification;

namespace TransportApp.Core.Specifications;

/// <summary>
/// This is a simple specification to filter the Car entities from the database via the constructors.
/// Note that this is a sealed class, meaning it cannot be further derived.
/// </summary>
public sealed class CarSpec : BaseSpec<CarSpec, Car>
{
    public CarSpec(Guid id) : base(id)
    {
    }

    public CarSpec(string registrationNumber)
    {
        Query.Where(e => e.RegistrationNumber == registrationNumber);
    }
}
