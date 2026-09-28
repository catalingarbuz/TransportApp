import { Button, Dialog, DialogContent, DialogTitle } from "@mui/material";
import { useRouteAddDialogController } from "./RouteAddDialog.controller";
import { RouteAddForm } from "@presentation/components/forms/Route/RouteAddForm";
import { useIntl } from "react-intl";

/**
 * This component wraps the user add form into a modal dialog.
 */
export const RouteAddDialog = () => {
  const { open, close, isOpen } = useRouteAddDialogController();
  const { formatMessage } = useIntl();

  return <div>
    <Button variant="contained" onClick={open}>
      {formatMessage({ id: "labels.addRoute" })}
    </Button>
    <Dialog
      open={isOpen}
      onClose={close}>
      <DialogTitle>
        {formatMessage({ id: "labels.addRoute" })}
      </DialogTitle>
      <DialogContent>
        <RouteAddForm onSubmit={close} />
      </DialogContent>
    </Dialog>
  </div>
};