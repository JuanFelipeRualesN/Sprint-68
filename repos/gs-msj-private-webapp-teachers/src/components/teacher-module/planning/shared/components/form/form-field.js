import React from "react";
import FormLabel from "./form-label";
import FormInput from './form-input';

export default function FormField({
  label,
  name,
  placeholder,
  value,
  disabled,
  onChange,
  onEnter = () => {},
  className = "",
    maxLength = 500,
}) {
  return (
    <div className={`form-group-fields ${className}`}>
      <FormInput
        name={name}
        placeholder={placeholder}
        onChange={onChange}
        onEnter={onEnter}
        value={value}
        disabled={disabled}
        maxLength={maxLength}
      />
      <FormLabel label={label} />
    </div>
  );
}
