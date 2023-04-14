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
    public interface ICarService
    {
        public Task<ServiceResponse<PagedResponse<CarDTO>>> GetCars(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetCar returns a car given its id.
        /// </summary>
        public Task<ServiceResponse<CarDTO>> GetCar(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddCar adds a car to the database.
        /// </summary>
        public Task<ServiceResponse> AddCar(CarAddDTO car, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateCar updates a car in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateCar(CarUpdateDTO car, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete car deletes a car from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteCar(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}
