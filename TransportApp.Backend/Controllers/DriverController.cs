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

[ApiController] 
[Route("api/[controller]/[action]")] 
public class DriverController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{
    private readonly IDriverService _driverService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public DriverController(IUserService userService, IDriverService driverService) : base(userService)
    {
        _driverService = driverService;
    }

    [Authorize] 
    [HttpGet("{id:guid}")] 
    public async Task<ActionResult<RequestResponse<DriverDTO>>> GetById([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverService.GetDriver(id)) :
            this.ErrorMessageResult<DriverDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpGet] 
    public async Task<ActionResult<RequestResponse<PagedResponse<DriverDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)                                                                                                                                         
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverService.GetDrivers(pagination)) :
            this.ErrorMessageResult<PagedResponse<DriverDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost] 
    public async Task<ActionResult<RequestResponse>> Add([FromBody] DriverAddDTO driver)
    {
        var currentUser = await GetCurrentUser();
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

        if (currentUser.Result != null) {
            await UserService.AddUser(user, currentUser.Result);
            return this.FromServiceResponse(await _driverService.AddDriver(driver, currentUser.Result));
        }
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> Update([FromBody] DriverUpdateDTO driver) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverService.UpdateDriver(driver with
            {
                Password = !string.IsNullOrWhiteSpace(driver.Password) ? PasswordUtils.HashPassword(driver.Password) : null
            }, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverService.DeleteDriver(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
