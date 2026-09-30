using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Specifications;

public sealed class LocationProjectionSpec : BaseSpec<LocationProjectionSpec, Location, LocationDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get LocationDTO object from the database.
    /// </summary>
    protected override Expression<Func<Location, LocationDTO>> Spec => e => new()
    {
        Id = e.Id,
        City = e.City,
        Country = e.Country,
        Adress = e.Adress
    };

    public LocationProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public LocationProjectionSpec(Guid id) : base(id)
    {
    }

    public LocationProjectionSpec(string? search, bool fake = false)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.City, searchExpr));
    }
}

