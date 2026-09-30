import { useAppSelector } from "@application/store";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";
import { ApiRouteGetPageGetRequest, LocationApi } from "../client";

const getLocationsQueryKey = "getLocationsQuery";

export const useLocationApi = () => {
    const { token } = useAppSelector(x => x.profileReducer); // You can use the data form the Redux storage. 
    const config = getAuthenticationConfiguration(token); // Use the token to configure the authentication header.

    const getLocations = (page: ApiRouteGetPageGetRequest) => new LocationApi(config).apiLocationGetGet(page); // Use the generated client code and adapt it.

    return {
        getLocations: { // Return the query object.
            key: getLocationsQueryKey, // Add the key to identify the query.
            query: getLocations // Add the query callback.
        }
    }
}