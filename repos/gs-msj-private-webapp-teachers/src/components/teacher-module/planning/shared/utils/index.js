import LanguageManager from "../../../../../custom-classes/language-manager";
import Settings from "../../../../../custom-classes/settings";
import moment from "moment";

export const translate = (key) => {
  return LanguageManager.getTranslation(key, Settings.LANGUAGES);
};

export function getMinAndMaxDates(dates) {
  if (dates.length === 0) return { minDate: null, maxDate: null };

  const minDate = new Date(
    Math.min(...dates.map((date) => new Date(date).getTime()))
  );
  const maxDate = new Date(
    Math.max(...dates.map((date) => new Date(date).getTime()))
  );

  return { minDate, maxDate };
}

export function format(date, format = "MM/DD/YYYY") {
  const langFormat = LanguageManager.getLanguage();
  return date === null ? "" : moment(date).locale(langFormat).format(format);
}

export function formatWithDayName(date) {
  return format(date, "ddd: MM/DD/YYYY");
}

export function addDaysToDate(date, days) {
  const result = new Date(date);
  result.setDate(result.getDate() + days);

  return result;
}

export function compareLocalAndUTC() {
  const localDate = new Date();
  const utcDate = new Date(localDate.toISOString());

  const localDateOnly = new Date(
    localDate.getFullYear(),
    localDate.getMonth(),
    localDate.getDate()
  );
  const utcDateOnly = new Date(
    utcDate.getUTCFullYear(),
    utcDate.getUTCMonth(),
    utcDate.getUTCDate()
  );

  if (localDateOnly.getTime() === utcDateOnly.getTime()) {
    return 1;
  } else if (utcDateOnly.getTime() > localDateOnly.getTime()) {
    return 2;
  } else {
    return 0;
  }
}

export function getDatesBetween(startDate, endDate) {
  const dates = [];
  const currentDate = new Date(startDate);

  // Normalize time to 00:00:00
  currentDate.setHours(0, 0, 0, 0);
  const finalDate = new Date(endDate);
  finalDate.setHours(0, 0, 0, 0);

  while (currentDate <= finalDate) {
    dates.push(new Date(currentDate));
    currentDate.setDate(currentDate.getDate() + 1);
  }

  return dates;
}
