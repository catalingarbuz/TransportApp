using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class DriverInfoEndpoints
    {
        public static IEndpointRouteBuilder MapDriverInfoEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/DriverInfo")
                           .RequireAuthorization()
                           .WithTags("DriverInfo");

            group.MapGet("GetById/{id:guid}", GetDriverInfo)
                  .WithName("GetDriverInfo")
                  .Produces<RequestResponse<DriverInfoDTO>>(StatusCodes.Status200OK)
                  .Produces<RequestResponse<DriverInfoDTO>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                    .WithName("AddDriverInfo")
                    .Produces<RequestResponse>(StatusCodes.Status200OK)
                    .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", Update)
                    .WithName("UpdateDriverInfo")
                    .Produces<RequestResponse>(StatusCodes.Status200OK)
                    .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", Delete)
                    .WithName("DeleteDriverInfo")
                    .Produces<RequestResponse>(StatusCodes.Status200OK)
                    .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetDriverInfo(Guid id,
            IDriverInfoService driverInfoService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverInfoService.GetDriverInfo(id)) :
                ControllerExtensions.ErrorMessageResult<DriverInfoDTO>(currentUser.Error);
        }

        private static async Task<IResult> Add([FromBody] DriverInfoAddDTO driverInfo,
            IDriverInfoService driverInfoService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverInfoService.AddDriverInfo(driverInfo, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Update([FromBody] DriverInfoUpdateDTO driverInfo,
            IDriverInfoService driverInfoService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverInfoService.UpdateDriverInfo(driverInfo, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Delete(Guid id,
            IDriverInfoService driverInfoService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverInfoService.DeleteDriverInfo(id)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
