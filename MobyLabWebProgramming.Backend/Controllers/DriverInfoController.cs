using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// This is a controller example for CRUD operations on users.
/// </summary>
[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
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

    /// <summary>
    /// This method implements the Read operation (R from CRUD) on a driver. 
    /// </summary>
    [Authorize] // You need to use this attribute to protect the route access, it will return a Forbidden status code if the JWT is not present or invalid, and also it will decode the JWT token.
    [HttpGet("{id:guid}")] // This attribute will make the controller respond to a HTTP GET request on the route /api/Driver/GetById/<some_guid>.
    public async Task<ActionResult<RequestResponse<DriverInfoDTO>>> GetById([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.GetDriverInfo(id)) :
            this.ErrorMessageResult<DriverInfoDTO>(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Create operation (C from CRUD) of a user. 
    /// </summary>
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/Driver/Add.
    public async Task<ActionResult<RequestResponse>> Add([FromBody] DriverInfoAddDTO driverInfo)
    {
        var currentUser = await GetCurrentUser();
  
        if (currentUser.Result != null) 
            return this.FromServiceResponse(await _driverInfoService.AddDriverInfo(driverInfo, currentUser.Result));
 
        return this.ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Update operation (U from CRUD) on a Driver. 
    /// </summary>
    [Authorize]
    [HttpPut] // This attribute will make the controller respond to a HTTP PUT request on the route /api/Driver/Update.
    public async Task<ActionResult<RequestResponse>> Update([FromBody] DriverInfoUpdateDTO driverInfo) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.UpdateDriverInfo(driverInfo, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// This method implements the Delete operation (D from CRUD) on a driver.
    /// Note that in the HTTP RFC you cannot have a body for DELETE operations.
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")] // This attribute will make the controller respond to a HTTP DELETE request on the route /api/Driver/Delete/<some_guid>.
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _driverInfoService.DeleteDriverInfo(id)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
