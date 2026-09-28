using TransportApp.Core.DataTransferObjects;
using TransportApp.Core.Responses;
using TransportApp.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApp.Infrastructure.Services.Interfaces
{
    public interface IDriverService
    {
        public Task<ServiceResponse<PagedResponse<DriverDTO>>> GetDrivers(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetDriver returns a driver given its id.
        /// </summary>
        public Task<ServiceResponse<DriverDTO>> GetDriver(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddDriver adds a driver to the database.
        /// </summary>
        public Task<ServiceResponse> AddDriver(DriverAddDTO driver, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateDriver updates a driver in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateDriver(DriverUpdateDTO driver, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteDriver deletes a driver from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteDriver(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}

