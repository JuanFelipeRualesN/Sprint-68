import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import { Dialog } from "@material-ui/core";
import PlanningsForm from "./plannings-form";
import useModifyPlanning from "./shared/hooks/useModifyPlanning";
import Button from "./shared/components/button";
import Heading from "./shared/components/heading";
import { Translate } from "react-localize-redux";
import {faExclamationCircle, faPenSquare, faTimes} from "@fortawesome/free-solid-svg-icons";
import SessionManager from "../../../custom-classes/session-manager";
import Loading from "./shared/components/loading";
import {Alert as AlertBootstrap} from "reactstrap";

export default function PlanningsEdit({ planning, onCompleted, disabled }) {
  const [open, setOpen] = useState(false);
  const [modifyPlanningModel, setModifyPlanningModel] = useState();
  const { isLoading, succeeded, modifyPlanning } = useModifyPlanning();
  const [isButtonSaveDisabled, setButtonSaveDisabled] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleConfirmation = async () => {
    if (modifyPlanningModel) {
      const { HasInvalidDates, ...planningPayload } = modifyPlanningModel;
      await modifyPlanning(planningPayload);
    }
  };

  const evaluateIfDisabled = (data) => {
    // If close date is before open date, set disabled to true
    const openDate = new Date(data.OpenDate);
    const closeDate = new Date(data.CloseDate);
    if (openDate > closeDate) {
      setButtonSaveDisabled(true);
    }
    else{
        setButtonSaveDisabled(false);
    }
  }

  const updatePlanningModel = (data) => {
    evaluateIfDisabled(data);
    setModifyPlanningModel({ ...data, Id: planning.id });
  }

  useEffect(() => {
    if (succeeded) {
      onCompleted();
      handleClose();
      const { Id, Name, OpenDate, CloseDate } = modifyPlanningModel;
      SessionManager.setSelectedPlanning(Id, Name, false, OpenDate, CloseDate);
    }
  }, [succeeded]);

  return (
    <>
      <Button
        variant="simple"
        icon={<FontAwesomeIcon icon={faPenSquare} />}
        onClick={handleOpen}
        disabled={disabled}
      >
        <Translate id="planning.edit" />
      </Button>
      <Dialog
        className="overflow-hidden"
        open={open}
        onClose={handleClose}
        fullWidth={true}
      >
        <div className="contentModal--confirmation">
          <div className="contentModal--close" onClick={handleClose}>
            <div className="wrapper-twoCol-highlight m-0">
              <div className="col pl-0">
                <Heading as="h2">
                  <Translate id="planning.edit" />
                </Heading>
              </div>
              <div className="col text-right">
                <FontAwesomeIcon icon={faTimes} />
              </div>
            </div>
          </div>
          <div className="Modal--confirmation-body text-left mw-500 px-0">
            <AlertBootstrap
                color="danger"
                isOpen={disabled}
                className="alert-info"
            >
              <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />
              {' '}
              <span>
                 <Translate id="planning.no-active-periods-no-edit" />
              </span>
            </AlertBootstrap>

            <PlanningsForm
              planning={planning}
              onChange={(data) =>
                  updatePlanningModel({ ...data, Id: planning.id })
              }
              isLoading={isLoading}
              isForEdit={true}
            />
          </div>
          <div className="Modal--confirmation-footer pr-0">
            <div className="Modal--confirmation-footer__colRight w-100 d-flex justify-content-end">
              <Button
                variant={disabled || isButtonSaveDisabled ? "secondary": "primary"}
                onClick={async () => await handleConfirmation()}
                disabled={isLoading || isButtonSaveDisabled || disabled || modifyPlanningModel?.Name?.trim() === "" || modifyPlanningModel?.OpenDate === "" || modifyPlanningModel?.CloseDate === ""}
              >
                {isLoading ? (
                  <Loading
                    show={isLoading}
                    type="button"
                    text={<Translate id="confirm-dialog.save" />}
                  />
                ) : (
                  <Translate id="confirm-dialog.save" />
                )}
              </Button>
              <Button
                variant="regular"
                className="ml-2"
                onClick={handleClose}
                disabled={isLoading}
              >
                <Translate id="confirm-dialog.button-cancel" />
              </Button>
            </div>
          </div>
        </div>
      </Dialog>
    </>
  );
}
