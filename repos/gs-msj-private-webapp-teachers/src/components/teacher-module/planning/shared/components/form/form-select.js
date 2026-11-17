import React from "react";
import FormLabel from "./form-label";
import Select from "../select";
import { faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "../../../shared/components/button";



export default function FormSelect({
  label,
  name,
  placeholder,
  options,
  onChange,
  className = "",
  disabled,
  searchable,
  value,
  clearable,
  toggleModalView,
  showPreviewButton = false,
  disabledPreviewIcon,
}) {
  const handleOnChangeEvent = (event) => {
    if (!onChange) return;
    onChange(event);
  };

  return (
    <div className={`form-group-fields ${className}`}>
      <Select
        name={name}
        placeholder={placeholder}
        options={options}
        value={value}
        onChange={handleOnChangeEvent}
        disabled={disabled}
        searchable={searchable}
        clearable={clearable}
        classNamePrefix="mantine-like"
        className="mantine-like__control"
      />
      <FormLabel forHtml="select" label={label} />
      {showPreviewButton && 
       <Button
        className="pl-2"
        variant="simple"
        icon={<FontAwesomeIcon icon={faEye} />}
        onClick={toggleModalView}
        disabled={disabledPreviewIcon}
      />}
    </div>
  );
}
