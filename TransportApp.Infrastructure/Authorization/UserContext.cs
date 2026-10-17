using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Infrastructure.Authorization;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Backend.Infrastructure;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private UserClaims? _userClaims;

    public UserContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    /// <summary>
    /// Lazily extracts and caches JWT claims for the current request context.
    /// </summary>
    private UserClaims ExtractClaims()
    {
        if (_userClaims != null)
        {
            return _userClaims;
        }

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var claims = user.Claims.ToList();
        var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => Guid.Parse(x.Value)).FirstOrDefault();
        var email = claims.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
        var name = claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
        var role = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();

        _userClaims = new UserClaims(userId, name, email, role);
        return _userClaims;
    }

    /// <summary>
    /// Gets the current logged-in user information directly from the database.
    /// </summary>
    public Task<ServiceResponse<UserDTO>> GetCurrentUserAsync()
        => _userService.GetUser(ExtractClaims().Id);
}
