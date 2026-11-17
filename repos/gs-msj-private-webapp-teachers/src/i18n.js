import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageManager from "./custom-classes/language-manager";
import loadVersionedTranslations from "./translations/i18next/load-versioned-translations";

import enAiTools from "./translations/i18next/en/ai-tools.json";
import enMyDocuments from "./translations/i18next/en/my-documents.json";
import enToolModal from "./translations/i18next/en/tool-modal.json";
import enToolsConfig from "./translations/i18next/en/tools-config.json";
import enNavigation from "./translations/i18next/en/navigation.json";
import enCommon from "./translations/i18next/en/common.json";
import enWelcome from "./translations/i18next/en/welcome.json";
import enRightColumn from "./translations/i18next/en/right-column.json";
import enCalendarEvents from "./translations/i18next/en/calendar-events.json";
import enAthenasLessonPicker from "./translations/i18next/en/athenas-lesson-picker.json";
import enAthenasCodePicker from "./translations/i18next/en/athenas-code-picker.json";
import enExportUtils from "./translations/i18next/en/export-utils.json";
import enAiAssistant from "./translations/i18next/en/ai-assistant.json";
import enEvents from "./translations/i18next/en/events.json";
import enProfile from "./translations/i18next/en/profile.json";
import enQuiz from "./translations/i18next/en/quiz.json";
import enUsers from "./translations/i18next/en/users.json";
import enSubscription from "./translations/i18next/en/subscription.json";

import esAiTools from "./translations/i18next/es/ai-tools.json";
import esMyDocuments from "./translations/i18next/es/my-documents.json";
import esToolModal from "./translations/i18next/es/tool-modal.json";
import esToolsConfig from "./translations/i18next/es/tools-config.json";
import esNavigation from "./translations/i18next/es/navigation.json";
import esCommon from "./translations/i18next/es/common.json";
import esWelcome from "./translations/i18next/es/welcome.json";
import esRightColumn from "./translations/i18next/es/right-column.json";
import esCalendarEvents from "./translations/i18next/es/calendar-events.json";
import esAthenasLessonPicker from "./translations/i18next/es/athenas-lesson-picker.json";
import esAthenasCodePicker from "./translations/i18next/es/athenas-code-picker.json";
import esExportUtils from "./translations/i18next/es/export-utils.json";
import esAiAssistant from "./translations/i18next/es/ai-assistant.json";
import esEvents from "./translations/i18next/es/events.json";
import esProfile from "./translations/i18next/es/profile.json";
import esQuiz from "./translations/i18next/es/quiz.json";
import esUsers from "./translations/i18next/es/users.json";
import esSubscription from "./translations/i18next/es/subscription.json";

/**
 * i18next setup for the views migrated from MED
 *
 * The rest of this app translates through react-localize-redux and a single
 * global.json per project version; that is untouched. These modules were written
 * against i18next's namespaced keys, so they keep their own catalogue rather
 * than being rewritten
 *
 * Language follows LanguageManager, which persists to the `i18nextLng` key —
 * the same one i18next uses by convention — so both systems stay in step.
 */
i18n
  .use(initReactI18next)
  .init({
    lng: LanguageManager.getLanguage(),
    fallbackLng: "es",
    interpolation: {
      escapeValue: false,
    },
    resources: {
      en: {
        "ai-tools": enAiTools,
        "my-documents": enMyDocuments,
        "tool-modal": enToolModal,
        "tools-config": enToolsConfig,
        navigation: enNavigation,
        common: enCommon,
        welcome: enWelcome,
        "right-column": enRightColumn,
        "calendar-events": enCalendarEvents,
        "athenas-lesson-picker": enAthenasLessonPicker,
        "athenas-code-picker": enAthenasCodePicker,
        "export-utils": enExportUtils,
        "ai-assistant": enAiAssistant,
        events: enEvents,
        profile: enProfile,
        quiz: enQuiz,
        users: enUsers,
        subscription: enSubscription,
      },
      es: {
        "ai-tools": esAiTools,
        "my-documents": esMyDocuments,
        "tool-modal": esToolModal,
        "tools-config": esToolsConfig,
        navigation: esNavigation,
        common: esCommon,
        welcome: esWelcome,
        "right-column": esRightColumn,
        "calendar-events": esCalendarEvents,
        "athenas-lesson-picker": esAthenasLessonPicker,
        "athenas-code-picker": esAthenasCodePicker,
        "export-utils": esExportUtils,
        "ai-assistant": esAiAssistant,
        events: esEvents,
        profile: esProfile,
        quiz: esQuiz,
        users: esUsers,
        subscription: esSubscription,
      },
    },
  });

loadVersionedTranslations(i18n);

export default i18n;
