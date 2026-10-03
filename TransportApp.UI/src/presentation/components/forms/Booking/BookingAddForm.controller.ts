import { BookingAddFormModel} from "./BookingAddForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import { BookingAddFormController } from "./BookingAddForm.types";
import { useBookingApi } from "@infrastructure/apis/api-management/booking";
import { useRouteApi } from "@infrastructure/apis/api-management/route";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (initialData?: BookingAddFormModel) => {
    const defaultValues = {
        routeName: "",
        departurePlace: "",
        routeId: "",
        bookingDate: new Date(),
        departureDate: new Date(),
        driverId: ""
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
const useInitBookingAddForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        departurePlace: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.departurePlace" }) }
            )),
        routeId: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                { fieldName: formatMessage({ id: "globals.arrivalPlace" }) }
            )),
        bookingDate: yup.date()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.email",
                    }),
                }))
            .default(defaultValues.bookingDate),
        departureDate: yup.date()
        .required(formatMessage(
            { id: "globals.validations.requiredField" },
            {
                fieldName: formatMessage({
                    id: "globals.departureDate",
                }),
            }))
        .default(defaultValues.departureDate),
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useBookingAddFormController = (onSubmit?: () => void): BookingAddFormController => {
    const { defaultValues, resolver } = useInitBookingAddForm();
    const { addBooking: { mutation, key: mutationKey }, getBookings: { key: queryKey } } = useBookingApi();
    const { getRoutesWithLocationsDictionary: { key: departurePlacesQueryKey, query: getDeparturePlaces } } = useRouteApi();
    const { data: departurePlacesResponse, isLoading: isLoadingDeparturePlaces, isError: isErrorLoadingDeparturePlaces } = useQuery(
        [departurePlacesQueryKey],
        getDeparturePlaces
    );
    const departurePlaces = Object.keys(departurePlacesResponse?.response ?? {});
    const routesByDeparturePlace = departurePlacesResponse?.response ?? {};
    const { mutateAsync: add, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const submit = useCallback((data: BookingAddFormModel) =>
        add({
            bookingDate: data.bookingDate,
            departureDate: data.departureDate,
            routeId: data.routeId
        }).then(() => {
            queryClient.invalidateQueries([queryKey]); // If the form submission succeeds then some other queries need to be refresh so invalidate them to do a refresh.

            if (onSubmit) {
                onSubmit();
            }
        }), [add, queryClient, queryKey]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        clearErrors,
        control,
        formState: { errors }
    } = useForm<BookingAddFormModel>({ // Use the useForm hook to get callbacks and variables to work with the form.
        defaultValues, // Initialize the form with the default values.
        resolver // Add the validation resolver.
    });
    const selectedDeparturePlace = watch("departurePlace");
    const arrivalRoutes = routesByDeparturePlace[selectedDeparturePlace] ?? [];

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch, // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
            control,
            setValue,
            clearErrors
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading", // Return if the form is still submitting or nit.
            departurePlaces,
            arrivalRoutes,
            hasDeparturePlaceSelected: Boolean(selectedDeparturePlace),
            isLoadingDeparturePlaces,
            isErrorLoadingDeparturePlaces
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}