import React from "react";
import FormLabel from "./form-label";

export default function FormTextarea({
  label,
  name,
  placeholder,
  rows = 4,
  onChange,
  value,
}) {
  return (
    <div className="form-group-fields">
      <textarea
        rows={rows}
        name={name}
        placeholder={placeholder}
        onChange={onChange}
        defaultValue={value}
        maxLength={250}
      />
      <i className="bar"></i>
      <FormLabel label={label} />
    </div>
  );
}
