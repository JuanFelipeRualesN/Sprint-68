import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import FormLabel from "./form-label";
import LanguageManager from "../../../../../../custom-classes/language-manager";
import { registerLocale } from "react-datepicker";

// If needed: import locale dynamically based on your language manager
import enUS from "date-fns/locale/en-US";
import es from "date-fns/locale/es"; // example for Spanish
// Add more locales as needed

// Register locales
registerLocale("en", enUS);
registerLocale("es", es);

export default function FormMultiDatePicker({
  label,
  minDate,
  maxDate,
  defaultDate,
  values = [],
  onChange = () => {},

                                              filterDate = null,
                                              isCatalog = false,
  labelClassName = ""
}) {
  const [selectedDates, setSelectedDates] = useState(values || []);

  const appLocale = LanguageManager.getLanguage(); // e.g., "en", "es"
  const locale = ["en", "es"].includes(appLocale) ? appLocale : "en";

  const handleDateClick = (date) => {
    const exists = selectedDates.some(
        (d) => d.toDateString() === date.toDateString()
    );

    const updatedDates = exists
        ? selectedDates.filter((d) => d.toDateString() !== date.toDateString())
        : [...selectedDates, date];

    setSelectedDates(updatedDates);
    onChange(updatedDates);
  };

  const isSelected = (date) =>
      selectedDates.some((d) => d.toDateString() === date.toDateString());

  const highlightWithRanges = [
    {
      "react-datepicker__day--highlighted-custom": selectedDates,
    },
  ];

  return (
      <div className="form-group-fields">

        <FormLabel label={label}  className={`${labelClassName}`}/>
        <div className={isCatalog ? "datepicker-padding" : ""}>        
          <DatePicker
            inline
            locale={locale}
            minDate={minDate}
            maxDate={maxDate}
            onChange={handleDateClick}
            selected={null} // Prevent single selection UI
            highlightDates={highlightWithRanges}
            dayClassName={(date) =>
                isSelected(date) ? "react-datepicker__day--selected" : undefined
            }
            filterDate={filterDate}
          />
        </div>
      </div>
  );
}
