import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
    ,Control,
    UseFormSetValue,
    UseFormClearErrors
} from "react-hook-form";
import { RouteDTO } from "@infrastructure/apis/client";


export type BookingEditFormModel = {
    id: string;
    bookingDate: string | null;
    departureDate: string | null;
    departurePlace: string;
    arrivalPlace: string;
    driverId: string | null;
    carId: string | null;
    routeId: string;
};

export type BookingEditFormState = {
    errors: FieldErrorsImpl<DeepRequired<BookingEditFormModel>>;
};

export type BookingEditFormActions = {
    register: UseFormRegister<BookingEditFormModel>;
    watch: UseFormWatch<BookingEditFormModel>;
    control: Control<BookingEditFormModel>;
    setValue: UseFormSetValue<BookingEditFormModel>;
    clearErrors: UseFormClearErrors<BookingEditFormModel>;
    handleSubmit: UseFormHandleSubmit<BookingEditFormModel>;
    submit: (body: BookingEditFormModel) => void;
};

export type BookingEditFormComputed = {
    defaultValues: BookingEditFormModel,
    isSubmitting: boolean,
    isLoadingBooking: boolean,
    isErrorLoadingBooking: boolean,
    departurePlaces: string[],
    arrivalRoutes: RouteDTO[],
    hasDeparturePlaceSelected: boolean,
    isLoadingDeparturePlaces: boolean,
    isErrorLoadingDeparturePlaces: boolean
};

export type BookingEditFormController = FormController<BookingEditFormState, BookingEditFormActions, BookingEditFormComputed>;