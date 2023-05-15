import { useAppSelector } from "@application/store";
import { ApiRouteGetPageGetRequest, RouteAddDTO, RouteApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";
import { get } from "lodash";
import { de } from "date-fns/locale";

/**
 * Use constants to identify mutations and queries.
 */
const getRoutesQueryKey = "getRoutesQuery";
const getRouteQueryKey = "getRouteQuery";
const addRouteMutationKey = "addRouteMutation";
const deleteRouteMutationKey = "deleteRouteMutation";

/**
 * Returns the an object with the callbacks that can be used for the React Query API, in this case to manage the user API.
 */
export const useRouteApi = () => {
    const { token } = useAppSelector(x => x.profileReducer); // You can use the data form the Redux storage. 
    const config = getAuthenticationConfiguration(token); // Use the token to configure the authentication header.

    const getRoutes = (page: ApiRouteGetPageGetRequest) => new RouteApi(config).apiRouteGetPageGet(page); // Use the generated client code and adapt it.
    const getRoute = (id: string) => new RouteApi(config).apiRouteGetByIdIdGet({ id });
    const addRoute = (route: RouteAddDTO) => new RouteApi(config).apiRouteAddPost({ routeAddDTO: route });
    const deleteRoute = (id: string) => new RouteApi(config).apiRouteDeleteIdDelete({ id });

    return {
        getRoutes: { // Return the query object.
            key: getRoutesQueryKey, // Add the key to identify the query.
            query: getRoutes // Add the query callback.
        },
        getRoute: {
            key: getRouteQueryKey,
            query: getRoute
        },
        addRoute: { // Return the mutation object.
            key: addRouteMutationKey, // Add the key to identify the mutation.
            mutation: addRoute // Add the mutation callback.
        },
        deleteRoute: {
            key: deleteRouteMutationKey,
            mutation: deleteRoute
        }
    }
}