using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Controllers;


[ApiController] 
[Route("api/[controller]/[action]")] 
public class CarRouteController : AuthorizedController 
{
    private readonly ICarRouteService _carRouteService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public CarRouteController(IUserService userService, ICarRouteService carRouteService) : base(userService)
    {
        _carRouteService = carRouteService;
    }

    [Authorize] 
    [HttpGet("{id:guid}")] 
    public async Task<ActionResult<RequestResponse<CarRoute>>> GetById([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carRouteService.GetCarRoute(id)) :
            this.ErrorMessageResult<CarRoute>(currentUser.Error);
    }

    [Authorize]
    [HttpPost] 
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CarRouteAddDTO carRoute)
    {
        var currentUser = await GetCurrentUser();

        if (currentUser.Result != null) {
            return this.FromServiceResponse(await _carRouteService.AddCarRoute(carRoute, currentUser.Result));
        }
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> Update([FromBody] CarRouteUpdateDTO carRoute) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carRouteService.UpdateCarRoute(carRoute, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carRouteService.DeleteCarRoute(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
