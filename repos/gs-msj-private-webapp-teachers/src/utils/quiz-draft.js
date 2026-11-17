/**
 * localStorage draft utilities for the Quiz creator.
 *
 * Two independent namespaces are maintained per quiz:
 *
 *   DRAFT  (quiz_draft_*)
 *     Write-through buffer updated on every form change (1 s debounce).
 *     Used by onSubmit as the authoritative data source to avoid react-hook-form
 *     internal state inconsistencies (useFieldArray + setValue desyncs).
 *     Cleared after a successful server save and on back-navigation.
 *
 *   SAVED  (quiz_saved_*)
 *     Immutable snapshot of the form state at the moment of the last confirmed
 *     server save (or the initial server load). Used exclusively for change
 *     detection via hasUnsavedChanges(). Never modified by user edits.
 *
 * Key scheme for both namespaces:
 *   - Existing quiz  → ..._<quizId>
 *   - New quiz       → ..._new_<sessionId>   (session-scoped to prevent key
 *                       collisions when two tabs create a quiz simultaneously)
 */

// One random ID per browser tab, persisted for the tab's lifetime via sessionStorage.
const SESSION_ID = sessionStorage.getItem('__qsid') || (() => {
  const id = Math.random().toString(36).slice(2);
  sessionStorage.setItem('__qsid', id);
  return id;
})();

/** Returns the localStorage key for a given quiz ID (or the session-scoped new-quiz key). */
export const getDraftKey = (id) => `quiz_draft_${id || `new_${SESSION_ID}`}`;

/**
 * Persists the full form values to localStorage.
 * Silently ignores quota errors or unavailable storage.
 */
export const saveDraft = (values) => {
  try {
    localStorage.setItem(getDraftKey(values.QuizModel?.Id), JSON.stringify(values));
  } catch {}
};

/**
 * Retrieves and parses the draft for a given quiz ID.
 * Returns null when no draft exists or when parsing fails.
 */
export const getDraft = (id) => {
  try {
    const raw = localStorage.getItem(getDraftKey(id));
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
};

/**
 * Removes the draft for a given quiz ID from localStorage.
 * Called after a successful server save and on back-navigation in edit mode.
 */
export const clearDraft = (id) => {
  try {
    localStorage.removeItem(getDraftKey(id));
  } catch {}
};

// ─── Last-saved snapshot (change detection) ──────────────────────────────────

const getSavedKey = (id) => `quiz_saved_${id || `new_${SESSION_ID}`}`;

/**
 * Stores a snapshot of the form values as the "last known saved state".
 * Should be called:
 *   1. Right after the initial server load (sets the baseline).
 *   2. After every successful server save (advances the baseline).
 */
export const saveLastSaved = (values) => {
  try {
    localStorage.setItem(getSavedKey(values.QuizModel?.Id), JSON.stringify(values));
  } catch {}
};

/**
 * Retrieves the last-saved snapshot for a given quiz ID.
 * Returns null when no snapshot exists or when parsing fails.
 */
export const getLastSaved = (id) => {
  try {
    const raw = localStorage.getItem(getSavedKey(id));
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
};

/**
 * Removes the last-saved snapshot.
 * Called on back-navigation so stale baselines do not persist across sessions.
 */
export const clearLastSaved = (id) => {
  try {
    localStorage.removeItem(getSavedKey(id));
  } catch {}
};

/**
 * Produces a deterministic JSON string by recursively sorting object keys.
 * Needed because JSON.stringify does not guarantee key order across objects
 * that were built through different code paths (initial load vs. user edits).
 */
const stableJSON = (val) => {
  if (val === null || typeof val !== 'object') return JSON.stringify(val);
  if (Array.isArray(val)) return `[${val.map(stableJSON).join(',')}]`;
  return `{${Object.keys(val).sort().map(k => `${JSON.stringify(k)}:${stableJSON(val[k])}`).join(',')}}`;
};

/**
 * Removes every quiz draft and saved-snapshot entry from localStorage.
 * Matches any key that starts with "quiz_draft_" or "quiz_saved_",
 * regardless of quiz ID or session suffix.
 */
export const clearAllDrafts = () => {
  try {
    const keys = Object.keys(localStorage).filter(
      k => k.startsWith('quiz_draft_') || k.startsWith('quiz_saved_')
    );
    keys.forEach(k => localStorage.removeItem(k));
  } catch {}
};

/**
 * Returns true when the current draft differs from the last-saved snapshot,
 * meaning the user has edits that have not yet been persisted to the server.
 *
 * Edge cases:
 *   - No draft and no saved baseline → false  (nothing has happened)
 *   - Draft exists but no baseline  → true    (new quiz, never saved)
 *   - No draft but baseline exists  → false   (just saved, draft was cleared)
 */
export const hasUnsavedChanges = (id) => {
  const draft     = getDraft(id);
  const lastSaved = getLastSaved(id);

  if (!draft && !lastSaved) return false;
  if (!lastSaved)           return true;
  if (!draft)               return false;

  return stableJSON(draft) !== stableJSON(lastSaved);
};
