using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LocationController : AuthorizedController
    {
        private readonly ILocationService _locationService;

        public LocationController(IUserService userService, ILocationService locationService)
            : base(userService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<RequestResponse<List<LocationDTO>>>> Get([FromQuery] PaginationSearchQueryParams pagination) // The FromQuery attribute will bind the parameters matching the names of
                                                                                                                                              // the PaginationSearchQueryParams properties to the object in the method parameter.
        {
            var currentUser = await GetCurrentUser();

            return currentUser.Result != null ?
                this.FromServiceResponse(await _locationService.GetLocations(pagination)) :
                this.ErrorMessageResult<List<LocationDTO>>(currentUser.Error);
        }
    }
}
