import { localizeReducer as locale } from 'react-localize-redux';

import { TITLE_CHANGE,
    DESC_CHANGE,
    QUIZ_TYPE_CHANGE,
    CUSTOM_VALUE_CHANGE,
    ADD_PREMISE,
    REMOVE_PREMISE,
    SELECT_PREMISE,
    SELECT_ANSWER,
    RUBRIC_CHANGE,
    ADD_PREMISE_FILE,
    ON_PREMISE_QUESTION_CHANGE,
    ON_PREMISE_ANSWERS_CHANGE,
    GET_ALL_PREMISES,
    FETCH_QUIZ,
    SAVE_QUIZ,
    ADD_QUIZ_FILE,
    REINITIALIZE_VALUES,
    CHANGE_LANGUAGE,
    FETCH_DESCRIPTION_DETAIL,
    FETCH_INITIAL_ACTIVITY_DETAIL,
    FETCH_DEVELOPMENT_ACTIVITY_DETAIL,
    FETCH_CLOSING_ACTIVITY_DETAIL,
    FETCH_FULLCODES_DETAIL,
    FETCH_OBJECTIVES_DETAIL,
    FETCH_TRANSVERSALTHEMES_DETAIL,
    FETCH_STRATEGIES_DETAIL,
    FETCH_PERFORMANCETASK_DETAIL,
    FETCH_CONCEPT_DETAIL,
    FETCH_DEFINITION_DETAIL,
    FETCH_EXAMPLE_DETAIL,
    FETCH_PRACTICE_DETAIL,
    FETCH_EXAMS_DETAIL,
    FETCH_PRETESTS_DETAIL,
    FETCH_POSTESTS_DETAIL,
    LESSON_DETAILS_CATALOG,
    RESET_LESSON_DETAILS,
    SET_QUIZ_ID
} from './action-types';
import { combineReducers } from 'redux';
import newId from '../utils/new-id';
import Alert from 'react-s-alert';
import Settings from "../custom-classes/settings";

const quizModelInitialState = {
    QuizTypeId: 4,
};

const quizRubricModelInitialState ={
    Desc: "",
};

const tofENTranslation = {
    Cierto: 'True',
    Falso: 'False',
};

const tofESTranslation = {
    False: 'Falso',
    True: 'Cierto',
};

const initialState = [];

const lessonDetailsInitialState = {
    description: '',
    initialActivity: '',
    developmentActivity: '',
    closingActivity: '',
    constructedCodes: [],
    objectives: [],
    strategies: [],
    performanceTasks: [],
    transversalThemes: [],
    concept: '',
    definitions: [],
    examples: [],
    quizzes: [],
    exams: [],
    preTests: [],
    postTests: []
};

const showMessage = messages => {
    Alert.info(messages.join('\n'), {
        position: 'top-right',
        effect: 'slide',
        timeout: 5000
    });
};

/**
 * Higher-Order Reducer to reset state to its initial value when a specific action is dispatched.
 * @param {Function} reducer - The original reducer function to wrap.
 * @param {Object} initialState - The initial state to reset to.
 * @returns {Function} A new reducer function that resets state on STATE_RESET action.
 */
const stateReset = (reducer, initialState, actionType) => {
    return (state = initialState, action) => {
        if (action.type === actionType) {
            return initialState;
        }
        return reducer(state, action);
    };
};

