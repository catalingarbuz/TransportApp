using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Specifications;

public sealed class CarProjectionSpec : BaseSpec<CarProjectionSpec, Car, CarDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get carDTO object from the database.
    /// </summary>
    protected override Expression<Func<Car, CarDTO>> Spec => e => new()
    {
        Id = e.Id,
        Brand = e.Brand,
        Model = e.Model,
        RegistrationNumber = e.RegistrationNumber,
        NumberOfSeats = e.NumberOfSeats
    };

    public CarProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public CarProjectionSpec(Guid id) : base(id)
    {
    }

    public CarProjectionSpec(string? search, bool fake = false)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.RegistrationNumber, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
                                                                  // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }
}

