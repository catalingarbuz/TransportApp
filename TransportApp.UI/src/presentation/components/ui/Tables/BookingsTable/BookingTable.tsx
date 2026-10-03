import { useIntl } from "react-intl";
import { isUndefined } from "lodash";
import { IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from "@mui/material";
import { DataLoadingContainer } from "../../LoadingDisplay";
import { useBookingTableController } from "./BookingTable.controller";
import { BookingDTO, UserRoleEnum } from "@infrastructure/apis/client";
import DeleteIcon from '@mui/icons-material/Delete';
import { BookingAddDialog } from "../../Dialogs/BookingAddDialog/BookingAddDialog";
import { useAppSelector } from "@application/store";
import { useOwnUserHasRole } from "@infrastructure/hooks/useOwnUser";
import { BookingEditDialog } from "../../Dialogs/BookingAddDialog/BookingEditDialog";
import countries from "i18n-iso-countries";
import englishCountryNames from "i18n-iso-countries/langs/en.json";
import romanianCountryNames from "i18n-iso-countries/langs/ro.json";

countries.registerLocale(englishCountryNames);
countries.registerLocale(romanianCountryNames);

/**
 * This hook returns a header for the table with translated columns.
 */
const useHeader = (): { key: keyof BookingDTO, name: string }[] => {
    const { formatMessage } = useIntl();

    return [
        { key: "startingLocationCity", name: formatMessage({ id: "globals.departurePlace" }) },
        { key: "finalLocationCity", name: formatMessage({ id: "globals.arrivalPlace" }) },
        { key: "departureDate", name: formatMessage({ id: "globals.departureDate" }) },
    ]
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

/**
 * The values in the table are organized as rows so this function takes the entries and creates the row values ordering them according to the order map.
 */
const getRowValues = (entries: BookingDTO[] | null | undefined, orderMap: { [key: string]: number }) =>
    entries?.map(
        entry => {
            return {
                entry: entry,
                data: Object.entries(entry).filter(([e]) => !isUndefined(orderMap[e])).sort(([a], [b]) => orderMap[a] - orderMap[b]).map(([key, value]) => { return { key, value } })
            }
        });

/**
 * Creates the user table.
 */
export const BookingTable = () => {
    const { userId: ownUserId } = useAppSelector(x => x.profileReducer);
    const isAdmin = useOwnUserHasRole(UserRoleEnum.Admin);
    const { formatMessage } = useIntl();
    const header = useHeader();
    const orderMap = header.reduce((acc, e, i) => { return { ...acc, [e.key]: i } }, {}) as { [key: string]: number }; // Get the header column order.
    const { handleChangePage, handleChangePageSize, pagedData, isError, isLoading, tryReload, labelDisplay, remove } = useBookingTableController(); // Use the controller hook.
    const rowValues = getRowValues(pagedData?.data, orderMap); // Get the row values.

    return <DataLoadingContainer isError={isError} isLoading={isLoading} tryReload={tryReload}> {/* Wrap the table into the loading container because data will be fetched from the backend and is not immediately available.*/}
        <BookingAddDialog /> {/* Add the button to open the user add modal. */}
        {!isUndefined(pagedData) && !isUndefined(pagedData?.totalCount) && !isUndefined(pagedData?.page) && !isUndefined(pagedData?.pageSize) &&
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
            />}

        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Edit</TableCell>
                        {
                            header.map(e => <TableCell key={`header_${String(e.key)}`}>{e.name}</TableCell>) // Add the table header.
                        }
                        <TableCell>{formatMessage({ id: "labels.actions" })}</TableCell> {/* Add additional header columns if needed. */}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        rowValues?.map(({ data, entry }, rowIndex) => <TableRow key={`row_${rowIndex + 1}`}>
                            {(() => {
                                const canManageBooking = isAdmin || entry.userId === ownUserId;
                                return <>
                            <TableCell>
                                {canManageBooking && <BookingEditDialog id={entry.id || ''} />} </TableCell>
                            {data.map((keyValue, index) => {
                                const countryKey = keyValue.key === "startingLocationCity"
                                    ? "startingLocationCountry"
                                    : keyValue.key === "finalLocationCity"
                                        ? "finalLocationCountry"
                                        : undefined;
                                const country = countryKey ? entry[countryKey] : undefined;
                                const value = keyValue.value instanceof Date
                                    ? keyValue.value.toLocaleDateString()
                                    : keyValue.value;
                                return <TableCell key={`cell_${rowIndex + 1}_${index + 1}`}>
                                    {countryKey && country ? `${value ?? ""} (${getCountryAbbreviation(country)})` : value}
                                </TableCell>;
                            })} {/* Add the row values. */}
                            {/* Add the row values. */}
                            <TableCell>
                                {canManageBooking && <IconButton color="error" onClick={() => remove(entry.id || '')}>
                                    <DeleteIcon color="error" fontSize='small' />
                                </IconButton>}
                            </TableCell>
                                </>;
                            })()}
                        </TableRow>)
                    }
                </TableBody>
            </Table>
        </TableContainer>
    </DataLoadingContainer >
}