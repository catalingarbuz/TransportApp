using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Enums;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Implementations;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Controllers;


[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
public class BookingController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{
    private readonly IBookingService _bookingService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public BookingController(IUserService userService, IBookingService bookingService) : base(userService)// Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _bookingService = bookingService;
    }

    [Authorize] 
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<BookingDTO>>> GetById([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _bookingService.GetBooking(id)) :
            this.ErrorMessageResult<BookingDTO>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on page of bookings.
    /// Generally, if you need to get multiple values from the database use pagination if there are many entries.
    /// It will improve performance and reduce resource consumption for both client and server.
    /// </summary>
    [Authorize]
    [HttpGet] 
    public async Task<ActionResult<RequestResponse<PagedResponse<BookingDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                         // the PaginationSearchQueryParams properties to the object in the method parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _bookingService.GetBookings(pagination)) :
            this.ErrorMessageResult<PagedResponse<BookingDTO>>(currentUser.Error);
    }

    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] BookingAddDTO booking)
    {
        var currentUser = await GetCurrentUser();

        if (currentUser.Result != null) {
            return this.FromServiceResponse(await _bookingService.AddBooking(booking, currentUser.Result));
        }
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] BookingUpdateDTO booking)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _bookingService.UpdateBooking(booking, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _bookingService.DeleteBooking(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
