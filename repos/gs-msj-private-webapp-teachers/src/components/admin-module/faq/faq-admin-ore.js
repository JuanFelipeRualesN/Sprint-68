import React from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faQuestionCircle } from '@fortawesome/free-solid-svg-icons';

const FaqAdminOre = () => {
    const data = {
        title: <div className='faq-title'><FontAwesomeIcon icon={faQuestionCircle} />{' '}Frequently asked questions (Administrator Ore)</div>,
        rows: [
            {
                title: LanguageManager.getTranslation("faq.admin-ore-info-user", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-info-user-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-connection-report", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-connection-report-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-academic-report", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-academic-report-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-receive-messages", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-receive-messages-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-cannot-see-messages", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-cannot-see-messages-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-cannot-sent-messages", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-cannot-sent-messages-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.admin-ore-reports", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.admin-ore-reports-exp", Settings.LANGUAGES)
            }
        ]
    };

    return (
        <Faq data={data} />
    );
};

export default FaqAdminOre;
