/**
 * Created by portiz on 3/12/19.
 */
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import { SingleDatePicker } from 'react-dates';
import moment from 'moment';
import LanguageManager from '../../../custom-classes/language-manager';
import 'moment/locale/es';

/**
 * Manages the date picker. It's the same date picker that AirBNB uses.
 * It's the most used and diverse date picker for react
 * However, is not so friendly as other classes
 */
const DatePicker = ({ date, name, disabled, updateParent, minDate }) => {
    let momentDate = moment();

    const appLocale = LanguageManager.getLanguage(); 
    const locale = ["en", "es"].includes(appLocale) ? appLocale : "en";
    moment.locale(locale);

    const [calendarFocused, setCalendarFocused] = useState(false);
    const [createdAt, setCreatedAt] = useState(date ?? momentDate);
    const [dateState, setDateState] = useState(null);

    useEffect(() => {
        if (typeof date !== 'undefined' && date !== null) {
            let momentDate = moment();
            let convertedDate = moment(date, "YYYY-MM-DD HH:mm:ss");
            setCreatedAt(typeof date === 'undefined' || date === null ? momentDate : convertedDate);
        }
    }, [date]);

    // Action when the date change
    const onDateChange = (createdAtParam) => {
        setCreatedAt(createdAtParam);
        
        try {
            let dateTime = new Date(createdAtParam.toDate());
            dateTime = moment(dateTime).format("YYYY-MM-DD HH:mm:ss");
            setDateState(dateTime);
            updateParent({
                calendarFocused,
                createdAt: createdAtParam,
                date: dateTime,
                name,
                disabled
            });
        } catch (err) {
            setDateState(null);
            updateParent({
                calendarFocused,
                createdAt: createdAtParam,
                date: null,
                name,
                disabled
            });
        }
    };

    // Action when the focus change
    const onFocusChange = ({ focused }) => {
        setCalendarFocused(focused);
    };

    const returnYears = () => {
        let years = [];
        for (let i = moment().year() - 100; i <= moment().year(); i++) {
            years.push(<option value={i}>{i}</option>);
        }
        for (let i = moment().year() + 1; i <= moment().year() + 10; i++) {
            years.push(<option value={i}>{i}</option>);
        }
        return years;
    };

    const renderMonthElement = ({ month, onMonthSelect, onYearSelect }) =>
        <div style={{ display: 'flex', justifyContent: 'center' }}>
            <div>
                <select
                    value={month.month()}
                    onChange={(e) => onMonthSelect(month, e.currentTarget.value)}
                >
                    {moment.months().map((label, value) => (
                        <option value={value}>{label}</option>
                    ))}
                </select>
            </div>
            <div>
                <select value={month.year()} onChange={(e) => onYearSelect(month, e.currentTarget.value)}>
                    {returnYears()}
                </select>
            </div>
        </div>;

    const handleOutsideRange = (day) => {
        return minDate ? moment(day).isBefore(minDate) : false;
    };

    return (
        <div>
            <SingleDatePicker
                displayFormat="MM/DD/YYYY"
                readOnly={true}
                date={createdAt}
                onDateChange={onDateChange}
                focused={calendarFocused}
                onFocusChange={onFocusChange}
                renderMonthElement={renderMonthElement}
                numberOfMonths={1}                    
                name={name}
                disabled={disabled}
                isOutsideRange={(day) => handleOutsideRange(day.format("YYYY-MM-DD"))}
            />
        </div>
    );
};

export default DatePicker;