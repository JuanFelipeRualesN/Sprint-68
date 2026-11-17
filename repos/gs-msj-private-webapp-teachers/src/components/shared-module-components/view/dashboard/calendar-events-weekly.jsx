import React, { useState, useEffect, useMemo } from 'react';
import { createPortal } from 'react-dom';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getEventsByMonthAndYearForTeachers } from '../../../../api/teachers';
import { todayKey, dateKey, buildEventsMap } from './utils/calendar-events-utils';

// ─── Timezone helpers (unchanged) ────────────────────────────────────────────
const USER_TZ = Intl.DateTimeFormat().resolvedOptions().timeZone;

// ─── Windows → IANA timezone resolution ──────────────────────────────────────
// The API sends Windows timezone IDs ("SA Western Standard Time"), but Intl
// only accepts IANA names and throws a RangeError on anything else. Every zone
// goes through resolveTimeZone() before it reaches a formatter.
const WINDOWS_TO_IANA = {
    'SA Western Standard Time':        'America/La_Paz',
    'SA Pacific Standard Time':        'America/Bogota',
    'SA Eastern Standard Time':        'America/Cayenne',
    'Pacific SA Standard Time':        'America/Santiago',
    'Central Brazilian Standard Time': 'America/Cuiaba',
    'E. South America Standard Time':  'America/Sao_Paulo',
    'Argentina Standard Time':         'America/Argentina/Buenos_Aires',
    'Montevideo Standard Time':        'America/Montevideo',
    'Paraguay Standard Time':          'America/Asuncion',
    'Venezuela Standard Time':         'America/Caracas',
    'Central America Standard Time':   'America/Guatemala',
    'Mexico Standard Time':            'America/Mexico_City',
    'Central Standard Time (Mexico)':  'America/Mexico_City',
    'Eastern Standard Time':           'America/New_York',
    'Central Standard Time':           'America/Chicago',
    'Mountain Standard Time':          'America/Denver',
    'US Mountain Standard Time':       'America/Phoenix',
    'Pacific Standard Time':           'America/Los_Angeles',
    'Alaskan Standard Time':           'America/Anchorage',
    'Hawaiian Standard Time':          'Pacific/Honolulu',
    'Atlantic Standard Time':          'America/Halifax',
    'GMT Standard Time':               'Europe/London',
    'Greenwich Standard Time':         'Atlantic/Reykjavik',
    'W. Europe Standard Time':         'Europe/Berlin',
    'Romance Standard Time':           'Europe/Paris',
    'Central Europe Standard Time':    'Europe/Budapest',
    'Central European Standard Time':  'Europe/Warsaw',
    'E. Europe Standard Time':         'Europe/Chisinau',
    'UTC':                             'Etc/UTC',
    'Coordinated Universal Time':      'Etc/UTC',
};

const tzCache = new Map();

function isValidTimeZone(tz) {
    try {
        new Intl.DateTimeFormat('en-US', { timeZone: tz });
        return true;
    } catch {
        return false;
    }
}

// Returns a usable IANA zone, or null when the value cannot be resolved.
// A null zone makes toInstant() fall back to the runtime's own parsing, which
// is wrong by at most a few hours — far better than an uncaught RangeError.
function resolveTimeZone(raw) {
    if (!raw) return null;
    const key = String(raw).trim();
    if (!key) return null;
    if (tzCache.has(key)) return tzCache.get(key);

    let resolved = null;
    if (isValidTimeZone(key)) {
        resolved = key;                                   // already IANA
    } else {
        const mapped = WINDOWS_TO_IANA[key];
        if (mapped && isValidTimeZone(mapped)) resolved = mapped;
        else if (process.env.NODE_ENV !== 'production') {
            console.warn(`[calendar] Unmapped timezone "${key}" — falling back to local time.`);
        }
    }

    tzCache.set(key, resolved);
    return resolved;
}

const hasExplicitOffset = (v) =>
    typeof v === 'string' && /(?:Z|[+-]\d{2}:?\d{2})$/.test(v.trim());

