import { useAppSelector } from "@application/store";
import { ApiRouteGetPageGetRequest, RouteAddDTO, RouteApi, RouteDTO, RouteUpdateDTO } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";
import { get } from "lodash";
import { de } from "date-fns/locale";

/**
 * Use constants to identify mutations and queries.
 */
const getRoutesQueryKey = "getRoutesQuery";
const getRouteQueryKey = "getRouteQuery";
const getRoutesWithLocationsDictionaryQueryKey = "getRoutesWithLocationsDictionaryQuery";
const addRouteMutationKey = "addRouteMutation";
const deleteRouteMutationKey = "deleteRouteMutation";
const updateRouteMutationKey = "updateRouteMutation";

const normalizeRouteDateAsUtc = (date?: Date) => {
    if (!date) {
        return date;
    }

    // The API returns DateTime values without an offset; the generated client parses them as local time.
    return new Date(Date.UTC(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        date.getMinutes(),
        date.getSeconds(),
        date.getMilliseconds()
    ));
};

const normalizeRouteDates = (route: RouteDTO): RouteDTO => ({
    ...route,
    departureTime: normalizeRouteDateAsUtc(route.departureTime),
    arrivalTime: normalizeRouteDateAsUtc(route.arrivalTime)
});

/**
 * Returns the an object with the callbacks that can be used for the React Query API, in this case to manage the user API.
 */
export const useRouteApi = () => {
    const { token } = useAppSelector(x => x.profileReducer); // You can use the data form the Redux storage. 
    const config = getAuthenticationConfiguration(token); // Use the token to configure the authentication header.

    const getRoutes = async (page: ApiRouteGetPageGetRequest) => {
        const result = await new RouteApi(config).apiRouteGetPageGet(page);
        if (!result.response?.data) {
            return result;
        }

        return {
            ...result,
            response: {
                ...result.response,
                data: result.response.data.map(normalizeRouteDates)
            }
        };
    };
    const getRoute = async (id: string) => {
        const result = await new RouteApi(config).apiRouteGetByIdIdGet({ id });
        return result.response
            ? { ...result, response: normalizeRouteDates(result.response) }
            : result;
    };
    const getRoutesWithLocationsDictionary = () => new RouteApi(config).apiRouteGetRoutesWithLocationsDictionaryGet();
    const addRoute = (route: RouteAddDTO) => new RouteApi(config).apiRouteAddPost({ routeAddDTO: route });
    const deleteRoute = (id: string) => new RouteApi(config).apiRouteDeleteIdDelete({ id });
    const updateRoute = (route: RouteUpdateDTO) => new RouteApi(config).apiRouteUpdatePut({ routeUpdateDTO: route });

    return {
        getRoutes: { // Return the query object.
            key: getRoutesQueryKey, // Add the key to identify the query.
            query: getRoutes // Add the query callback.
        },
        getRoute: {
            key: getRouteQueryKey,
            query: getRoute
        },
        getRoutesWithLocationsDictionary: {
            key: getRoutesWithLocationsDictionaryQueryKey,
            query: getRoutesWithLocationsDictionary
        },
        addRoute: { // Return the mutation object.
            key: addRouteMutationKey, // Add the key to identify the mutation.
            mutation: addRoute // Add the mutation callback.
        },
        deleteRoute: {
            key: deleteRouteMutationKey,
            mutation: deleteRoute
        },
        updateRute: {
            key: updateRouteMutationKey,
            mutation: updateRoute
        }
    }
}