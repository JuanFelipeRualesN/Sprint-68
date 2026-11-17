import axios from "./interceptors";
import Settings from "../custom-classes/settings";
import { getAuthHeaders } from "./interceptors";

/**
 * Summarizes the prompt activity of a batch of users (counts + flagged), so the
 * profiles table can show who raised an alert. The alerts column is auxiliary,
 * so the call is silent: a failure must not pop the global error dialog.
 *
 * @param {string[]} users User ids to summarize.
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 */
export const getPromptMonitoringUsersSummary = (users, from, to) => {
  return axios({
    method: "POST",
    url: Settings.getPromptMonitoringUsersSummaryAPI(),
    headers: { "Content-Type": "application/json", ...getAuthHeaders() },
    isSilent: true,
    data: JSON.stringify({ Users: users, From: from ?? null, To: to ?? null }),
  });
};

/**
 * UNREAD alert total for the current viewer across all of their students.
 * Token-scoped and role-aware on the backend (admin = whole platform,
 * teacher/director = their students), so it takes no user list — just the
 * optional date range. Response: { totalUnreadAlerts }. Silent, as it feeds an
 * auxiliary badge.
 *
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 */
export const getPromptMonitoringAlertsUnread = (from, to) => {
  return axios({
    method: "GET",
    url: Settings.getPromptMonitoringAlertsUnreadAPI(from, to),
    headers: { "Content-Type": "application/json", ...getAuthHeaders() },
    isSilent: true,
  });
};

/**
 * Reads one page of a single user's AI tutor prompt history, newest first.
 * Silent for the same reason as the summary call: it feeds an additive block on
 * the profile screen, not the core editing flow.
 *
 * @param {string} userId User whose history is read (required, the shard key).
 * @param {number} [page] Page to read, 1-based.
 * @param {number} [pageSize] How many prompts to return.
 * @param {string} [from] Optional inclusive start of the date range (ISO).
 * @param {string} [to] Optional inclusive end of the date range (ISO).
 */
export const getPromptMonitoringUserHistory = (userId, page = 1, pageSize = 20, from, to) => {
  return axios({
    method: "GET",
    url: Settings.getPromptMonitoringUserHistoryAPI(userId, page, pageSize, from, to),
    headers: { "Content-Type": "application/json", ...getAuthHeaders() },
    isSilent: true,
  });
};
