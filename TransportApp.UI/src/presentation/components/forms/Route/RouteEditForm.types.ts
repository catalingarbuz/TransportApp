import { FormController } from "../FormController";
import {
    UseFormHandleSubmit,
    UseFormRegister,
    FieldErrorsImpl,
    DeepRequired,
    UseFormWatch
} from "react-hook-form";
import { SelectChangeEvent } from "@mui/material";

export type RouteEditFormModel = {
    id: string;
    routeName: any;
    description: any;
    routeLength: any;
};

export type RouteEditFormState = {
    errors: FieldErrorsImpl<DeepRequired<RouteEditFormModel>>;
};

export type RouteEditFormActions = {
    register: UseFormRegister<RouteEditFormModel>;
    watch: UseFormWatch<RouteEditFormModel>;
    handleSubmit: UseFormHandleSubmit<RouteEditFormModel>;
    submit: (body: RouteEditFormModel) => void;
};

export type RouteEditFormComputed = {
    defaultValues: RouteEditFormModel,
    isSubmitting: boolean
};

export type RouteEditFormController = FormController<RouteEditFormState, RouteEditFormActions, RouteEditFormComputed>;