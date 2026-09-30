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
import { Controller } from "react-hook-form";

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
                    <FormControl fullWidth error={!isUndefined(state.errors.startingLocationId)}>
                        <FormLabel required id="edit-starting-location-label">
                            <FormattedMessage id="globals.startingLocation" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="startingLocationId"
                            render={({ field }) => <Select {...field} labelId="edit-starting-location-label" displayEmpty disabled={computed.isLoadingRoute || computed.isLoadingLocations || computed.isErrorLoadingLocations}>
                                <MenuItem value="" disabled>
                                    {computed.isLoadingLocations ? formatMessage({ id: "globals.loading" }) : formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: formatMessage({ id: "globals.startingLocation" }) })}
                                </MenuItem>
                                {computed.locations.map(location => <MenuItem key={location.id} value={location.id ?? ""}>
                                    {[location.city, location.country].filter(Boolean).join(", ")}
                                </MenuItem>)}
                            </Select>}
                        />
                        <FormHelperText hidden={isUndefined(state.errors.startingLocationId)}>
                            {state.errors.startingLocationId?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.finalLocationId)}>
                        <FormLabel required id="edit-final-location-label">
                            <FormattedMessage id="globals.finalLocation" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="finalLocationId"
                            render={({ field }) => <Select {...field} labelId="edit-final-location-label" displayEmpty disabled={computed.isLoadingRoute || computed.isLoadingLocations || computed.isErrorLoadingLocations}>
                                <MenuItem value="" disabled>
                                    {computed.isLoadingLocations ? formatMessage({ id: "globals.loading" }) : formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: formatMessage({ id: "globals.finalLocation" }) })}
                                </MenuItem>
                                {computed.locations.map(location => <MenuItem key={location.id} value={location.id ?? ""}>
                                    {[location.city, location.country].filter(Boolean).join(", ")}
                                </MenuItem>)}
                            </Select>}
                        />
                        <FormHelperText hidden={isUndefined(state.errors.finalLocationId)}>
                            {state.errors.finalLocationId?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.departureTime)}>
                        <FormLabel required><FormattedMessage id="globals.departureTime" /></FormLabel>
                        <OutlinedInput type="time" inputProps={{ step: 60 }} disabled={computed.isLoadingRoute} {...actions.register("departureTime")} />
                        <FormHelperText hidden={isUndefined(state.errors.departureTime)}>
                            {state.errors.departureTime?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.arrivalTime)}>
                        <FormLabel required><FormattedMessage id="globals.arrivalTime" /></FormLabel>
                        <OutlinedInput type="time" inputProps={{ step: 60 }} disabled={computed.isLoadingRoute} {...actions.register("arrivalTime")} />
                        <FormHelperText hidden={isUndefined(state.errors.arrivalTime)}>
                            {state.errors.arrivalTime?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                {(computed.isErrorLoadingRoute || computed.isErrorLoadingLocations) && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
            </Grid>
            <Grid container item direction="row" xs={12} className="padding-top-sm">
                <Grid container item direction="column" xs={12} md={4}></Grid>
                <Grid container item direction="column" justifyContent={"center"} xs={4}>
                    <Button variant="contained" type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting || computed.isLoadingRoute || computed.isErrorLoadingRoute || computed.isLoadingLocations || computed.isErrorLoadingLocations}> {/* Add a button with type submit to call the submission callback if the button is a descended of the form element. */}
                        {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                        {computed.isSubmitting && <CircularProgress />}
                    </Button>
                </Grid>
            </Grid>
        </Stack>
    </form>
};