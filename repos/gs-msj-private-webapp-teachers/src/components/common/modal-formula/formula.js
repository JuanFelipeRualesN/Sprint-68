import React from "react";
import { useRef } from "react";
import { IconButton } from "@material-ui/core";
import Backspace from "@material-ui/icons/Backspace";
import "./formula.css";
import { Translate, withLocalize } from "react-localize-redux";

/**
 * Component for displaying and editing a formula with a clear button.
 * @param {Object} props - The component props.
 * @param {string} props.formula - The current formula value.
 * @param {Function} props.setFormula - Function to update the formula value.
 * @returns {JSX.Element} The rendered component.
 */
const Formula = ({ formula, setFormula }) => {
  const showClear = true;
  const inputRef = useRef();

  /**
   * Clears the formula input field and sets focus on the input element.
   */
  const clearFormula = () => {
    setFormula("");
    inputRef.current.focus();
  };

  return (
    <div className="Formula">
      <label style={{ width: '90px' }}>
        <Translate id="formulas.raw" />
      </label>
      <input
        ref={inputRef}
        type="text"
        spellCheck="false"
        autoComplete="off"
        value={formula}
        onChange={(event) => {
          setFormula(event.target.value);
        }}
      />
      {showClear && (
        <IconButton
          size="small"
          disableRipple
          isableelevation="true"
          onClick={clearFormula}
        >
          <Backspace />
        </IconButton>
      )}
    </div>
  );
}

export default withLocalize(Formula);
