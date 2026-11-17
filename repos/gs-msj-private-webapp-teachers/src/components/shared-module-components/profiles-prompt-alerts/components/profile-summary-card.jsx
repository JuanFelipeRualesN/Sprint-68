import React from "react";
import { useTranslation } from "react-i18next";
import "./profile-summary-card.css";

/**
 * Read-only summary of the selected profile. Every value is received already
 * formatted so the component stays presentational and reusable (the profiles
 * table, for instance, hands over the last session as a formatted string).
 *
 * @param {string} username Username shown as the card title.
 * @param {string} [recordNumber] School record number.
 * @param {string} [invitationCode] Invitation code of the profile.
 * @param {string} [lastSession] Last session, already formatted for display.
 */
const ProfileSummaryCard = ({
  username,
  recordNumber,
  invitationCode,
  lastSession,
}) => {
  const { t } = useTranslation(["users"]);

  const rows = [
    { label: t("prompt-alerts.summary-record"), value: recordNumber },
    { label: t("prompt-alerts.summary-invitation-code"), value: invitationCode },
    { label: t("prompt-alerts.summary-last-session"), value: lastSession },
  ];

  return (
    <aside className="profile-summary-card">
      <span className="profile-summary-card__eyebrow">
        {t("prompt-alerts.summary-eyebrow")}
      </span>
      <h3 className="profile-summary-card__title">{username}</h3>
      <dl className="profile-summary-card__list">
        {rows.map((row) => (
          <div className="profile-summary-card__row" key={row.label}>
            <dt>{row.label}</dt>
            <dd>{row.value || "—"}</dd>
          </div>
        ))}
      </dl>
    </aside>
  );
};

export default ProfileSummaryCard;
