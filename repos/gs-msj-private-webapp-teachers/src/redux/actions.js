import {getHomeworkDetails, updateHomework, setHomework } from "../api/teachers";

import newId from '../utils/new-id';
import { TITLE_CHANGE,
    DESC_CHANGE,
    QUIZ_TYPE_CHANGE,
    CUSTOM_VALUE_CHANGE,
    ADD_PREMISE,
    ON_PREMISE_QUESTION_CHANGE,
    ON_PREMISE_ANSWERS_CHANGE,
    REMOVE_PREMISE,
    SELECT_PREMISE,
    SELECT_ANSWER,
    ADD_PREMISE_FILE,
    FETCH_QUIZ,
    SAVE_QUIZ,
    ADD_QUIZ_FILE,
    RUBRIC_CHANGE,
    REINITIALIZE_VALUES,
    LESSON_DETAILS_CATALOG,
    SET_QUIZ_ID
} from './action-types';

export const onTitleChange = title => {
    return {
        type: TITLE_CHANGE,
        payload: title
    }
};

export const onDescChange = description => {
    return {
        type: DESC_CHANGE,
        payload: description
    }
};

export const onRubricChange = rubric =>{

    return {
        type: RUBRIC_CHANGE,
        payload: rubric
    }
};

export const onQuizTypeChange = quizType => {
    return {
        type: QUIZ_TYPE_CHANGE,
        payload: quizType
    }
};

export const onCustomValueChange = customValue => {
    return {
        type: CUSTOM_VALUE_CHANGE,
        payload: customValue
    }
};

export const addPremise = () => {
    return {
        type: ADD_PREMISE,
        payload: {
            pid: newId(),
            QuizQuestionModel: {
                Desc: ''
            },
            QuizQuestionResponseModelList: []
        }
    }
};

export const onPremiseQuestionChange = (pid, question) => {
    return {
        type: ON_PREMISE_QUESTION_CHANGE,
        payload: {
            pid,question
        }
    }
};

export const onPremiseAnswersChange = answers => {
    return {
        type: ON_PREMISE_ANSWERS_CHANGE,
        payload: answers
    }
};

export const removePremise = pid => {
    return {
        type: REMOVE_PREMISE,
        payload: pid
    }
};

export const selectPremise = pid => {
    return {
        type: SELECT_PREMISE,
        payload: pid
    }
};

export const selectAnswer = (pid, index) => {
    return {
        type: SELECT_ANSWER,
        payload: {
            pid,
            index
        }
    }
};

export const addPremiseFile = (pid, index, imageUrl) => {
    return {
        type: ADD_PREMISE_FILE,
        payload: {
            pid,
            index,
            imageUrl
        }
    }
};

export const addQuizFile = imageUrl => {
    return {
        type: ADD_QUIZ_FILE,
        payload: imageUrl
    }
};

export const fetchQuiz = quizId => async dispatch => {
    const response = await getHomeworkDetails(quizId);
    dispatch({ type: FETCH_QUIZ, payload: response.data})
    return response.data;
};

export const setQuizId = id => dispatch => {
    dispatch({
        type: SET_QUIZ_ID,
        payload: id
    });
};

export const saveQuiz = quizModel => async dispatch => {
    
    let response;

    try{
        if(quizModel.QuizModel.Id){
            response = await updateHomework(quizModel);
        }else {
            response = await setHomework(quizModel);
        }

        dispatch({
            type: SAVE_QUIZ,
            payload: response.data, // store quiz data in redux
        });

        return response.data; // Return response so publish() can use it
    } catch(error){
        console.error("Failed to save quiz:", error);
        throw error;  // So publish() can catch it
    }
};

export const fetchLessonDetails = ({ lessonId, apiMethod, typeDispatch, isCatalog, lesson, examType }) => async dispatch => {
    try {
        if (isCatalog) {
            dispatch({
                type: LESSON_DETAILS_CATALOG,
                payload: lesson
            });
            return;
        }
        if (typeDispatch === "RESET_LESSON_DETAILS") {
            dispatch({ type: typeDispatch });
            return;
        }
        if (examType) {
            const response = await apiMethod(lessonId, examType);
            dispatch({
                type: typeDispatch,
                payload: response.data
            });
            return;
        }
        const response = await apiMethod(lessonId);
        dispatch({
            type: typeDispatch,
            payload: response.data
        });
    } catch (error) {
        console.error("Failed to fetch lesson details", error);
    }
};

export const reinitializeValues = () => {
    return {
        type: REINITIALIZE_VALUES,
        payload: {
            QuizModel: {
                QuizTypeId: 4
            },
            QuestionRequestModelList: [],
            QuizAttachmentModelList: [],
            QuizRubricModel:{
              Desc: "",
            },
            SelectedPremise: null,
            SelectedAnswer: null,
            Published: false,
            CanAddPremise: true
        }
    }
};