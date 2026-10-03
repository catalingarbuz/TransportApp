import { useIntl } from "react-intl";
import { isUndefined, update } from "lodash";
import {
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
} from "@mui/material";
import { DataLoadingContainer } from "../../LoadingDisplay";
import { useRouteTableController } from "./RouteTable.controller";
import { RouteDTO, UserRoleEnum } from "@infrastructure/apis/client";
import DeleteIcon from "@mui/icons-material/Delete";
import { RouteAddDialog } from "../../Dialogs/RouteAddDialog/RouteAddDialog";
import { useAppSelector } from "@application/store";
import { useOwnUserHasRole } from "@infrastructure/hooks/useOwnUser";
import { RouteEditDialog } from "../../Dialogs/RouteAddDialog/RouteEditDialog";
import countries from "i18n-iso-countries";
import englishCountryNames from "i18n-iso-countries/langs/en.json";
import romanianCountryNames from "i18n-iso-countries/langs/ro.json";

countries.registerLocale(englishCountryNames);
countries.registerLocale(romanianCountryNames);

/**
 * This hook returns a header for the table with translated columns.
 */
type RouteTableColumn =
  | "startingLocationCity"
  | "finalLocationCity"
  | "departureTime"
  | "arrivalTime"
  | "assignedCars";

const useHeader = (): { key: RouteTableColumn; name: string }[] => {
  const { formatMessage } = useIntl();

  return [
    {
      key: "startingLocationCity",
      name: formatMessage({ id: "globals.departurePlace" }),
    },
    {
      key: "finalLocationCity",
      name: formatMessage({ id: "globals.arrivalPlace" }),
    },
    {
      key: "departureTime",
      name: formatMessage({ id: "globals.departureTime" }),
    },
    { key: "arrivalTime", name: formatMessage({ id: "globals.arrivalTime" }) },
    { key: "assignedCars", name: formatMessage({ id: "globals.assignedCars" }) },
  ];
};

const getCountryAbbreviation = (country: string) => {
  const name = country.trim();
  if (/^[a-z]{2}$/i.test(name) && countries.isValid(name)) {
    return name.toUpperCase();
  }

  return countries.getAlpha2Code(name, "en")
    ?? countries.getAlpha2Code(name, "ro")
    ?? country;
};

const formatTime = (date: Date) => {
  const hours = date.getUTCHours();
  const twelveHour = hours % 12 || 12;
  const minutes = String(date.getUTCMinutes()).padStart(2, "0");
  const period = hours >= 12 ? "PM" : "AM";

  return `${String(twelveHour).padStart(2, "0")}:${minutes} ${period}`;
};

/**
 * The values in the table are organized as rows so this function takes the entries and creates the row values ordering them according to the order map.
 */
const getRowValues = (
  entries: RouteDTO[] | null | undefined,
  orderMap: Record<RouteTableColumn, number>,
) =>
  entries?.map((entry) => {
    return {
      entry: entry,
      data: Object.entries(orderMap)
        .sort(([, firstOrder], [, secondOrder]) => firstOrder - secondOrder)
        .map(([rawKey]) => {
          const key = rawKey as RouteTableColumn;
          if (key === "assignedCars") {
            const value = (entry.assignedCars ?? [])
              .map(car => [car.brand, car.registrationNumber].filter(Boolean).join(" - "))
              .filter(Boolean)
              .join(", ");
            return { key, value };
          }

          const value = entry[key];
          const countryKey =
            key === "startingLocationCity"
              ? "startingLocationCountry"
              : key === "finalLocationCity"
                ? "finalLocationCountry"
                : undefined;
          const country = countryKey ? entry[countryKey] : undefined;
          const displayValue = countryKey && !isUndefined(country) && country !== null
            ? `${value ?? ""} (${getCountryAbbreviation(country)})`
            : value;
          return { key, value: displayValue };
        }),
    };
  });

/**
 * Creates the routes table.
 */
export const RouteTable = () => {
  const { userId: ownUserId } = useAppSelector((x) => x.profileReducer);
  const isAdmin = useOwnUserHasRole(UserRoleEnum.Admin);
  const { formatMessage } = useIntl();
  const header = useHeader();
  const orderMap = header.reduce((acc, e, i) => {
    return { ...acc, [e.key]: i };
  }, {}) as Record<RouteTableColumn, number>; // Get the header column order.
  const {
    handleChangePage,
    handleChangePageSize,
    pagedData,
    isError,
    isLoading,
    tryReload,
    labelDisplay,
    remove,
    update,
  } = useRouteTableController(); // Use the controller hook.
  const rowValues = getRowValues(pagedData?.data, orderMap); // Get the row values.

  return (
    <DataLoadingContainer
      isError={isError}
      isLoading={isLoading}
      tryReload={tryReload}
    >
      {" "}
      {/* Wrap the table into the loading container because data will be fetched from the backend and is not immediately available.*/}
      <RouteAddDialog /> {/* Add the button to open the user add modal. */}
      {!isUndefined(pagedData) &&
        !isUndefined(pagedData?.totalCount) &&
        !isUndefined(pagedData?.page) &&
        !isUndefined(pagedData?.pageSize) && (
          <TablePagination // Use the table pagination to add the navigation between the table pages.
            component="div"
            count={pagedData.totalCount} // Set the entry count returned from the backend.
            page={pagedData.totalCount !== 0 ? pagedData.page - 1 : 0} // Set the current page you are on.
            onPageChange={handleChangePage} // Set the callback to change the current page.
            rowsPerPage={pagedData.pageSize} // Set the current page size.
            onRowsPerPageChange={handleChangePageSize} // Set the callback to change the current page size.
            labelRowsPerPage={formatMessage({ id: "labels.itemsPerPage" })}
            labelDisplayedRows={labelDisplay}
            showFirstButton
            showLastButton
          />
        )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Edit</TableCell>
              {
                header.map((e) => (
                  <TableCell key={`header_${String(e.key)}`}>
                    {e.name}
                  </TableCell>
                )) // Add the table header.
              }
              <TableCell>{formatMessage({ id: "labels.actions" })}</TableCell>{" "}
              {/* Add additional header columns if needed. */}
            </TableRow>
          </TableHead>
          <TableBody>
            {rowValues?.map(({ data, entry }, rowIndex) => (
              <TableRow key={`row_${rowIndex + 1}`}>
                <TableCell>
                  {isAdmin && <RouteEditDialog id={entry.id || ""} />}{" "}
                </TableCell>
                {data.map((keyValue, index) => (
                  <TableCell key={`cell_${rowIndex + 1}_${index + 1}`}>
                    {keyValue.value instanceof Date
                      ? formatTime(keyValue.value)
                      : keyValue.value}
                  </TableCell>
                ))}{" "}
                {/* Add the row values. */}
                <TableCell>
                  {" "}
                  {/* Add other cells like action buttons. */}
                  {isAdmin && (
                    <IconButton
                      color="error"
                      onClick={() => remove(entry.id || "")}
                    >
                      <DeleteIcon color="error" fontSize="small" />
                    </IconButton>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </DataLoadingContainer>
  );
};
