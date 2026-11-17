/*
* By: Mario Cancel 
*/
import LanguageManager from './../custom-classes/language-manager';
/* 
*  Add a new format extention to concatenate string 
*/
String.prototype.format = function(){
    let formatted = this;
    for( let arg in arguments ) {
        formatted = formatted.replace("{" + arg + "}", arguments[arg]);
    }
    return formatted;
};

// Quiz Type CONSTANTS
const SELECTION = 1;
const MATCH = 2;
const OPEN = 3;
const TOF = 4;
const MIX = 5;

export const validateQuiz = (quizObj,languages) => {
    let valid = true;
    let validationMessages = [];

    // Validates whether the quiz title is empty or not
    if(stringIsNullOrEmpty(quizObj.Title)){
        valid = false;
        validationMessages = [...validationMessages, LanguageManager.getTranslation("quiz.error-title", languages)];
    }
    // Validates whether the quiz Description is empty or not
    if(stringIsNullOrEmpty(quizObj.Instruction)){
        valid = false;
        validationMessages = [...validationMessages, LanguageManager.getTranslation("quiz.error-Description", languages)];
    }
    // Validates whether the quiz Value is empty or not
    if(stringIsNullOrEmpty(quizObj.CustomValue)){
        valid = false;
        validationMessages = [...validationMessages, LanguageManager.getTranslation("quiz.error-quiz-value", languages)];
    }

    if(quizObj.CustomValue <= 0 || quizObj.CustomValue > 100){
        valid = false;
        validationMessages = [...validationMessages, LanguageManager.getTranslation("quiz.error-validate-question-score-exceeds-maximum", languages)];
    }

    if(arrayIsEmpty(quizObj.QuestionRequestModelList)){
        valid = false;
        validationMessages = [...validationMessages,LanguageManager.getTranslation("quiz.add-premise", languages)];
    }

    // Validates the current quiz by its type
    if(!arrayIsEmpty(quizObj.QuestionRequestModelList)){
        switch (parseInt(quizObj.QuizTypeId)) {

            case SELECTION:
                const selectionValidation = validateSelection(quizObj.QuestionRequestModelList,languages);
                if(!selectionValidation.valid){
                    valid = false;
                    validationMessages = [...validationMessages, ...selectionValidation.messages];
                }
                break;
            case MATCH:
                const matchValidation = validateMatch(quizObj.QuestionRequestModelList,languages);
                if(!matchValidation.valid){
                    valid = false;
                    validationMessages = [...validationMessages, ...matchValidation.messages];
                }
                break;
            case OPEN:
                const openValidation = validateOpen(quizObj.QuestionRequestModelList,languages);
                if(!openValidation.valid){
                    valid = false;
                    validationMessages = [...validationMessages, ...openValidation.messages];
                }
                break;
            case TOF:
                const tofValidation = validateTrueOrFalse(quizObj.QuestionRequestModelList,languages);
                if(!tofValidation.valid){
                    valid = false;
                    validationMessages = [...validationMessages, ...tofValidation.messages];
                }
                break;
            case MIX:
                const mixedValidation = validateMix(quizObj.QuestionRequestModelList,languages);
                if(!mixedValidation.valid){
                    valid = false;
                    validationMessages = [...validationMessages, ...mixedValidation.messages];
                }
                break;
            default:
                break;
        }
    }
    return {
        valid: valid,
        messages: validationMessages.filter(message => message)
    }
};

const stringIsNullOrEmpty = text => {
    return text === '' || text === null || text === undefined;
};

const arrayIsEmpty = arr => {
    return !arr || arr === null || arr.length === 0
};

const hasDuplicates = arr => arr.some(item => arr.indexOf(item) !== arr.lastIndexOf(item));

const validateSelection = (questionRequestModelList,languages) => {
    // Validate if the question of the premise is empty
    const questionMessages = questionRequestModelList.map((premise, index) => {
        if(stringIsNullOrEmpty(premise.QuizQuestionModel.Desc)){
            return LanguageManager.getTranslation("quiz.error-selection-empty", languages).format(index+1);
        }
        return undefined;
    });

    // Validate if the answer is empty
    const answerMessages = questionRequestModelList.map((premise, index) => {
        const responseMessages = premise.QuizQuestionResponseModelList.map((answer, i) => {
            if(stringIsNullOrEmpty(answer.Response)){
                return LanguageManager.getTranslation("quiz.error-selection-option-empty", languages).format(i+1,index+1);
            }
            return undefined;
        });

        if(responseMessages.length > 0){
            return responseMessages.filter(message => message).join(', ');
        }
        return undefined;
    });

    // Validate if there are duplicate answers in same the premise
    const duplicateAnswers = questionRequestModelList.map((premise, index) => {
        if(hasDuplicates(premise.QuizQuestionResponseModelList.map(answer => answer.Response.toLowerCase()))){

            return LanguageManager.getTranslation("quiz.error-selection-option-duplicate", languages).format(index+1);

        }
        return undefined;
    });

    // Validate that the premise need to has one correct answer
    const correctAnswerMessage = questionRequestModelList.map((premise, index) => {
        const responseMessage = premise.QuizQuestionResponseModelList.find(answer => answer.Correct === '1');
        if(!responseMessage){
            return  LanguageManager.getTranslation("quiz.error-selection-correct-answer-message", languages).format(index+1);
        }
        return undefined;
    });

    // Validate that the quiz does not has duplicate premise questions
    const duplicateMessages = hasDuplicates(questionRequestModelList.map(premise => premise.QuizQuestionModel.Desc.toLowerCase())) ? [LanguageManager.getTranslation("quiz.error-validate-duplicates",languages)] : [];

    const validationMessages = [...questionMessages, ...answerMessages, ...correctAnswerMessage, ...duplicateMessages, ...duplicateAnswers];

    return {
        messages: [...validationMessages.filter(message => message)],
        valid:  validationMessages.filter(message => message).length === 0
    }
};

