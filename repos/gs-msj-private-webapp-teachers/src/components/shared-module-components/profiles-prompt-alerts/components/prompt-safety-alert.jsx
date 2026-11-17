import React from "react";
import { useTranslation } from "react-i18next";
import AlertIcon from "./alert-icon";
import "./prompt-safety-alert.css";

/**
 * Banner shown on top of a student profile when the AI tutor flagged at least
 * one of their prompts. Renders nothing when there is nothing to report, so the
 * caller can mount it unconditionally.
 *
 * @param {number} count How many prompts were flagged.
 */
const PromptSafetyAlert = ({ count = 0 }) => {
  const { t } = useTranslation(["users"]);

  const total = Number(count) || 0;

  if (total <= 0) {
    return null;
  }

  const alertWord = t(
    total === 1 ? "prompt-alerts.alert-singular" : "prompt-alerts.alert-plural"
  );

  return (
    <section className="prompt-safety-alert" role="alert">
      <div className="prompt-safety-alert__icon">
        <AlertIcon />
      </div>
      <div className="prompt-safety-alert__body">
        <span className="prompt-safety-alert__eyebrow">
          {t("prompt-alerts.safety-eyebrow")}
        </span>
        <h2 className="prompt-safety-alert__title">{t("prompt-alerts.safety-title")}</h2>
        <p className="prompt-safety-alert__text">{t("prompt-alerts.safety-description")}</p>
      </div>
      <span className="prompt-safety-alert__count">
        {total} {alertWord}
      </span>
    </section>
  );
};

export default PromptSafetyAlert;
