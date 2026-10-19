using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Core.Specifications;
using TransportApp.Infrastructure.Database;
using TransportApp.Infrastructure.Repositories.Interfaces;
using TransportApp.Infrastructure.Services.Interfaces;
using TransportApp.Core.Entities;
using TransportApp.Core.Mapper;
using TransportApp.Infrastructure.Services.Caching;

namespace TransportApp.Infrastructure.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<WebAppDatabaseContext> _repository;
        private readonly IRedisCacheService _cacheService;

        /// <summary>
        /// Inject the required services through the constructor.
        /// </summary>
        public LocationService(IRepository<WebAppDatabaseContext> repository, IRedisCacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<ServiceResponse<List<LocationDTO>>> GetLocations(CancellationToken cancellationToken = default)
        {
            List<LocationDTO>? cachedLocations = _cacheService.GetData<List<LocationDTO>>(CacheKeys.Location);
            if (cachedLocations != null)
            {
                return ServiceResponse<List<LocationDTO>>.ForSuccess(cachedLocations);
            }

            List<Location> result = await _repository.ListNoTrackingAsync(new LocationProjectionSpec(), cancellationToken);
            LocationMapper mapper = new LocationMapper();
            List<LocationDTO> dtoList = result.Select(mapper.ToDto).ToList();

            _cacheService.SetData(CacheKeys.Location, dtoList);

            return ServiceResponse<List<LocationDTO>>.ForSuccess(dtoList);
        }
    }
}