const onQuizModelChange = (quizModel = quizModelInitialState, action) => {
    switch (action.type) {
        case TITLE_CHANGE:
            return {
                ...quizModel,
                Title: action.payload
            };
        case SET_QUIZ_ID:
        return {
                ...quizModel,
                Id: action.payload
            };
        case ADD_PREMISE_FILE:
            if(action.payload.index === null && action.payload.pid === null){
                return {
                    ...quizModel,
                    Instruction: quizModel.Instruction ? quizModel.Instruction + action.payload.imageUrl : action.payload.imageUrl
                }
            }
            return  {...quizModel};
        case DESC_CHANGE:
            return {
                ...quizModel,
                Instruction: action.payload
            };
        case QUIZ_TYPE_CHANGE:
            return {
                ...quizModel,
                QuizTypeId: parseInt(action.payload)
            };
        case CUSTOM_VALUE_CHANGE:
            return {
                ...quizModel,
                CustomValue: action.payload
            };

        case FETCH_QUIZ:
            if(action.payload !== null && action.payload.TransactionValid){
                return action.payload.QuizRequestModel.QuizModel;
            }
            return quizModel;
        case SAVE_QUIZ:
            return quizModel;
        case REINITIALIZE_VALUES:
            return action.payload.QuizModel;
        default:
            return quizModel;
    }
};

const onQuizRubricChange = (quizRubricModel = quizRubricModelInitialState, action) => {
    switch (action.type) {
        case RUBRIC_CHANGE:
            return {
                ...quizRubricModel,
                Desc: action.payload
            };
        case FETCH_QUIZ:
        case SAVE_QUIZ:
            if(action.payload !== null && action.payload.TransactionValid){
                return action.payload.QuizRequestModel.QuizRubricModel;
            }
            return quizRubricModel;
        case REINITIALIZE_VALUES:
            return action.payload.QuizRubricModel;
        case CHANGE_LANGUAGE:
            if(action.payload.languageCode === 'en'){
                quizRubricModel.Desc = Settings.getRubricTemplateEn();
            } else if(action.payload.languageCode === 'es'){
                quizRubricModel.Desc = Settings.getRubricTemplateEs();
            }
            return quizRubricModel;
        default:
            return quizRubricModel;
    }
};

const onQuizAttachFile = (quizAttachmentModelList = initialState, action) => {
    switch(action.type)
    {
        case ADD_QUIZ_FILE:
            return [...quizAttachmentModelList, {AttachmentUrl: action.payload} ];
        case FETCH_QUIZ:
        case SAVE_QUIZ:
            if(!action.payload.TransactionValid){
                return quizAttachmentModelList;
            }

            if(action.payload === null){
                return quizAttachmentModelList;
            }
            return action.payload.QuizRequestModel.QuizAttachmentModelList;
        case REINITIALIZE_VALUES:
            return action.payload.QuizAttachmentModelList;
        default:
            return quizAttachmentModelList;
    }
};

