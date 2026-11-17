import { fetchSubjects, fetchLevels } from '../../../../../../api/ai-module'
import { GRADES as FALLBACK_GRADES, SUBJECTS as FALLBACK_SUBJECTS } from './tools-config-base.js';

// --- Mappers: API model -> { label, value } option ---

const mapSubject = (s) => ({
    label: s.name ?? s.Name,   // handle either JSON casing
    value: s.code ?? s.Code,
});

const mapLevel = (l) => ({
    label: l.name ?? l.Name,
    value: l.code ?? l.Code,
});

// --- Cached loaders (fetch once per session) ---

let subjectsCache = null;
let levelsCache = null;

export async function getSubjectOptions() {
    if (subjectsCache) return subjectsCache;
    try {
        const { data } = await fetchSubjects();
        const list = Array.isArray(data) ? data : data?.items ?? [];
        subjectsCache = list.map(mapSubject).filter(o => o.label && o.value);
        if (!subjectsCache.length) subjectsCache = FALLBACK_SUBJECTS;
    } catch (err) {
        console.error('Failed to fetch subjects, using fallback:', err);
        subjectsCache = FALLBACK_SUBJECTS;
    }
    return subjectsCache;
}

export async function getLevelOptions() {
    if (levelsCache) return levelsCache;
    try {
        const { data } = await fetchLevels();
        const list = Array.isArray(data) ? data : data?.items ?? [];
        levelsCache = list.map(mapLevel).filter(o => o.label && o.value);
        if (!levelsCache.length) levelsCache = FALLBACK_GRADES;
    } catch (err) {
        console.error('Failed to fetch levels, using fallback:', err);
        levelsCache = FALLBACK_GRADES;
    }
    return levelsCache;
}

// Optional: call after a language switch so labels re-fetch in the new language
export function invalidateOptionsCache() {
    subjectsCache = null;
    levelsCache = null;
}