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
import { useBookingEditFormController } from "./BookingEditForm.controller";
import { isEmpty, isUndefined } from "lodash";
import { Controller } from "react-hook-form";

/**
 * Here we declare the user add form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const BookingEditForm = (props: { id: string, onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useBookingEditFormController(props.id, props.onSubmit); // Use the controller.

    return <form onSubmit={actions.handleSubmit(actions.submit)}>
        <Stack spacing={4} sx={{ width: "100%" }}>
            <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                    <FormControl fullWidth>
                        <FormLabel><FormattedMessage id="globals.bookingDate" /></FormLabel>
                        <OutlinedInput type="date" {...actions.register("bookingDate")} />
                    </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                    <FormControl fullWidth error={!isUndefined(state.errors.departurePlace)}>
                        <FormLabel required><FormattedMessage id="globals.departurePlace" /></FormLabel>
                        <Controller
                            control={actions.control}
                            name="departurePlace"
                            render={({ field }) => <Select {...field} displayEmpty disabled={computed.isLoadingDeparturePlaces || computed.isErrorLoadingDeparturePlaces} onChange={event => {
                                field.onChange(event);
                                actions.setValue("routeId", "");
                                actions.setValue("arrivalPlace", "");
                                actions.clearErrors("routeId");
                                actions.clearErrors("arrivalPlace");
                            }}>
                                <MenuItem value="" disabled>
                                    {computed.isLoadingDeparturePlaces ? formatMessage({ id: "globals.loading" }) : formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: formatMessage({ id: "globals.departurePlace" }) })}
                                </MenuItem>
                                {computed.departurePlaces.map(place => <MenuItem key={place} value={place}>{place}</MenuItem>)}
                            </Select>}
                        />
                        <FormHelperText hidden={isUndefined(state.errors.departurePlace)}>{state.errors.departurePlace?.message}</FormHelperText>
                        {computed.isErrorLoadingDeparturePlaces && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                    <FormControl fullWidth error={!isUndefined(state.errors.routeId)}>
                        <FormLabel required><FormattedMessage id="globals.arrivalPlace" /></FormLabel>
                        <Controller
                            control={actions.control}
                            name="routeId"
                            render={({ field }) => <Select {...field} displayEmpty disabled={!computed.hasDeparturePlaceSelected || computed.isLoadingDeparturePlaces || computed.isErrorLoadingDeparturePlaces} onChange={event => {
                                field.onChange(event);
                                const route = computed.arrivalRoutes.find(candidate => candidate.id === event.target.value);
                                actions.setValue("arrivalPlace", route ? [route.finalLocationCity, route.finalLocationCountry].filter(Boolean).join(", ") : "");
                            }}>
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
                        <FormHelperText hidden={isUndefined(state.errors.routeId)}>{state.errors.routeId?.message}</FormHelperText>
                    </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                    <FormControl fullWidth error={!isUndefined(state.errors.departureDate)}>
                        <FormLabel><FormattedMessage id="globals.departureDate" /></FormLabel>
                        <OutlinedInput type="date" {...actions.register("departureDate")} />
                        <FormHelperText hidden={isUndefined(state.errors.departureDate)}>{state.errors.departureDate?.message}</FormHelperText>
                    </FormControl>
                </Grid>
            </Grid>
            {computed.isErrorLoadingBooking && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
            <Button variant="contained" type="submit" disabled={computed.isSubmitting || computed.isLoadingBooking || computed.isErrorLoadingBooking || computed.isLoadingDeparturePlaces || computed.isErrorLoadingDeparturePlaces}>
                {computed.isSubmitting ? <CircularProgress size={24} /> : <FormattedMessage id="globals.submit" />}
            </Button>
        </Stack>
    </form>
};