using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Extensions;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Controllers;


[ApiController] 
[Route("api/[controller]/[action]")] 
public class CarController : AuthorizedController 
{
    private readonly ICarService _carService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public CarController(IUserService userService, ICarService carService) : base(userService)// Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _carService = carService;
    }

    [HttpGet("{id:guid}")] 
    public async Task<ActionResult<RequestResponse<CarDTO>>> GetById([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carService.GetCar(id)) :
            this.ErrorMessageResult<CarDTO>(currentUser.Error);
    }

    [HttpGet] 
    public async Task<ActionResult<RequestResponse<PagedResponse<CarDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)                                                                                                                                         
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carService.GetCars(pagination)) :
            this.ErrorMessageResult<PagedResponse<CarDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost] 
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CarAddDTO car)
    {
        var currentUser = await GetCurrentUser();

        if (currentUser.Result != null) {
            return this.FromServiceResponse(await _carService.AddCar(car, currentUser.Result));
        }
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> Update([FromBody] CarUpdateDTO car) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carService.UpdateCar(car, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _carService.DeleteCar(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
