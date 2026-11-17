/**
 * Created by portiz on 8/21/2018.
 */
import React from "react";
import { withLocalize } from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGlobe } from '@fortawesome/free-solid-svg-icons';

/**
 * Manages the languages selected by the user
 * @param languages The available languages
 * @param activeLanguage The active language
 * @param setActiveLanguage Action when the language is selected
 * @constructor
 */
const LanguageToggle = ({ languages, activeLanguage, setActiveLanguage }) => (
    <div className="text-center">
        <FontAwesomeIcon icon={faGlobe} />
        {languages.map(lang => (
            <span className="language-selector-list" key={lang.code} onClick={() => {setActiveLanguage(lang.code);
                LanguageManager.setLanguage(lang.code);}}>
                    {lang.name}
            </span>

        ))}
    </div>
);

export default withLocalize(LanguageToggle);