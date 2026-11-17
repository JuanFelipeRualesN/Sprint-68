// ----- Academic period helpers -----------------------------------------

// "2024-03-21T00:00:00" has no timezone → parsed as local time.
export const parseApiDate = (value) => (value ? new Date(value) : null);

export const MS_PER_WEEK = 7 * 24 * 60 * 60 * 1000;

export const keyOf = (p) =>
    [p.PeriodNumber, p.PeriodFrom, p.PeriodTo, p.FullTermStartDate, p.FullTermEndDate].join('|');

// The API returns the same period several times → collapse duplicates.
export const dedupePeriods = (periods = []) => {
    const seen = new Map();
    periods.forEach((p) => {
        if (!seen.has(keyOf(p))) seen.set(keyOf(p), p);
    });
    return [...seen.values()];
};

// Adds progress info to each period, sorted by start date.
export const buildPeriods = (periods = [], now = new Date()) =>
    dedupePeriods(periods)
        .map((p) => {
            const startFullTerm = parseApiDate(p.FullTermStartDate);
            const endFullTerm = parseApiDate(p.FullTermEndDate);

            const startPeriod = parseApiDate(p.PeriodFrom);
            const endPeriod = parseApiDate(p.PeriodTo);

            const totalWeeks = Math.max(1, Math.ceil((endFullTerm - startFullTerm) / MS_PER_WEEK));
            const elapsed = Math.floor((now - startFullTerm) / MS_PER_WEEK) + 1;
            const current = Math.min(totalWeeks, Math.max(1, elapsed));

            return {
                id: keyOf(p),
                number: p.PeriodNumber,
                startPeriod,
                endPeriod,
                startFullTerm,
                endFullTerm,
                weekNow: current,
                weekTotal: totalWeeks,
                pct: Math.min(100, Math.max(0, ((now - startPeriod) / (endPeriod - startPeriod)) * 100)),
                isActive: now >= startPeriod && now <= endPeriod,
            };
        })
        .sort((a, b) => a.startPeriod - b.startPeriod);

// Locale-aware date: "June 6, 2033" / "6 de junio de 2033"
export const formatDate = (date, locale) =>
    date
        ? new Intl.DateTimeFormat(locale, { day: 'numeric', month: 'long', year: 'numeric' }).format(date)
        : '';

export const formatYearRange = (start, end, locale) =>
    `${new Intl.DateTimeFormat(locale, { year: 'numeric' }).format(start)} – ${new Intl.DateTimeFormat(locale, { year: 'numeric' }).format(end)}`;


// -----------------------------------------------------------------------