using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;

namespace TransportApp.Infrastructure.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<ServiceResponse<List<LocationDTO>>> GetLocations(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    }
}
