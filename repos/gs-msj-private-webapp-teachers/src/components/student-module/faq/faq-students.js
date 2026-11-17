import React  from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

const FaqStudents = () => {
    const data = {
        title:  LanguageManager.getTranslation("faq.title-student", Settings.LANGUAGES),
        rows: [
            {
                title:  LanguageManager.getTranslation("faq.student-password-forgot", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-password-forgot-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-access-tests", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-access-tests-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-lessons-assignments", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-lessons-assignments-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-assignments-completed", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-assignments-completed-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-grades", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-grades-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-forums", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-forums-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-chat", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-chat-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-comments", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-comments-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-library", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-library-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-what-is-app", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-what-is-app-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-notifications", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-notifications-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-connection-problem", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-connection-problem-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-forum", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-forum-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-incident-report", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-incident-report-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-average", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-average-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-profile", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-profile-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-subtitles", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-subtitles-exp", Settings.LANGUAGES)
            },
            {
                title:  LanguageManager.getTranslation("faq.student-lesson", Settings.LANGUAGES),
                content:  LanguageManager.getTranslation("faq.student-lesson-exp", Settings.LANGUAGES)
            }
        ]
    };

    return (
        <Faq data={data}/>
    )
};
export default FaqStudents;