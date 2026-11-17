import React from "react";
import { withLocalize } from "react-localize-redux";
import "../style.css";

// The english and spanish variants of a story are read and scored independently,
// so the metrics endpoints return one section per language
export const STORY_LANGUAGES = [
    { key: "en", labelKey: "dashboards.english" },
    { key: "es", labelKey: "dashboards.spanish" },
];

// Picks the section of the selected language from a metrics response split by story language
export const getLanguageSection = (metrics, language) => {
    return language === "es" ? metrics?.Spanish : metrics?.English;
};

export const getLanguageLabelKey = (language) => {
    return STORY_LANGUAGES.find(storyLanguage => storyLanguage.key === language)?.labelKey;
};

// Switches the story language the charts and metrics are showing
const LanguageSwitch = ({ translate, language = "en", onChange = () => {} }) => (
    <div className="language-switch-container">
        <span style={{ fontSize: 13, color: "#64748b", whiteSpace: "nowrap" }}>
            {translate("dashboards.story-language")}
        </span>
        <div className="period-selector-tabs">
            {STORY_LANGUAGES.map(({ key, labelKey }) => (
                <button
                    key={key}
                    type="button"
                    onClick={() => onChange(key)}
                    className={`period-tab${language === key ? " active" : ""}`}
                >
                    {translate(labelKey)}
                </button>
            ))}
        </div>
    </div>
);

export default withLocalize(LanguageSwitch);
