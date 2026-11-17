import React, { useState } from "react";
import { Dialog, DialogContent } from "@material-ui/core";
import Button from "./button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Heading from "./heading";
import Loading from "./loading";
import { Translate } from "react-localize-redux";
import {faEdit, faInfoCircle, faTimes} from "@fortawesome/free-solid-svg-icons";

export default function EditModal({
  disabled,
  onConfirmation,
  title,
  confirmButtonText,
  cancelButtonText,
  editButtonText,
  maxWidth = "lg",
  children,
  onCompleted = () => {},
  isLoading = false,
  disableSaveButton = false,
  isForGrading = null,
  setCountTowardGrade = () => {},
  handleGetDetailsForQuiz = () => {},
  customErrorMessage = null,
  className = "", // if you need hidden scrollbars, add "scroll-hidden" class
}) {
  const [open, setOpen] = useState(false);

  const handleOpen = (e) => {
    handleGetDetailsForQuiz();
    e.currentTarget.blur();
    setOpen(true);
  };

  const handleClose = () => {
    if (isForGrading != null)
      setCountTowardGrade(isForGrading);
    setOpen(false);
  };

  const handleConfirmation = async () => {
    const completed = await onConfirmation();
    if (!completed) return;

    if (isForGrading != null)
      setCountTowardGrade(isForGrading);
    onCompleted();
    setOpen(false); 
  };

  return (
    <>
      <Button
        onClick={handleOpen}
        variant="secondary"
        // disabled={disabled}
        icon={<FontAwesomeIcon icon={faEdit} />}
      >
        <Translate id={editButtonText} />
      </Button>
      <Dialog
        className="overflow-hidden"
        open={open}
        onClose={handleClose}
        maxWidth={maxWidth}
        fullWidth={true}
        style={{ zIndex: 100 }}
        PaperProps={{ className: className }} 
      >
        <div className="contentModal--confirmation">
          <div className="contentModal--close" onClick={handleClose}>
            <div className="wrapper-twoCol-highlight m-0">
              <div className="col pl-0">
                <Heading as="h2">
                  <Translate id={title} />
                </Heading>
              </div>
              <div className="col text-right">
                <FontAwesomeIcon icon={faTimes} />
              </div>
            </div>
          </div>
        </div>
        <DialogContent
          className={`${className}`}
          style={{ maxHeight: "80vh", overflowY: "auto", padding: 0 }}
        >
          <>
            <div className={`Modal--confirmation-body text-left mw-500 ${className}`}>
              {children}
            </div>
            <div className="Modal--confirmation-footer">
                {customErrorMessage && (
              <span className="planning-error-custom-message">
                <FontAwesomeIcon icon={faInfoCircle} />{' '}
                <Translate id={customErrorMessage} />
              </span>)}
              <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
                <Button
                  variant="primary"
                  onClick={async () => await handleConfirmation()}
                  disabled={isLoading || disableSaveButton || disabled}
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
          </>
        </DialogContent>
      </Dialog>
    </>
  );
}
