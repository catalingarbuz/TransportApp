import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch,
    UseFormSetValue
} from "react-hook-form";
import { Control } from "react-hook-form";
import { CarDTO } from "@infrastructure/apis/client";
import { LocationDTO } from "@infrastructure/apis/client/models/LocationDTO";

export type RouteAddFormModel = {
    startingLocationId: string;
    finalLocationId: string;
    departureTime: string;
    arrivalTime: string;
    carIds: string[];
};

export type RouteAddFormState = {
    errors: FieldErrorsImpl<DeepRequired<RouteAddFormModel>>;
};

export type RouteAddFormActions = {
    register: UseFormRegister<RouteAddFormModel>;
    watch: UseFormWatch<RouteAddFormModel>;
    control: Control<RouteAddFormModel>;
    handleSubmit: UseFormHandleSubmit<RouteAddFormModel>;
    setValue: UseFormSetValue<RouteAddFormModel>;
    submit: (body: RouteAddFormModel) => void;
};

export type RouteAddFormComputed = {
    defaultValues: RouteAddFormModel,
    isSubmitting: boolean,
    locations: LocationDTO[],
    isLoadingLocations: boolean,
    isErrorLoadingLocations: boolean,
    cars: CarDTO[],
    isLoadingCars: boolean,
    isErrorLoadingCars: boolean
};

export type RouteAddFormController = FormController<RouteAddFormState, RouteAddFormActions, RouteAddFormComputed>;