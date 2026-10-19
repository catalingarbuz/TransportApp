using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Services.Interfaces;
using TransportApp.Infrastructure.Extensions;

namespace TransportApp.Backend.Endpoints;

public static class LocationEndpoints
{
    public static IEndpointRouteBuilder MapLocationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/Location")
                       .RequireAuthorization()
                       .WithTags("Location");

        group.MapGet("Get", GetLocationsAsync)
             .WithName("GetLocations")
             .Produces<RequestResponse<List<LocationDTO>>>(StatusCodes.Status200OK)
             .Produces<RequestResponse<List<LocationDTO>>>(StatusCodes.Status400BadRequest);

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
            return ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        ServiceResponse<List<LocationDTO>> serviceResponse = await locationService.GetLocations(cancellationToken);

        return ControllerExtensions.FromServiceResponse(serviceResponse);
    }
}