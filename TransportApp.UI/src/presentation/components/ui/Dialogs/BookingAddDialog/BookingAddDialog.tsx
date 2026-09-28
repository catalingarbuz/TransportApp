import { Button, Dialog, DialogContent, DialogTitle } from "@mui/material";
import { useUserAddDialogController } from "./BookingAddDialog.controller";
import { BookingAddForm } from "@presentation/components/forms/Booking/BookingAddForm";
import { useIntl } from "react-intl";

/**
 * This component wraps the user add form into a modal dialog.
 */
export const BookingAddDialog = () => {
  const { open, close, isOpen } = useUserAddDialogController();
  const { formatMessage } = useIntl();

  return <div>
    <Button variant="contained" onClick={open}>
      {formatMessage({ id: "labels.addBooking" })}
    </Button>
    <Dialog
      open={isOpen}
      onClose={close}>
      <DialogTitle>
        {formatMessage({ id: "labels.addBooking" })}
      </DialogTitle>
      <DialogContent>
        <BookingAddForm onSubmit={close} />
      </DialogContent>
    </Dialog>
  </div>
};