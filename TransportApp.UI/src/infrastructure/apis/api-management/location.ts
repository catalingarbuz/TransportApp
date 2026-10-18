import { useAppSelector } from "@application/store";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";
import { LocationApi } from "../client";
import { LocationDTO } from "../client/models/LocationDTO";

const getLocationsQueryKey = "getLocationsQuery";

export const useLocationApi = () => {
    const { token } = useAppSelector(x => x.profileReducer); // You can use the data form the Redux storage. 
    const config = getAuthenticationConfiguration(token); // Use the token to configure the authentication header.

    const getLocations = async (): Promise<LocationDTO[]> => {
        const response = await new LocationApi(config).getLocations();
        return response.response ?? [];
    };

    return {
        getLocations: { // Return the query object.
            key: getLocationsQueryKey, // Add the key to identify the query.
            query: getLocations // Add the query callback.
        }
    }
}