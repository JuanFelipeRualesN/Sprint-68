import React, { useEffect, useState } from "react";

export default function NativeSelect({
  name,
  placeholder,
  options,
  value = "",
  disabled,
  onChange,
}) {
  const [selected, setSelected] = useState();

  useEffect(() => {
    setSelected(value);
  }, [value]);

  const handleSelected = (event) => {
    setSelected(event.target.value);
    if (onChange) onChange(event);
  };

  return (
    <>
      <select
        name={name}
        onChange={handleSelected}
        value={selected}
        className={selected === "" ? "placeholder" : ""}
        disabled={disabled}
      >
        {placeholder && (
          <option value="" className="placeholder">
            {placeholder}
          </option>
        )}
        {options?.map((option, index) => {
          const { label, value } =
            typeof option === "string"
              ? { label: option, value: option }
              : option;

          return (
            <option key={`option-${index}`} value={value}>
              {label}
            </option>
          );
        })}
      </select>
      <i className="bar"></i>
    </>
  );
}
