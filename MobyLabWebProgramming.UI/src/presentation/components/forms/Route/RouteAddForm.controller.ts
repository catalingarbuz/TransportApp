import { RouteAddFormModel} from "./RouteAddForm.types";
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
const getDefaultValues = (initialData?: RouteAddFormModel) => {
    const defaultValues = {
        routeName: "",
        description: "",
        routeLength: ""
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
const useInitRouteAddForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        routeName: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.routeName",
                    }),
                }))
            .default(defaultValues.routeName),
        description: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.description",
                    }),
                }))
            .default(defaultValues.description),
        routeLength: yup.string()
            .default(defaultValues.routeLength)
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
    const { mutateAsync: add, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const submit = useCallback((data: RouteAddFormModel) => // Create a submit callback to send the form data to the backend.
        add(data).then(() => {
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