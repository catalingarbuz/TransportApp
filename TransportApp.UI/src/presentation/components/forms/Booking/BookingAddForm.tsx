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
import { useBookingAddFormController } from "./BookingAddForm.controller";
import { isUndefined } from "lodash";
import { Controller } from "react-hook-form";

/**
 * Here we declare the user add form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const BookingAddForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useBookingAddFormController(props.onSubmit); // Use the controller.

    return <form onSubmit={actions.handleSubmit(actions.submit)}> {/* Wrap your form into a form tag and use the handle submit callback to validate the form and call the data submission. */}
        <Stack spacing={4} style={{ width: "100%" }}>
            <Grid container item direction="row" xs={12} columnSpacing={4}>
                <Grid container item direction="column" xs={4}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.departurePlace)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.departurePlace" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="departurePlace"
                            render={({ field }) => <Select {...field} displayEmpty disabled={computed.isLoadingDeparturePlaces || computed.isErrorLoadingDeparturePlaces} onChange={event => {
                                field.onChange(event);
                                actions.setValue("routeId", "");
                                actions.clearErrors("routeId");
                            }}>
                                <MenuItem value="" disabled>
                                    {computed.isLoadingDeparturePlaces ? formatMessage({ id: "globals.loading" }) : formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: formatMessage({ id: "globals.departurePlace" }) })}
                                </MenuItem>
                                {computed.departurePlaces.map(place => <MenuItem key={place} value={place}>{place}</MenuItem>)}
                            </Select>}
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.departurePlace)}
                        >
                            {state.errors.departurePlace?.message}
                        </FormHelperText>
                        {computed.isErrorLoadingDeparturePlaces && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={4}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.routeId)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.arrivalPlace" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="routeId"
                            render={({ field }) => <Select {...field} displayEmpty disabled={!computed.hasDeparturePlaceSelected || computed.isLoadingDeparturePlaces || computed.isErrorLoadingDeparturePlaces}>
                                <MenuItem value="" disabled>
                                    {formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: computed.hasDeparturePlaceSelected
                                        ? formatMessage({ id: "globals.arrivalPlace" })
                                        : formatMessage({ id: "globals.departurePlace" }) })}
                                </MenuItem>
                                {computed.arrivalRoutes.map(route => <MenuItem key={route.id} value={route.id ?? ""}>
                                    {[route.finalLocationCity, route.finalLocationCountry].filter(Boolean).join(", ")}
                                </MenuItem>)}
                            </Select>}
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.routeId)}
                        >
                            {state.errors.routeId?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={4}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.departureDate)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.departureDate" />
                        </FormLabel>
                        <OutlinedInput
                            type="Date"
                            {...actions.register("departureDate")}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.departureDate)}
                        >
                            {state.errors.departureDate?.message}
                        </FormHelperText>
                        <FormHelperText
                            hidden={isUndefined(state.errors.departureDate)}
                        >
                            {state.errors.departureDate?.message}
                        </FormHelperText>
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