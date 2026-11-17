import LanguageManager from "../../../../../../../custom-classes/language-manager";

/**
 * To append the language for the instruction.
 * @param prompt The prompt details.
 * @returns {*|string} Returns the instruction.
 */
export function appendLanguageInstruction(prompt) {
    const LANGUAGE_NAMES = {
        en: "English",
        es: "Spanish",
    };
    let languageName = LANGUAGE_NAMES[LanguageManager.getLanguage()] ?? "English";
    if (!prompt?.trim()) return prompt;
    return `${prompt}\n\nRespond in ${languageName}.`;
}
