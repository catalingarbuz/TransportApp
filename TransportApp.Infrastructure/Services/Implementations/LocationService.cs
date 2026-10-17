using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Requests;
using TransportApp.Core.Responses;
using TransportApp.Core.Specifications;
using TransportApp.Infrastructure.Database;
using TransportApp.Infrastructure.Repositories.Interfaces;
using TransportApp.Infrastructure.Services.Interfaces;
using TransportApp.Core.Entities;
using TransportApp.Core.Mapper;

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

        public async Task<ServiceResponse<List<LocationDTO>>> GetLocations(CancellationToken cancellationToken = default)
        {
            List<Location> result = await _repository.ListNoTrackingAsync(new LocationProjectionSpec(), cancellationToken);

            LocationMapper mapper = new LocationMapper();
            List<LocationDTO> dtoList = result.Select(mapper.ToDto).ToList();

            return ServiceResponse<List<LocationDTO>>.ForSuccess(dtoList);
        }
    }
}
