using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Endpoints
{
    public static class BookingEndpoints
    {
        public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Booking")
                           .WithTags("Booking");

            group.MapGet("GetById/{id:guid}", GetById)
                .WithName("GetBookingById")
                .Produces<RequestResponse<BookingDTO>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<BookingDTO>>(StatusCodes.Status400BadRequest);

            group.MapGet("GetPage", GetPage)
                .WithName("GetBookingsPage")
                .Produces<RequestResponse<PagedResponse<BookingDTO>>>(StatusCodes.Status200OK)
                .Produces<RequestResponse<PagedResponse<BookingDTO>>>(StatusCodes.Status400BadRequest);

            group.MapPost("Add", Add)
                .WithName("AddBooking")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapPut("Update", Update)
                .WithName("UpdateBooking")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            group.MapDelete("Delete/{id:guid}", Delete)
                .WithName("DeleteBooking")
                .Produces<RequestResponse>(StatusCodes.Status200OK)
                .Produces<RequestResponse>(StatusCodes.Status400BadRequest);

            return app;
        }

        private static async Task<IResult> GetById(Guid id,
            IBookingService bookingService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await bookingService.GetBooking(id)) :
                ControllerExtensions.ErrorMessageResult<BookingDTO>(currentUser.Error);
        }

        private static async Task<IResult> GetPage([AsParameters] PaginationSearchQueryParams pagination,
            IBookingService bookingService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await bookingService.GetBookings(pagination)) :
                ControllerExtensions.ErrorMessageResult<PagedResponse<BookingDTO>>(currentUser.Error);
        }

        private static async Task<IResult> Add([FromBody] BookingAddDTO booking,
            IBookingService bookingService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null 
                ? ControllerExtensions.FromServiceResponse(await bookingService.AddBooking(booking, currentUser.Result))
                : ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Update([FromBody] BookingUpdateDTO booking,
            IBookingService bookingService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await bookingService.UpdateBooking(booking, currentUser.Result)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }

        private static async Task<IResult> Delete(Guid id,
            IBookingService bookingService,
            IUserContext userContext)
        {
            var currentUser = await userContext.GetCurrentUserAsync();

            return currentUser.Result != null ?
                ControllerExtensions.FromServiceResponse(await bookingService.DeleteBooking(id)) :
                ControllerExtensions.ErrorMessageResult(currentUser.Error);
        }
    }
}
