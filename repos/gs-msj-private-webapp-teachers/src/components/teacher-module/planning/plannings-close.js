import React, { useState } from "react";
import { Dialog } from "@material-ui/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "./shared/components/button";
import Alert from "./shared/components/alert";
import Heading from "./shared/components/heading";
import { faTimes } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";

export default function PlanningsClose({ onConfirmation, onCompleted }) {
  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleConfirmation = async () => {
    const completed = await onConfirmation();
    if (!completed) return;

    onCompleted();
    handleClose();
  };

  return (
    <>
      <Button
        className="space-margin-left-sm btn-w-iconAlert"
        variant="simple"
        icon={<FontAwesomeIcon icon={faTimes} />}
        onClick={handleOpen}
      >
        <Translate id="planning.close-planning" />
      </Button>
      <Dialog className="overflow-hidden" open={open} onClose={handleClose}>
        <div className="contentModal--confirmation">
          <div className="contentModal--close" onClick={handleClose}>
            <div className="wrapper-twoCol-highlight m-0">
              <div className="col pl-0">
                <Heading as="h2">
                  <Translate id="planning.close-planning" />
                </Heading>
              </div>
              <div className="col text-right">
                <FontAwesomeIcon icon={faTimes} />
              </div>
            </div>
          </div>
          <div className="Modal--confirmation-body px-0">
            <Alert
              message={<Translate id="planning.close-planning-alert" />}
              variant="alert"
              size="lg"
            />
          </div>
          <div className="Modal--confirmation-footer">
            <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
              <Button
                variant="primary"
                onClick={async () => await handleConfirmation()}
              >
                <Translate id="planning.close-planning-confirmation" />
              </Button>
              <Button variant="regular" className="ml-2" onClick={handleClose}>
                <Translate id="confirm-dialog.button-cancel" />
              </Button>
            </div>
          </div>
        </div>
      </Dialog>
    </>
  );
}
