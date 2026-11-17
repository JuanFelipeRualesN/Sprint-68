import React  from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faQuestionCircle} from '@fortawesome/free-solid-svg-icons';

const FaqAdmin = () => {
    const data = {        
        title: <div className='faq-title'><FontAwesomeIcon icon={faQuestionCircle}/>{' '}{LanguageManager.getTranslation("faq.faq-title-admin", Settings.LANGUAGES)}</div>,
        rows: [
            {
                title:  LanguageManager.getTranslation("faq.admin-update-lessons", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-update-lessons-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-add-lessons", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-add-lessons-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-add-edit-courses", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-add-edit-courses-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-school-info", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-school-info-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-add-users", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-add-users-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-delete-users", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-delete-users-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-transfer-users", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-transfer-users-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-edit-roles", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-edit-roles-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-connection-reports", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-connection-reports-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.admin-academics-reports", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.admin-academics-reports-exp", Settings.LANGUAGES)
            }
        ]
    };

    return (
        <Faq data={data}/>
    )
};
export default FaqAdmin;