
using Riok.Mapperly.Abstractions;
using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Entities;

namespace TransportApp.Core.Mapper
{
    [Mapper]
    public partial class LocationMapper
    {
        [MapperIgnoreSource(nameof(Location.Latitude))]
        [MapperIgnoreSource(nameof(Location.Longitude))]
        [MapperIgnoreSource(nameof(Location.CreatedAt))]
        [MapperIgnoreSource(nameof(Location.UpdatedAt))]

        public partial LocationDTO ToDto(Location location);
    }
}