function wallTimeToInstant(str, timeZone) {
    if (!timeZone) return new Date(String(str).replace(' ', 'T'));
    const m = String(str).trim().match(/^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})(?::(\d{2}))?/);
    if (!m) return new Date(str);
    const [, y, mo, d, h, mi, s] = m.map(Number);
    const utcGuess = Date.UTC(y, mo - 1, d, h, mi, s || 0);
    const parts = new Intl.DateTimeFormat('en-US', {
        timeZone, hour12: false,
        year: 'numeric', month: '2-digit', day: '2-digit',
        hour: '2-digit', minute: '2-digit', second: '2-digit',
    }).formatToParts(new Date(utcGuess)).reduce((acc, p) => { acc[p.type] = p.value; return acc; }, {});
    const seen = Date.UTC(+parts.year, +parts.month - 1, +parts.day, +parts.hour % 24, +parts.minute, +parts.second);
    return new Date(utcGuess - (seen - utcGuess));
}

function toInstant(value, backendTz) {
    if (!value) return null;
    if (hasExplicitOffset(value) || !backendTz) return new Date(value);
    return wallTimeToInstant(value, backendTz);
}

function formatTimeRangeInUserTz(startDate, endDate, backendTz, locale, onlyEndDate = false) {
    const start = toInstant(startDate, backendTz);
    if (!start || isNaN(start)) return null;
    const fmt = (d) => new Intl.DateTimeFormat(locale, {
        hour: '2-digit', minute: '2-digit', hour12: true, timeZone: USER_TZ,
    }).format(d);
    const end = toInstant(endDate, backendTz);
    if (onlyEndDate) return end && !isNaN(end) ? `${fmt(end)}` : null;
    return end && !isNaN(end) ? `${fmt(start)} – ${fmt(end)}` : fmt(start);
}

// Per-event source zone: homework is forced UTC, everything else keeps the
// backend-provided zone (matches the original behaviour).
const eventSourceTz = (event) =>
    String(event.type) === 'homework'
        ? 'Etc/UTC'
        : resolveTimeZone(event.timezone ?? event.timeZone);

// Minutes-past-midnight for an instant, expressed in the user's zone.
function minutesOfDayInUserTz(date) {
    const p = new Intl.DateTimeFormat('en-US', {
        timeZone: USER_TZ, hour12: false, hour: '2-digit', minute: '2-digit',
    }).formatToParts(date).reduce((a, x) => { a[x.type] = x.value; return a; }, {});
    return (Number(p.hour) % 24) * 60 + Number(p.minute);
}

// ─── All-day / "no real time" detection ──────────────────────────────────────
// Some records (typically lessons) come back as midnight → midnight, which the
// formatter renders as "12:00 AM – 12:00 AM". That is a placeholder, not a
// schedule, so we treat those events as all-day and never print a clock time.
const rawIsMidnight = (v) =>
    typeof v === 'string' && /[T ]00:00(:00)?(\.0+)?(?:Z|[+-]00:?00)?$/.test(v.trim());

function isAllDayEvent(event) {
    const tz = eventSourceTz(event);
    const start = toInstant(event.startDate, tz);
    if (!start || isNaN(start)) return true;                     // nothing usable to show
    if (rawIsMidnight(event.startDate) && rawIsMidnight(event.endDate)) return true;

    const end = toInstant(event.endDate, tz);
    if (!end || isNaN(end)) return minutesOfDayInUserTz(start) === 0;
    if (+end === +start) return true;                            // zero-length
    return minutesOfDayInUserTz(start) === 0 && minutesOfDayInUserTz(end) === 0;
}

// Single entry point for "what time should we print for this event?".
// Returns null when there is nothing meaningful to show.
function eventTime(event, locale) {
    if (isAllDayEvent(event)) return null;
    return formatTimeRangeInUserTz(
        event.startDate, event.endDate, eventSourceTz(event),
        locale, String(event.type) === 'homework',
    );
}