const onPremiseChange = (questionRequestModelList = initialState, action) => {
    switch (action.type) {
        case ADD_PREMISE:
            return [
                ...questionRequestModelList,
                action.payload
            ];
        case QUIZ_TYPE_CHANGE:
            return questionRequestModelList.map(premise => ({...premise, QuizQuestionResponseModelList: []}));
        case ON_PREMISE_QUESTION_CHANGE:
            return [...questionRequestModelList.map(premise => premise.pid === action.payload.pid ? {...premise, QuizQuestionModel:{...premise.QuizQuestionModel, Desc: action.payload.question}} : premise)];
        case ON_PREMISE_ANSWERS_CHANGE:
            return questionRequestModelList.map(premise => premise.pid === action.payload.pid ? {...premise, QuizQuestionResponseModelList: action.payload.answers} : premise);
        case ADD_PREMISE_FILE:
            if(action.payload.index === null && action.payload.pid !== null){
                return [...questionRequestModelList.map(premise => premise.pid === action.payload.pid ? {...premise, QuizQuestionModel:{...premise.QuizQuestionModel, Desc: premise.QuizQuestionModel.Desc + action.payload.imageUrl }} : premise)];
            } else if(action.payload.index !== null) {
                return [...questionRequestModelList.map(premise => premise.pid === action.payload.pid ? {...premise, QuizQuestionResponseModelList: premise.QuizQuestionResponseModelList.map((answer,index) => index === action.payload.index ? {...answer, Response: answer.Response + action.payload.imageUrl } : answer)} : premise)];
            }
            return [...questionRequestModelList];
        case REMOVE_PREMISE:
            return questionRequestModelList.filter(premise => premise.pid !== action.payload);
        case GET_ALL_PREMISES:
            return questionRequestModelList;
        case FETCH_QUIZ:
            if(action.payload !== null && action.payload.Messages && action.payload.Messages !== null){
                showMessage(action.payload.Messages);
            }

            if(!action.payload.TransactionValid){
                return questionRequestModelList;
            }

            if(action.payload.TransactionValid && action.payload != null && action.payload.TransactionValid && action.payload.QuizRequestModel.QuestionRequestModelList !== null){
                return action.payload.QuizRequestModel.QuestionRequestModelList.map(premise => ({...premise, pid: newId()}));
            }

            return questionRequestModelList;
        case SAVE_QUIZ:
            if(action.payload !== null && action.payload.Messages && action.payload.Messages !== null){
                showMessage(action.payload.Messages);
            }

            if(!action.payload.TransactionValid){
                return questionRequestModelList;
            }

            return questionRequestModelList;
        case REINITIALIZE_VALUES:
            return action.payload.QuestionRequestModelList;
        case CHANGE_LANGUAGE:
            if(questionRequestModelList.length > 0 && questionRequestModelList[0].QuizQuestionResponseModelList.length > 0){
                const tof = questionRequestModelList[0].QuizQuestionResponseModelList[0].Response;
                if(tof === 'Cierto' && action.payload.languageCode === 'en'){
                    return questionRequestModelList.map(premise => ({...premise, QuizQuestionResponseModelList: premise.QuizQuestionResponseModelList.map(answer => ({...answer, Response: tofENTranslation[answer.Response]}))}));
                } else if(tof === 'True' && action.payload.languageCode === 'es'){
                    return questionRequestModelList.map(premise => ({...premise, QuizQuestionResponseModelList: premise.QuizQuestionResponseModelList.map(answer => ({...answer, Response: tofESTranslation[answer.Response]}))}));
                }
            }
            return questionRequestModelList;
        default:
            return questionRequestModelList;
    }
};

const selectPremise = (selectedPremise = null, action) => {
    switch (action.type) {
        case REMOVE_PREMISE:
            if(action.payload === selectedPremise){
                return null;
            }
            return selectedPremise;
        case QUIZ_TYPE_CHANGE:
            return null;
        case SELECT_PREMISE:
            return action.payload;
        case SELECT_ANSWER:
            return action.payload.pid;
        case REINITIALIZE_VALUES:
            return action.payload.SelectedPremise;
        default:
            return selectedPremise;
    }
};

const selectAnswer = (selectedAnswer = null, action) => {
    switch (action.type) {
        case QUIZ_TYPE_CHANGE:
            return null;
        case SELECT_PREMISE:
            return null;
        case SELECT_ANSWER:
            return action.payload.index;
        case REINITIALIZE_VALUES:
            return action.payload.SelectedAnswer;
        default:
            return selectedAnswer;
    }
};

const isPublished = (published = false, action) => {
    switch (action.type) {
        case FETCH_QUIZ:
        case SAVE_QUIZ:
            if(action.payload.TransactionValid){
                if(action.payload.QuizRequestModel.QuizModel.ReadyToPublish === '1'){
                    return true;
                }else{
                    return false;
                }
            }
            return published;
        case REINITIALIZE_VALUES:
            return action.payload.Published;
        default:
            return published;
    }
};

const canAddPremise = (canAdd = true, action) => {
    switch (action.type) {
        case FETCH_QUIZ:
        case SAVE_QUIZ:
            if(action.payload.TransactionValid){
                return action.payload != null && action.payload.QuizRequestModel.QuizModel.Id === undefined;
            }
            return canAdd;
        case REINITIALIZE_VALUES:
            return action.payload.CanAddPremise;
        default:
            return canAdd;
    }
};

const validSession = (validSession = true, action) => {
    switch (action.type) {
        case FETCH_QUIZ:
        case SAVE_QUIZ:
            return action.payload.SessionValid;
        default:
            return validSession;
    }
};

