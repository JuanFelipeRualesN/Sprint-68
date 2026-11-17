import React from 'react';

import FormMultipleChoice from './form-multiple-choice';
import FormOpenQuestion from './form-open-question';
import FormMatch from './form-match';
import Settings from '../../../../custom-classes/settings';

// Preview for a mixed quiz, the single-type previews read the kind from
// quizModel.QuizTypeId, so questions are split by their own QuestionTypeId
// (missing ones fall back to selection) and each slice goes to its preview.
const FormMixed = ({ preview, authorizationCode, token, quizModel, questionRequestModelList, quizAttachmentModelList, rubricModel }) => {
    const questions = questionRequestModelList || [];

    const questionsOfType = (typeId) =>
        questions.filter((question) => {
            const questionTypeId =
                question?.QuizQuestionModel?.QuestionTypeId
                ?? Settings.CUSTOM_QUIZ_TYPE_SELECTION;
            return String(questionTypeId) === typeId;
        });

    const selectionQuestions = questionsOfType(Settings.CUSTOM_QUIZ_TYPE_SELECTION);
    const trueOrFalseQuestions = questionsOfType(Settings.CUSTOM_QUIZ_TYPE_TRUE_OR_FALSE);
    const openQuestions = questionsOfType(Settings.CUSTOM_QUIZ_TYPE_OPEN);
    const matchQuestions = questionsOfType(Settings.CUSTOM_QUIZ_TYPE_MATCH);

    const sharedProps = { preview, authorizationCode, token };
    let headerRendered = false;
    const quizModelFor = () => {
        if (headerRendered) return { ...quizModel, Title: '', Instruction: '' };
        headerRendered = true;
        return quizModel;
    };

    let attachmentsRendered = false;
    const attachmentsFor = () => {
        if (attachmentsRendered) return [];
        attachmentsRendered = true;
        return quizAttachmentModelList;
    };

    return (
        <>
            {selectionQuestions.length > 0 && (
                <FormMultipleChoice
                    {...sharedProps}
                    quizModel={quizModelFor()}
                    questionRequestModelList={selectionQuestions}
                    quizAttachmentModelList={attachmentsFor()}
                />
            )}

            {trueOrFalseQuestions.length > 0 && (
                <FormMultipleChoice
                    {...sharedProps}
                    quizModel={quizModelFor()}
                    questionRequestModelList={trueOrFalseQuestions}
                    quizAttachmentModelList={attachmentsFor()}
                    showResponseExtra={true}
                />
            )}

            {openQuestions.length > 0 && (
                <FormOpenQuestion
                    {...sharedProps}
                    quizModel={quizModelFor()}
                    questionRequestModelList={openQuestions}
                    quizAttachmentModelList={attachmentsFor()}
                    rubricModel={rubricModel}
                />
            )}

            {matchQuestions.length > 0 && (
                <FormMatch
                    {...sharedProps}
                    quizModel={quizModelFor()}
                    questionRequestModelList={matchQuestions}
                    quizAttachmentModelList={attachmentsFor()}
                />
            )}
        </>
    );
};

export default FormMixed;
