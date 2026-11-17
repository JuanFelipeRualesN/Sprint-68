import React, { useState } from "react";
import { Dialog } from "@material-ui/core";
import Button from "./button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Loading from "./loading";
import {
  faExclamationTriangle,
  faTimes,
  faTrashAlt,
} from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";

export default function DeleteModal({
                                      disabled,
  onConfirmation,
  title,
  description,
  confirmButtonText,
  cancelButtonText,
  isLoading = false,
  onCompleted = () => {},
}) {
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
        icon={<FontAwesomeIcon icon={faTrashAlt} />}
        onClick={handleOpen}
        disabled={disabled}
      />

      <Dialog className="overflow-hidden" open={open} onClose={handleClose}>
        <div className="contentModal--confirmation">
          <div className="contentModal--close" onClick={handleClose}>
            <FontAwesomeIcon icon={faTimes} />
          </div>
          <div className="region-modalIcon modalIcon__warning">
            <FontAwesomeIcon icon={faExclamationTriangle} />
          </div>
          <div className="Modal--confirmation-body">
            <p className="Modal--confirmation__title">
              <Translate id={title} />
            </p>
            <p className="Modal--confirmation__desc">
              <Translate id={description} />
            </p>
          </div>
          <div className="Modal--confirmation-footer">
            <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
              <Button
                variant="primary"
                onClick={async () => await handleConfirmation()}
                disabled={isLoading || disabled}
              >
                {isLoading ? (
                  <Loading
                    show={isLoading}
                    type="button"
                    text={<Translate id={confirmButtonText} />}
                  />
                ) : (
                  <Translate id={confirmButtonText} />
                )}
              </Button>
              <Button
                variant="regular"
                className="ml-2"
                onClick={handleClose}
                disabled={isLoading}
              >
                <Translate id={cancelButtonText} />
              </Button>
            </div>
          </div>
        </div>
      </Dialog>
    </>
  );
}
