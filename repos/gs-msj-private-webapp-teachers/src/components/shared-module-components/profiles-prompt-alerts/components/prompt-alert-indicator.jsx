import React from "react";
import AlertIcon from "./alert-icon";
import "./prompt-alert-indicator.css";

/**
 * Red pill showing how many prompt alerts the current viewer has NOT read yet
 * for this user, with a warning icon and a "new" word. Renders nothing when
 * there are none (unreadCount <= 0), so the cell only calls attention to
 * students with alerts pending review (reading clears it).
 *
 * Kept i18n-free (the only fully portable file of the module): the caller passes
 * the already-localized word via `label`.
 *
 * @param {number} unreadCount Unread alerts for the current viewer.
 * @param {string} [label] Localized word next to the count (e.g. "Nuevo").
 * @param {string} [title] Optional tooltip / accessible label.
 */
const PromptAlertIndicator = ({ unreadCount = 0, label = "", title }) => {
  const unread = Number(unreadCount) || 0;

  if (unread <= 0) {
    return null;
  }

  return (
    <span className="prompt-alert-indicator" title={title} aria-label={title}>
      <AlertIcon /> {unread} {label}
    </span>
  );
};

export default PromptAlertIndicator;
