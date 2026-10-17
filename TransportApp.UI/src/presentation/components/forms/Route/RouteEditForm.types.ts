import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    Control,
    UseFormWatch,
    UseFormSetValue
} from "react-hook-form";
import { CarDTO } from "@infrastructure/apis/client";
import { LocationDTO } from "@infrastructure/apis/client/models/LocationDTO";

export type RouteEditFormModel = {
    id: string;
    startingLocationId: string;
    finalLocationId: string;
    departureTime: string;
    arrivalTime: string;
    carIds: string[];
};

export type RouteEditFormState = {
    errors: FieldErrorsImpl<DeepRequired<RouteEditFormModel>>;
};

export type RouteEditFormActions = {
    register: UseFormRegister<RouteEditFormModel>;
    control: Control<RouteEditFormModel>;
    handleSubmit: UseFormHandleSubmit<RouteEditFormModel>;
    watch: UseFormWatch<RouteEditFormModel>;
    setValue: UseFormSetValue<RouteEditFormModel>;
    submit: (body: RouteEditFormModel) => void;
};

export type RouteEditFormComputed = {
    defaultValues: RouteEditFormModel,
    isSubmitting: boolean,
    isLoadingRoute: boolean,
    isErrorLoadingRoute: boolean,
    locations: LocationDTO[],
    isLoadingLocations: boolean,
    isErrorLoadingLocations: boolean,
    cars: CarDTO[],
    isLoadingCars: boolean,
    isErrorLoadingCars: boolean
};

export type RouteEditFormController = FormController<RouteEditFormState, RouteEditFormActions, RouteEditFormComputed>;