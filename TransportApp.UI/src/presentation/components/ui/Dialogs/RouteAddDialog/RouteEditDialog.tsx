import { Button, Dialog, DialogContent, DialogTitle } from "@mui/material";
import { useRouteAddDialogController } from "./RouteAddDialog.controller";
import { RouteEditForm } from "@presentation/components/forms/Route/RouteEditForm";
import { useIntl } from "react-intl";
import { Edit } from "@mui/icons-material";
import { IconButton } from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';

/**
 * This component wraps the user add form into a modal dialog.
 */
// Add id parameter to the function

export const RouteEditDialog = (props: {id: string}) => {
  const { open, close, isOpen } = useRouteAddDialogController();
  const { formatMessage } = useIntl();

  return <div>
    <IconButton color="inherit" onClick={open}>
      <EditIcon />
    </IconButton>
    <Dialog
      open={isOpen}
      onClose={close}>
      <DialogTitle>
        {formatMessage({ id: "labels.editRoute" })}
      </DialogTitle>
      <DialogContent>
        <RouteEditForm id={props.id}  onSubmit={close} />
      </DialogContent>
    </Dialog>
  </div >
};