import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";


export type BookingEditFormModel = {
    id: string;
    bookingDate: Date | null;
    departureDate: Date | null;
    departurePlace: string | null;
    arrivalPlace: string | null;
    driverId: string | null;
    carId: string | null;
    routeId: string | null;
};

export type BookingEditFormState = {
    errors: FieldErrorsImpl<DeepRequired<BookingEditFormModel>>;
};

export type BookingEditFormActions = {
    register: UseFormRegister<BookingEditFormModel>;
    watch: UseFormWatch<BookingEditFormModel>;
    handleSubmit: UseFormHandleSubmit<BookingEditFormModel>;
    submit: (body: BookingEditFormModel) => void;
};

export type BookingEditFormComputed = {
    defaultValues: BookingEditFormModel,
    isSubmitting: boolean
};

export type BookingEditFormController = FormController<BookingEditFormState, BookingEditFormActions, BookingEditFormComputed>;