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
public class DriverInfoController : AuthorizedController // Here we use the AuthorizedController as the base class because it derives ControllerBase and also has useful methods to retrieve user information.
{
    private readonly IDriverInfoService _driverInfoService;
    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public DriverInfoController(IUserService userService, IDriverInfoService driverInfoService) : base(userService)// Also, you may pass constructor parameters to a base class constructor and call as specific constructor from the base class.
    {
        _driverInfoService = driverInfoService;
    }

    [Authorize] 
    [HttpGet("{id:guid}")] 
    public async Task<ActionResult<RequestResponse<DriverInfoDTO>>> GetById([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.GetDriverInfo(id)) :
            this.ErrorMessageResult<DriverInfoDTO>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] DriverInfoAddDTO driverInfo)
    {
        var currentUser = await GetCurrentUser();
  
        if (currentUser.Result != null) 
            return this.FromServiceResponse(await _driverInfoService.AddDriverInfo(driverInfo, currentUser.Result));
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut] 
    public async Task<ActionResult<RequestResponse>> Update([FromBody] DriverInfoUpdateDTO driverInfo) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.UpdateDriverInfo(driverInfo, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")] 
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) 
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.DeleteDriverInfo(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
