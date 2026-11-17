import React, { useEffect, useState } from "react";

export default function FormCheckbox({
  label,
  name,
  value,
  onChange = () => {},
  checked,
  disabled,
  className = "",
}) {
  const [isChecked, setIsChecked] = useState(checked ?? false);

  const handleCheckboxChange = (event) => {
    setIsChecked(!isChecked);
    onChange(!isChecked);
  };

  useEffect(() => {
    setIsChecked(checked || false);
  }, [checked]);

  return (
    <div className="form-group-fields__checkbox">
      <label>
        <input
          className={className}
          name={name}
          type="checkbox"
          onChange={handleCheckboxChange}
          disabled={disabled}
          checked={isChecked}
          value={value}
        />
        <i className="helper"></i>
        {label}
      </label>
    </div>
  );
}
