using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class DriverEndpoints
    {
        public static IEndpointRouteBuilder MapDriverEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Driver")
                           .RequireAuthorization()
                           .WithTags("Driver");

            group.MapGet("GetById/{id:guid}", GetDriverById)
                .WithName("GetDriverById")
                .Produces<RequestResponse<DriverDTO>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<DriverDTO>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetPage", GetDriversPage)
                .WithName("GetDriversPage")
                .Produces<RequestResponse<PagedResponse<DriverDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<DriverDTO>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", AddDriver)
                .WithName("AddDriver")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", UpdateDriver)
                .WithName("UpdateDriver")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", DeleteDriver)
                .WithName("DeleteDriver")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetDriverById(Guid id,
            IDriverService driverService,
            IUserContext userContext,
            CancellationToken cancellationToken)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverService.GetDriver(id)) :
                ControllerExtensions.ErrorMessageResult<DriverDTO>(currentUser.Error);
        }

        private static async Task<IResult> GetDriversPage([AsParameters] PaginationSearchQueryParams pagination,
            IDriverService driverService,
            IUserContext userContext,
            CancellationToken cancellationToken)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverService.GetDrivers(pagination)) :
                ControllerExtensions.ErrorMessageResult<PagedResponse<DriverDTO>>(currentUser.Error);
        }

        private static async Task<IResult> AddDriver([FromBody] DriverAddDTO driver,
            IDriverService driverService,
            IUserService userService,
            IUserContext userContext,
            CancellationToken cancellationToken)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            driver.Password = PasswordUtils.HashPassword(driver.Password);

            // add driver in the user table as well
            var user = new UserAddDTO
            {
                Name = driver.Name,
                Email = driver.Email,
                Password = PasswordUtils.HashPassword(driver.Password),
                PhoneNumber = driver.PhoneNumber,
                Role = driver.Role
            };

            if (currentUser.Result != null)
            {
                await userService.AddUser(user, currentUser.Result);
                return ControllerExtensions.FromServiceResponse(await driverService.AddDriver(driver, currentUser.Result));
            }

            return ControllerExtensions.ErrorMessageResult<DriverDTO>(currentUser.Error);
        }

        private static async Task<IResult> UpdateDriver([FromBody] DriverUpdateDTO driver,
            IDriverService driverService,
            IUserContext userContext,
            CancellationToken cancellationToken)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverService.UpdateDriver(driver with
                {
                    Password = !string.IsNullOrWhiteSpace(driver.Password) ? PasswordUtils.HashPassword(driver.Password) : null
                }, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> DeleteDriver(Guid id,
            IDriverService driverService,
            IUserContext userContext,
            CancellationToken cancellationToken)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await driverService.DeleteDriver(id)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