// Multi-day events need the date as well as the clock time, otherwise
// "4:00 PM – 5:00 PM" hides the fact that the event runs across three days.
function spansMultipleDays(event) {
    const tz = eventSourceTz(event);
    const start = toInstant(event.startDate, tz);
    const end = toInstant(event.endDate, tz);
    if (!start || !end || isNaN(start) || isNaN(end)) return false;
    const dayOf = (d) => new Intl.DateTimeFormat('en-CA', {
        timeZone: USER_TZ, year: 'numeric', month: '2-digit', day: '2-digit',
    }).format(d);
    return dayOf(start) !== dayOf(end);
}

function formatDateTimeRange(event, locale) {
    const tz = eventSourceTz(event);
    const start = toInstant(event.startDate, tz);
    const end = toInstant(event.endDate, tz);
    if (!start || isNaN(start)) return null;
    const fmt = (d) => new Intl.DateTimeFormat(locale, {
        day: 'numeric', month: 'short',
        hour: '2-digit', minute: '2-digit', hour12: true, timeZone: USER_TZ,
    }).format(d);
    return end && !isNaN(end) ? `${fmt(start)} – ${fmt(end)}` : fmt(start);
}

// ─── Colors ──────────────────────────────────────────────────────────────────
// `purple` is the EventType 4 (CalendarEvent) palette; the mapping from the
// backend enum to these keys lives in calendar-events-utils.
const EVENT_HEX = {
    blue:   '#185FA5',
    red:    '#982837',
    teal:   '#0F6E56',
    purple: '#5B3FA8',
};
const EVENT_BG = {
    blue:   '#b4d8fc',
    red:    '#f8a5b1',
    teal:   '#abffec',
    purple: '#e2dbf7',
};

const colorKey = (event) => (EVENT_HEX[event.color] ? event.color : 'teal');
const hexFor   = (event) => EVENT_HEX[colorKey(event)];

function rgba(hex, alpha) {
    const h = hex.replace('#', '');
    const r = parseInt(h.slice(0, 2), 16);
    const g = parseInt(h.slice(2, 4), 16);
    const b = parseInt(h.slice(4, 6), 16);
    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
}

