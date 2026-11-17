import React from "react";
import { MultiSelect } from "@mantine/core";
import FormLabel from "./form-label";

export default function FormMultiSelect({
  label,
  name,
  placeholder,
  options,
  values,
  disabled,
  onChange,
  className = "",
}) {
  const handleChange = (values) => {
    if (!onChange) return;
    onChange(values);
  };

  return (
    <div className={`form-group-fields ${className}`}>
      <MultiSelect
        withinPortal={false}
        classNames={{
          label: "control-label-w-placeholder",
        }}
        name={name}
        placeholder={placeholder}
        data={options}
        onChange={handleChange}
        value={values?.map((item) => item.value)}
        disabled={disabled}
        searchable
        styles={{
          dropdown: { zIndex: 90000, backgroundColor: "#eeefef" },
          item: {
            fontFamily: "'Montserrat', Arial, Helvetica, sans-serif",
            paddingTop: "2px",
            paddingBottom: "2px",
            fontSize: "var(--fontSize-xsm) !important",
          },
          selected: {
            backgroundColor: "#eeefef",
            color: "black",
            fontWeight: "600",
          },
        }}
      />
      <i className="bar"></i>
      <FormLabel forHtml="select" label={label} />
    </div>
  );
}
