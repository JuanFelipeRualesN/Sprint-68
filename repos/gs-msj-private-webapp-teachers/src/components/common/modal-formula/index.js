import React, { useRef, useState } from "react";
import Draggable from 'react-draggable'
import { Paper, IconButton, Dialog, DialogContent, DialogActions, Button } from "@material-ui/core";
import {
  GetApp, Height,
  Close as CloseIcon, ArrowDownward
} from "@material-ui/icons";
import Formula from "./formula";
import Panel from "./panel";
import FormulaPreview from "./formula-preview";
import './modal-formula.css';
import { Translate, withLocalize } from "react-localize-redux";

/**
 * PaperComponent is a functional component that renders a draggable Paper element.
 * @param {Object} props - The properties passed to the component.
 * @returns {JSX.Element} A draggable Paper component.
 */
function PaperComponent(props) {
  const nodeRef = useRef(null)

  return (
    <Draggable nodeRef={nodeRef} handle="#draggable-dialog-title" cancel={'[class*="MuiDialogContent-root"]'}>
      <Paper ref={nodeRef} {...props} />
    </Draggable>
  )
}

/**
 * ModalFormulaView component renders a modal dialog for building and previewing formulas.
 * @param {Object} props - The component props.
 * @param {boolean} props.isOpen - Determines if the modal is open.
 * @param {function} props.onClose - Function to call when the modal is closed.
 * @param {Object} props.fieldRef - Reference to the field where the formula will be inserted.
 * @param {function} props.onChange - Function to call when the formula is changed.
 * @returns {JSX.Element} The rendered ModalFormulaView component.
 */
const ModalFormulaView = ({ isOpen, onClose, fieldRef, onChange }) => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const [insert, setInsert] = useState();
  const [formula, setFormula] = useState();

  const handleClose = () => {
    setFormula('');
    setFormula('');
    onClose();
  };

  return (
    <Dialog
      open={isOpen}
      onClose={handleClose}
      maxWidth="lg"
      fullScreen={isFullScreen}
      PaperComponent={PaperComponent}
      aria-labelledby="draggable-dialog-title"
    >
      <div
        className="Dialog-Title"
        style={{ cursor: "move" }}
        id="draggable-dialog-title"
      >
        <span>
          <Translate id="formulas.formula-builder" />
        </span>
        <div className="Dialog-Title-Actions">
          <IconButton
            size="small"
            disableRipple
            isableelevation="true"
            className="fullscreen"
            onClick={() => {
              setIsFullScreen(!isFullScreen);
            }}
          >
            {isFullScreen ? <ArrowDownward /> : <Height />}
          </IconButton>
          <IconButton
            size="small"
            disableRipple
            isableelevation="true"
            color="secondary"
            onClick={handleClose}
          >
            <CloseIcon />
          </IconButton>
        </div>
      </div>
      <DialogContent>
        <Panel
          onChanges={(value) => {
            setInsert(value);
          }}
        />
        <Formula formula={formula} setFormula={setFormula} />
        <FormulaPreview
          formula={formula}
          insert={insert}
          setFormula={setFormula}
          fieldRef={fieldRef}
        />
      </DialogContent>
      <DialogActions>
        <Button
          size="small"
          disableRipple
          isableelevation="true"
          autoFocus
          onClick={handleClose}
        >
          <Translate id="formulas.cancel"/>
        </Button>
        <Button
          size="small"
          variant="contained"
          disableRipple
          isableelevation="true"
          color="primary"
          startIcon={<GetApp />}
          onClick={() => {
            onChange();
            onClose();
          }}
        >
          <Translate id="formulas.place"/>
        </Button>
      </DialogActions>
    </Dialog>
  );

}

export default withLocalize(ModalFormulaView);