const lessonDetails = (lessonDetail = lessonDetailsInitialState, action) => {
    switch (action.type) {
        case FETCH_OBJECTIVES_DETAIL:
            return {
            ...lessonDetail,
            objectives: action.payload
            };
        case FETCH_DESCRIPTION_DETAIL:
            return {
            ...lessonDetail,
            description: action.payload
            };
        case FETCH_INITIAL_ACTIVITY_DETAIL:
            return {
            ...lessonDetail,
            initialActivity: action.payload
            };
        case FETCH_DEVELOPMENT_ACTIVITY_DETAIL:
            return {
            ...lessonDetail,
            developmentActivity: action.payload
            };
        case FETCH_CLOSING_ACTIVITY_DETAIL:
            return {
            ...lessonDetail,
            closingActivity: action.payload
            };
        case FETCH_FULLCODES_DETAIL:
            return {
            ...lessonDetail,
            constructedCodes: action.payload
            };
        case FETCH_STRATEGIES_DETAIL:
            return {
            ...lessonDetail,
            strategies: action.payload
            };
        case FETCH_PERFORMANCETASK_DETAIL:
            return {
            ...lessonDetail,
            performanceTasks: action.payload
            };
        case FETCH_TRANSVERSALTHEMES_DETAIL:
            return {
            ...lessonDetail,
            transversalThemes: action.payload
            };
        case FETCH_CONCEPT_DETAIL:
            return {
            ...lessonDetail,
            concept: action.payload
            };
        case FETCH_DEFINITION_DETAIL:
            return {
            ...lessonDetail,
            definitions: action.payload
            };
        case FETCH_EXAMPLE_DETAIL:
            return {
            ...lessonDetail,
            examples: action.payload
            };
        case FETCH_PRACTICE_DETAIL:
            return {
            ...lessonDetail,
            quizzes: action.payload
            };
        case FETCH_EXAMS_DETAIL:
            return {
            ...lessonDetail,
            exams: action.payload
            };
        case FETCH_PRETESTS_DETAIL:
            return {
            ...lessonDetail,
            preTests: action.payload
            };
        case FETCH_POSTESTS_DETAIL:
            return {
            ...lessonDetail,
            postTests: action.payload
            };
        case LESSON_DETAILS_CATALOG:
            if(action.payload !== null){
                return {
                    ...lessonDetail,
                    description: action.payload.LessonDetailModel.Description,
                    constructedCodes: action.payload.ConstructedCodeHandleModelList,
                    objectives: action.payload.LessonObjectiveModelList,
                    strategies: action.payload.LessonStrategyModelList,
                    performanceTasks: action.payload.LessonPerformanceTaskModelList,
                    transversalThemes: action.payload.LessonTransversalThemeModelList,
                    concept: action.payload.LessonDetailModel.Concept,
                    definitions: action.payload.LessonDefinitionModelList,
                    examples: action.payload.LessonExampleModelList,
                    quizzes: action.payload.QuizRequestModelList.filter(quiz => quiz.QuizModel.IsExam == '0'),
                    exams: action.payload.QuizRequestModelList.filter(quiz => quiz.QuizModel.IsExam == '1'),
                    preTests: action.payload.QuizRequestModelList.filter(quiz => quiz.QuizModel.IsExam == '2'),
                    postTests: action.payload.QuizRequestModelList.filter(quiz => quiz.QuizModel.IsExam == '3'),
                };
            }
            return lessonDetail;
        default:
            return lessonDetail;
    }
};

export default combineReducers({
    CanAddPremise: canAddPremise,
    Published: isPublished,
    QuizModel: onQuizModelChange,
    QuizAttachmentModelList: onQuizAttachFile,
    QuestionRequestModelList: onPremiseChange,
    QuizRubricModel: onQuizRubricChange,
    SelectedPremise: selectPremise,
    SelectedAnswer: selectAnswer,
    ValidSession: true,
    localize: locale,
    lessonDetails: stateReset(lessonDetails, lessonDetailsInitialState, RESET_LESSON_DETAILS)
});