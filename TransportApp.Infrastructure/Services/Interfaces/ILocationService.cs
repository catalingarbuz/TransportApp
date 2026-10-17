using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;

namespace TransportApp.Infrastructure.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<ServiceResponse<List<LocationDTO>>> GetLocations(CancellationToken cancellationToken = default);
    }
}
