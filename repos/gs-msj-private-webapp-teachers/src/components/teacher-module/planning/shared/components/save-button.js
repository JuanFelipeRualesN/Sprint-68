import React, { useEffect, useState } from "react";
import { Dialog } from "@material-ui/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Translate } from "react-localize-redux";
import { faCheckCircle, faTimes } from "@fortawesome/free-solid-svg-icons";
import Button from "./button";
import Loading from "./loading";
import LanguageManager from "../../../../../custom-classes/language-manager";
import Settings from "../../../../../custom-classes/settings";

export default function SaveButton({
  onClick,
  onConfirmation = () => {},
  className,
  disabled = false,
  isLoading = false,
  label,
}) {
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [isDisabled, setIsDisabled] = useState(false);

  const handleClose = () => {
    setOpen(false);
    onConfirmation();
  };

  const handleOpen = async () => {
    const completed = await onClick();
    if (completed) setOpen(true);
  };

  useEffect(() => {
    setLoading(isLoading);
  }, [isLoading]);

  useEffect(() => {
    setIsDisabled(disabled);
  }, [disabled]);

  const getButtonLabel = () => {
    return (
      label ||
      LanguageManager.getTranslation("confirm-dialog.save", Settings.LANGUAGES)
    );
  };

  return (
    <>
      <Button
        className={className}
        variant="primary"
        onClick={handleOpen}
        disabled={isDisabled || loading}
      >
        {loading ? (
          <Loading show={loading} type="button" text={getButtonLabel()} />
        ) : (
          getButtonLabel()
        )}
      </Button>
      <Dialog className="overflow-hidden" open={open} onClose={handleClose}>
        <div className="contentModal--confirmation mw-500">
          <div className="contentModal--close" onClick={handleClose}>
            <FontAwesomeIcon icon={faTimes} />
          </div>
          <div className="region-modalIcon modalIcon__success">
            <FontAwesomeIcon icon={faCheckCircle} />
          </div>
          <div className="Modal--confirmation-body">
            <p className="Modal--confirmation__title">
              <Translate id="confirm-dialog.success-title" />
            </p>
          </div>
          <div className="Modal--confirmation-footer">
            <div className="Modal--confirmation-footer__colRight w-100">
              <Button variant="primary" onClick={handleClose}>
                <Translate id="confirm-dialog.button-ok" />
              </Button>
            </div>
          </div>
        </div>
      </Dialog>
    </>
  );
}
