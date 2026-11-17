/**
 * Created by portiz on 8/22/2018.
 */
import Settings from '../custom-classes/settings';

/**
 * To handle the languages
 */
class LanguageManager {
    static loadedTranslations = null;

    static setTranslations(translations) {
        this.loadedTranslations = translations;
    }

    // Saves the language in the local storage
    static setLanguage(langCode){
        localStorage.setItem('i18nextLng', langCode);

        // The migrated views translate through i18next, which keeps its own in-memory language.
        try {
            const i18n = require('../i18n').default;
            if (i18n && i18n.language !== langCode) {
                i18n.changeLanguage(langCode);
            }
        } catch (e) {
        }
    }

    // Returns the language form the local storage
    static getLanguage() {
        try {
            const langCode = localStorage.getItem('i18nextLng');
            if (langCode && langCode.length > 0) {
                return langCode;
            }
        } catch (error) {
            console.warn('localStorage is not available:', error);
        }

        return Settings.DEFAULT_LANGUAGE;
    }

    static getTranslation(key, languages){
       try {
           // Use the loaded translations
           const translateText = this.loadedTranslations || {};
           
           let index = key.indexOf(".");  // Gets the first index where a space occours
           let part = key.substr(0, index); // Gets the first part
           let subpart = key.substr(index + 1);  // Gets the text part

           // Now get the language
           let langCode = this.getLanguage("es");

           // Then find the language index by language code
           let indexLanguage = 0;
           for (let i = 0; i < languages.length; i++) {
               if (languages[i].code === langCode) {
                   indexLanguage = i;
                   break;
               }
           }
           // We need the index to find the index in the translation
           return translateText[part][subpart][indexLanguage];
       }
       catch (e) {
           return "";
       }
    }

    static getTranslationByLangCode(key, languages, langCode){
        try {
        const translateText = this.loadedTranslations || {};
        let index = key.indexOf(".");  // Gets the first index where a space occours
        let part = key.substr(0, index); // Gets the first part
        let subpart = key.substr(index + 1);  // Gets the text part

        // Then find the language index by language code
        let indexLanguage = 0;
        for(let i=0; i < languages.length;i++)
        {
            if(languages[i].code === langCode) {
                indexLanguage = i;
                break;
            }
            }
        // We need the index to find the index in the translation
        return translateText[part][subpart][indexLanguage];
        }
        catch (e) {
            console.error('Translation error:', e);
            return "";
        }
    }
}

export default LanguageManager;