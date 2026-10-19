using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class CarRouteEndpoints
    {
        public static IEndpointRouteBuilder MapCarRouteEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/CarRoute")
                           .WithTags("CarRoute");
            group.MapGet("GetById/{id:guid}", GetById)
                .WithName("GetCarRouteById")
                .Produces<RequestResponse<CarRoute>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<CarRoute>>(StatusCodes.Status400BadRequest);
            group.MapPost("Add", Add)
                .WithName("AddCarRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);
            group.MapPut("Update", Update)
                .WithName("UpdateCarRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);
            group.MapDelete("Delete/{id:guid}", Delete)
                .WithName("DeleteCarRoute")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);
            return app;
        }

        private static async Task<IResult> GetById(Guid id,
            ICarRouteService carRouteService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carRouteService.GetCarRoute(id)) :
                ControllerExtensions.ErrorMessageResult<CarRoute>(currentUser.Error);
        }

        private static async Task<IResult> Add([FromBody] CarRouteAddDTO carRoute,
            ICarRouteService carRouteService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carRouteService.AddCarRoute(carRoute, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Update([FromBody] CarRouteUpdateDTO carRoute,
            ICarRouteService carRouteService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carRouteService.UpdateCarRoute(carRoute, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Delete(Guid id,
            ICarRouteService carRouteService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carRouteService.DeleteCarRoute(id, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

    }
}
