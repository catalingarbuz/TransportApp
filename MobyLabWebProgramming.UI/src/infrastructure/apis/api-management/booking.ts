import { useAppSelector } from "@application/store";
import { ApiUserGetPageGetRequest, BookingAddDTO, BookingUpdateDTO, UserAddDTO, UserApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";
import { BookingApi } from "../client";

/**
 * Use constants to identify mutations and queries.
 */
const getBookingsQueryKey = "getBookingsQuery";
const getBookingQueryKey = "getBookingQuery";
const addBookingMutationKey = "addBookingMutation";
const deleteBookingMutationKey = "deleteBookingMutation";
const updateBookingMutationKey = "updateBookingMutation";

/**
 * Returns the an object with the callbacks that can be used for the React Query API, in this case to manage the user API.
 */
export const useBookingApi = () => {
    const { token } = useAppSelector(x => x.profileReducer); // You can use the data form the Redux storage. 
    const config = getAuthenticationConfiguration(token); // Use the token to configure the authentication header.

    const getBookings = (page: ApiUserGetPageGetRequest) => new BookingApi(config).apiBookingGetPageGet(page); // Use the generated client code and adapt it.
    const getBooking = (id: string) => new BookingApi(config).apiBookingGetByIdIdGet({ id });
    const addBooking = (booking: BookingAddDTO) => new BookingApi(config).apiBookingAddPost({ bookingAddDTO: booking });
    const deleteBooking = (id: string) => new BookingApi(config).apiBookingDeleteIdDelete({ id });
    const updateBooking = (booking: BookingUpdateDTO) => new BookingApi(config).apiBookingUpdatePut({ bookingUpdateDTO: booking });

    return {
        getBookings: { // Return the query object.
            key: getBookingsQueryKey, // Add the key to identify the query.
            query: getBookings // Add the query callback.
        },
        getBooking: {
            key: getBookingQueryKey,
            query: getBooking
        },
        addBooking: { // Return the mutation object.
            key: addBookingMutationKey, // Add the key to identify the mutation.
            mutation: addBooking // Add the mutation callback.
        },
        deleteBooking: {
            key: deleteBookingMutationKey,
            mutation: deleteBooking
        },
        updateBooking: {
            key: updateBookingMutationKey,
            mutation: updateBooking
        }
    }
}