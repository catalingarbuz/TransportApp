using Microsoft.AspNetCore.Mvc;
using TransportApp.Backend.Infrastructure;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Controllers
{
    public static class RouteEndpoints
    {
        public static IEndpointRouteBuilder MapRouteEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Route")
                           .RequireAuthorization()
                           .WithTags("Route");

            group.MapGet("GetById/{id:guid}", GetById)
                .WithName("GetRouteById")
                .Produces<RequestResponse<RouteDTO>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<RouteDTO>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetPage", GetPage)
                .WithName("GetRoutesPage")
                .Produces<RequestResponse<PagedResponse<RouteDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<RouteDTO>>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetRoutesWithLocationsDictionary", GetRoutesWithLocationsDictionary)
                .WithName("GetRoutesWithLocationsDictionary")
                .Produces<RequestResponse<Dictionary<string, List<RouteDTO>>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<Dictionary<string, List<RouteDTO>>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                .WithName("AddRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", Update)
                .WithName("UpdateRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", Delete)
                .WithName("DeleteRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetById(Guid id,
            IRouteService routeService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await routeService.GetRoute(id)) :
                ControllerExtensions.ErrorMessageResult<RouteDTO>(currentUser.Error);
        }

        private static async Task<IResult> GetPage([AsParameters] PaginationSearchQueryParams pagination,
            IRouteService routeService,
            IUserContext userContext) 
                                                                                                                                              
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await routeService.GetRoutes(pagination)) :
                ControllerExtensions.ErrorMessageResult<PagedResponse<RouteDTO>>(currentUser.Error);
        }

        private static async Task<IResult> GetRoutesWithLocationsDictionary(IRouteService routeService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await routeService.GetRoutesWithLocationsDictionary()) :
                ControllerExtensions.ErrorMessageResult<Dictionary<string, List<RouteDTO>>>(currentUser.Error);
        }

        private static async Task<IResult> Add([FromBody] RouteAddDTO route,
            IRouteService routeService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            if (currentUser.Result != null)
            {
                return ControllerExtensions.FromServiceResponse(await routeService.AddRoute(route, currentUser.Result));
            }

            return ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Update([FromBody] RouteUpdateDTO route,
            IRouteService routeService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await routeService.UpdateRoute(route, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Delete([FromRoute] Guid id, IRouteService routeService, IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await routeService.DeleteRoute(id)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
