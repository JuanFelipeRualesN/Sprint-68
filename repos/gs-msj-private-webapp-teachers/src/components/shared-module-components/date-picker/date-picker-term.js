import React, { useState, useEffect } from "react";
import DatePicker from "./date-picker-recent"

const SelectorTermPicker = ({ id, onChange, date, minDate, disabled }) => {
    const [selectedDate, setSelectedDate] = useState("");
    const handleOnChangeDatePicker = (value) => {
        onChange(value, id) //sent to parent the TermId and the Date
        setSelectedDate(value)
    }
    return (<DatePicker date={date} minDate={minDate} disabled={disabled} updateParent={handleOnChangeDatePicker} />)
}
export default SelectorTermPicker;