import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired
} from "react-hook-form";

export type SignupFormModel = {
    name: string;
    email: string;
    password: string;
    phoneNumber: string;
};

export type SignupFormState = {
    errors: FieldErrorsImpl<DeepRequired<SignupFormModel>>;
};

export type SignupFormActions = {
    register: UseFormRegister<SignupFormModel>;
    handleSubmit: UseFormHandleSubmit<SignupFormModel>;
    submit: (body: SignupFormModel) => void;
};
export type SignupFormComputed = {
    defaultValues: SignupFormModel,
    isSubmitting: boolean
};

export type SignupFormController = FormController<SignupFormState, SignupFormActions, SignupFormComputed>;