// ─── Description rendering ───────────────────────────────────────────────────
// The backend sends either plain text, a bare URL, or a fragment of HTML.
// Plain text is escaped and linkified; HTML is passed through with every link
// forced to open in a new tab.
const URL_RE        = /(https?:\/\/[^\s<>"')]+)/gi;
const looksLikeHtml = (s) => /<\/?[a-z][\s\S]*>/i.test(s);

const escapeHtml = (s) => s.replace(/[&<>"']/g, (c) => (
    { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]
));

const LINK_ATTRS = 'target="_blank" rel="noopener noreferrer" ' +
    'style="color:var(--text-accent);word-break:break-all"';

function descriptionToHtml(raw) {
    const text = String(raw ?? '').trim();
    if (!text) return null;

    if (looksLikeHtml(text)) {
        // If descriptions can be authored by users, run this through
        // DOMPurify.sanitize(text) before returning it.
        return text.replace(/<a\s/gi, '<a target="_blank" rel="noopener noreferrer" ');
    }

    return escapeHtml(text)
        .replace(URL_RE, (url) => `<a href="${url}" ${LINK_ATTRS}>${url}</a>`)
        .replace(/\n/g, '<br/>');
}

// ─── Location ────────────────────────────────────────────────────────────────
// Lessons, quizzes/exams and assignments always happen on the platform.
// Everything else uses the backend value, and the row is hidden when it is null.
// Note: matched against the *mapped* type, never the raw enum. 'calendarEvent'
// deliberately does not match — those events carry a real Location.
const PLATFORM_TYPES = ['homework', 'lesson', 'lessonQuiz'];
const isPlatformType = (type) => PLATFORM_TYPES.includes(String(type ?? ''));

function locationFor(event, t) {
    if (isPlatformType(event.type)) {
        return t('calendar-events:modal.platformOnline', { defaultValue: 'Platform (Online)' });
    }
    const loc = typeof event.location === 'string' ? event.location.trim() : '';
    return loc || null;
}

// ─── Week helpers ─────────────────────────────────────────────────────────────
function addDays(date, n) { const d = new Date(date); d.setDate(d.getDate() + n); return d; }

function startOfWeek(date) { // Monday, at 00:00 local
    const d = new Date(date);
    const day = d.getDay();                 // 0 Sun … 6 Sat
    d.setDate(d.getDate() + (day === 0 ? -6 : 1 - day));
    d.setHours(0, 0, 0, 0);
    return d;
}

function isoWeek(date) {
    const d = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
    const dayNum = (d.getUTCDay() + 6) % 7;
    d.setUTCDate(d.getUTCDate() - dayNum + 3);
    const firstThursday = new Date(Date.UTC(d.getUTCFullYear(), 0, 4));
    const fDay = (firstThursday.getUTCDay() + 6) % 7;
    firstThursday.setUTCDate(firstThursday.getUTCDate() - fDay + 3);
    return 1 + Math.round((d - firstThursday) / (7 * 24 * 3600 * 1000));
}

// ─── Lesson / quiz routing (unchanged) ───────────────────────────────────────
function getEventLink(event) {
    const type = String(event.type || '').toLowerCase();

    const isQuiz = /quiz|test|exam/.test(type) || event.quizId != null;
    if (isQuiz) {
        const quizId = event.quizId ?? event.quizID ?? event.testId ?? event.id;
        if (quizId != null) {
            return { to: `/students/lesson/quiz/${quizId}?fromDashboard=true`, labelKey: 'calendar-events:modal.goToQuiz' };
        }
    }

    const isLesson = /lesson|class/.test(type) || event.lessonNumber != null || event.lessonId != null;
    if (isLesson) {
        const lessonNumber = event.lessonNumber ?? event.lessonNo ?? event.lessonId ?? event.id;
        if (lessonNumber != null) {
            return { to: `/students/lesson/${lessonNumber}?fromDashboard=true`, labelKey: 'calendar-events:modal.goToLesson' };
        }
    }
    return null;
}

// ─── Event modal — full detail for a single event ────────────────────────────
function EventModal({ event, dateLabel, onClose }) {
    const { t, i18n } = useTranslation(['calendar-events', 'common']);

    useEffect(() => {
        const handler = (e) => { if (e.key === 'Escape') onClose(); };
        document.addEventListener('keydown', handler);
        return () => document.removeEventListener('keydown', handler);
    }, [onClose]);

    useEffect(() => {
        document.body.style.overflow = 'hidden';
        return () => { document.body.style.overflow = ''; };
    }, []);

    const key             = colorKey(event);
    const color           = EVENT_HEX[key];
    const backgroundColor = EVENT_BG[key];
    const badgeStyle      = { background: backgroundColor, color };

    const type  = String(event.type);
    const time  = spansMultipleDays(event) && !isAllDayEvent(event)
        ? formatDateTimeRange(event, i18n.language)
        : eventTime(event, i18n.language);

    const place           = locationFor(event, t);
    const descriptionHtml = descriptionToHtml(event.description);
    const group           = event.group ?? event.groupName;
    const attachments     = Array.isArray(event.attachments) ? event.attachments : [];

    const eventLink = getEventLink(event);

    // Every row gates itself on real data — nothing renders as "N/A" any more.
    const fields = [];

    if (type !== 'lesson' && time) {
        fields.push({ label: t('calendar-events:modal.time'), value: time });
    }
    if (place) {
        fields.push({ label: t('calendar-events:modal.place'), value: place });
    }
    if (event.subject) {
        fields.push({
            label: t('calendar-events:modal.subject', { defaultValue: 'Subject' }),
            value: event.subject,
        });
    }
    if (event.unit != null) {
        fields.push({
            label: t('calendar-events:modal.unit', { defaultValue: 'Unit' }),
            value: String(event.unit),
        });
    }
    if (group) {
        fields.push({
            label: t('calendar-events:modal.group', { defaultValue: 'Group' }),
            value: group,
        });
    }
    if (event.students) {
        fields.push({
            label: t('calendar-events:modal.students', { defaultValue: 'Students' }),
            value: event.students,
        });
    }
    if (descriptionHtml) {
        fields.push({
            label: t('calendar-events:modal.description', { defaultValue: 'Description' }),
            value: (
                <div
                    className="med-modal-desc"
                    style={{ lineHeight: 1.45, wordBreak: 'break-word' }}
                    dangerouslySetInnerHTML={{ __html: descriptionHtml }}
                />
            ),
        });
    }
    if (event.hasAttachment && attachments.length > 0) {
        fields.push({
            label: t('calendar-events:modal.attachment'),
            value: (
                <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                    {attachments.map((att, i) => {
                        const name = att.name || att.fileName || att.title || t('common:general.na');
                        const url  = att.url || att.path || att.link;
                        return url
                            ? (<a key={i} href={url} target="_blank" rel="noopener noreferrer"
                                  style={{ color: 'var(--text-accent)', textDecoration: 'none', wordBreak: 'break-all' }}>{name}</a>)
                            : <span key={i} style={{ color: 'var(--text-primary)' }}>{name}</span>;
                    })}
                </div>
            ),
        });
    }

    const modal = (
        <div
            onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}
            style={{
                position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.45)',
                display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 9999,
                backdropFilter: 'blur(5px)',
            }}
        >
            <div role="dialog" aria-modal="true"
                 style={{
                     background: 'var(--surface-2)', border: '0.5px solid var(--border)', borderRadius: 12,
                     padding: '1.25rem 1.5rem', width: 360, maxWidth: 'calc(100vw - 2rem)',
                     maxHeight: 'calc(100vh - 4rem)', overflowY: 'auto',
                     boxShadow: '0 8px 32px rgba(0,0,0,0.18)', backgroundColor: 'white',
                 }}
            >
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: 12 }}>
                    <div style={{ fontSize: 12, color: 'var(--text-muted)' }}>{dateLabel}</div>
                    <button onClick={onClose} aria-label={t('common:actions.close')}
                            style={{ background: 'none', border: 'none', cursor: 'pointer', color: 'var(--text-muted)',
                                fontSize: 20, padding: '0 0 0 12px', lineHeight: 1, flexShrink: 0 }}>✕</button>
                </div>
                <hr style={{ border: 'none', borderTop: '0.5px solid var(--border)', margin: '0 0 16px' }} />

                <span style={{ ...badgeStyle, display: 'inline-block', fontSize: 11, padding: '3px 10px',
                    borderRadius: 20, marginBottom: 8, fontWeight: 500 }}>
                    {t(`calendar-events:eventTypes.${event.type}`)}
                </span>
                <div style={{ fontSize: 16, fontWeight: 500, color: 'var(--text-primary)', lineHeight: 1.3 }}>
                    {event.title}
                </div>

                {fields.length > 0 && (
                    <div style={{ marginTop: 12 }}>
                        {fields.map((field) => (
                            <div key={field.label} style={{ display: 'flex', gap: 10, marginBottom: 10, fontSize: 13, alignItems: 'flex-start' }}>
                                <span style={{ color: 'var(--text-muted)', minWidth: 72, paddingTop: 1 }}>{field.label}</span>
                                <span style={{ color: 'var(--text-primary)', flex: 1 }}>{field.value}</span>
                            </div>
                        ))}
                    </div>
                )}

                {eventLink && (
                    <Link to={eventLink.to} onClick={onClose}
                          style={{ display: 'inline-flex', alignItems: 'center', gap: 6, marginTop: 4,
                              padding: '8px 14px', borderRadius: 8, background: color, color: '#fff',
                              fontSize: 13, fontWeight: 500, textDecoration: 'none' }}>
                        {t(eventLink.labelKey)}
                        <span aria-hidden="true">→</span>
                    </Link>
                )}
            </div>
        </div>
    );

    return createPortal(modal, document.body);
}

