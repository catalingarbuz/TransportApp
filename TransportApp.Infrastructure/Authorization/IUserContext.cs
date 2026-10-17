using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;

namespace TransportApp.Infrastructure.Authorization
{
    public interface IUserContext
    {
        /// <summary>
        ///     Gets the current user from the context.
        /// </summary>
        Task<ServiceResponse<UserDTO>> GetCurrentUserAsync();
    }
}