const validateMatch = (questionRequestModelList, languages) => {
    // Validate that the quiz should have at least two premises
    const premiseLengthMessage = questionRequestModelList.length < 2 ? [LanguageManager.getTranslation("quiz.match-premises-count", languages)] : [];
    const questionMessages = questionRequestModelList.map((premise, index) => {
        if(stringIsNullOrEmpty(premise.QuizQuestionModel.Desc)){
            return LanguageManager.getTranslation("quiz.error-validateMatch-empty", languages).format(index+1);
        }
        return undefined;
    });
    // Validate if the answer is empty
    const answerMessages = questionRequestModelList.map((premise, index) => {
        const responseMessages = premise.QuizQuestionResponseModelList.map((answer, i) => {
            if(stringIsNullOrEmpty(answer.Response)){
                return LanguageManager.getTranslation("quiz.error-validateMatch-option-empty", languages).format(i+1,index+1);
            }
            return undefined;
        });

        if(responseMessages.length > 0){
            return responseMessages.filter(message => message).join('\n');
        }
        return undefined;
    });
    // Validate if the premis and response has not same 
    const duplicateMessages = hasDuplicates(questionRequestModelList.map(premise => premise.QuizQuestionModel.Desc.toLowerCase())) ? [LanguageManager.getTranslation("quiz.error-validate-duplicates",languages)] : [];

    const validationMessages = [...questionMessages, ...answerMessages, ...premiseLengthMessage, ...duplicateMessages];

    return {
        messages: [...validationMessages.filter(message => message)],
        valid:  validationMessages.filter(message => message).length === 0
    }
};

const validateOpen = (questionRequestModelList, languages) => {
    // Validate if the question of the premise is empty
    const questionMessages = questionRequestModelList.map((premise, index) => {
        if(stringIsNullOrEmpty(premise.QuizQuestionModel.Desc)){
            return LanguageManager.getTranslation("quiz.error-Open-empty", languages).format(index+1);
        }
        return undefined;
    });
    // Validar si la cuestión de la premisa no tiene el mismo
    const duplicateMessages = hasDuplicates(questionRequestModelList.map(premise => premise.QuizQuestionModel.Desc.toLowerCase())) ? [LanguageManager.getTranslation("quiz.error-validate-duplicates",languages)] : [];
    const validationMessages = [...questionMessages, ...duplicateMessages];
    return {
        messages: [...validationMessages.filter(message => message)],
        valid: validationMessages.filter(message => message).length === 0
    }
};

// Validates a mixed quiz by grouping questions by QuestionTypeId
// and handing each group to its validator
const validateMix = (questionRequestModelList, languages) => {
    const validatorsByType = {
        [SELECTION]: validateSelection,
        [MATCH]: validateMatch,
        [OPEN]: validateOpen,
        [TOF]: validateTrueOrFalse,
    };

    const validationMessages = Object.keys(validatorsByType).reduce((messages, typeId) => {
        const questionsOfType = questionRequestModelList.filter(question => {
            const questionTypeId = question?.QuizQuestionModel?.QuestionTypeId ?? String(SELECTION);
            return String(questionTypeId) === typeId;
        });

        if (questionsOfType.length === 0) {
            return messages;
        }

        const validation = validatorsByType[typeId](questionsOfType, languages);
        return validation.valid ? messages : [...messages, ...validation.messages];
    }, []);

    return {
        messages: validationMessages,
        valid: validationMessages.length === 0
    }
};

const validateTrueOrFalse = (questionRequestModelList, languages) => {
    // Validates whether the premis description is empty or not
    const questionMessages = questionRequestModelList.map((premise, index) => {
        if(stringIsNullOrEmpty(premise.QuizQuestionModel.Desc)){
            return LanguageManager.getTranslation("quiz.error-validateTrueOrFalse", languages).format(index+1);
        }
        return undefined;
    });

    // Validate that the premise need to has a value more than 0
    const answerMessages = questionRequestModelList.map((premise, index) => {
        return premise.QuizQuestionResponseModelList.map((answer, i) => {
            if(answer.Correct === '1'&& answer.Value==='0' ){
                return LanguageManager.getTranslation("quiz.error-validateTrueOrFalse-valor", languages).format(index+1);
            }
            return undefined;
        });
    });
    // Validate if the premis has not same 
    const duplicateMessages = hasDuplicates(questionRequestModelList.map(premise => premise.QuizQuestionModel.Desc.toLowerCase())) ? [LanguageManager.getTranslation("quiz.error-validate-duplicates",languages)] : [];
    const validationMessages = [...questionMessages, ...answerMessages.flat(), ...duplicateMessages];

    return {
        messages: [...validationMessages.filter(message => message)],
        valid: validationMessages.filter(message => message).length === 0
    }
};