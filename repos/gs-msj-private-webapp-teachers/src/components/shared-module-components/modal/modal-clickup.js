import React from "react";
import { Translate } from "react-localize-redux";
import { Modal, ModalHeader, ModalBody } from "reactstrap";

const HelpDialog = ({ onClose, open }) => {
  const handleClose = () => onClose();

  return (
      <Modal isOpen={open} toggle={handleClose} className="modal-clickupDialog">
        <ModalHeader toggle={handleClose}></ModalHeader>
        <ModalBody>
            <Translate
                id="navbar.help-dialog"
                options={{ renderInnerHtml: true }}
            />
        </ModalBody>
      </Modal>
  );
};

export default HelpDialog;