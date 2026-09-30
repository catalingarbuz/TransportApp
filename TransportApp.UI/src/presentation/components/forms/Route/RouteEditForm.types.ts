import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    Control
} from "react-hook-form";
import { LocationDTO } from "@infrastructure/apis/client";

export type RouteEditFormModel = {
    id: string;
    startingLocationId: string;
    finalLocationId: string;
    departureTime: string;
    arrivalTime: string;
};

export type RouteEditFormState = {
    errors: FieldErrorsImpl<DeepRequired<RouteEditFormModel>>;
};

export type RouteEditFormActions = {
    register: UseFormRegister<RouteEditFormModel>;
    control: Control<RouteEditFormModel>;
    handleSubmit: UseFormHandleSubmit<RouteEditFormModel>;
    submit: (body: RouteEditFormModel) => void;
};

export type RouteEditFormComputed = {
    defaultValues: RouteEditFormModel,
    isSubmitting: boolean,
    isLoadingRoute: boolean,
    isErrorLoadingRoute: boolean,
    locations: LocationDTO[],
    isLoadingLocations: boolean,
    isErrorLoadingLocations: boolean
};

export type RouteEditFormController = FormController<RouteEditFormState, RouteEditFormActions, RouteEditFormComputed>;