import {
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    FormLabel,
    Grid,
    Stack,
    OutlinedInput,
    Select,
    MenuItem
} from "@mui/material";
import { FormattedMessage, useIntl } from "react-intl";
import { useRouteAddFormController } from "./RouteAddForm.controller";
import { isEmpty, isUndefined } from "lodash";
import { UserRoleEnum } from "@infrastructure/apis/client";

/**
 * Here we declare the user add form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const RouteAddForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useRouteAddFormController(props.onSubmit); // Use the controller.

    return <form onSubmit={actions.handleSubmit(actions.submit)}> {/* Wrap your form into a form tag and use the handle submit callback to validate the form and call the data submission. */}
        <Stack spacing={4} style={{ width: "100%" }}>
            <Grid container item direction="row" xs={12} columnSpacing={4}>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.routeName)}
                    > {/* Wrap the input into a form control and use the errors to show the input invalid if needed. */}
                        <FormLabel required>
                            <FormattedMessage id="globals.routeName" />
                        </FormLabel> {/* Add a form label to indicate what the input means. */}
                        <OutlinedInput
                            {...actions.register("routeName")} // Bind the form variable to the UI input.
                            // put only departurePlace in placeHolder
                            placeholder={formatMessage({ id: "globals.routeName" })}
                            autoComplete="none"
                        /> {/* Add a input like a textbox shown here. */}
                        <FormHelperText
                            hidden={isUndefined(state.errors.routeName)}
                        >
                            {state.errors.routeName?.message}
                        </FormHelperText> {/* Add a helper text that is shown then the input has a invalid value. */}
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={8} md={12}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.description)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.description" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("description")}
                            placeholder={formatMessage({ id: "globals.description" })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.description)}
                        >
                            {state.errors.description?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.routeLength)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.routeLength" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("routeLength")}
                            placeholder={formatMessage({ id: "globals.routeLength" })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.routeLength)}
                        >
                            {state.errors.routeLength?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
            </Grid>
            <Grid container item direction="row" xs={12} className="padding-top-sm">
                <Grid container item direction="column" xs={12} md={4}></Grid>
                <Grid container item direction="column" justifyContent={"center"} xs={4}>
                    <Button variant="contained" type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting}> {/* Add a button with type submit to call the submission callback if the button is a descended of the form element. */}
                        {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                        {computed.isSubmitting && <CircularProgress />}
                    </Button>
                </Grid>
            </Grid>
        </Stack>
    </form>
};