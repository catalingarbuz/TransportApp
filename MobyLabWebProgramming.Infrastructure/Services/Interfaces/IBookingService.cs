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
    public interface IBookingService
    {
        public Task<ServiceResponse<PagedResponse<BookingDTO>>> GetBookings(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetRoute returns a route given its id.
        /// </summary>
        public Task<ServiceResponse<BookingDTO>> GetBooking(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddRoute adds a route to the database.
        /// </summary>
        public Task<ServiceResponse> AddBooking(BookingAddDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateRoute updates a route in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateBooking(BookingUpdateDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteRoute deletes a route from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteBooking(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}
