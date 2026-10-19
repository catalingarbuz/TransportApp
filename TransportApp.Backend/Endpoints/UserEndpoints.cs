using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/User")
                           .RequireAuthorization()
                           .WithTags("User");

            group.MapGet("GetById/{id:guid}", GetById)
                 .WithName("GetUserById")
                 .Produces<RequestResponse<UserDTO>>(StatusCodes.Status200OK)
                 .Produces<RequestResponse<UserDTO>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetPage", GetPage)
                .WithName("GetUsersPage")
                .Produces<RequestResponse<PagedResponse<UserDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<UserDTO>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                .WithName("AddUser")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", Update)
                .WithName("UpdateUser")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", Delete)
                .WithName("DeleteUser")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetById(Guid id, 
            IUserService userService,
            IUserContext userContext)
        {
            ServiceResponse<UserDTO> currentUser = await userContext.GetCurrentUserAsync();

            if (currentUser.Result == null)
            {
                return Results.BadRequest(RequestResponse<List<LocationDTO>>.FromError(currentUser.Error));
            }

            ServiceResponse<UserDTO> result = await userService.GetUser(id);

            return ControllerExtensions.FromServiceResponse(result);
        }

        private static async Task<IResult> GetPage([AsParameters] PaginationSearchQueryParams pagination,
            IUserService userService,
            IUserContext userContext)
        {
            ServiceResponse<UserDTO> currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null
                ? ControllerExtensions.FromServiceResponse(await userService.GetUsers(pagination))
                : ControllerExtensions.FromServiceResponse(currentUser);
        }

        private static async Task<IResult> Add([FromBody] UserAddDTO user,
            IUserService userService)
        {
            user.Password = PasswordUtils.HashPassword(user.Password);

            return ControllerExtensions.FromServiceResponse(await userService.AddUser(user, null));
        }

        private static async Task<IResult> Update([FromBody] UserUpdateDTO user,
            IUserService userService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await userService.UpdateUser(user with
                {
                    Password = !string.IsNullOrWhiteSpace(user.Password) ? PasswordUtils.HashPassword(user.Password) : null
                }, currentUser.Result)) :
                Results.BadRequest(currentUser.Error);
        }

        private static async Task<IResult> Delete(Guid id,
            IUserService userService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();
            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await userService.DeleteUser(id)) :
                Results.BadRequest(currentUser.Error);
        }
    }
}
