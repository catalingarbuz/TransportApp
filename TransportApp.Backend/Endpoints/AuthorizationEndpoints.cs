using Microsoft.AspNetCore.Mvc;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;

namespace TransportApp.Backend.Endpoints
{
    public static class AuthorizationEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Authorization")
                           .WithTags("Authorization");

            group.MapPost("Login", Login)
                 .WithName("Login")
                 .Produces<RequestResponse<LoginResponseDTO>>(StatusCodes.Status200OK)
                 .Produces<RequestResponse<LoginResponseDTO>>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> Login([FromBody] LoginDTO login,
            IUserService userService,
            CancellationToken cancellationToken)
        {
            return ControllerExtensions.FromServiceResponse(await userService.Login(login with { Password = PasswordUtils.HashPassword(login.Password) }));
        }
    }
}
