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
    MenuItem,
    Chip
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { FormattedMessage, useIntl } from "react-intl";
import { useRouteAddFormController } from "./RouteAddForm.controller";
import { isEmpty, isUndefined } from "lodash";
import { Controller } from "react-hook-form";
import { useState } from "react";

/**
 * Here we declare the user add form component.
 * This form may be used in modals so the onSubmit callback could close the modal on completion.
 */
export const RouteAddForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useRouteAddFormController(props.onSubmit); // Use the controller.
    const [carsOpen, setCarsOpen] = useState(false);

    return <form onSubmit={actions.handleSubmit(actions.submit)}> {/* Wrap your form into a form tag and use the handle submit callback to validate the form and call the data submission. */}
        <Stack spacing={4} style={{ width: "100%" }}>
            <Grid container item direction="row" xs={12} columnSpacing={4}>
            </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.startingLocationId)}>
                        <FormLabel required id="starting-location-label">
                            <FormattedMessage id="globals.startingLocation" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="startingLocationId"
                            render={({ field }) => <Select {...field} labelId="starting-location-label" displayEmpty disabled={computed.isLoadingLocations || computed.isErrorLoadingLocations}>
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
                        {computed.isErrorLoadingLocations && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.finalLocationId)}>
                        <FormLabel required id="final-location-label">
                            <FormattedMessage id="globals.finalLocation" />
                        </FormLabel>
                        <Controller
                            control={actions.control}
                            name="finalLocationId"
                            render={({ field }) => <Select {...field} labelId="final-location-label" displayEmpty disabled={computed.isLoadingLocations || computed.isErrorLoadingLocations}>
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
                        {computed.isErrorLoadingLocations && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.departureTime)}>
                        <FormLabel required><FormattedMessage id="globals.departureTime" /></FormLabel>
                        <OutlinedInput type="time" inputProps={{ step: 60 }} {...actions.register("departureTime")} />
                        <FormHelperText hidden={isUndefined(state.errors.departureTime)}>
                            {state.errors.departureTime?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={12}>
                    <FormControl fullWidth error={!isUndefined(state.errors.arrivalTime)}>
                        <FormLabel required><FormattedMessage id="globals.arrivalTime" /></FormLabel>
                        <OutlinedInput type="time" inputProps={{ step: 60 }} {...actions.register("arrivalTime")} />
                        <FormHelperText hidden={isUndefined(state.errors.arrivalTime)}>
                            {state.errors.arrivalTime?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={12}>
                    <FormControl fullWidth>
                        <FormLabel id="route-cars-label"><FormattedMessage id="globals.cars" /></FormLabel>
                        <Controller
                            control={actions.control}
                            name="carIds"
                            render={({ field }) => <Select
                                {...field}
                                multiple
                                displayEmpty
                                labelId="route-cars-label"
                                open={carsOpen}
                                onOpen={() => setCarsOpen(true)}
                                onClose={() => setCarsOpen(false)}
                                onChange={event => {
                                    field.onChange(event);
                                    setCarsOpen(false);
                                }}
                                disabled={computed.isLoadingCars || computed.isErrorLoadingCars}
                                renderValue={selected => {
                                    const selectedIds = selected as string[];
                                    if (selectedIds.length === 0) {
                                        return formatMessage({ id: "globals.placeholders.selectInput" }, { fieldName: formatMessage({ id: "globals.cars" }) });
                                    }

                                    return <Stack direction="row" spacing={0.5} useFlexGap flexWrap="wrap">
                                        {selectedIds.map(id => {
                                            const car = computed.cars.find(candidate => candidate.id === id);
                                            const label = car ? [car.brand, car.model, car.registrationNumber].filter(Boolean).join(" ") : id;
                                            return <Chip
                                                key={id}
                                                label={label}
                                                size="small"
                                                deleteIcon={<CloseIcon fontSize="small" />}
                                                onMouseDown={event => event.stopPropagation()}
                                                onDelete={event => {
                                                    event.stopPropagation();
                                                    field.onChange((field.value ?? []).filter(selectedId => selectedId !== id));
                                                }}
                                            />;
                                        })}
                                    </Stack>;
                                }}
                            >
                                {computed.cars.map(car => {
                                    const carId = car.id ?? "";
                                    const label = [car.brand, car.model, car.registrationNumber].filter(Boolean).join(" ");
                                    return <MenuItem key={carId} value={carId}>{label}</MenuItem>;
                                })}
                            </Select>}
                        />
                        {computed.isErrorLoadingCars && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    </FormControl>
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