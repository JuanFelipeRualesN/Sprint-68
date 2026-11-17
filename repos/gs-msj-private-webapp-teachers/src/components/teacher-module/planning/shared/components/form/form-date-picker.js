import React, { useState, useEffect } from "react";
import FormLabel from "./form-label";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendar, faEye } from "@fortawesome/free-solid-svg-icons";
import Button from "../../../shared/components/button";
import DatePicker from 'react-datepicker';
import enUS from 'date-fns/locale/en-US';
import es from 'date-fns/locale/es'; // Add other locales as needed
import LanguageManager from "../../../../../../custom-classes/language-manager";

export default function FormDatePicker({
    label,
    name,
    placeholder,
    value,
    onChange,
    format = "MM/dd/yyyy",
    minDate,
    maxDate,
    disabled,
    showIcon = true,
    filterDate = null,
    toggleModalView,
    showPreviewButton = false,
    labelClassName = "",
}) {
    const localeKey = LanguageManager.getLanguage();
    const locale = localeKey === 'es' ? es : enUS;

    // Normalize any value to a valid Date object
    const normalizeDate = (val) => {
        if (!val) return null;
        const date = new Date(val);
        return isNaN(date) ? null : date;
    };

    const [currentValue, setCurrentValue] = useState(normalizeDate(value));
    const [minDateState, setMinDateState] = useState(normalizeDate(minDate));
    const [maxDateState, setMaxDateState] = useState(normalizeDate(maxDate));

    useEffect(() => {
        setCurrentValue(normalizeDate(value));
        setMinDateState(normalizeDate(minDate));
        setMaxDateState(normalizeDate(maxDate));
    }, [value, minDate, maxDate]);

    const handleOnChange = (date) => {
        setCurrentValue(date);
        if (onChange) onChange(date);
    };

    const handleRawChange = (e) => {
        // Prevent clearing the input manually via keyboard
        if (e.target.value === "") {
            e.preventDefault();
            setCurrentValue(minDateState);
            if (onChange) onChange(minDateState);
        }
    };

    return (
        <div className="form-group-fields">
            {label && <FormLabel label={label} className={`${labelClassName}`} />}
            <div className="date-picker-wrapper" style={{ position: 'relative' }}>
                <DatePicker
                    name={name}
                    selected={currentValue}
                    onChange={handleOnChange}
                    placeholderText={placeholder}
                    onChangeRaw={handleRawChange}
                    dateFormat={format}
                    minDate={minDateState}
                    maxDate={maxDateState}
                    disabled={disabled}
                    locale={locale}
                    className="form-control"
                    customInput={<input readOnly />}
                    filterDate={filterDate}
                />
                {showIcon && (
                    <FontAwesomeIcon icon={faCalendar} style={{
                    position: 'absolute',
                    right: '10px',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    pointerEvents: 'none',
                    color: '#aaa'
                    }} 
                    />
                )}
            </div>
            {showPreviewButton && 
                <Button
                className="pl-2"
                variant="simple"
                icon={<FontAwesomeIcon icon={faEye} />}
                onClick={toggleModalView}
            />}
            <i className="bar"></i>
        </div>
    );
}