import { useContext, useEffect } from 'react';
import { LocalizeContext } from 'react-localize-redux';
import Settings from '../custom-classes/settings';
import { renderToStaticMarkup } from 'react-dom/server';
import LanguageManager from '../custom-classes/language-manager';

// Custom hook to initialize translations based on the current version
const useVersionedTranslations = () => {
    const { initialize, addTranslation } = useContext(LocalizeContext);

    useEffect(() => {

        const version = Settings.PROJECT_VERSION;

        initialize({
            languages: Settings.LANGUAGES,
            options: {
                renderToStaticMarkup,
                defaultLanguage: LanguageManager.getLanguage()
            }
        });
        import(`../translations/${version}/global.json`)
            .then((translations) => {
                // Set translations in LanguageManager
                LanguageManager.setTranslations(translations);

                addTranslation(translations);
            })
            .catch((error) => {
                console.warn(`Version ${version} translations not found`, error);
            });
    }, []);
};

export default useVersionedTranslations;