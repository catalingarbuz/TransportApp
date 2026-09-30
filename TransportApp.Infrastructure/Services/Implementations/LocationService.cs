using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Core.Specifications;
using TransportApp.Infrastructure.Database;
using TransportApp.Infrastructure.Repositories.Interfaces;
using TransportApp.Infrastructure.Services.Interfaces;

namespace TransportApp.Infrastructure.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;

        /// <summary>
        /// Inject the required services through the constructor.
        /// </summary>
        public LocationService(IRepository<WebAppDatabaseContext> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<List<LocationDTO>>> GetLocations(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
        {
            List<LocationDTO> result = await _repository.ListAsync(new LocationProjectionSpec(pagination.Search, false), cancellationToken);

            return ServiceResponse<List<LocationDTO>>.ForSuccess(result);
        }
    }
}
