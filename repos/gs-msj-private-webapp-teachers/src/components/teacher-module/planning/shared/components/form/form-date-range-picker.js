import React, { useEffect, useState } from "react";
import { DateRangePicker } from "@mantine/dates";
import FormLabel from "./form-label";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendar } from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../../../../custom-classes/language-manager";

export default function FormDateRangePicker({
  label,
  name,
  placeholder,
  value,
  onChange,
  format = "MM/DD/YYYY",
  minDate,
  maxDate,
  disabled,
}) {
  const [currentValue, setCurrentValue] = useState();
  const locale = LanguageManager.getLanguage();

  useEffect(() => {
    setCurrentValue(value);
  }, [value]);

  const handleOnChange = (values) => {
    setCurrentValue(values);
    if (onChange) {
      onChange(values);
    }
  };

  return (
    <div className="form-group-fields">
      {label && <FormLabel label={label} />}
      <DateRangePicker
        name={name}
        value={currentValue}
        placeholder={placeholder}
        onChange={handleOnChange}
        firstDayOfWeek="sunday"
        locale={locale}
        inputFormat={format}
        rightSection={<FontAwesomeIcon icon={faCalendar} />}
        minDate={minDate}
        maxDate={maxDate}
        disabled={disabled}
      />
      <i className="bar"></i>
    </div>
  );
}
