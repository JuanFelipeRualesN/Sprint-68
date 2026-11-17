import React, { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getEventsByMonthAndYearForTeachers } from '../../../../api/teachers';
import { todayKey, DAY_INDICES, dateKey, buildEventsMap } from './utils/calendar-events-utils';

// ─── Timezone helpers ────────────────────────────────────────────────────────
// The user's own IANA zone, resolved once. This is what we render times in.
const USER_TZ = Intl.DateTimeFormat().resolvedOptions().timeZone;

// True when the string already carries a UTC offset (…Z or ±HH:MM). If it does,
// `new Date()` already resolves the correct absolute instant and we must NOT
// re-interpret it against the backend zone.
const hasExplicitOffset = (v) =>
    typeof v === 'string' && /(?:Z|[+-]\d{2}:?\d{2})$/.test(v.trim());

// Interpret a naive "wall clock" string (e.g. "2026-01-15T14:00:00") as a time
// in `timeZone`, and return the matching absolute UTC instant.
function wallTimeToInstant(str, timeZone) {
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
    return new Date(utcGuess - (seen - utcGuess)); // utcGuess - offset
}

// Backend date → absolute Date. Honors the backend zone only when the raw
// string has no offset of its own.
function toInstant(value, backendTz) {
    if (!value) return null;
    if (hasExplicitOffset(value) || !backendTz) return new Date(value);
    return wallTimeToInstant(value, backendTz);
}

// Formats a start–end range in the user's local zone.
function formatTimeRangeInUserTz(startDate, endDate, backendTz, locale, onlyEndDate = false) {
    const start = toInstant(startDate, backendTz);
    if (!start || isNaN(start)) return null;
    const fmt = (d) => new Intl.DateTimeFormat(locale, {
        hour: '2-digit', minute: '2-digit', hour12: true, timeZone: USER_TZ,
    }).format(d);
    const end = toInstant(endDate, backendTz);
    if(onlyEndDate){
        return `${fmt(end)}`;
    }
    return end && !isNaN(end) ? `${fmt(start)} – ${fmt(end)}` : fmt(start);
}

// ─── Lesson / quiz routing ───────────────────────────────────────────────────
// Decides whether an event links to a lesson or a lesson's quiz, and to where.
// Adjust the type keywords / id field names to match your event payload.
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

