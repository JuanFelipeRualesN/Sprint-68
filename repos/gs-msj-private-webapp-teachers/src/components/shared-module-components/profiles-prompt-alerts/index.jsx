/**
 * Public API of the profiles-prompt-alerts feature.
 *
 * Import from the folder root, e.g.:
 *   import { ProfilesDataTableWithAlerts } from "../shared-module-components/profiles-prompt-alerts";
 */
export { default as ProfilesDataTableWithAlerts } from "./components/profiles-data-table-with-alerts";
export { default as PromptAlertIndicator } from "./components/prompt-alert-indicator";
export { default as PromptSafetyAlert } from "./components/prompt-safety-alert";
export { default as ProfileSummaryCard } from "./components/profile-summary-card";
export { default as TutorPromptHistory } from "./components/tutor-prompt-history";
export { default as AlertIcon } from "./components/alert-icon";
export { usePromptMonitoring } from "./use-prompt-monitoring";
export {
  toUiPrompt,
  fetchTutorPromptHistory,
  fetchUsersPromptSummary,
  fetchUnreadAlertsTotal,
  fetchUserFlaggedCount,
} from "./prompt-monitoring-data";
export {
  PROMPT_ALERTS_FIELD,
  PROMPT_UNREAD_FIELD,
  PROMPT_STATUS,
  countFlaggedPrompts,
} from "./constants";
