import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";
import { SelectChangeEvent } from "@mui/material";

export type RouteAddFormModel = {
    routeName: string;
    description: string;
    routeLength: string;
};

export type RouteAddFormState = {
    errors: FieldErrorsImpl<DeepRequired<RouteAddFormModel>>;
};

export type RouteAddFormActions = {
    register: UseFormRegister<RouteAddFormModel>;
    watch: UseFormWatch<RouteAddFormModel>;
    handleSubmit: UseFormHandleSubmit<RouteAddFormModel>;
    submit: (body: RouteAddFormModel) => void;
};

export type RouteAddFormComputed = {
    defaultValues: RouteAddFormModel,
    isSubmitting: boolean
};

export type RouteAddFormController = FormController<RouteAddFormState, RouteAddFormActions, RouteAddFormComputed>;