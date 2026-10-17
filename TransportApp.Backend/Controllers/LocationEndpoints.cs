using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Services.Interfaces;
namespace TransportApp.Backend.Controllers;

public static class LocationEndpoints
{
    // The 'this' keyword makes this an extension method on IEndpointRouteBuilder
    public static IEndpointRouteBuilder MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/Location")
                       .RequireAuthorization()
                       .WithTags("Location");

        group.MapGet("Get", GetLocationsAsync)
             .WithName("GetLocations");

        return app;
    }

    // Keep the handler logic cleanly separated in its own private method
    private static async Task<IResult> GetLocationsAsync(
        ILocationService locationService,
        IUserService userService,
        IUserContext userContext,
        CancellationToken cancellationToken)
    {
        var currentUser = await userContext.GetCurrentUserAsync();

        if (currentUser.Result == null)
        {
            return Results.BadRequest(RequestResponse<List<LocationDTO>>.FromError(currentUser.Error));
        }

        ServiceResponse<List<LocationDTO>> serviceResponse = await locationService.GetLocations(cancellationToken);

        return serviceResponse.IsOk
            ? Results.Ok(serviceResponse.Result)
            : Results.BadRequest(serviceResponse);
    }
}