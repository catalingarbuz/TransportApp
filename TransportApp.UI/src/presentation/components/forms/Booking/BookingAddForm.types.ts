import { UserRoleEnum } from "@infrastructure/apis/client";
import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";
import { SelectChangeEvent } from "@mui/material";

export type BookingAddFormModel = {
    routeName: string;
    bookingDate: Date;
    departureDate: Date;
    departurePlace: string;
    arrivalPlace: string;
    driverId: string;
};

export type BookingAddFormState = {
    errors: FieldErrorsImpl<DeepRequired<BookingAddFormModel>>;
};

export type BookingAddFormActions = {
    register: UseFormRegister<BookingAddFormModel>;
    watch: UseFormWatch<BookingAddFormModel>;
    handleSubmit: UseFormHandleSubmit<BookingAddFormModel>;
    submit: (body: BookingAddFormModel) => void;
};

export type BookingAddFormComputed = {
    defaultValues: BookingAddFormModel,
    isSubmitting: boolean
};

export type BookingAddFormController = FormController<BookingAddFormState, BookingAddFormActions, BookingAddFormComputed>;