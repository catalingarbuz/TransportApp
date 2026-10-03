import { RouteEditFormModel, RouteEditFormController } from "./RouteEditForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback, useEffect } from "react";
import { useRouteApi } from "@infrastructure/apis/api-management/route";
import { useLocationApi } from "@infrastructure/apis/api-management/location";
import { CarDTO, LocationDTO, RouteUpdateDTO } from "@infrastructure/apis/client";
import { useCarApi } from "@infrastructure/apis/api-management/car";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (id: string, initialData?: RouteEditFormModel) => {
    const defaultValues = {
        id,
        startingLocationId: "",
        finalLocationId: "",
        departureTime: "",
        arrivalTime: "",
        carIds: [] as string[]
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

/**
 * Create a hook to get the validation schema.
 */
const useInitRouteEditForm = (id: string) => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues(id);

    const schema = yup.object().shape({
        id: yup.string().required(),
        startingLocationId: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.startingLocation" }) }
            )),
        finalLocationId: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.finalLocation" }) }
            )),
        departureTime: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.departureTime" }) }
            )),
        arrivalTime: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.arrivalTime" }) }
            )),
        carIds: yup.array().of(yup.string().required()).default(defaultValues.carIds)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

const getTimeInputValue = (date?: Date) => date
    ? `${String(date.getUTCHours()).padStart(2, "0")}:${String(date.getUTCMinutes()).padStart(2, "0")}`
    : "";

const getLocationId = (locations: LocationDTO[], city?: string | null, country?: string | null) => {
    if (!city || !country) {
        return "";
    }

    const location = locations.find(candidate =>
        candidate.city?.trim().toLocaleLowerCase() === city.trim().toLocaleLowerCase() &&
        candidate.country?.trim().toLocaleLowerCase() === country.trim().toLocaleLowerCase()
    );

    return location?.id ?? "";
};

const dateAtSelectedTime = (time: string) => {
    const [hours, minutes] = time.split(":").map(Number);
    const date = new Date();
    date.setUTCHours(hours, minutes, 0, 0);
    return date;
};

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useRouteEditFormController = (id: string, onSubmit?: () => void): RouteEditFormController => {
    const { defaultValues, resolver } = useInitRouteEditForm(id);
    const { updateRute: { mutation, key: mutationKey }, getRoutes: { key: queryKey }, getRoute: { key: routeQueryKey, query: getRoute } } = useRouteApi();
    const { getLocations: { key: locationsQueryKey, query: getLocations } } = useLocationApi();
    const { getCars: { key: carsQueryKey, query: getCars } } = useCarApi();
    const { mutateAsync: update, status } = useMutation([mutationKey], mutation);
    const { data: routeData, isLoading: isLoadingRoute, isError: isErrorLoadingRoute } = useQuery(
        [routeQueryKey, id],
        () => getRoute(id),
        { enabled: Boolean(id) }
    );
    const loadAllLocations = async () => {
        const allLocations: LocationDTO[] = [];
        const pageSize = 100;
        let page = 1;
        let hasMoreLocations = true;

        while (hasMoreLocations) {
            const response = await getLocations({ page, pageSize });
            const pageLocations = response.response ?? [];
            allLocations.push(...pageLocations);
            hasMoreLocations = pageLocations.length === pageSize;
            page += 1;
        }

        return allLocations;
    };
    const { data: locationsData, isLoading: isLoadingLocations, isError: isErrorLoadingLocations } = useQuery(
        [locationsQueryKey, "all"],
        loadAllLocations
    );
    const locations = locationsData ?? [];
    const { data: carsResponse, isLoading: isLoadingCars, isError: isErrorLoadingCars } = useQuery(
        [carsQueryKey, "route-edit", 1, 1000],
        () => getCars({ page: 1, pageSize: 1000 })
    );
    const cars: CarDTO[] = carsResponse?.response?.data ?? [];
    const queryClient = useQueryClient();
    const { register, handleSubmit, reset, control, formState: { errors, dirtyFields } } = useForm<RouteEditFormModel>({
        defaultValues,
        resolver
    });
    const route = routeData?.response;

    useEffect(() => {
        if (route && locationsData) {
            reset({
                id,
                startingLocationId: getLocationId(locations, route.startingLocationCity, route.startingLocationCountry),
                finalLocationId: getLocationId(locations, route.finalLocationCity, route.finalLocationCountry),
                departureTime: getTimeInputValue(route.departureTime),
                arrivalTime: getTimeInputValue(route.arrivalTime),
                carIds: route.assignedCars?.map(car => car.id).filter((carId): carId is string => Boolean(carId)) ?? []
            });
        }
    }, [id, locations, locationsData, reset, route]);

    const submit = useCallback((data: RouteEditFormModel) => {
        const routeUpdate: RouteUpdateDTO = {
            id: data.id,
            startingLocationId: data.startingLocationId,
            finalLocationId: data.finalLocationId,
            departureTime: dateAtSelectedTime(data.departureTime),
            arrivalTime: dateAtSelectedTime(data.arrivalTime),
            carIds: data.carIds
        };

        return update(routeUpdate).then(() => {
            queryClient.invalidateQueries([queryKey]); // If the form submission succeeds then some other queries need to be refresh so invalidate them to do a refresh.

            if (onSubmit) {
                onSubmit();
            }
        });
    }, [update, queryClient, queryKey, onSubmit]);

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            control
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading", // Return if the form is still submitting or nit.
            isLoadingRoute,
            isErrorLoadingRoute,
            locations,
            isLoadingLocations,
            isErrorLoadingLocations,
            cars,
            isLoadingCars,
            isErrorLoadingCars
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}