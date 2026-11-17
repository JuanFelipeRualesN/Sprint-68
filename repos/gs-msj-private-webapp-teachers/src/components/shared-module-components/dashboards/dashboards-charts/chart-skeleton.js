export const MONTH_KEYS = [
  "dashboards.chart-january","dashboards.chart-february","dashboards.chart-march",
  "dashboards.chart-april","dashboards.chart-may","dashboards.chart-june",
  "dashboards.chart-july","dashboards.chart-august","dashboards.chart-september",
  "dashboards.chart-october","dashboards.chart-november","dashboards.chart-december"
];

export const DAY_KEYS = [
  "dashboards.chart-monday","dashboards.chart-tuesday","dashboards.chart-wednesday",
  "dashboards.chart-thursday","dashboards.chart-friday","dashboards.chart-saturday",
  "dashboards.chart-sunday"
];

// handle x-axis tick labels for week and month granularity
export const tickTranslate = (translate) => (value) => {
  return value.startsWith("dashboards.") ? translate(value) : value;
};

// handle tooltip labels for month keys
export const tooltipLabel = (translate) => (value, payload) => {
    const monthKey = payload?.[0]?.payload?.monthKey;

    if (monthKey) {
      return `${value} ${translate(monthKey)}`;
    }

    const isTranslationKey = value.startsWith("dashboards.");

    return isTranslationKey ? translate(value) : value;
};

// builds a complete X-axis data array for a chart, filling gaps with 0 where the backend has no records.
export const buildSkeleton = (granularity, records, selectedPeriod, valueKey, outputKey) => {
  if (granularity === "year") {
    // show one record per month
    const byMonth = {};
    records.forEach(record => { byMonth[record.Month] = record[valueKey] ?? 0; });
    return MONTH_KEYS.map((label, i) => ({ label, [outputKey]: byMonth[i + 1] ?? 0 }));
  }

  if (granularity === "month") {
    const monthRef = selectedPeriod?.dateFrom ? new Date(selectedPeriod.dateFrom + "T00:00:00") : new Date();

    const monthKey = MONTH_KEYS[monthRef.getMonth()];

    // get number of days in the month
    const daysInMonth = new Date(monthRef.getFullYear(), monthRef.getMonth() + 1, 0).getDate();

    // show one record per calendar day
    return Array.from({ length: daysInMonth }, (_, i) => {
      const day = i + 1;
      const match = records.find(record => record.Date && new Date(record.Date + "T00:00:00").getDate() === day);
      const value = match ? (match[valueKey] ?? 0) : 0;

      return { 
        label: String(day),
        monthKey,
        [outputKey]: value 
      };
    });
  }

  // show one record per day Mon–Sun (use week granularity by default)
  const toMondayIndex = (day) => {
      return day === 0 ? 6 : day - 1;
  };

  const byWeekday = {};

  records.forEach((record) => {
      if (!record.Date) return;

      const date = new Date(`${record.Date}T00:00:00`);
      const weekday = date.getDay();
      const index = toMondayIndex(weekday);

      byWeekday[index] = record;
  });

  return DAY_KEYS.map((label, index) => {
      const record = byWeekday[index];

      return {
        label,
        [outputKey]: record ? record[valueKey] : 0,
      };
  });
};
