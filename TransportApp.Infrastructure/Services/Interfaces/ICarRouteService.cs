using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportApp.Core.Entities;

namespace TransportApp.Infrastructure.Services.Interfaces
{
    public interface ICarRouteService
    {
        /// <summary>
        /// GetCarRoute returns a carroute given its id.
        /// </summary>
        public Task<ServiceResponse<CarRoute>> GetCarRoute(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddCarRoute adds a car route entry to the database.
        /// </summary>
        public Task<ServiceResponse> AddCarRoute(CarRouteAddDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateCarRoute updates a car route entry in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateCarRoute(CarRouteUpdateDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteCarRoute deletes a car route entry from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteCarRoute(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}

