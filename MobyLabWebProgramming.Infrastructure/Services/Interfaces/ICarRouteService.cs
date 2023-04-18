using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface ICarRouteService
    {
        /// <summary>
        /// GetRoute returns a route given its id.
        /// </summary>
        public Task<ServiceResponse<CarRoute>> GetCarRoute(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddRoute adds a route to the database.
        /// </summary>
        public Task<ServiceResponse> AddCarRoute(CarRouteAddDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateRoute updates a route in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateCarRoute(CarRouteUpdateDTO carRoute, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteRoute deletes a route from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteCarRoute(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}
