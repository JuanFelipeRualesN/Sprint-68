import React  from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faQuestionCircle} from '@fortawesome/free-solid-svg-icons';

const FaqPrincipal = () => {
    const data = {        
        title: <div className='faq-title'><FontAwesomeIcon icon={faQuestionCircle}/>{' '}{LanguageManager.getTranslation("faq.faq-title-principal", Settings.LANGUAGES)}</div>,
        rows: [
            {
                title:  LanguageManager.getTranslation("faq.director-password", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-password-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-transfer-school", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-transfer-school-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-connections-report", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-connections-report-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-academics-report", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-academics-report-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-content", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-content-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-usage-teachers", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-usage-teachers-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-usage-students", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-usage-students-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-progress", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-progress-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-student-progress", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-student-progress-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-group-progress", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-group-progress-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-sie", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-sie-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-messages-students", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-messages-students-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-messages-receives", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-messages-receives-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-messages-sent", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-messages-sent-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-plan-instructionals-calendar", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-plan-instructionals-calendar-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-content-academic", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-content-academic-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.director-reports", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.director-reports-exp", Settings.LANGUAGES)
            }
        ]
    };

    return (
        <Faq data={data}/>
    )
};
export default FaqPrincipal;