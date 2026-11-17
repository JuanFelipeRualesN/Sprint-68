import React, { useState } from "react";
import { Select as SelectMantine } from "@mantine/core";

export default function Select({
  name,
  placeholder,
  options,
  value,
  onChange,
  disabled,
  searchable,
    clearable
}) {

  const handleChange = (selectedValue) => {
    if (!onChange) return;
    onChange(selectedValue);
  };

  return (
    <>
      <SelectMantine
        classNames={{
          label: "control-label-w-placeholder",
        }}
        name={name}
        placeholder={placeholder}
        data={options || []}
        onChange={handleChange}
        value={value}
        defaultValue={value}
        searchable={searchable}
        disabled={disabled}
        clearable={clearable}
        zIndex={100000}
        styles={{
          dropdown: { backgroundColor: "#eeefef" },
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
    </>
  );
}
