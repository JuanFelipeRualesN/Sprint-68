import React, { useState } from "react";
import { useTranslation } from "react-i18next";
import PaginationCustom from "../../datatables/pagination";
import AlertIcon from "./alert-icon";
import { PROMPT_STATUS } from "../constants";
import "./tutor-prompt-history.css";

/**
 * Formats a prompt date for display.
 * @param {string} value ISO date string.
 * @param {string} langCode e.g. "es" | "en"
 * @returns {string}
 */
const formatPromptDate = (value, langCode) => {
  if (!value) {
    return "";
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return "";
  }

  return new Intl.DateTimeFormat(langCode === "en" ? "en-US" : "es-PR", {
    day: "numeric",
    month: "short",
    year: "numeric",
    hour: "numeric",
    minute: "2-digit",
  }).format(date);
};

/**
 * Collapsible log of the prompts a student sent to the AI tutor, highlighting
 * the ones the tutor flagged.
 *
 * Presentational: it renders the page it is given and asks the parent for a new
 * page via onPageChange; the parent owns the data fetching and page state.
 *
 * @param {Array} prompts Prompts of the current page. Each item is
 *        { id, date, subject, text, status: "normal" | "flagged", reason }.
 * @param {number} [page] Current page, 0-based.
 * @param {number} [totalPages] Total number of pages.
 * @param {(page: number) => void} [onPageChange] Called with the next 0-based page.
 * @param {boolean} [loading] Whether a page is currently being fetched.
 * @param {string} [from] Current start of the date range filter (YYYY-MM-DD or "").
 * @param {string} [to] Current end of the date range filter (YYYY-MM-DD or "").
 * @param {(range: {from: string, to: string}) => void} [onDateRangeChange] Called
 *        when either bound of the date range filter changes.
 * @param {Array} [periods] Academic years to offer as a quick date-range preset.
 *        Each item is { id, from, to, label } with from/to as YYYY-MM-DD and
 *        label the text shown in the dropdown.
 * @param {boolean} [defaultOpen] Whether the list starts expanded.
 */
const TutorPromptHistory = ({
  prompts = [],
  page = 0,
  totalPages = 1,
  onPageChange,
  loading = false,
  from = "",
  to = "",
  onDateRangeChange,
  periods = [],
  defaultOpen = true,
}) => {
  const [open, setOpen] = useState(defaultOpen);
  const { t, i18n } = useTranslation(["users"]);
  const language = (i18n.language || "es").split("-")[0];

  // The dropdown reflects the current range only when it matches a period
  // exactly; a manual date edit leaves it on the "all periods" option.
  const selectedPeriodId =
    (periods.find((period) => period.from === from && period.to === to) || {}).id || "";

  return (
    <section className="tutor-prompt-history">
      <header className="tutor-prompt-history__head">
        <div>
          <span className="tutor-prompt-history__eyebrow">
            {t("prompt-alerts.history-eyebrow")}
          </span>
          <h2 className="tutor-prompt-history__title">{t("prompt-alerts.history-title")}</h2>
          <p className="tutor-prompt-history__description">
            {t("prompt-alerts.history-description")}
          </p>
        </div>
        <button
          type="button"
          className="tutor-prompt-history__toggle"
          aria-expanded={open}
          onClick={() => setOpen((value) => !value)}
        >
          {t(open ? "prompt-alerts.history-hide" : "prompt-alerts.history-show")}
        </button>
      </header>

      {open && (
        <>
          <div className="tutor-prompt-history__filters">
            {periods.length > 0 && (
              <label className="tutor-prompt-history__filter">
                <span>{t("prompt-alerts.filter-period")}</span>
                <select
                  value={selectedPeriodId}
                  onChange={(event) => {
                    if (!onDateRangeChange) {
                      return;
                    }
                    const period = periods.find((item) => item.id === event.target.value);
                    onDateRangeChange(
                      period ? { from: period.from, to: period.to } : { from: "", to: "" }
                    );
                  }}
                >
                  <option value="">{t("prompt-alerts.filter-period-all")}</option>
                  {periods.map((period) => (
                    <option key={period.id} value={period.id}>
                      {period.label}
                    </option>
                  ))}
                </select>
              </label>
            )}
            <label className="tutor-prompt-history__filter">
              <span>{t("prompt-alerts.filter-from")}</span>
              <input
                type="date"
                value={from}
                max={to || undefined}
                onChange={(event) =>
                  onDateRangeChange && onDateRangeChange({ from: event.target.value, to })
                }
              />
            </label>
            <label className="tutor-prompt-history__filter">
              <span>{t("prompt-alerts.filter-to")}</span>
              <input
                type="date"
                value={to}
                min={from || undefined}
                onChange={(event) =>
                  onDateRangeChange && onDateRangeChange({ from, to: event.target.value })
                }
              />
            </label>
            {(from || to) && (
              <button
                type="button"
                className="tutor-prompt-history__filter-clear"
                onClick={() => onDateRangeChange && onDateRangeChange({ from: "", to: "" })}
              >
                {t("prompt-alerts.filter-clear")}
              </button>
            )}
          </div>

          <div className={`tutor-prompt-history__list${loading ? " tutor-prompt-history__list--loading" : ""}`}>
          {prompts.map((prompt) => {
            const flagged = prompt.status === PROMPT_STATUS.flagged;

            return (
              <article
                key={prompt.id}
                className={`tutor-prompt${flagged ? " tutor-prompt--flagged" : ""}${
                  prompt.isNew ? " tutor-prompt--new" : ""
                }`}
              >
                <div className="tutor-prompt__meta">
                  {prompt.isNew && (
                    <span className="tutor-prompt__new">{t("prompt-alerts.new-badge")}</span>
                  )}
                  <span>{formatPromptDate(prompt.date, language)}</span>
                  <span>{prompt.subject}</span>
                  {flagged ? (
                    <strong className="tutor-prompt__badge tutor-prompt__badge--flagged">
                      <AlertIcon size={15} /> {t("prompt-alerts.prompt-flagged")}
                    </strong>
                  ) : (
                    <em className="tutor-prompt__badge">{t("prompt-alerts.prompt-allowed")}</em>
                  )}
                </div>
                <blockquote className="tutor-prompt__text">{prompt.text}</blockquote>
                {prompt.reason && (
                  <p className="tutor-prompt__reason">
                    <strong>{t("prompt-alerts.prompt-reason")}</strong> {prompt.reason}
                  </p>
                )}
              </article>
            );
          })}

          {prompts.length === 0 && (
            <div className="tutor-prompt-history__empty">
              {t("prompt-alerts.history-empty")}
            </div>
          )}

          <div className="tutor-prompt-history__pager">
            <PaginationCustom
              totalPages={totalPages}
              currentPage={page}
              updateParent={(nextPage) => onPageChange && onPageChange(nextPage)}
              dapper={true}
            />
          </div>
          </div>
        </>
      )}
    </section>
  );
};

export default TutorPromptHistory;
