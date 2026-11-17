import React  from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faQuestionCircle} from '@fortawesome/free-solid-svg-icons';

const FaqParents = () => {
    const data = {        
        title: <div className='faq-title'><FontAwesomeIcon icon={faQuestionCircle}/>{' '}{LanguageManager.getTranslation("faq.title-parents", Settings.LANGUAGES)}</div>,
        rows: [
            {
                title: LanguageManager.getTranslation("faq.see-student-results-parents", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.see-student-results-parents-exp", Settings.LANGUAGES),
            },
            {
                title: LanguageManager.getTranslation("faq.why-i-dont-see-practice-results", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.why-i-dont-see-practice-results-exp", Settings.LANGUAGES),
            },
            {
                title: LanguageManager.getTranslation("faq.how-do-i-access-to-parents-area", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.how-do-i-access-to-parents-area-exp", Settings.LANGUAGES),
            },
            {
                title: LanguageManager.getTranslation("faq.how-do-i-update-payment-option", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.how-do-i-update-payment-option-exp", Settings.LANGUAGES),
            },
            {
                title: LanguageManager.getTranslation("faq.how-do-i-add-a-new-plan", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.how-do-i-add-a-new-plan-exp", Settings.LANGUAGES),
            },
            {
                title: LanguageManager.getTranslation("faq.how-to-cancel-a-plan", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.how-to-cancel-a-plan-exp", Settings.LANGUAGES),
            }
    
        ]
    };

    return (
        <Faq data={data}/>
    )
};
export default FaqParents;