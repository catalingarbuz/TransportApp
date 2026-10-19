using Org.BouncyCastle.Utilities.Zlib;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class CarEndpoints
    {
        public static IEndpointRouteBuilder MapCarEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Car")
                           .WithTags("Car");

            group.MapGet("GetById/{id:guid}", GetById)
                .WithName("GetCarById")
                .Produces<RequestResponse<CarDTO>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<CarDTO>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetPage", GetPage)
                .WithName("GetCarsPage")
                .Produces<RequestResponse<PagedResponse<CarDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<CarDTO>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                .WithName("AddCar")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", Update)
                .WithName("UpdateCar")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", Delete)
                .WithName("DeleteCar")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetById(Guid id,
            ICarService carService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carService.GetCar(id)) :
                ControllerExtensions.ErrorMessageResult<CarDTO>(currentUser.Error);
        }

        private static async Task<IResult> GetPage([AsParameters] PaginationSearchQueryParams pagination,
            ICarService carService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await carService.GetCars(pagination)) :
                ControllerExtensions.ErrorMessageResult<PagedResponse<CarDTO>>(currentUser.Error);
        }

        private static async Task<IResult> Add(CarAddDTO car,
            ICarService carService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null
                ? ControllerExtensions.FromServiceResponse(await carService.AddCar(car, currentUser.Result))
                : ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Update(CarUpdateDTO car,
            ICarService carService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null
                ? ControllerExtensions.FromServiceResponse(await carService.UpdateCar(car, currentUser.Result))
                : ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Delete(Guid id,
            ICarService carService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null
                ? ControllerExtensions.FromServiceResponse(await carService.DeleteCar(id, currentUser.Result))
                : ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
