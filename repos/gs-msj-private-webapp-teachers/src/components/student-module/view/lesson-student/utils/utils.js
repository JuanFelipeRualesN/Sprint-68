import moment from "moment";

export const isAuthenticated = () => {
  return localStorage.getItem("auth") !== null;
};

export const getUser = () => {
  return JSON.parse(localStorage.getItem("auth"));
};

export const getLanguage = () => {
  return localStorage.getItem("i18nextLng");
};

export const getSuject = () => {
  return JSON.parse(localStorage.getItem("suject"));
};

export const isMST = () => {
  if (window.name === "embedded-page-container" || window.name === "extension-tab-frame") {
    return true;
  }
  return false;
};

export const hasRole = (user, roles) => {
   return roles.some(role => user.Roles.includes(role));
}

export const getTimeZone = () => {
  return Intl.DateTimeFormat().resolvedOptions().timeZone;
};

export const convertMaxLengthString = (value, maxLength) => {
  if (value.length > maxLength) {
      value = value.slice(0, maxLength);
  }
  return value;
};

export const isBefore = (date) => {
  const today = moment.utc().tz(getTimeZone());

  return moment(date, "MM/DD/YYYY HH:mm:ss").utc().tz(getTimeZone()).isBefore(today);

}

export const getDate = (startDate, endDate) => {
  const start = new Date(startDate)
  const end = new Date(endDate)

  const dayOfWeek = start.toLocaleDateString('es-ES', {weekday: 'long'})
  const day = start.getDate()
  const monthName = start.toLocaleString('default', {month: 'long'})

  let startHour = start.getHours()
  const startMinutes = start.getMinutes()

  let endHour = end.getHours()
  const endMinutes = end.getMinutes()

  const startPeriod = startHour >= 12 ? 'PM' : 'AM';
  const endPeriod = endHour >= 12 ? 'PM' : 'AM';

  startHour = startHour % 12;
  startHour = startHour ? startHour : 12; // the hour '0' should be '12'

  endHour = endHour % 12;
  endHour = endHour ? endHour : 12; // the hour '0' should be '12'

  const startHourFormatted = startHour < 10 ? `0${startHour}` : startHour
  const startMinutesFormatted = startMinutes < 10 ? `0${startMinutes}` : startMinutes

  const endHourFormatted = endHour < 10 ? `0${endHour}` : endHour
  const endMinutesFormatted = endMinutes < 10 ? `0${endMinutes}` : endMinutes

  return `${dayOfWeek}, ${monthName} ${day}, ${startHourFormatted}:${startMinutesFormatted} ${startPeriod} - ${endHourFormatted}:${endMinutesFormatted} ${endPeriod}`

}

export const bytesToSize = (bytes) => {
  let sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
  if (bytes == 0) return '0 Byte';
  let i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
  return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
};

export const alphabet = {
  0: "a",
  1: "b",
  2: "c",
  3: "d",
  4: "e",
  5: "f",
  6: "g",
  7: "h",
  8: "i",
  9: "j",
  10: "k",
  11: "l",
  12: "m",
  13: "n",
  14: "o",
  15: "p",
  16: "q",
  17: "r",
  18: "s",
  19: "t",
  20: "u",
  21: "v",
  22: "w",
  23: "x",
  24: "y",
  25: "z",
};



