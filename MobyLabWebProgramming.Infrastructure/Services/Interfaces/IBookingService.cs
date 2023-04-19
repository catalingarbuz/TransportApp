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
        /// GetBooking returns a booking given its id.
        /// </summary>
        public Task<ServiceResponse<BookingDTO>> GetBooking(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// AddBooking adds a booking to the database.
        /// </summary>
        public Task<ServiceResponse> AddBooking(BookingAddDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// UpdateBooking updates a booking in the database.
        /// </summary>
        public Task<ServiceResponse> UpdateBooking(BookingUpdateDTO booking, UserDTO? requestingUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// DeleteBooking deletes a booking from the database.
        /// </summary>
        public Task<ServiceResponse> DeleteBooking(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    }
}
