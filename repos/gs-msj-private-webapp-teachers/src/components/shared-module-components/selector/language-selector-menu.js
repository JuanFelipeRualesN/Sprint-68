/**
 * Created by portiz on 2/20/19.
 */
import React from "react";
import { withLocalize } from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
import {DropdownItem } from 'reactstrap';
/**
 * Manages the languages selected by the user
 * @param languages The available languages
 * @param activeLanguage The active language
 * @param setActiveLanguage Action when the language is selected
 * @constructor
 */
const LanguageToggleMenu = ({ languages, activeLanguage, setActiveLanguage }) => (
    <div>
        {languages.map(lang => (
            <DropdownItem key={lang.code} onClick={() => {setActiveLanguage(lang.code);
                LanguageManager.setLanguage(lang.code);}}>
                {lang.name}
            </DropdownItem>

        ))}
    </div>
);

export default withLocalize(LanguageToggleMenu);