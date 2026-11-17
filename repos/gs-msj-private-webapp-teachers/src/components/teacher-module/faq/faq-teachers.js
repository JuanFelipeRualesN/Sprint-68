import React from 'react';
import Faq from 'react-faq-component';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faQuestionCircle } from '@fortawesome/free-solid-svg-icons';

const FaqTeachers = () => {
    const data = {
        title: <div className='faq-title'><FontAwesomeIcon icon={faQuestionCircle} />{' '}{LanguageManager.getTranslation("faq.title-teacher", Settings.LANGUAGES)}</div>,
        rows: [
            {
                title: LanguageManager.getTranslation("faq.teacher-password-forget", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-password-forget-exp", Settings.LANGUAGES)
            },            
            {
                title: LanguageManager.getTranslation("faq.teacher-invite-students", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-invite-students-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-groups", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-groups-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-student-messages", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-student-messages-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-chat", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-chat-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-reports", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-reports-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-calendar-lessons", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-calendar-lessons-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-create", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-create-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-edit", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-edit-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-lessons-vs-assignments", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-lessons-vs-assignments-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-view", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-view-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-lessons-added", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-lessons-added-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-plan-lessons-list", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-plan-lessons-list-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-student-connections", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-student-connections-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-student-progress", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-student-progress-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-student-pre-post", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-student-pre-post-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-student-pre-post-no-view", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-student-pre-post-no-view-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-students-connections", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-students-connections-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-scores-print", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-scores-print-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-assign-lessons", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-assign-lessons-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-assign-present", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-assign-present-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-comments", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-comments-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-create-homework", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-create-homework-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-audios", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-audios-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-pdf-docs", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-pdf-docs-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-assignments-correction", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-assignments-correction-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-assignments-created-platforms", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-assignments-created-platforms-exp", Settings.LANGUAGES)
            },

            {
                title: LanguageManager.getTranslation("faq.teacher-assignments-assignments-print", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-assignments-assignments-print-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-sie", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-sie-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-instructional-plan", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-instructional-plan-exp", Settings.LANGUAGES)
            },
            {
                title: LanguageManager.getTranslation("faq.teacher-forum", Settings.LANGUAGES),
                content: LanguageManager.getTranslation("faq.teacher-forum-exp", Settings.LANGUAGES)
            }
        ]
    };

    return (
        <Faq data={data} />
    );
};

export default FaqTeachers;