// ─── Timeline layout for the selected day ────────────────────────────────────
// Vertical rail: events flow top-to-bottom in chronological order and each card
// is free to grow, so titles wrap instead of being clipped. Events without a
// real clock time are lifted into a separate "all day" strip above the rail.
function buildTimeline(events, locale) {
    const allDay = [];
    const rows = [];

    events.forEach((ev) => {
        const hex = hexFor(ev);
        const tz = eventSourceTz(ev);
        const start = toInstant(ev.startDate, tz);

        if (isAllDayEvent(ev) || !start || isNaN(start)) {
            allDay.push({ ev, hex });
            return;
        }

        const end = toInstant(ev.endDate, tz);
        const startMin = minutesOfDayInUserTz(start);
        const endMin = end && !isNaN(end) ? minutesOfDayInUserTz(end) : null;

        rows.push({
            ev, hex, startMin,
            time: eventTime(ev, locale),
            durationMin: endMin != null && endMin > startMin ? endMin - startMin : null,
        });
    });

    rows.sort((a, b) => a.startMin - b.startMin);
    return { allDay, rows };
}

const metaOf = (ev) => [ev.group ?? ev.groupName, ev.students].filter(Boolean).join(' · ');

// ─── Main component ──────────────────────────────────────────────────────────
export default function CalendarEventsWeekly() {
    const { t, i18n } = useTranslation(['calendar-events', 'common']);

    const [anchor, setAnchor] = useState(() => new Date());   // any date inside the visible week
    const [view, setView]     = useState('timeline');         // 'timeline' | 'list'
    const [events, setEvents] = useState({});                 // { 'YYYY-MM-DD': Event[] }
    const [loading, setLoading] = useState(false);
    const [modalEvent, setModalEvent] = useState(null);

    const monday = useMemo(() => startOfWeek(anchor), [anchor]);
    const weekStartKey = dateKey(monday.getFullYear(), monday.getMonth(), monday.getDate());
    const todayStr = todayKey();

    const [day, setDay] = useState(0);   // selected day index 0..6 (Mon..Sun)

    // Fetch the month(s) the visible week spans (a week can cross a month boundary).
    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        setModalEvent(null);

        const monthsMap = new Map();
        for (let i = 0; i < 7; i++) {
            const d = addDays(monday, i);
            monthsMap.set(`${d.getFullYear()}-${d.getMonth()}`, { y: d.getFullYear(), m: d.getMonth() });
        }
        const months = [...monthsMap.values()];

        Promise.all(months.map(({ m, y }) =>
            getEventsByMonthAndYearForTeachers(m + 1, y).then((r) => r.data).catch(() => [])
        ))
            .then((datas) => {
                if (cancelled) return;
                const merged = {};
                datas.forEach((data) => {
                    const map = buildEventsMap(data || []);
                    Object.entries(map).forEach(([k, v]) => {
                        merged[k] = (merged[k] || []).concat(v);
                    });
                });
                setEvents(merged);
            })
            .finally(() => { if (!cancelled) setLoading(false); });

        return () => { cancelled = true; };
    }, [weekStartKey]);

    // On week change, jump selection to today when it's in view, else Monday.
    useEffect(() => {
        let found = -1;
        for (let i = 0; i < 7; i++) {
            const d = addDays(monday, i);
            if (dateKey(d.getFullYear(), d.getMonth(), d.getDate()) === todayStr) { found = i; break; }
        }
        setDay(found >= 0 ? found : 0);
    }, [weekStartKey]);

    const changeWeek = (dir) => setAnchor((prev) => addDays(startOfWeek(prev), dir * 7));

    // Build the seven days with their events.
    const weekDays = useMemo(() => {
        const shortFmt = new Intl.DateTimeFormat(i18n.language, { weekday: 'short' });
        const longFmt  = new Intl.DateTimeFormat(i18n.language, { weekday: 'long' });
        return Array.from({ length: 7 }, (_, i) => {
            const date = addDays(monday, i);
            const key = dateKey(date.getFullYear(), date.getMonth(), date.getDate());
            return {
                date, key,
                dowShort: shortFmt.format(date),
                dowFull: longFmt.format(date),
                dateNum: date.getDate(),
                isToday: key === todayStr,
                events: events[key] || [],
            };
        });
    }, [weekStartKey, events, i18n.language, todayStr]);

    const selDay = weekDays[Math.min(day, 6)] || weekDays[0];
    const selectedEvents = selDay?.events || [];

    const timeline = useMemo(
        () => buildTimeline(selectedEvents, i18n.language),
        [selectedEvents, i18n.language],
    );

    // Hours scheduled for the selected day — all-day records are skipped so a
    // midnight → midnight lesson never inflates the total.
    const selectedHours = useMemo(() => {
        let mins = 0;
        selectedEvents.forEach((ev) => {
            if (isAllDayEvent(ev)) return;
            const tz = eventSourceTz(ev);
            const s = toInstant(ev.startDate, tz);
            const e = toInstant(ev.endDate, tz);
            if (s && e && !isNaN(s) && !isNaN(e) && e > s) mins += (e - s) / 60000;
        });
        const h = mins / 60;
        return Number.isInteger(h) ? h : Math.round(h * 10) / 10;
    }, [selectedEvents]);

    const weekNum = isoWeek(monday);
    const isCurrentWeek = weekDays.some((d) => d.isToday);

    const rangeLabel = useMemo(() => {
        const sun = addDays(monday, 6);
        const fmt = new Intl.DateTimeFormat(i18n.language, { day: 'numeric', month: 'long', year: 'numeric' });
        try { return fmt.formatRange(monday, sun); }
        catch { return `${fmt.format(monday)} – ${fmt.format(sun)}`; }
    }, [weekStartKey, i18n.language]);

    const dayLabel = t('calendar-events:dateLabel', {
        day: selDay?.dateNum, month: selDay?.dowFull, year: monday.getFullYear(),
    });

    // ── inline styles for the small controls (no extra CSS needed) ──
    const navBtn = {
        background: 'none', border: '0.5px solid var(--border)', borderRadius: 'var(--radius)',
        padding: '4px 8px', cursor: 'pointer', color: 'var(--text-primary)', fontSize: 13, flexShrink: 0,
    };
    const toggleBtn = (active) => ({
        background: active ? 'var(--bg-accent)' : 'transparent',
        color: active ? 'var(--text-accent)' : 'var(--text-muted)',
        border: '0.5px solid var(--border)', padding: '3px 10px', fontSize: 12, fontWeight: 500,
        cursor: 'pointer',
    });

    return (
        <div className="widget">
            <div className="w-head">
                <h3>{t('calendar-events:title')}</h3>
                <div style={{ display: 'inline-flex', borderRadius: 'var(--radius)', overflow: 'hidden' }}>
                    {/*}  <button type="button" style={{ ...toggleBtn(view === 'timeline'), borderRadius: '6px 0 0 6px' }}
                            onClick={() => setView('timeline')}>
                        {t('calendar-events:view.timeline')}
                    </button>
                    <button type="button" style={{ ...toggleBtn(view === 'list'), borderLeft: 'none', borderRadius: '0 6px 6px 0' }}
                            onClick={() => setView('list')}>
                        {t('calendar-events:view.list')}
                    </button> */}
                </div>
            </div>

            <section className="med-week-card">
                <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                    <button type="button" style={navBtn} onClick={() => changeWeek(-1)}
                            aria-label={t('calendar-events:nav.previousWeek')}>‹</button>

                    <div className="med-week-head" style={{ flex: 1 }}>
                        <strong>{rangeLabel}</strong>
                    </div>

                    <button type="button" style={navBtn} onClick={() => changeWeek(1)}
                            aria-label={t('calendar-events:nav.nextWeek')}>›</button>
                </div>

                <div className="med-week-strip">
                    {weekDays.map((d, i) => {
                        const off = d.events.length === 0;
                        const sel = i === day;
                        return (
                            <button key={i} type="button"
                                    className={`med-day ${sel ? 'sel' : ''} ${off ? 'off' : ''}`}
                                    onClick={() => setDay(i)}>
                                <span className="dow">{d.dowShort}</span>
                                <span className="num">{d.dateNum}</span>
                                <span className="count">
                                    {off
                                        ? t('calendar-events:week.free')
                                        : t('calendar-events:week.eventCount', { count: d.events.length })}
                                </span>
                                {d.isToday && <span className="dot" />}
                            </button>
                        );
                    })}
                </div>

                <div className="med-day-detail">
                    <div className="med-day-label">
                        {selDay?.dowFull}, {selDay?.dateNum}
                        {selDay?.isToday ? ` · ${t('calendar-events:week.today')}` : ''}
                    </div>

                    {selectedEvents.length === 0 ? (
                        <div className="med-day-empty">
                            <strong>{t('calendar-events:day.empty.title')}</strong>
                            <span>{t('calendar-events:day.empty.hint')}</span>
                        </div>
                    ) : view === 'timeline' ? (
                        <div className="med-timeline">
                            {timeline.allDay.length > 0 && (
                                <div className="med-tl-allday">
                                    <span className="med-tl-allday-label">
                                        {t('calendar-events:day.allDay', { defaultValue: 'All day' })}
                                    </span>
                                    <div className="med-tl-allday-items">
                                        {timeline.allDay.map((b, i) => (
                                            <button key={b.ev.id ?? i} type="button" className="med-tl-chip"
                                                    onClick={() => setModalEvent(b.ev)}
                                                    style={{
                                                        background: rgba(b.hex, 0.10),
                                                        borderColor: rgba(b.hex, 0.30),
                                                        color: b.hex,
                                                    }}>
                                                {b.ev.title}
                                            </button>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {timeline.rows.length > 0 && (
                                <ul className="med-tl-rows">
                                    {timeline.rows.map((b, i) => {
                                        const meta = metaOf(b.ev);
                                        return (
                                            <li key={b.ev.id ?? i} className="med-tl-row">
                                                <span className="med-tl-rail" aria-hidden="true">
                                                    <span className="med-tl-dot" style={{ background: b.hex }} />
                                                </span>
                                                <button type="button" className="med-tl-card"
                                                        onClick={() => setModalEvent(b.ev)}
                                                        style={{
                                                            background: rgba(b.hex, 0.08),
                                                            borderColor: rgba(b.hex, 0.25),
                                                            borderLeftColor: b.hex,
                                                        }}>
                                                    <span className="med-tl-head">
                                                        <span className="med-tl-type"
                                                              style={{ background: rgba(b.hex, 0.14), color: b.hex }}>
                                                            {t(`calendar-events:eventTypes.${b.ev.type}`)}
                                                        </span>
                                                        {b.time && (
                                                            <span className="med-tl-when" style={{ color: b.hex }}>
                                                                {b.time}
                                                            </span>
                                                        )}
                                                    </span>
                                                    <span className="med-tl-title">{b.ev.title}</span>
                                                    {meta && <span className="med-tl-meta">{meta}</span>}
                                                </button>
                                            </li>
                                        );
                                    })}
                                </ul>
                            )}
                        </div>
                    ) : (
                        <ul className="plan-list">
                            {selectedEvents.map((ev, i) => {
                                const hex = hexFor(ev);
                                const group = ev.group ?? ev.groupName;
                                const time = eventTime(ev, i18n.language);
                                return (
                                    <li key={ev.id ?? i} className="plan-item"
                                        onClick={() => setModalEvent(ev)} style={{ cursor: 'pointer' }}>
                                        <span style={{
                                            display: 'inline-block', fontSize: 11, padding: '2px 9px',
                                            borderRadius: 20, marginBottom: 6, fontWeight: 500,
                                            background: rgba(hex, 0.12), color: hex,
                                        }}>
                                            {t(`calendar-events:eventTypes.${ev.type}`)}
                                        </span>

                                        {ev.subject && <div className="plan-subj">{ev.subject}</div>}

                                        <div className="plan-unit">
                                            {ev.unit != null
                                                ? `${t('calendar-events:list.unit', { unit: ev.unit })} · `
                                                : ''}
                                            {ev.lessonTitle ?? ev.title}
                                        </div>

                                        {group && <div className="plan-groups">{group}</div>}
                                        {time && <div className="plan-time" style={{ color: hex, fontSize: 12, marginTop: 2 }}>{time}</div>}
                                    </li>
                                );
                            })}
                        </ul>
                    )}
                </div>
            </section>

            {modalEvent && (
                <EventModal
                    event={modalEvent}
                    dateLabel={dayLabel}
                    onClose={() => setModalEvent(null)}
                />
            )}
        </div>
    );
}