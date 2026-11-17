
export const DAY_INDICES   = [0, 1, 2, 3, 4, 5, 6];

export function pad(n) { return String(n).padStart(2, '0'); }
export function dateKey(year, monthIndex, day) {
    const mm = String(monthIndex + 1).padStart(2, '0');
    const dd = String(day).padStart(2, '0');
    return `${year}-${mm}-${dd}`;
}
export function todayKey() {
    const d = new Date();
    return dateKey(d.getFullYear(), d.getMonth(), d.getDate());
}

// Maps the C# EventType enum to the component's display type + color.
// Handles both numeric (1/2/3, the default) and string ("Homework") serialization,
// so it works whether or not JsonStringEnumConverter is registered on the API.

// Small helper: read a property by name regardless of first-letter casing.
export function pick(obj, name) {
    if (!obj) return undefined;
    const lower = name.charAt(0).toLowerCase() + name.slice(1);
    const upper = name.charAt(0).toUpperCase() + name.slice(1);
    return obj[lower] !== undefined ? obj[lower] : obj[upper];
}

// ─── EventType mapping ───────────────────────────────────────────────────────
// Mirrors the backend enum exactly. Keep this total: an unknown value must not
// silently inherit another type's behaviour, because `type` drives the label,
// the palette, the timezone source, and the location rule downstream.
//
//   1 Homework      — quiz attached directly to a plan (Evidencias de Aprendizaje)
//   2 Lesson        — a lesson within a plan
//   3 LessonQuiz    — quiz attached to a specific lesson
//   4 CalendarEvent — custom event from the events module
export const EVENT_TYPE_MAP = {
    1: { type: 'homework',      color: 'red'    },
    2: { type: 'lesson',        color: 'blue'   },
    3: { type: 'lessonQuiz',    color: 'blue'   },
    4: { type: 'calendarEvent', color: 'purple' },
};

// Unknown types render as a neutral calendar entry rather than masquerading as
// homework — homework is forced to UTC and locked to "Platform (Online)", which
// is wrong for anything that isn't actually homework.
export function mapEventType(raw) {
    return EVENT_TYPE_MAP[Number(raw)] ?? { type: 'calendarEvent', color: 'teal' };
}

// ─── Normalisation ───────────────────────────────────────────────────────────
const nullIfBlank = (v) => {
    if (v == null) return null;
    const s = String(v).trim();
    return s === '' ? null : s;
};

// 'YYYY-MM-DD HH:mm:ss' → the calendar day it belongs to, without constructing
// a Date (which would shift the day for timezones behind/ahead of the wall
// time the backend sent).
const dayKeyFromRaw = (value) => {
    const m = String(value ?? '').trim().match(/^(\d{4})-(\d{2})-(\d{2})/);
    return m ? `${m[1]}-${m[2]}-${m[3]}` : null;
};
function normalizeEvent(item) {
    const m = item?.EventModel ?? item;
    if (!m) return null;

    const { type, color } = mapEventType(m.EventType);
    const attachments = item?.EventAttachmentModelList ?? [];

    return {
        id: m.Id,
        type,
        color,
        title: nullIfBlank(m.Title),
        description: nullIfBlank(m.Description),
        location: nullIfBlank(m.Location),
        startDate: m.StartDate,
        endDate: m.EndDate,
        timezone: nullIfBlank(m.TimeZoneCode),
        hasAttachment: attachments.length > 0,
        attachments,
        // Kept for the list view / meta line when the API supplies them.
        subject: nullIfBlank(m.Subject),
        unit: m.Unit ?? null,
        group: nullIfBlank(m.Group ?? m.GroupName),
        students: nullIfBlank(m.Students),
        lessonTitle: nullIfBlank(m.LessonTitle),
        lessonNumber: m.LessonNumber ?? null,
        lessonId: m.LessonId ?? null,
        quizId: m.QuizId ?? null,
        available: m.Available === '1' || m.Available === 1 || m.Available === true,
    };
}

// A multi-day event should appear on every day it covers, not only on its start
// date. Capped so a malformed end date can't spray an event across the year.
const MAX_SPAN_DAYS = 31;

function dayKeysCovered(event) {
    const startKey = dayKeyFromRaw(event.startDate);
    if (!startKey) return [];

    const endKey = dayKeyFromRaw(event.endDate) ?? startKey;
    if (endKey <= startKey) return [startKey];

    const keys = [];
    const [y, mo, d] = startKey.split('-').map(Number);
    const cursor = new Date(y, mo - 1, d);

    for (let i = 0; i < MAX_SPAN_DAYS; i++) {
        const key = dateKey(cursor.getFullYear(), cursor.getMonth(), cursor.getDate());
        keys.push(key);
        if (key >= endKey) break;
        cursor.setDate(cursor.getDate() + 1);
    }
    return keys;
}

export function resolveTypeMeta(eventType) {
    const byNumber = {
        1: { type: 'homework',   color: 'blue' },
        2: { type: 'lesson',     color: 'teal' },
        3: { type: 'lessonQuiz', color: 'red' },
    };
    const byName = { homework: byNumber[1], lesson: byNumber[2], lessonquiz: byNumber[3] };

    if (typeof eventType === 'number') return byNumber[eventType] || byNumber[1];
    if (typeof eventType === 'string') {
        const n = Number(eventType);
        if (!Number.isNaN(n) && byNumber[n]) return byNumber[n];
        return byName[eventType.toLowerCase()] || byNumber[1];
    }
    return byNumber[1];
}

// ─── Public entry point ──────────────────────────────────────────────────────
// Raw API payload → { 'YYYY-MM-DD': Event[] }, each day sorted by start time.
export function buildEventsMap(data) {
    const map = {};
    if (!Array.isArray(data)) return map;

    data.forEach((item) => {
        const event = normalizeEvent(item);
        if (!event || !event.available) return;

        dayKeysCovered(event).forEach((key) => {
            (map[key] ||= []).push(event);
        });
    });

    Object.values(map).forEach((list) => {
        list.sort((a, b) => String(a.startDate).localeCompare(String(b.startDate)));
    });

    return map;
}


// "yyyy-MM-dd HH:mm:ss" → "HH:mm", collapsing to a single time when start == end.
export function timeRange(start, end) {
    const s = start ? start.slice(11, 16) : '';
    const e = end ? end.slice(11, 16) : '';
    if (s && e && e !== s) return `${s} – ${e}`;
    return s;
}
