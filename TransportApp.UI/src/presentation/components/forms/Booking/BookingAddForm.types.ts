import { UserRoleEnum } from "@infrastructure/apis/client";
import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch,
    Control,
    UseFormSetValue,
    UseFormClearErrors
} from "react-hook-form";
import { SelectChangeEvent } from "@mui/material";
import { RouteDTO } from "@infrastructure/apis/client";

export type BookingAddFormModel = {
    routeName: string;
    bookingDate: Date;
    departureDate: Date;
    departurePlace: string;
    routeId: string;
    driverId: string;
};

export type BookingAddFormState = {
    errors: FieldErrorsImpl<DeepRequired<BookingAddFormModel>>;
};

export type BookingAddFormActions = {
    register: UseFormRegister<BookingAddFormModel>;
    watch: UseFormWatch<BookingAddFormModel>;
    control: Control<BookingAddFormModel>;
    setValue: UseFormSetValue<BookingAddFormModel>;
    clearErrors: UseFormClearErrors<BookingAddFormModel>;
    handleSubmit: UseFormHandleSubmit<BookingAddFormModel>;
    submit: (body: BookingAddFormModel) => void;
};

export type BookingAddFormComputed = {
    defaultValues: BookingAddFormModel,
    isSubmitting: boolean,
    departurePlaces: string[],
    arrivalRoutes: RouteDTO[],
    hasDeparturePlaceSelected: boolean,
    isLoadingDeparturePlaces: boolean,
    isErrorLoadingDeparturePlaces: boolean
};

export type BookingAddFormController = FormController<BookingAddFormState, BookingAddFormActions, BookingAddFormComputed>;