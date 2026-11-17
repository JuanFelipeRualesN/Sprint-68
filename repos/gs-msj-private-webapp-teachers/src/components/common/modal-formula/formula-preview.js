import React, { useEffect } from "react";
import "./formula-preview.css";
import "mathlive";
import { withLocalize } from "react-localize-redux";

/**
 * Component to preview and edit a mathematical formula.
 * @param {Object} props - The properties object.
 * @param {string} props.formula - The initial formula to display.
 * @param {string} props.insert - The string to insert into the formula.
 * @param {Function} props.setFormula - Function to update the formula state.
 * @param {Object} props.fieldRef - Reference to the math field element.
 * @returns {JSX.Element} The rendered component.
 */
const FormulaPreview = ({ formula, insert, setFormula, fieldRef }) => {

  useEffect(() => {
    const mathField = fieldRef.current;

    mathField.popoverPolicy = "off";
    mathField.addEventListener("change", (event) =>
      setFormula(event.target.value)
    );
  }, []);

  useEffect(() => {
    if (!insert) return;

    fieldRef.current.insert(insert, {
      silenceNotifications: false
    });
  }, [insert]);



  return (
    <div className="formula-container">
      <math-field id="formula-preview" ref={fieldRef}>
        {formula}
      </math-field>
    </div>
  );
}

export default withLocalize(FormulaPreview);