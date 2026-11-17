/**
 * Data access for the prompt-alerts feature.
 *
 * All knowledge of the prompt-monitoring API shape lives here: the components are
 * presentational and receive already-mapped data. This is the only file to touch
 * if the backend contract changes.
 *
 * Backend contract (aiservicesapi/prompt-monitoring). BaseAPI serializes in
 * PascalCase, while the original standalone service answered in camelCase. 
 * Every read below goes through `pick`, so the same code works against either casing:
 *   POST /users        -> { users: PromptMonitoringUserSummaryResponseModel[] }
 *     summary = { userId, promptCount, flaggedCount, lastPromptAtUtc }
 *   GET  /user-history -> { history: PromptMonitoringUserHistoryResponseModel }
 *     history = { prompts: PromptMonitoringPromptResponseModel[], total, page, pageSize }
 *     prompt  = { id, prompt, response, role, isFlagged, alertReason, createdAtUtc }
 */

import {
  getPromptMonitoringUsersSummary,
  getPromptMonitoringAlertsUnread,
  getPromptMonitoringUserHistory,
} from "../../../api/prompt-monitoring";
import { PROMPT_STATUS, PROMPT_ALERTS_FIELD, PROMPT_UNREAD_FIELD } from "./constants";

// Reads a field regardless of the casing the API used: BaseAPI answers in
// PascalCase, the standalone AI service answered in camelCase
const pick = (source, name) => {
  if (!source) return undefined;
  if (source[name] !== undefined) return source[name];
  return source[name.charAt(0).toUpperCase() + name.slice(1)];
};

/**
 * Maps a stored prompt (PromptMonitoringPromptResponseModel) to the shape the UI
 * components expect: { id, date, subject, text, response, status, reason }.
 *
 * The backend has no subject field, so `subject` is left undefined (the history
 * meta row simply omits it). `reason` is built from AlertCategories, which the
 * backend leaves empty unless the prompt was flagged.
 *
 * @param {Object} dto Raw prompt record from the API.
 * @returns {Object}
 */
export const toUiPrompt = (dto) => ({
  id: pick(dto, "id"),
  date: pick(dto, "createdAtUtc"),
  text: pick(dto, "prompt"),
  response: pick(dto, "response") || "",
  status: pick(dto, "isFlagged") ? PROMPT_STATUS.flagged : PROMPT_STATUS.normal,
  // Human-readable reason of the flag. Empty when the prompt was not flagged.
  reason: pick(dto, "alertReason") || "",
  // Flagged alert the viewer had not read before opening this history. Always
  // false for clean prompts. Turns false on the next visit (marked as read).
  isNew: !!pick(dto, "isNew"),
});

/**
 * Reads one page of a user's tutor prompt history, already mapped for the UI.
 *
 * @param {string} userId User whose history is read.
 * @param {number} [page] Page to read, 1-based.
 * @param {number} [pageSize] How many prompts to return.
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 * @returns {Promise<{prompts: Array, total: number, page: number, pageSize: number}>}
 */
export const fetchTutorPromptHistory = async (userId, page = 1, pageSize = 20, from, to) => {
  const response = await getPromptMonitoringUserHistory(userId, page, pageSize, from, to);
  const history = pick(response.data, "history") || {};

  return {
    prompts: (pick(history, "prompts") || []).map(toUiPrompt),
    total: Number(pick(history, "total")) || 0,
    page: pick(history, "page") || page,
    pageSize: pick(history, "pageSize") || pageSize,
  };
};

/**
 * Fetches the prompt-alert summary for a batch of users.
 *
 * Returns, per user, the total flagged count and the subset the current viewer
 * has not read yet (unreadCount), plus the unread total across every user sent
 * for a global badge. "Read" state is viewer-scoped on the backend and moves
 * automatically when a student's history is opened.
 *
 * Users with no activity in the range are absent from the response (the backend
 * omits them), so they are simply missing from the map, read as zeros.
 *
 * @param {string[]} userIds Users to summarize.
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 * @returns {Promise<{byUser: Object<string, {flaggedCount: number, unreadCount: number}>, totalUnread: number}>}
 */
export const fetchUsersPromptSummary = async (userIds, from, to) => {
  const ids = (userIds || []).filter(Boolean);

  if (ids.length === 0) {
    return { byUser: {}, totalUnread: 0 };
  }

  const response = await getPromptMonitoringUsersSummary(ids, from, to);
  const data = response.data || {};
  const summaries = pick(data, "users") || [];

  const byUser = summaries.reduce((map, summary) => {
    map[pick(summary, "userId")] = {
      flaggedCount: Number(pick(summary, PROMPT_ALERTS_FIELD)) || 0,
      unreadCount: Number(pick(summary, PROMPT_UNREAD_FIELD)) || 0,
    };
    return map;
  }, {});

  return { byUser, totalUnread: Number(pick(data, "totalUnread")) || 0 };
};

/**
 * UNREAD alert total for the current viewer across ALL of their students,
 * independent of the table's pagination. The endpoint is token-scoped and
 * role-aware on the backend (admin = whole platform, teacher/director = their own
 * students), so it takes no user list — just the optional date range — and
 * answers { totalUnreadAlerts }.
 *
 * This backs the global notifications badge, which must reflect unread alerts on
 * ANY page, not only the rows currently rendered. The call rejects (rather than
 * returning a false 0) when the backend cannot resolve the viewer's students, so
 * callers can keep the last known value instead of painting a misleading zero.
 *
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 * @returns {Promise<number>} Total unread alerts for the current viewer.
 */
export const fetchUnreadAlertsTotal = async (from, to) => {
  const response = await getPromptMonitoringAlertsUnread(from, to);
  return Number(pick(response.data || {}, "totalUnreadAlerts")) || 0;
};

/**
 * Convenience wrapper for a single user's flagged count (the profile banner).
 *
 * @param {string} userId User to summarize.
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 * @returns {Promise<number>}
 */
export const fetchUserFlaggedCount = async (userId, from, to) => {
  const { byUser } = await fetchUsersPromptSummary([userId], from, to);
  return (byUser[userId] || {}).flaggedCount || 0;
};
