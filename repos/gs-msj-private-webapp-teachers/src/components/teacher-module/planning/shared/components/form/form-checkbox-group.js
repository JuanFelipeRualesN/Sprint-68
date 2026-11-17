import React, { useState } from "react";
import FormCheckbox from "./form-checkbox";

export default function FormCheckboxGroup({ name, options, onChange, values }) {
  const [selectedItems, setSelectedItems] = useState(values || []);

  const handleCheckboxChange = (value) => {
    const index = selectedItems.findIndex((s) => s === value);
    const newState =
      index < 0
        ? [...selectedItems, value]
        : selectedItems.filter((item) => item !== value);

    setSelectedItems(newState);
    onChange(newState);
  };

  const isChecked = (value) => {
    return selectedItems.some((item) => item === value);
  };

  return (
    <div className="checkbox-container">
      {options?.map((option) => {
        return (
          <FormCheckbox
            key={`${name}-check-${option.value}`}
            name={option.value}
            label={option.label}
            checked={isChecked(option.value)}
            onChange={(value) => handleCheckboxChange(option.value)}
          />
        );
      })}
    </div>
  );
}
