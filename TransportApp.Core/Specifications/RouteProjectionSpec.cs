using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Specifications;

public sealed class RouteProjectionSpec : BaseSpec<RouteProjectionSpec, Route, RouteDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get RouteDTO object from the database.
    /// </summary>
    protected override Expression<Func<Route, RouteDTO>> Spec => e => new()
    {
        Id = e.Id,
        Description = e.Description,
        RouteName = e.RouteName,
        RouteLength = e.RouteLength
    };

    public RouteProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public RouteProjectionSpec(Guid id) : base(id)
    {
    }

    public RouteProjectionSpec(string? search, bool fake = false)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.RouteName, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
                                                                  // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }
}

