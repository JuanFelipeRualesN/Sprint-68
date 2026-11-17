import React, { useState } from 'react';
import { Calendar } from '@mantine/dates';
import { Button } from '@material-ui/core';
import { Calendar as CalendarIcon } from 'lucide-react';
import { Translate } from 'react-localize-redux';
import './style.css';

const CalendarSelector = ({ onDatesSelected }) => {
  const [selectedDates, setSelectedDates] = useState([]);

  const handleSubmit = () => {
    if (selectedDates.length === 0) {
      return;
    }

    const formattedDates = selectedDates
      .map(date => {
        const d = new Date(date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
      })
      .sort();

    const payload = JSON.stringify({ selected_dates: formattedDates });
    
    onDatesSelected(payload);
  };

  return (
    <div className="calendar-selector">
      <div className="calendar-header">
        <CalendarIcon size={20} />
        <span><Translate id="chatbot.select-dates" /></span>
      </div>
      
      <Calendar
        multiple
        value={selectedDates}
        onChange={setSelectedDates}
        minDate={new Date()}
      />
      
      <div className="calendar-footer">
        <span>{selectedDates.length} <Translate id="chatbot.selected-dates" /></span>
        <Button
          variant="contained"
          color="primary"
          onClick={handleSubmit}
          disabled={selectedDates.length === 0}
          style={{
            backgroundColor: '#66BE4E',
            color: '#ffffff'
          }}
        >
          <Translate id="chatbot.confirm-dates" />
        </Button>
      </div>
    </div>
  );
};

export default CalendarSelector;