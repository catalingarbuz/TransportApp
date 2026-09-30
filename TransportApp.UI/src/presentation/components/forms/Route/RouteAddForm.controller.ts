import { RouteAddFormModel} from "./RouteAddForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import { useRouteApi } from "@infrastructure/apis/api-management/route";
import { RouteAddFormController } from "./RouteAddForm.types";
import { useLocationApi } from "@infrastructure/apis/api-management/location";
import { LocationDTO, RouteAddDTO } from "@infrastructure/apis/client";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (initialData?: RouteAddFormModel) => {
    const defaultValues = {
        startingLocationId: "",
        finalLocationId: "",
        departureTime: "",
        arrivalTime: ""
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

const dateAtSelectedTime = (time: string) => {
    const [hours, minutes] = time.split(":").map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
};

/**
 * Create a hook to get the validation schema.
 */
const useInitRouteAddForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
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
            ))
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useRouteAddFormController = (onSubmit?: () => void): RouteAddFormController => {
    const { defaultValues, resolver } = useInitRouteAddForm();
    const { addRoute: { mutation, key: mutationKey }, getRoutes: { key: queryKey } } = useRouteApi();
    const { getLocations: { key: locationsQueryKey, query: getLocations } } = useLocationApi();
    const { mutateAsync: add, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
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
    const { data: locations, isLoading: isLoadingLocations, isError: isErrorLoadingLocations } = useQuery(
        [locationsQueryKey],
        loadAllLocations
    );
    const availableLocations = locations ?? [];
    const submit = useCallback((data: RouteAddFormModel) => {
        const startingLocation = availableLocations.find(location => location.id === data.startingLocationId);
        const finalLocation = availableLocations.find(location => location.id === data.finalLocationId);

        if (isUndefined(startingLocation) || isUndefined(finalLocation)) {
            return;
        }

        const route: RouteAddDTO = {
            startingLocationCity: startingLocation.city,
            startingLocationCountry: startingLocation.country,
            finalLocationCity: finalLocation.city,
            finalLocationCountry: finalLocation.country,
            departureTime: dateAtSelectedTime(data.departureTime),
            arrivalTime: dateAtSelectedTime(data.arrivalTime)
        };

        return add(route).then(() => {
            queryClient.invalidateQueries([queryKey]); // If the form submission succeeds then some other queries need to be refresh so invalidate them to do a refresh.

            if (onSubmit) {
                onSubmit();
            }
        });
    }, [add, availableLocations, onSubmit, queryClient, queryKey]);

    const {
        register,
        handleSubmit,
        watch,
        control,
        formState: { errors }
    } = useForm<RouteAddFormModel>({ // Use the useForm hook to get callbacks and variables to work with the form.
        defaultValues, // Initialize the form with the default values.
        resolver // Add the validation resolver.
    });

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch, // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
            control
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading", // Return if the form is still submitting or nit.
            locations: availableLocations,
            isLoadingLocations,
            isErrorLoadingLocations
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}