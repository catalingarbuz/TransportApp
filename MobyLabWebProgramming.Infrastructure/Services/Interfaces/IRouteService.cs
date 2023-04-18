using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces
{
    public interface IRouteService
    {
        public Task<ServiceResponse<PagedResponse<RouteDTO>>> GetRoutes(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetRoute returns a route given its id.
        /// </summary>
        public Task<ServiceResponse<RouteDTO>> GetRoute(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddRoute adds a route to the database.
        /// </summary>
        public Task<ServiceResponse> AddRoute(RouteAddDTO route, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateRoute updates a route in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateRoute(RouteUpdateDTO route, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteRoute deletes a route from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteRoute(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}
