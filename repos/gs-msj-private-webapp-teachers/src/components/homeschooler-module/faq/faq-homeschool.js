import React  from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faQuestionCircle} from '@fortawesome/free-solid-svg-icons';

const FaqHomeschool = () => {
    const data = {
        title: <div className='faq-title'>
            <FontAwesomeIcon icon={faQuestionCircle}/>{' '}
            {LanguageManager.getTranslation("faq.homeschool-client-title", Settings.LANGUAGES)}
        </div>,
        rows: [
            {
                title: LanguageManager.getTranslation("faq.homeschool-subscription-cancellation", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.homeschool-subscription-cancellation-exp", Settings.LANGUAGES),
            }
        ]
    };

    return (
        <Faq data={data}/>
    )
};
export default FaqHomeschool;