// ─── Event modal — now renders every event on the selected day ───────────────
function EventModal({ events, dateLabel, onClose }) {
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

    const renderEvent = (event, idx) => {

        let color = '#0F6E56';
        let backgroundColor = '#abffec';
        if (event.color === 'blue') {
            color = '#185FA5';
            backgroundColor = '#b4d8fc';
        } else if (event.color === 'red') {
            color = '#982837';
            backgroundColor = '#f8a5b1';
        }

        const badgeStyle = { background: backgroundColor, color };

        console.log("EVENT TYPE", event.type)
        // Normalize so numeric or string types both match.
        const type = String(event.type);

        // Type 1 -> time only | Type 2 -> nothing | Type 3 -> time only | else -> everything
        const showTime       = type !== 'lesson';
        const showPlace      = !['homework', 'lesson', 'lessonQuiz'].includes(type);
        const showAttachment = !['homework', 'lesson', 'lessonQuiz'].includes(type);

        // Type 1 forces UTC -> user tz; other types keep the backend-provided tz ("as the code is right now").
        const backendTz  = event.timezone ?? event.timeZone ?? null;
        const sourceTz   = type === 'homework' ? 'UTC' : backendTz;
        const time       = formatTimeRangeInUserTz(event.startDate, event.endDate, sourceTz, i18n.language, type === 'homework');
        const tzAdjusted = Boolean(sourceTz && sourceTz !== USER_TZ);

        const eventLink = getEventLink(event);

        const fields = [];

        if (showTime) {
            fields.push({
                label: t('calendar-events:modal.time'),
                value: time
                    ? (
                        <span>
                        {time}
                            {tzAdjusted && (
                                <span style={{ color: 'var(--text-muted)', marginLeft: 6, fontSize: 11 }}>
                            </span>
                            )}
                    </span>
                    )
                    : <span style={{ color: 'var(--text-muted)' }}>{t('common:general.na')}</span>,
            });
        }

        if (showPlace) {
            fields.push({
                label: t('calendar-events:modal.place'),
                value: event.location
                    ? event.location
                    : <span style={{ color: 'var(--text-muted)' }}>{t('common:general.na')}</span>,
            });
        }

        if (showAttachment) {
            fields.push({
                label: t('calendar-events:modal.attachment'),
                value: event.hasAttachment
                    ? (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
                            {event.attachments.map((att, i) => {
                                const name = att.name || att.fileName || att.title || t('common:general.na');
                                const url  = att.url || att.path || att.link;
                                return url
                                    ? (
                                        <a key={i} href={url} target="_blank" rel="noopener noreferrer"
                                           style={{ color: 'var(--text-accent)', textDecoration: 'none', wordBreak: 'break-all' }}>
                                            {name}
                                        </a>
                                    )
                                    : <span key={i} style={{ color: 'var(--text-primary)' }}>{name}</span>;
                            })}
                        </div>
                    )
                    : <span style={{ color: 'var(--text-muted)' }}>{t('common:general.na')}</span>,
            });
        }

        return (
            <div key={event.id || idx} style={{ marginBottom: idx < events.length - 1 ? 20 : 0 }}>
            <span style={{
                ...badgeStyle, display: 'inline-block', fontSize: 11, padding: '3px 10px',
                borderRadius: 20, marginBottom: 8, fontWeight: 500,
            }}>
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
                    <Link
                        to={eventLink.to}
                        onClick={onClose}
                        style={{
                            display: 'inline-flex', alignItems: 'center', gap: 6,
                            marginTop: 4, padding: '8px 14px', borderRadius: 8,
                            background: color, color: '#fff', fontSize: 13, fontWeight: 500,
                            textDecoration: 'none',
                        }}
                    >
                        {t(eventLink.labelKey)}
                        <span aria-hidden="true">→</span>
                    </Link>
                )}

                {idx < events.length - 1 && (
                    <hr style={{ border: 'none', borderTop: '0.5px solid var(--border)', margin: '16px 0 0' }} />
                )}
            </div>
        );
    };

    const modal = (
        <div
            onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}
            style={{
                position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.45)',
                display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 9999,
                backdropFilter: 'blur(5px)'
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
                {events.map(renderEvent)}
            </div>
        </div>
    );

    return createPortal(modal, document.body);
}

