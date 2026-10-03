import { BookingEditFormModel, BookingEditFormController} from "./BookingEditForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback, useEffect } from "react";
import { useBookingApi } from "@infrastructure/apis/api-management/booking";
import { BookingUpdateDTO } from "@infrastructure/apis/client";
import { useRouteApi } from "@infrastructure/apis/api-management/route";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (id: string, initialData?: BookingEditFormModel) => {
    const defaultValues = {
        id : id,
        bookingDate: "",
        departureDate: "",
        departurePlace: "",
        arrivalPlace: "",
        driverId: null,
        carId: null,
        routeId: ""
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
const useInitBookingEditForm = (id: string) => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues(id);

    const schema = yup.object().shape({
        id: yup.string().required(),
        bookingDate: yup.string().nullable()
            .default(defaultValues.bookingDate),
        departureDate: yup.string().nullable()
            .default(defaultValues.departureDate),
        departurePlace: yup.string().required(formatMessage(
            { id: "globals.validations.requiredField" },
            { fieldName: formatMessage({ id: "globals.departurePlace" }) }
        )),
        arrivalPlace: yup.string().required(formatMessage(
            { id: "globals.validations.requiredField" },
            { fieldName: formatMessage({ id: "globals.arrivalPlace" }) }
        )),
        routeId: yup.string().required(formatMessage(
            { id: "globals.validations.requiredField" },
            { fieldName: formatMessage({ id: "globals.arrivalPlace" }) }
        )),
        driverId: yup.string().nullable()
            .default(defaultValues.driverId),
        carId: yup.string().nullable()
            .default(defaultValues.carId),
        routeId: yup.string().nullable()
            .default(defaultValues.routeId)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

const dateToInputValue = (date?: Date) => date?.toISOString().slice(0, 10) ?? "";

const dateFromInputValue = (value: string | null) =>
    value ? new Date(`${value}T00:00:00.000Z`) : null;

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useBookingEditFormController = (id: string, onSubmit?: () => void): BookingEditFormController => {
    const { defaultValues, resolver } = useInitBookingEditForm(id);
    const { updateBooking: { mutation, key: mutationKey }, getBookings: { key: queryKey }, getBooking: { key: bookingQueryKey, query: getBooking } } = useBookingApi();
    const { getRoutesWithLocationsDictionary: { key: departurePlacesQueryKey, query: getDeparturePlaces } } = useRouteApi();
    const { mutateAsync: update, status } = useMutation([mutationKey], mutation);
    const { data: bookingResponse, isLoading: isLoadingBooking, isError: isErrorLoadingBooking } = useQuery(
        [bookingQueryKey, id],
        () => getBooking(id),
        { enabled: Boolean(id) }
    );
    const { data: departurePlacesResponse, isLoading: isLoadingDeparturePlaces, isError: isErrorLoadingDeparturePlaces } = useQuery(
        [departurePlacesQueryKey],
        getDeparturePlaces
    );
    const routesByDeparturePlace = departurePlacesResponse?.response ?? {};
    const departurePlaces = Object.keys(routesByDeparturePlace);
    const queryClient = useQueryClient();
    const { register, handleSubmit, watch, reset, setValue, clearErrors, control, formState: { errors } } = useForm<BookingEditFormModel>({
        defaultValues,
        resolver
    });
    const booking = bookingResponse?.response;

    useEffect(() => {
        if (!booking || !departurePlacesResponse?.response) {
            return;
        }

        const matchingDeparturePlace = Object.entries(routesByDeparturePlace).find(([place, routes]) =>
            place === booking.startingLocationCity || routes.some(route => route.id === booking.routeId)
        )?.[0] ?? booking.startingLocationCity ?? "";
        const selectedRoute = routesByDeparturePlace[matchingDeparturePlace]?.find(route => route.id === booking.routeId);

        reset({
            id,
            bookingDate: dateToInputValue(booking.bookingDate),
            departureDate: dateToInputValue(booking.departureDate),
            departurePlace: matchingDeparturePlace,
            arrivalPlace: [selectedRoute?.finalLocationCity ?? booking.finalLocationCity, selectedRoute?.finalLocationCountry ?? booking.finalLocationCountry].filter(Boolean).join(", "),
            driverId: booking.driverId ?? null,
            carId: booking.carId ?? null,
            routeId: booking.routeId ?? ""
        });
    }, [booking, departurePlacesResponse, id, reset, routesByDeparturePlace]);

    const selectedDeparturePlace = watch("departurePlace");
    const arrivalRoutes = routesByDeparturePlace[selectedDeparturePlace] ?? [];

    useEffect(() => {
        const selectedRoute = arrivalRoutes.find(route => route.id === watch("routeId"));
        if (selectedRoute) {
            setValue("arrivalPlace", [selectedRoute.finalLocationCity, selectedRoute.finalLocationCountry].filter(Boolean).join(", "));
        }
    }, [arrivalRoutes, setValue, watch]);

    const submit = useCallback((data: BookingEditFormModel) => {
        const bookingUpdate: BookingUpdateDTO = {
            ...data,
            bookingDate: dateFromInputValue(data.bookingDate),
            departureDate: dateFromInputValue(data.departureDate)
        };

        return update(bookingUpdate).then(() => {
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
            watch, // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
            control,
            setValue,
            clearErrors
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading", // Return if the form is still submitting or nit.
            isLoadingBooking,
            isErrorLoadingBooking,
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