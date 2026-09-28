import { useTableController } from "../Table.controller";
import { useRouteApi } from "@infrastructure/apis/api-management/route";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import { usePaginationController } from "../Pagination.controller";
import { RouteUpdateDTO } from "@infrastructure/apis/client";

/**
 * This is controller hook manages the table state including t
 * he pagination and data retrieval from the backend.
 */
export const useRouteTableController = () => { 
    const { getRoutes: { key: queryKey, query }, deleteRoute: { key: deleteRouteKey, mutation: deleteRoute }, updateRute: { key: updateRouteKey, mutation: updateRoute} } = useRouteApi(); // Use the API hook.
    const queryClient = useQueryClient(); // Get the query client.
    const { page, pageSize, setPagination } = usePaginationController(); // Get the pagination state.
    const { data, isError, isLoading } = useQuery([queryKey, page, pageSize], () => query({ page, pageSize })); // Retrieve the table page from the backend via the query hook.
    const { mutateAsync: deleteMutation } = useMutation([deleteRouteKey], deleteRoute); // Use a mutation to remove an entry.
    const remove = useCallback(
        (id: string) => deleteMutation(id).then(() => queryClient.invalidateQueries([queryKey])),
        [queryClient, deleteMutation, queryKey]); // Create the callback to remove an entry.
    
    const update = useCallback(
        (routeUpdate: RouteUpdateDTO) => updateRoute(routeUpdate).then(() => queryClient.invalidateQueries([queryKey])),
        [queryClient, updateRouteKey, queryKey]); // Create the callback to remove an entry.

    const tryReload = useCallback(
        () => queryClient.invalidateQueries([queryKey]),
        [queryClient, queryKey]); // Create a callback to try reloading the data for the table via query invalidation.

    const tableController = useTableController(setPagination, data?.response?.pageSize); // Adapt the pagination for the table.

    return { // Return the controller state and actions.
        ...tableController,
        tryReload,
        pagedData: data?.response,
        isError,
        isLoading,
        remove,
        update
    };
}
