/**
 * Created by portiz on 4/11/19.
 */
class DateFormater {

    /**
     * Formats the day to 'YYYY-mm-DD'
     * @param The default currency code
     * @returns {*}
     */
    static formatDate(date)
    {
        let d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }
}

export default DateFormater;