import React, { useState } from "react";

export default function RadioButtom({
  label,
  name,
  options,
  value,
  onChange = (value) => {},
}) {
  const [state, setState] = useState(value);

  const handleOptionChange = (changeEvent) => {
    setState(changeEvent.target.value);
    onChange(changeEvent.target.value);
  };

  return (
    <div className="contentForms-inline form-group-fields__radio space-margin-top-sm">
      {label && <div className="titDesc">{label}</div>}
      {options.map((option, index) => (
        <div
          key={`group-${name}-${index}`}
          className="form-group-fields__radio--radio"
        >
          <label>
            <input
              key={`radio-${name}-${index}`}
              type="radio"
              name={name}
              checked={option.value === state}
              value={option.value}
              onChange={handleOptionChange}
            />
            <i className="helper"></i>
            {option.label}
          </label>
        </div>
      ))}
    </div>
  );
}
