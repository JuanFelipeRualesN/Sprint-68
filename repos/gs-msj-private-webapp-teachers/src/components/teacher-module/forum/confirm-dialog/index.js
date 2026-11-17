import React, { useState} from "react";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import "./style.css";
import {Translate} from "react-localize-redux";

const ConfirmDialog = ({ onClose, open, handleDeleteItem }) => {
    const [fullWidth] = useState(true);
    const [maxWidth] = useState("sm");

    const handleClose = () => {
        onClose();
    };

    return (
        <>
            <Dialog
                className="modal-delete"
                open={open}
                onClose={handleClose}
                fullWidth={fullWidth}
                maxWidth={maxWidth}
            >
                <DialogTitle><Translate id="confirm-dialog.title-confirm"
                                        options={{renderInnerHtml: true}}/></DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        <i className="fas fa-exclamation-circle"></i>
                        <Translate id="confirm-dialog.description"
                                   options={{renderInnerHtml: true}}/>
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button
                        className="button-secondary"
                        onClick={handleClose}
                        color="primary"
                    >
                        <Translate id="confirm-dialog.button-cancel"
                                   options={{renderInnerHtml: true}}/>
                    </Button>
                    <Button
                        className="button-primary"
                        onClick={handleDeleteItem}
                        color="primary"
                        autoFocus
                    >
                        <Translate id="confirm-dialog.button-accept"
                                   options={{renderInnerHtml: true}}/>
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
};

export default ConfirmDialog;
