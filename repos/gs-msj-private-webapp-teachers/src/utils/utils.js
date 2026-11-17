import moment from "moment";

export const isRoleAllowed = (allowRoles, roles) => {
    return allowRoles.some(role => roles.includes(role))
}

export const isRoleDenied = (deniedRoles, roles) => {
    return !deniedRoles.some(role => roles.includes(role))
}

export const removeDuplicates = (arr) => {

    const arrMap = arr.map(elemento => {
        return [JSON.stringify(elemento), elemento]
    });

    return [...new Map(arrMap).values()];
}

export const getTimeZone = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
};

/** To check if tablet **/
export const isTablet = () => {
    const ua = navigator.userAgent || window.opera;
    const isIpadOS13OrLater = ua.includes("Macintosh") && navigator.maxTouchPoints && navigator.maxTouchPoints > 1;
    const isOldIpad = ua.includes("iPad");
    const isGenericTablet = /iPad|Tablet|PlayBook|Silk|Android(?!.*Mobile)/i.test(ua);
    return isGenericTablet || isOldIpad || isIpadOS13OrLater;
};

/** To convert the date from UTC to local time */
export const formatUtcToLocal = (dateString, language = "en") => {
    try {
        if (!dateString) return "";

        const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

        const parsedDate = moment.utc(dateString);

        if (!parsedDate.isValid()) {
            console.warn("Invalid date:", dateString);
            return dateString;
        }

        const localizedDate = parsedDate
            .tz(userTimeZone)
            .locale(language === "es" ? "es" : "en")
            .format(
                language === "es"
                    ? "D [de] MMMM [de] YYYY hh:mm a"
                    : "MMMM D, YYYY hh:mm a"
            );

        return localizedDate;
    } catch (error) {
        console.error(`Error formatting date: ${error.message}`);
        return dateString;
    }
};

export const convertMaxLengthSting = (value, maxLength) => {
    if (value.length > maxLength) {
        value = value.slice(0, maxLength);
    }
    return value;
};


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

export function convertTo24Hour(time) {
    var hours = parseInt(time.substr(0, 2));
    if(time.indexOf('am') !== -1 && hours === 12) {
        time = time.replace('12', '0');
    }
    if(time.indexOf('pm')  !== -1 && hours < 12) {
        time = time.replace(hours, (hours + 12));
    }
    return time.replace(/(am|pm)/, '').trim();
}

export function getFileExtension(filename) {
    return /[.]/.exec(filename) ? /[^.]+$/.exec(filename)[0] : undefined;
}

// Reads a field from an API response regardless of its casing.
export const readApiField = (source, name, fallback) => {
  const camel = name.charAt(0).toLowerCase() + name.slice(1);
  return source?.[camel] ?? source?.[name] ?? fallback;
};
