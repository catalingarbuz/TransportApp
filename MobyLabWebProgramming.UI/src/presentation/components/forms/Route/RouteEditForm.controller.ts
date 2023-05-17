import { RouteEditFormModel, RouteEditFormController} from "./RouteEditForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useUserApi } from "@infrastructure/apis/api-management";
import { useCallback } from "react";
import { UserRoleEnum } from "@infrastructure/apis/client";
import { SelectChangeEvent } from "@mui/material";
import { useRouteApi } from "@infrastructure/apis/api-management/route";
import { RouteAddFormController } from "./RouteAddForm.types";

/**
 * Use a function to return the default values of the form and the validation schema.
 * You can add other values as the default, for example when populating the form with data to update an entity in the backend.
 */
const getDefaultValues = (id: string, initialData?: RouteEditFormModel) => {
    const defaultValues = {
        id : id,
        routeName: null,
        description: null,
        routeLength: null
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
        routeName: yup.string().nullable()
            .default(defaultValues.routeName),
        description: yup.string().nullable()
            .default(defaultValues.description),
        routeLength: yup.string().nullable()
            .default(defaultValues.routeLength)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

/**
 * Create a controller hook for the form and return any data that is necessary for the form.
 */
export const useRouteEditFormController = (id: string, onSubmit?: () => void): RouteEditFormController => {
    const { defaultValues, resolver } = useInitRouteEditForm(id);
    const { updateRute: { mutation, key: mutationKey }, getRoutes: { key: queryKey } } = useRouteApi();
    const { mutateAsync: update, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const submit = useCallback((data: RouteEditFormModel) => // Create a submit callback to send the form data to the backend.
        update(data).then(() => {
            queryClient.invalidateQueries([queryKey]); // If the form submission succeeds then some other queries need to be refresh so invalidate them to do a refresh.

            if (onSubmit) {
                onSubmit();
            }
        }), [update, queryClient, queryKey]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors }
    } = useForm<RouteEditFormModel>({ // Use the useForm hook to get callbacks and variables to work with the form.
        defaultValues, // Initialize the form with the default values.
        resolver // Add the validation resolver.
    });

    return {
        actions: { // Return any callbacks needed to interact with the form.
            handleSubmit, // Add the form submit handle.
            submit, // Add the submit handle that needs to be passed to the submit handle.
            register, // Add the variable register to bind the form fields in the UI with the form variables.
            watch // Add a watch on the variables, this function can be used to watch changes on variables if it is needed in some locations.
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading" // Return if the form is still submitting or nit.
        },
        state: {
            errors // Return what errors have occurred when validating the form input.
        }
    }
}