// ─── Main component ──────────────────────────────────────────────────────────
export default function CalendarEventsMonthly() {
    const { t } = useTranslation(['calendar-events', 'common']);
    const now = new Date();
    const [year, setYear]         = useState(now.getFullYear());
    const [month, setMonth]       = useState(now.getMonth());   // 0-11
    const [selected, setSelected] = useState(null);             // { events, dateLabel }
    const [events, setEvents]     = useState({});               // { 'YYYY-MM-DD': Event[] }
    const [loading, setLoading]   = useState(false);

    // Fetch whenever the visible month/year changes. month + 1 → API's 1-12 convention.
    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        setSelected(null);

        getEventsByMonthAndYearForTeachers(month + 1, year)
            .then((res) => { if (!cancelled) setEvents(buildEventsMap(res.data)); })
            .catch(() => { if (!cancelled) setEvents({}); })
            .finally(() => { if (!cancelled) setLoading(false); });

        return () => { cancelled = true; };
    }, [year, month]);

    const changeMonth = (direction) => {
        const next = new Date(year, month + direction, 1);
        setYear(next.getFullYear());
        setMonth(next.getMonth());
    };

    const firstWeekday  = new Date(year, month, 1).getDay();
    const daysInMonth   = new Date(year, month + 1, 0).getDate();
    const prevMonthDays = new Date(year, month, 0).getDate();
    const todayStr      = todayKey();

    const cells = [];
    for (let offset = firstWeekday - 1; offset >= 0; offset--) {
        const cm = month === 0 ? 11 : month - 1;
        const cy = month === 0 ? year - 1 : year;
        cells.push({ day: prevMonthDays - offset, current: false, m: cm, y: cy });
    }
    for (let day = 1; day <= daysInMonth; day++) {
        cells.push({ day, current: true, m: month, y: year });
    }
    let nextDay = 1;
    while (cells.length % 7 !== 0) {
        const nm = month === 11 ? 0 : month + 1;
        const ny = month === 11 ? year + 1 : year;
        cells.push({ day: nextDay++, current: false, m: nm, y: ny });
    }

    const dotColor = { blue: '#378ADD', teal: '#1D9E75', red: '#f57090' };

    return (
        <div className="widget">
            <div className="w-head">
                <h3>{t('calendar-events:title')}</h3>
            </div>

            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 12 }}>
                <button onClick={() => changeMonth(-1)} aria-label={t('calendar-events:nav.previousMonth')}
                        style={{ background: 'none', border: '0.5px solid var(--border)', borderRadius: 'var(--radius)',
                            padding: '4px 8px', cursor: 'pointer', color: 'var(--text-primary)', fontSize: 13 }}>‹</button>
                <span style={{ fontSize: 14, fontWeight: 500, color: 'var(--text-primary)' }}>
                    {t(`calendar-events:months.${month}`)} {year}
                    {loading && <span style={{ marginLeft: 8, fontSize: 11, color: 'var(--text-muted)' }}>…</span>}
                </span>
                <button onClick={() => changeMonth(1)} aria-label={t('calendar-events:nav.nextMonth')}
                        style={{ background: 'none', border: '0.5px solid var(--border)', borderRadius: 'var(--radius)',
                            padding: '4px 8px', cursor: 'pointer', color: 'var(--text-primary)', fontSize: 13 }}>›</button>
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: 2 }}>
                {DAY_INDICES.map((idx) => (
                    <div key={idx} style={{ textAlign: 'center', fontSize: 11, color: 'var(--text-muted)', padding: '4px 0', fontWeight: 500 }}>
                        {t(`calendar-events:days.${idx}`)}
                    </div>
                ))}

                {cells.map((cell, idx) => {
                    const key       = dateKey(cell.y, cell.m, cell.day);
                    const dayEvents = cell.current ? (events[key] || []) : [];
                    const hasEvents = dayEvents.length > 0;
                    const isToday   = cell.current && key === todayStr;

                    const handleSelect = () => {
                        if (!hasEvents) return;
                        const monthName = t(`calendar-events:months.${cell.m}`);
                        setSelected({
                            events: dayEvents,
                            dateLabel: t('calendar-events:dateLabel', { day: cell.day, month: monthName, year: cell.y }),
                        });
                    };

                    return (
                        <div key={idx}
                             role={hasEvents ? 'button' : undefined}
                             tabIndex={hasEvents ? 0 : undefined}
                             aria-label={hasEvents
                                 ? t('calendar-events:cellAriaLabel', { day: cell.day, month: t(`calendar-events:months.${cell.m}`), title: dayEvents[0].title })
                                 : undefined}
                             onClick={handleSelect}
                             onKeyDown={(e) => (e.key === 'Enter' || e.key === ' ') && handleSelect()}
                             style={{
                                 minHeight: 36, borderRadius: 6, display: 'flex', flexDirection: 'column',
                                 alignItems: 'center', justifyContent: 'center',
                                 cursor: hasEvents ? 'pointer' : 'default', gap: 3,
                                 background: isToday ? 'var(--bg-accent)' : 'transparent',
                                 opacity: cell.current ? 1 : 0.35,
                             }}
                        >
                            <span style={{ fontSize: 13, fontWeight: (hasEvents || isToday) ? 500 : 400,
                                color: isToday ? 'var(--text-accent)' : 'var(--text-secondary)' }}>
                                {cell.day}
                            </span>
                            {hasEvents && (
                                <span style={{ display: 'flex', gap: 2 }}>
                                    {dayEvents.slice(0, 3).map((ev, i) => (
                                        <span key={i} style={{ width: 5, height: 5, borderRadius: '50%',
                                            background: dotColor[ev.color] || '#378ADD', display: 'block' }} />
                                    ))}
                                </span>
                            )}
                        </div>
                    );
                })}
            </div>

            {selected && (
                <EventModal
                    events={selected.events}
                    dateLabel={selected.dateLabel}
                    onClose={() => setSelected(null)}
                />
            )}
        </div>
    );
}
