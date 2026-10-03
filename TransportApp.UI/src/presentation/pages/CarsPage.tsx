import { yupResolver } from "@hookform/resolvers/yup";
import { useCarApi, useDriverApi } from "@infrastructure/apis/api-management";
import { CarAddDTO, CarDTO, DriverDTO } from "@infrastructure/apis/client";
import { Button, CircularProgress, FormControl, FormHelperText, FormLabel, Grid, MenuItem, OutlinedInput, Paper, Select, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from "@mui/material";
import { Box } from "@mui/system";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { Seo } from "@presentation/components/ui/Seo";
import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { Controller, useForm } from "react-hook-form";
import { useIntl } from "react-intl";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { isUndefined } from "lodash";
import { Fragment } from "react";
import * as yup from "yup";
import { usePaginationController } from "@presentation/components/ui/Tables/Pagination.controller";
import { useTableController } from "@presentation/components/ui/Tables/Table.controller";

type CarFormValues = {
  brand: string;
  model: string;
  registrationNumber: string;
  numberOfSeats: number;
  driverId: string;
};

export const CarsPage = () => {
  const { formatMessage } = useIntl();
  const { getCars: { key: carsKey, query: getCars }, addCar: { key: addCarKey, mutation: addCar } } = useCarApi();
  const { getDrivers: { key: driversKey, query: getDrivers } } = useDriverApi();
  const queryClient = useQueryClient();
  const { page, pageSize, setPagination } = usePaginationController();
  const { data: carsResponse, isLoading: isLoadingCars, isError: isCarsError } = useQuery(
    [carsKey, page, pageSize],
    () => getCars({ page, pageSize })
  );
  const carsPage = carsResponse?.response;
  const cars: CarDTO[] = carsPage?.data ?? [];
  const { handleChangePage, handleChangePageSize, labelDisplay } = useTableController(setPagination, carsPage?.pageSize);
  const { data: driversResponse, isLoading: isLoadingDrivers, isError: isDriversError } = useQuery(
    [driversKey, "car-form", 1, 1000],
    () => getDrivers({ page: 1, pageSize: 1000 })
  );
  const drivers: DriverDTO[] = driversResponse?.response?.data ?? [];
  const { mutateAsync: createCar, status } = useMutation([addCarKey], addCar);
  const defaults: CarFormValues = { brand: "", model: "", registrationNumber: "", numberOfSeats: 1, driverId: "" };
  const requiredMessage = (fieldName: string) => formatMessage(
    { id: "globals.validations.requiredField" },
    { fieldName }
  );
  const schema = yup.object({
    brand: yup.string().required(requiredMessage(formatMessage({ id: "globals.brand" }))),
    model: yup.string().required(requiredMessage(formatMessage({ id: "globals.model" }))),
    registrationNumber: yup.string().required(requiredMessage(formatMessage({ id: "globals.registrationNumber" }))),
    numberOfSeats: yup.number().typeError(requiredMessage(formatMessage({ id: "globals.numberOfSeats" }))).integer().min(1).required(requiredMessage(formatMessage({ id: "globals.numberOfSeats" }))),
    driverId: yup.string().nullable()
  });
  const { register, control, handleSubmit, reset, formState: { errors } } = useForm<CarFormValues>({ defaultValues: defaults, resolver: yupResolver(schema) });

  const submit = (values: CarFormValues) => {
    const car: CarAddDTO = { ...values, driverId: values.driverId || null };
    return createCar(car).then(() => {
      reset(defaults);
      return queryClient.invalidateQueries([carsKey]);
    });
  };

  return <Fragment>
    <Seo title="Transport Company | Cars" />
    <WebsiteLayout>
      <Box sx={{ padding: "0 50px", justifyItems: "center" }}>
        <ContentCard>
          <Typography variant="h5" component="h1" gutterBottom>{formatMessage({ id: "globals.addCar" })}</Typography>
          <form onSubmit={handleSubmit(submit)}>
            <Stack spacing={3}>
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.brand)}>
                    <FormLabel required>{formatMessage({ id: "globals.brand" })}</FormLabel>
                    <OutlinedInput {...register("brand")} />
                    <FormHelperText>{errors.brand?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.model)}>
                    <FormLabel required>{formatMessage({ id: "globals.model" })}</FormLabel>
                    <OutlinedInput {...register("model")} />
                    <FormHelperText>{errors.model?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.registrationNumber)}>
                    <FormLabel required>{formatMessage({ id: "globals.registrationNumber" })}</FormLabel>
                    <OutlinedInput {...register("registrationNumber")} />
                    <FormHelperText>{errors.registrationNumber?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.numberOfSeats)}>
                    <FormLabel required>{formatMessage({ id: "globals.numberOfSeats" })}</FormLabel>
                    <OutlinedInput type="number" inputProps={{ min: 1, step: 1 }} {...register("numberOfSeats", { valueAsNumber: true })} />
                    <FormHelperText>{errors.numberOfSeats?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12}>
                  <FormControl fullWidth>
                    <FormLabel>{formatMessage({ id: "globals.driver" })}</FormLabel>
                    <Controller
                      control={control}
                      name="driverId"
                      render={({ field }) => <Select {...field} displayEmpty disabled={isLoadingDrivers || isDriversError}>
                        {isLoadingDrivers && <MenuItem value="" disabled>{formatMessage({ id: "globals.loading" })}</MenuItem>}
                        {!isLoadingDrivers && <MenuItem value="">{formatMessage({ id: "globals.noDriverAssigned" })}</MenuItem>}
                        {drivers.map(driver => <MenuItem key={driver.id} value={driver.id ?? ""}>{driver.name || driver.email || driver.id}</MenuItem>)}
                      </Select>}
                    />
                    {isDriversError && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
                    {!isLoadingDrivers && !isDriversError && drivers.length === 0 && <FormHelperText>{formatMessage({ id: "globals.noDriversAvailable" })}</FormHelperText>}
                  </FormControl>
                </Grid>
              </Grid>
              <Button variant="contained" type="submit" disabled={status === "loading"}>
                {status === "loading" ? <CircularProgress size={24} /> : formatMessage({ id: "globals.addCar" })}
              </Button>
            </Stack>
          </form>
          <Typography variant="h6" component="h2" sx={{ mt: 5, mb: 2 }}>{formatMessage({ id: "globals.cars" })}</Typography>
          {isCarsError && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
          {isLoadingCars && <CircularProgress size={24} />}
          {carsPage && <TablePagination
            component="div"
            count={carsPage.totalCount ?? 0}
            page={carsPage.totalCount ? (carsPage.page ?? page) - 1 : 0}
            onPageChange={handleChangePage}
            rowsPerPage={carsPage.pageSize ?? pageSize}
            onRowsPerPageChange={handleChangePageSize}
            labelRowsPerPage={formatMessage({ id: "labels.itemsPerPage" })}
            labelDisplayedRows={labelDisplay}
            showFirstButton
            showLastButton
          />}
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>{formatMessage({ id: "globals.brand" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.model" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.registrationNumber" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.numberOfSeats" })}</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {cars.map(car => <TableRow key={car.id}>
                  <TableCell>{car.brand}</TableCell>
                  <TableCell>{car.model}</TableCell>
                  <TableCell>{car.registrationNumber}</TableCell>
                  <TableCell>{car.numberOfSeats}</TableCell>
                </TableRow>)}
              </TableBody>
            </Table>
          </TableContainer>
        </ContentCard>
      </Box>
    </WebsiteLayout>
  </Fragment>;
};