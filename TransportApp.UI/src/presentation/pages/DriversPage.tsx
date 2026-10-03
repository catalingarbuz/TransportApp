import { yupResolver } from "@hookform/resolvers/yup";
import { useDriverApi } from "@infrastructure/apis/api-management";
import { DriverAddDTO, DriverDTO, UserRoleEnum } from "@infrastructure/apis/client";
import { Button, CircularProgress, FormControl, FormHelperText, FormLabel, Grid, OutlinedInput, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Typography } from "@mui/material";
import { Box } from "@mui/system";
import { ContentCard } from "@presentation/components/ui/ContentCard";
import { Seo } from "@presentation/components/ui/Seo";
import { WebsiteLayout } from "presentation/layouts/WebsiteLayout";
import { useForm } from "react-hook-form";
import { useIntl } from "react-intl";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { isUndefined } from "lodash";
import { Fragment } from "react";
import * as yup from "yup";
import { usePaginationController } from "@presentation/components/ui/Tables/Pagination.controller";
import { useTableController } from "@presentation/components/ui/Tables/Table.controller";

type DriverFormValues = {
  name: string;
  email: string;
  password: string;
  phoneNumber: string;
};

export const DriversPage = () => {
  const { formatMessage } = useIntl();
  const { getDrivers: { key: driversKey, query: getDrivers }, addDriver: { key: addDriverKey, mutation: addDriver } } = useDriverApi();
  const queryClient = useQueryClient();
  const { page, pageSize, setPagination } = usePaginationController();
  const { data: driversResponse, isLoading: isLoadingDrivers, isError: isDriversError } = useQuery(
    [driversKey, page, pageSize],
    () => getDrivers({ page, pageSize })
  );
  const driversPage = driversResponse?.response;
  const drivers: DriverDTO[] = driversPage?.data ?? [];
  const { handleChangePage, handleChangePageSize, labelDisplay } = useTableController(setPagination, driversPage?.pageSize);
  const { mutateAsync: createDriver, status } = useMutation([addDriverKey], addDriver);
  const defaults: DriverFormValues = { name: "", email: "", password: "", phoneNumber: "" };
  const requiredMessage = (fieldName: string) => formatMessage(
    { id: "globals.validations.requiredField" },
    { fieldName }
  );
  const schema = yup.object({
    name: yup.string().required(requiredMessage(formatMessage({ id: "globals.name" }))),
    email: yup.string().required(requiredMessage(formatMessage({ id: "globals.email" }))).email(),
    password: yup.string().required(requiredMessage(formatMessage({ id: "globals.password" }))),
    phoneNumber: yup.string().required(requiredMessage(formatMessage({ id: "globals.number" })))
  });
  const { register, handleSubmit, reset, formState: { errors } } = useForm<DriverFormValues>({ defaultValues: defaults, resolver: yupResolver(schema) });

  const submit = (values: DriverFormValues) => {
    const driver: DriverAddDTO = { ...values, role: UserRoleEnum.Driver };
    return createDriver(driver).then(() => {
      reset(defaults);
      return queryClient.invalidateQueries([driversKey]);
    });
  };

  return <Fragment>
    <Seo title="Transport Company | Drivers" />
    <WebsiteLayout>
      <Box sx={{ padding: "0 50px", justifyItems: "center" }}>
        <ContentCard>
          <Typography variant="h5" component="h1" gutterBottom>{formatMessage({ id: "globals.addDriver" })}</Typography>
          <form onSubmit={handleSubmit(submit)}>
            <Stack spacing={3}>
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.name)}>
                    <FormLabel required>{formatMessage({ id: "globals.name" })}</FormLabel>
                    <OutlinedInput {...register("name")} />
                    <FormHelperText>{errors.name?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.email)}>
                    <FormLabel required>{formatMessage({ id: "globals.email" })}</FormLabel>
                    <OutlinedInput type="email" {...register("email")} />
                    <FormHelperText>{errors.email?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.password)}>
                    <FormLabel required>{formatMessage({ id: "globals.password" })}</FormLabel>
                    <OutlinedInput type="password" {...register("password")} />
                    <FormHelperText>{errors.password?.message}</FormHelperText>
                  </FormControl>
                </Grid>
                <Grid item xs={12} md={6}>
                  <FormControl fullWidth error={!isUndefined(errors.phoneNumber)}>
                    <FormLabel required>{formatMessage({ id: "globals.number" })}</FormLabel>
                    <OutlinedInput type="tel" {...register("phoneNumber")} />
                    <FormHelperText>{errors.phoneNumber?.message}</FormHelperText>
                  </FormControl>
                </Grid>
              </Grid>
              <Button variant="contained" type="submit" disabled={status === "loading"}>
                {status === "loading" ? <CircularProgress size={24} /> : formatMessage({ id: "globals.addDriver" })}
              </Button>
            </Stack>
          </form>
          <Typography variant="h6" component="h2" sx={{ mt: 5, mb: 2 }}>{formatMessage({ id: "globals.drivers" })}</Typography>
          {isDriversError && <FormHelperText error>{formatMessage({ id: "globals.loadingFailed" })}</FormHelperText>}
          {isLoadingDrivers && <CircularProgress size={24} />}
          {driversPage && <TablePagination
            component="div"
            count={driversPage.totalCount ?? 0}
            page={driversPage.totalCount ? (driversPage.page ?? page) - 1 : 0}
            onPageChange={handleChangePage}
            rowsPerPage={driversPage.pageSize ?? pageSize}
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
                  <TableCell>{formatMessage({ id: "globals.name" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.email" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.number" })}</TableCell>
                  <TableCell>{formatMessage({ id: "globals.role" })}</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {drivers.map(driver => <TableRow key={driver.id}>
                  <TableCell>{driver.name}</TableCell>
                  <TableCell>{driver.email}</TableCell>
                  <TableCell>{driver.phoneNumber}</TableCell>
                  <TableCell>{driver.role}</TableCell>
                </TableRow>)}
              </TableBody>
            </Table>
          </TableContainer>
        </ContentCard>
      </Box>
    </WebsiteLayout>
  </Fragment>;
};