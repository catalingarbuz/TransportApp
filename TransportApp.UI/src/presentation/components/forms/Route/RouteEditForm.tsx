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
import { useRouteEditFormController } from "./RouteEditForm.controller";
import { isEmpty, isUndefined } from "lodash";
import { UserRoleEnum } from "@infrastructure/apis/client";

/**
 * Here we declare the user add form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const RouteEditForm = (props: { id: string, onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useRouteEditFormController(props.id, props.onSubmit); // Use the controller.

    return <form onSubmit={actions.handleSubmit(actions.submit)}> {/* Wrap your form into a form tag and use the handle submit callback to validate the form and call the data submission. */}
        <Stack spacing={4} style={{ width: "100%" }}>
            <Grid container item direction="row" xs={12} columnSpacing={4}>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl
                        fullWidth
                    > {/* Wrap the input into a form control and use the errors to show the input invalid if needed. */}
                        <FormLabel>
                            <FormattedMessage id="globals.routeName" />
                        </FormLabel> {/* Add a form label to indicate what the input means. */}
                        <OutlinedInput
                            {...actions.register("routeName")} // Bind the form variable to the UI input.
                            // put only departurePlace in placeHolder
                            placeholder={formatMessage({ id: "globals.routeName" })}
                            autoComplete="none"
                        /> {/* Add a input like a textbox shown here. */}
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={8} md={12}>
                    <FormControl
                        fullWidth
                    >
                        <FormLabel>
                            <FormattedMessage id="globals.description" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("description")}
                            placeholder={formatMessage({ id: "globals.description" })}
                            autoComplete="none"
                        />
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl
                        fullWidth
                    >
                        <FormLabel>
                            <FormattedMessage id="globals.routeLength" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("routeLength")}
                            placeholder={formatMessage({ id: "globals.routeLength" })}
                            autoComplete="none"
                        />
                    </FormControl>
                </Grid>
            </Grid>
            <Grid container item direction="row" xs={12} className="padding-top-sm">
                <Grid container item direction="column" xs={12} md={4}></Grid>
                <Grid container item direction="column" justifyContent={"center"} xs={4}>
                    <Button variant="contained" type="submit" disabled={computed.isSubmitting}> {/* Add a button with type submit to call the submission callback if the button is a descended of the form element. */}
                        {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                        {computed.isSubmitting && <CircularProgress />}
                    </Button>
                </Grid>
            </Grid>
        </Stack>
    </form>
};