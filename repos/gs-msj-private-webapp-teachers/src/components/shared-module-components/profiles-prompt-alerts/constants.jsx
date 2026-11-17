/**
 * Configuration for the prompt-alerts feature.
 *
 * Kept in one place so that, when this folder is exported to another project,
 * the only thing that may need tweaking (the backend field name) is centralized
 * and dependency-free. The visible strings live in the "users" translation
 * namespace, under the "prompt-alerts" key (public/es|en/users.json).
 */

/**
 * Field on each user summary (POST /api/prompt-monitoring/users) that carries the
 * number of flagged prompts. A value greater than zero drives the alert
 * indicator in the profiles table.
 *
 * Note the prompt-monitoring API (GSAIServicesAPI) serializes in camelCase,
 * unlike the profiles API. If the backend renames the field, change it here only.
 */
export const PROMPT_ALERTS_FIELD = "flaggedCount";

/**
 * Field on each user summary that carries the number of flagged prompts the
 * current viewer has NOT read yet (a subset of flaggedCount). Drives the "new"
 * row indicator; opening a student's history marks their alerts as read, so this
 * drops to 0 on the next summary load. Viewer-scoped on the backend.
 */
export const PROMPT_UNREAD_FIELD = "unreadAlertCount";

/**
 * Status of a single tutor prompt.
 */
export const PROMPT_STATUS = {
  normal: "normal",
  flagged: "flagged",
};

/**
 * Counts how many prompts of a list are flagged.
 * @param {Array} prompts List of tutor prompts.
 * @returns {number}
 */
export const countFlaggedPrompts = (prompts) =>
  (prompts || []).filter((prompt) => prompt.status === PROMPT_STATUS.flagged).length;
