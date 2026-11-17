import React from 'react';
import { withLocalize } from 'react-localize-redux';
import LocalizedDatePicker from '../../../../utils/datepicker-locale';

const toISO = (date) =>
  `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;

// Get monday–sunday range for the week
export const getWeekRange  = (date) => {
  const d = new Date(date);
  const day = d.getDay() || 7;

  const monday = new Date(d);
  monday.setDate(d.getDate() - (day - 1));

  const sunday = new Date(monday);
  sunday.setDate(monday.getDate() + 6);

  return { dateFrom: toISO(monday), dateTo: toISO(sunday) };
};

// Get first and last day of the given month
const getMonthRange = (yearMonth) => {
  const [year, month] = yearMonth.split('-').map(Number);
  return {
    dateFrom: `${year}-${String(month).padStart(2, '0')}-01`,
    dateTo: toISO(new Date(year, month, 0)),
  };
};

// Get Jan 1 – Dec 31 range for the given year
const getYearRange  = (year) => ({
  dateFrom: `${year}-01-01`,
  dateTo: `${year}-12-31`,
});

const tabs = [
  { key: 'week', labelKey: 'dashboards.period-week'  },
  { key: 'month', labelKey: 'dashboards.period-month' },
  { key: 'year', labelKey: 'dashboards.period-year'  },
];

const PeriodSelector = ({ translate, onChange, granularity = 'week', weekDate, monthValue, yearValue }) => {

  const triggerDateChange = (period, dateFrom, dateTo) => {
    if (onChange) onChange({ period, dateFrom, dateTo });
  };

  // Change the date range when the user switches between week/month/year tabs
  const handleTabChange = (tab) => {
    if (tab === 'week')  {
      const { dateFrom, dateTo } = getWeekRange(weekDate);
      triggerDateChange('week',  dateFrom, dateTo); 
    }

    if (tab === 'month') {
      const { dateFrom, dateTo } = getMonthRange(monthValue); 
      triggerDateChange('month', dateFrom, dateTo);
    }

    if (tab === 'year')  { 
      const { dateFrom, dateTo } = getYearRange(yearValue);
      triggerDateChange('year',  dateFrom, dateTo); 
    }
  };

  const handleWeekChange  = (date) => {
    if (!date) return;
    
    const { dateFrom, dateTo } = getWeekRange(date);
    triggerDateChange('week', dateFrom, dateTo);
  };

  const handleMonthChange = (e) => {
    const { dateFrom, dateTo } = getMonthRange(e.target.value);
    triggerDateChange('month', dateFrom, dateTo);
  };

  const handleYearChange = (e) => {
    const { dateFrom, dateTo } = getYearRange(e.target.value);
    triggerDateChange('year', dateFrom, dateTo);
  };

  // Generate last 5 years for the year selector dropdown
  const currentYear = new Date().getFullYear();
  const yearOptions = Array.from({ length: 6 }, (_, i) => currentYear - i);

  return (
    <div className="period-container">
      <div className="period-selector-tabs">
        {tabs.map(({ key, labelKey }) => (
          <button
            key={key}
            onClick={() => handleTabChange(key)}
            className={`period-tab${granularity === key ? ' active' : ''}`}
          >
            {translate(labelKey)}
          </button>
        ))}
      </div>

      {granularity === 'week' && (
        <LocalizedDatePicker
          selected={weekDate}
          onChange={handleWeekChange}
          showWeekNumbers
          dateFormat={`'${translate("dashboards.period-week")}' w (dd/MM/yyyy)`}
          className="form-control form-control-sm"
        />
      )}

      {granularity === 'month' && (
        <input type="month" value={monthValue} onChange={handleMonthChange} className="period-input" />
      )}

      {granularity === 'year' && (
        <select value={yearValue} onChange={handleYearChange} className="period-input">
          {yearOptions.map(year => (
            <option key={year} value={year}>{year}</option>
          ))}
        </select>
      )}
    </div>
  );
};

export default withLocalize(PeriodSelector);
