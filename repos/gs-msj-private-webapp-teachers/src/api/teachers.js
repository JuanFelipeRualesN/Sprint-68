import axios from "./interceptors";
import * as axios_ni from 'axios';
import Settings from "../custom-classes/settings";
import LanguageManager from "../custom-classes/language-manager";
import SessionManager from "../custom-classes/session-manager";


/**
 * To archive a group.
 * @param groupId Id of the group.
 * @returns {Promise<any | string>}
 */
export const archiveGroup = (groupId) =>{
    return axios({
        method: "POST",
        url: Settings.getArchiveGroupAPI(groupId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To create student annotation for teacher record.
 * @param planningId Academic Plan Id
 * @param studentId Student Id
 * @param annotation Annotation
 * @returns {Promise<any | string>}
 */
export const createAnnotation = (planningId, studentId, annotation) =>{
    return axios({
        method: "POST",
        url: Settings.getTeacherStudentAnnotationsCreateAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            TeacherPlanId: planningId,
            StudentId: studentId,
            Annotation: annotation,
        })
    });
};

/**
 * To archive a curricular plan.
 * @param planningId The curricular plan's id.
 * @returns {AxiosPromise}
 */
export const archivePlanning = (planningId) =>{
    return axios({
        method: "POST",
        url: Settings.getArchivePlanningAPI(planningId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To archive a quiz.
 * @param quizId
 * @returns {Promise<any | string>}
 */
export const archiveQuiz = (quizId) =>{
    return axios({
        method: "POST",
        url: Settings.getArchiveQuizAPI(quizId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Endpoint Call to add teacher plan lessons
 * @param planningId Planning ID
 * @param lessonId Lesson Id to attach
 * @param dueDate The dueDate to submit the assignment
 * @param forGrading for grading
 * @param triesQuantity number of tries allowed for studend.
 * @returns {Promise<any | string>}
 */
export const addTeacherPlanLesson = (planningId, lessonId, dueDate, forGrading, triesQuantity, isRequiredPrePost, dueDatesQuiz, lessonDates) =>{
    let data = {
        'PlanningId': planningId,
        'LessonId': lessonId,
        'ForGrading': forGrading ? "1" : "0",
        'TriesQuantity': triesQuantity,
        'DueDate': dueDate,
        'LangCode': LanguageManager.getLanguage(),
        "PrePostRequired": isRequiredPrePost ? "1" : "0",
        'TeacherPlanLessonQuizModelList': dueDatesQuiz,
        'TeacherPlanLessonDateModelList': lessonDates
    };

    return axios({
        method: "POST",
        url: Settings.getAttachLessonToPlanAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};
/**
 * Endpoint Call to modify teacher plan lessons
 * @param id  ID
 * @param planningId Planning ID
 * @param lessonId Lesson Id to attach
 * @param forGrading for grading
 * @param triesQuantity number of tries allowed for studend.
 * @returns {Promise<any | string>}
 */
export const modifyTeacherLesson = (id, dueDate, forGrading, triesQuantity, isRequiredPrePost, dueDatesQuiz, planningId) => {
    let data = {
        'Id': id,
        'PlanningId': planningId,
        'DueDate': dueDate,
        'ForGrading': forGrading ? "1" : "0",
        'Available': "1",
        'TriesQuantity':triesQuantity,
        "PrePostRequired": isRequiredPrePost ? "1" : "0",
        'TeacherPlanLessonQuizModelList': dueDatesQuiz
    };
    return axios({
        method: "POST",
        url: Settings.getModifyTeacherLessonAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
}
/**
 * Endpoint Call to modify teacher plan quizzes
 * @param planningId Planning ID
 * @param quizId Quiz Id to attach
 * @param forGrading for grading
 * @param triesQuantity number of tries allowed for studend.
 * @returns {Promise<any | string>}
 */
 export const modifyTeacherQuiz= (id,planningId, quizId, forGrading, dueDate, triesQuantity) => {
    let data = {
        'Id': id,
        'PlanningId': planningId,
        'QuizId': quizId,
        'ForGrading': forGrading ? "1" : "0",
        'DueDate': dueDate,
        'TriesQuantity':triesQuantity,
        'Available': "1"
    };
    return axios({
        method: "POST",
        url: Settings.getModifyTeacherQuizAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
}
/**
 * Endpoint Call to archive an lesson that corresponds to a Teacher Planning
 * @param planningId Planning ID
 * @param lessonPlanningId Lesson Planning Id to archive
 * @returns {Promise<any | string>}
 */
export const archiveTeacherPlanLesson = (planningId, lessonPlanningId) =>{
    return axios({
        method: "POST",
        url: Settings.getArchiveLessonOfPlanningAPI(planningId, lessonPlanningId),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * Endpoint Call to add teacher plan assignments
 * @param planningId Planning ID
 * @param assignmentId Assignment (quiz id)
 * @param dueDate The dueDate to submit the assignment
 * @param forGrading ForGrading
 * @param triesQuantity number of retries allowed for student
 * @returns {Promise<any | string>}
 */
export const addTeacherPlanAssignment = (planningId, assignmentId, dueDate, forGrading, triesQuantity) =>{
    let data = {
        'PlanningId': planningId,
        'QuizId': assignmentId,
        'ForGrading': forGrading ? "1" : "0",
        'TriesQuantity': triesQuantity,
        'DueDate': dueDate,
        'LangCode': LanguageManager.getLanguage()
    };

    return axios({
        method: "POST",
        url: Settings.getAttachQuizToPlanAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Endpoint call to archive an assignment that corresponds to a Teacher Planning
 * @param planningId Planning ID
 * @param planningQuizId Planning Quiz Id
 * @returns {Promise<any | string>}
 */
export const archiveTeacherPlanAssignment = (planningId, planningQuizId) =>{
    return axios({
        method: "POST",
        url: Settings.getArchiveQuizOfPlanningAPI(planningId, planningQuizId),
        headers: { "Content-Type": "application/json" }
    });
};

/**
 * To get the student teacher planning average.
 * @returns {Promise<any | string>}
 */
export const getTeacherPlanningStudentHomeworkDetails = (planningId, studentId) =>{
    return axios.get(Settings.getTeacherStudentPlanningHomeworkDetailsAPI(planningId, studentId));
};

/**
 * To create a group
 * @param groupName (The group name)
 * @param subjectCode (The Subject of the group)
 * @param levelCode (The Level of the group)
 * @returns {Promise<any | string>}
 */
export const createGroup = (groupName,levelCode,subjectCode) =>{
    let request = {
        Name: groupName,
        SubjectCode: subjectCode,
        LevelCode: levelCode
    };
    return axios({
        method: "POST",
        url: Settings.getCreateGroupAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * To get the custom quiz created by a teacher by the id.
 * @param quizId The quiz's id.
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getCustomQuizByTeacher = (quizId) =>{
    return axios.get(Settings.getCustomTeacherQuizAPI(quizId));
};

/**
 * To get a custom quiz for the student that works in Unity APP.
 * Since our gets from interceptors change the process a little bit,
 * we had to use the vanilla one.
 * @param authorizationCode
 * @param token
 * @returns {AxiosPromise}
 */
export const getCustomQuiz = (authorizationCode, token) =>{
    return axios_ni({
        method: "GET",
        url: Settings.getCustomQuizAPI(authorizationCode),
        headers: { "Content-Type": "application/json", "Token": token, 'Accept-Language': LanguageManager.getLanguage() },
    });
};

/**
 * To archive a plan
 * @param planningName
 * @param subjectCode
 * @param levelCode
 * @returns {Promise<any | string>}
 */
export const createPlanning = (planningName, periodId, subjectCode, levelCode, periodCloseDate, openDate, closeDate, weekNumber) =>{
    let request = {
        Name: planningName,
        SubjectCode: subjectCode,
        LevelCode: levelCode,
        PeriodId: periodId,
        PeriodCloseDate: periodCloseDate,
        OpenDate: openDate,
        CloseDate: closeDate,
        WeekNumber: weekNumber
    };
    return axios({
        method: "POST",
        url: Settings.getCreatePlanningAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

export const updatePlanName = (planId, planName) =>{
    let request = {
        Name: planName,
    };
    return axios({
        method: "PATCH",
        url: Settings.getUpdatePlanNameAPI(planId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 *
 * @param planId
 * @returns {Promise<any | string>}
 */
export const getClosePlanning = (planId) => {
    let request = {
        Id: planId,
        Opened: "0"
    };
    return axios({
        method: "POST",
        url: Settings.getModifyPlanAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};
/**
 * To get the teacher quizzes.
 * @returns {Promise<any | string>}
 */
export const getTeacherQuizzes = (onlyPublished) =>{
    return axios.get(Settings.getTeacherQuizzesAPI(onlyPublished));
};

/**
 * To get the teacher groups
 * @returns {Promise<any | string>}
 */
export const getTeacherGroups = (limiter, page) =>{
    return axios.get(Settings.getTeacherGroupsAPI(limiter, page));
};

// To get the groups of the teachers under a director, grouped by teacher.
// Each group already includes its students.
export const getDirectorGroups = (limiter, page) =>{
    return axios.get(Settings.getDirectorGroupsAPI(limiter, page));
};

/**
 * To get the teacher plannings
 * @returns {Promise<any | string>}
 */
export const getTeacherPlannings = () => {
    return axios.get(Settings.getTeacherPlanningAPI());
};

/**
 * To get the student teacher planning average.
 * @returns {Promise<any | string>}
 */
export const getTeacherPlanningStudentAverage = (planningId, studentId) => {
    return axios.get(Settings.getTeacherStudentPlanningAverageAPI(planningId, studentId));
};

export const getPrePostResultsTeacher = (teacherPlanLessonId, studentId) => {
    return axios.get(Settings.getPrePostResultsTeacherAPI(teacherPlanLessonId, studentId));
};

/**
 * To get the annotation details.
 * @returns {Promise<any | string>}
 */
export const getTeacherStudentAnnotations = (planningId, studentId, fromPage, limiter) => {
    return axios.get(Settings.getTeacherStudentAnnotationsAPI(planningId, studentId, fromPage, limiter));
};

/**
 * To get the student plannings.
 * @param studentId
 * @param filterByTeacher
 * @returns {Promise<any | string>}
 */
export const getStudentPlannings = (studentId,filterByTeacher) => {
    return axios.get(Settings.getStudentsPlanningsAPI(studentId,filterByTeacher));
};

export const getStudentPlanningsv2 = (studentId) => {
    return axios.get(Settings.getStudentsPlanningsAPIv2(studentId));
};

/**
 * To get the teacher planning details
 * @param planningId The id of the planning.
 * @returns {Promise<any | string>}
 */
export const getTeacherPlanningDetails = (planningId) => {
    return axios.get(Settings.getTeacherPlanningDetailsAPI(planningId));
};

/**
 * To add a student to a group.
 * @param groupId The Id of the group.
 * @param studentId The student Id.
 * @returns {Promise<any | string>}
 */
export const addStudentToGroup = (groupId, studentId) =>{
    return axios({
        method: "POST",
        url: Settings.getAddStudentToGroupAPI(groupId, studentId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To delete a student to a group.
 * @param groupId The Id of the group.
 * @param studentId The student Id.
 * @returns {Promise<any | string>}
 */
export const deleteStudentToGroup = (groupId, studentId) =>{
    return axios({
        method: "POST",
        url: Settings.getAddStudentDeleteFromGroupAPI(groupId, studentId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get a teacher group details
 * @param groupId The group ID
 * @returns {Promise<any | string>} Returns the profiles data
 */
export const getGroupDetails = (groupId) => {
    return axios.get(Settings.getTeacherGroupDetails(groupId));
};

/**
 * To get a teacher plans.
 * @param teacherUserId The teacher user id.
 * @returns {Promise<any | string>} Returns the teacher plans.
 */
export const getTeacherPlansSameSchool = (teacherUserId) => {
    return axios.get(Settings.getTeacherPlansSameSchoolAPI(teacherUserId));
};

/**
 * To get a teacher plans.
 * @returns {Promise<any | string>} Returns the teacher plans.
 */
export const getInstruccionalPlanning = (teacherId, subjectCode, level) => {
    return axios.get(Settings.getInstruccionalPlanningAPI(teacherId, subjectCode, level));
};

/**
 * To get a teacher plans dates.
 * @param planningId The planning Id.
 * @returns {Promise<any | string>} Returns the teacher planning details.
 */
export const getTeacherCalendar = (planningId) => {
    return axios.get(Settings.getTeacherPlanCalendarAPI(planningId));
};

/**
 * Get User custom student quiz details.
 * @param studentId  Student Id
 * @param studentQuizId Student Quiz Id
 * @returns {Promise<any>}
 */
export const getUserCustomQuizDetails = (studentId, studentQuizId) => {
    return axios.get(Settings.getUserCustomQuizDetailsAPI(studentId, studentQuizId));
};


/**
 * To correct a quiz manually by a teacher.
 * @param questions Questions with details as score, etc.
 * @param userQuizResultsId The user Quiz Results' id
 * @param feedback The feedback by a teacher.
 * @returns {AxiosPromise}
 */
export const correctCustomQuizManual = (questions, userQuizResultsId, feedback, planQuizId, autoCorrect) =>{

    let userQuizRequestList = [];
    for(let i=0; i < questions.length;i++)
    {
        const responses = questions[i].QuestionResponses;
        // For true/false questions the graded response isn't always at index 0, so use the one
        // the student selected. Open questions have a single response, so it falls back to index 0.
        const target = responses.find(r => r.SelectedByUser) || responses[0];
        userQuizRequestList.push({
            AssignedValue : target.AssignedValue,
            UserQuizResultId : userQuizResultsId,
            QuestionId: questions[i].QuestionId,
            Id: target.Id
        });
    }

    let request = {
        UserQuizResultsId: userQuizResultsId,
        UserResponseModelList: userQuizRequestList,
        Feedback: feedback,
        PlanQuizId: planQuizId,
        AutoCorrect: autoCorrect
    };

    return axios({
        method: "POST",
        url: Settings.getCorrectCustomQuizManualAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * To submit the quiz by the student. This is a part for Unity when the student make the assignments. Do not change.
 * @param studentToken Student token.
 * @param authorizationCode The authorization code.
 * @param questionQuizList The question list with selected responses.
 * @returns {AxiosPromise}
 */
export const submitStudentCustomQuiz = (studentToken, authorizationCode, questionQuizList) =>{

    let request = {
        AuthorizationCode: authorizationCode,
        UserQuizFullDetailModel:
            {
                QuestionQuizList: questionQuizList,
            }
    };

    return axios_ni({
        method: "POST",
        url: Settings.getSubmitStudentCustomQuiz(),
        headers: { "Content-Type": "application/json", "Token": studentToken, 'Accept-Language': LanguageManager.getLanguage() },
        data: JSON.stringify(request)
    });
};

/**
 * To get the homeworks files' rules.
 * @param token This is the token solution for Unity. If there is no token sent, then it will use the one from the session.
 * @returns {AxiosPromise}
 */
export const getQuizFileRules = (token) => {
    return axios_ni({
        method: "GET",
        url: Settings.getQuizFileRulesAPI(),
        headers: {
            "Content-Type": "application/json",
            "Token": typeof token === 'undefined' || token === null ? SessionManager.getSession() : token,
            'Accept-Language': LanguageManager.getLanguage()
        },
    });
};

/**
 * Get lesson or Lesson Quiz PDF.
 * @returns {Promise<string|any>}
 */
export const getLessonPDF = (lessonId, quizId = "", outPutFormat = "pdf") => {
    if (quizId !== "") 
        return axios.get(Settings.getLessonQuizPDFPrintAPI(lessonId, quizId, outPutFormat));

    return axios.get(Settings.getLessonPDFPrintAPI(lessonId));
};

/**
 * Returns the lesson details by id
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDetailsById = async (lessonId) =>{
    return await axios.get(Settings.getLessonDetailsByIdAPI(lessonId));
}

/**
 * Returns the lesson description details
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDescriptionDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonDescriptionDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Returns one of the suggested activities of the lesson.
 *
 * The activities live in the lesson detail model (ls_lesson_details), the same
 * model that holds the description, so there is no endpoint per activity. The
 * result is wrapped as { data } to keep the shape the lesson detail accordions expect.
 *
 * @param lessonId The lesson Id
 * @param field The lesson detail field to read
 * @returns {Promise<any>}
 */
const getLessonActivityDetails = async (lessonId, field) =>{
    const response = await axios.get(Settings.getLessonDetailsTeachersAPI(lessonId));
    return { data: response.data?.LessonModifierRequestModel?.LessonDetailModel?.[field] ?? "" };
}

/**
 * Returns the suggested opening activity of the lesson
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonInitialActivityDetails = async (lessonId) =>{
    return await getLessonActivityDetails(lessonId, "ActivityInitial");
}

/**
 * Returns the suggested development activity of the lesson
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDevelopmentActivityDetails = async (lessonId) =>{
    return await getLessonActivityDetails(lessonId, "ActivityDevelopment");
}

/**
 * Returns the suggested closing activity of the lesson
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonClosingActivityDetails = async (lessonId) =>{
    return await getLessonActivityDetails(lessonId, "ActivityClosing");
}

/**
 * Get the lesson  full codes
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonFullCodesDetails = async (lessonId) =>{
    return await axios.get(Settings.getLessonFullCodesDetailsAPI(lessonId));
}

/**
 * Get the lesson objective details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonObjectiveDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonObjectivesDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson transversal themes details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonTransversalThemesDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonTransversalThemesDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson strategies details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonStrategiesDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonStrategiesDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson performance task details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonPerformanceTaskDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonPerformanceTaskDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson concept details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonConceptDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonConceptDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson definition details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonDefinitionDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonDefinitionDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson example details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonExampleDetails = async (lessonId) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonExampleDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true}),
        headers: { "Content-Type": "application/json" },
    });
}

/**
 * Get the lesson quizzes details
 * @param lessonId Lesson ID
 * @returns {Promise<any>}
 */
export const getLessonQuizzesDetails = async (lessonId, examType) =>{
    return await axios({
        method: "POST",
        url: Settings.getLessonQuizzesDetailsAPI(),
        data: JSON.stringify({LessonId: lessonId, IsForOutsideSource: true, AddQuizDetails: true, ExamType: examType}),
        headers: { "Content-Type": "application/json" },
    });
}


/**
 * Get lesson PDF.
 * @returns {Promise<string|any>}
 */
export const getQuizTypes = () => {
    return axios.get(Settings.getQuizTypesAPI());
};

/**
 * Endpoint Call to send teacher plans to students
 * @param groupsArray Group Id List
 * @param planningId planningId
 * @returns {Promise<any | string>}
 */
export const sendTeacherPlanToStudentLesson = (groupsArray, planningId) =>{
    // Prepare teacher plan
    let teacherPlanStudentArray = [];
    for(let i=0; i < groupsArray.length; i++)
    {
        teacherPlanStudentArray.push({
            TeacherPlanId: planningId,
            GroupId: groupsArray[i],
        })
    }

    return axios({
        method: "POST",
        url: Settings.getSendPlanningToStudentsAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(teacherPlanStudentArray)
    });
};

/**
 * Updates a homework
 * @param quizModel Quiz Model to update
 * @returns {Promise<string|any>}
 */
export const updateHomework = (quizModel) =>{
    return axios({
        method: "POST",
        url: Settings.getUpdateHomeworkAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(quizModel)
    });
};

/**
 * Get the homework details
 * @param homeworkId Homework Id
 * @returns {Promise<string|any>}
 */
export const getHomeworkDetails = (homeworkId) => {
    return axios.get(Settings.getHomeworkDetailsAPI(homeworkId));
};

/**
 * Sets the homework.
 * @param quizModel Quiz model details.
 * @returns {Promise<string|any>}
 */
export const setHomework = (quizModel) =>{
    // If the rubric is not null and the description is not null, then we set it to null.
    if(quizModel.QuizRubricModel !== null && quizModel.QuizRubricModel.Desc !== null && quizModel.QuizRubricModel.Desc.length > 0 && quizModel.QuizModel.QuizTypeId !== "3")
    {
        quizModel.QuizRubricModel = null;
    }

    return axios({
        method: "POST",
        url: Settings.getCreateHomeworkAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(quizModel)
    });
};

/**
* The tutor checks if at least one of the users he has has an academic planning
* or if the student has an academic planning.
* @returns {Promise<void>}
*/
export const getIfStudentsHasPlannings = () => {
    return axios.get(Settings.getCheckUsersPlanningAvailableAPI());
};

/**
 * Returns the pending assignments lessons.
 * @param fromPage from page.
 * @param limiter How many per page.
 * @returns {Promise<any | string>}
 */
export const getPendingLessonsAssignments = (studentId, planningId, fromPage, limiter) => {
    return axios.get(Settings.getStudentLessonsPendingAPI(studentId, planningId, fromPage, limiter));
};

/**
 * Returns the pending homework.
 * @param fromPage from page.
 * @param limiter How many per page.
 * @returns {Promise<any | string>}
 */
export const getPendingHomeworkAssignments = (studentId, planningId, fromPage, limiter) => {
    return axios.get(Settings.getStudentHomeworkPendingAPI(studentId, planningId, fromPage, limiter));
};

/**
 * Call to submit a forum creation.
 * @param forumTitle The title of the forum.
 * @param forumDescription Description of the forum.
 * @param teacherPlanId Id of the teacher plan.
 * @returns {Promise<any>}
 */
export const createForum = (teacherPlanId, forumTitle, forumDescription) =>{
    let request = {
        TeacherPlanId: teacherPlanId,
        Title: forumTitle,
        Description: forumDescription,
    };

    return axios({
        method: "POST",
        url: Settings.getForumCreationAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * Call to submit a post creation.
 * @param forumId The id of the forum.
 * @param textBody The text body.
 * @returns {Promise<any>}
 */
export const createPost = (data) =>{
    return axios({
        method: "POST",
        url: Settings.getForumPostCreationAPI(data.ForumId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * Update forum post
    @param postId 
    @param textBody 
    @returns {Promise<any>}
 */
export const updatePost = (data) =>{
    return axios({
        method: "PUT",
        url: Settings.getForumPostModifyAPI(data.ForumId, data.Id),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
}

/**
 * Call to submit a forum modify.
 * @param forumTitle The title of the forum.
 * @param forumDescription Description of the forum.
 * @param forumId Id of the forum.
 * @returns {Promise<any>}
 */
export const updateForum = (forumId, forumTitle, forumDescription) =>{
    let request = {
        Active: "1",
        Id: forumId,
        Title: forumTitle,
        Description: forumDescription,
    };

    return axios({
        method: "PUT",
        url: Settings.getForumModifyAPI(forumId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(request)
    });
};

/**
 * Returns the teacher forums.
 * @param teacherPlanId the teacher plan id
 * @param fromPage from page.
 * @param limiter How many per page.
 * @param title The title of the forum.
 * @param createdAt The date of creation.
 * @returns {Promise<any | string>}
 */
export const getTeacherForums = (teacherPlanId, fromPage, limiter, title, createdAt) => {
    return axios.get(Settings.getForumsTeacher(teacherPlanId,fromPage, limiter, title, createdAt));
};

/**
 * Returns the teacher forum details.
 * @param forumId the forum id
 * @param fromPage from page.
 * @param limiter How many per page.
 * @returns {Promise<any | string>}
 */
export const getTeacherForumsDetails = (forumId, page, pageSize, postId) => {
    return axios.get(Settings.getForumDetails(forumId, page, pageSize, postId));
};

/**
 * Call to archive a forum.
 * @param forumId Id of the forum.
 * @returns {Promise<any>}
 */
export const archiveForum = (forumId) =>{
    return axios({
        method: "DELETE",
        url: Settings.getForumArchiveAPI(forumId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Call to archive a forum post.
 * @param postId Id of the post.
 * @returns {Promise<any>}
 */
export const archiveForumPost = (forumId, postId) =>{
    return axios({
        method: "DELETE",
        url: Settings.getForumPostArchiveAPI(forumId, postId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Returns the students of a teacher plan.
 * @param teacherPlanId Teacher Plan id.
 * @returns {Promise<any | string>}
 */
export const getTeacherPlanStudents = (teacherPlanId) => {
    return axios.get(Settings.getTeacherStudentsPlansAPI(teacherPlanId));
};

/**
 * To delete student from plan.
 * @param groupId The id of the group
 * @param teacherPlanId Teacher Plan id.
 * @returns {Promise<any | string>}
 */
export const deleteGroupFromPlan = (groupId, teacherPlanId) =>{
    return axios({
        method: "POST",
        url: Settings.getTeacherGroupPlansRemoveAPI(groupId,teacherPlanId),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Returns the pendings teacher assignments for all instructional palns.
 * @param pageFrom Current page in pagination.
 * @param limiter How many items per page.
 * @returns {AxiosPromise}
 */
export const getTeacherPendingAssignmentsForAllPlans = (pageFrom, limiter) =>{
    let data = {
        "fromPage": pageFrom,
        "limiter": limiter,
    };

    return axios({
        method: "POST",
        url: Settings.getTeacherPendingAssignmentsForAllPlansAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

export const getTotalPendingAssignmentsForAllPlans = () => {
    return axios.get(Settings.getTotalPendingAssignmentsForAllPlansAPI());
};

export const getLessonsTeachers = (subjectCode, levelCode, isGapClosing) => {
    return axios.get(Settings.getLessonTeachersAPI(subjectCode, levelCode, isGapClosing));
};

export const getLessonExamsTeachers = (lessonId) => {
    return axios.get(Settings.getLessonExamsTeachersAPI(lessonId));
};

export const getPlanLessonDetail = (planLessonId) => {
    return axios.get(Settings.getPlanLessonDetailAPI(planLessonId));
};

export const getAllTimeZones = () => {
    return axios.get(Settings.getAllTimeZonesAPI());
};

/**
 * To get the lesson average per plan, lesson and profile.
 * @param planId The id of the plan.
 * @param profileId The id of the profile.
 * @param lessonId The id of the lesson.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getStudentLessonAverage = (planId, profileId, lessonId) =>{
    return axios.get(Settings.getStudentLessonAverageAPITeacher(planId, profileId, lessonId));
};

/**
 * To get the plan lesson id quiz results.
 * @param planLessonId Plan lesson id.
 * @param profileId Profile id.
 * @param quizId Quiz Id.
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getStudentPlanLessonIdQuizResult = (planLessonId, profileId, quizId) => {
    return axios.get(Settings.getProfilePlanLessonQuizResults(planLessonId, profileId, quizId));
}

/** Learning gap **/

/**
 * To read the lesson status per student.
 * @param studentId The id of the student.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapReadStudentLessonStatus = (studentId) => {
    return axios.get(Settings.getLearningGapReadStudentLessonStatusAPI(studentId));
};

/**
 * To get the year score of the student.
 * @param studentId The id of the student.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapReadStudentYearScore = (studentId) => {
    return axios.get(Settings.getLearningGapReadStudentAcademicYearScoreModelAPI(studentId));
}

/**
 * To read the assigned lesson id.
 * @param assignedLessonId The assigned lesson id.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapReadStudentLessonQuizResultList = (assignedLessonId) => {
    return axios.get(Settings.getLearningGapReadStudentLessonQuizResultListAPI(assignedLessonId));
}

/**
 * To assign a lesson to student (learning gap).
 * @param studentId The id of the student.
 * @param lessonId The id of the lesson.
 * @param subjectCode The subject's code.
 * @param dueDate The limit date.
 * @returns {*}
 */
export const postLearningGapAssignLessonToStudent = (studentId, lessonId, subjectCode, dueDate) => {

    let data = {
        "LimitDate": dueDate,
        "LessonId": lessonId,
        "SubjectCode": subjectCode
    }
    return axios({
        method: "POST",
        url: Settings.getLearningGapAssignLessonToStudentAPI(studentId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
}

/**
 * To update the due date of a learning gap assigned lesson to a student.
 * @param assignedLessonId The assigned lesson id.
 * @param dueDate The new due date.
 * @returns {*}
 */
export const putLearningGapAssignLessonToStudent = (assignedLessonId, dueDate, lessonId) => {

        let data = {
            "LimitDate": dueDate,
            "LessonId": lessonId,
        }
        return axios({
            method: "PUT",
            url: Settings.getLearningGapPutAssignLessonToStudentAPI(assignedLessonId),
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(data)
        });
}

/**
 * To get the learning gap subjects.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapRequiredSubjects = () => {
    return axios.get(Settings.getLearningGapRequiredSubjectsAPI());
};

/**
 * To get the learning gap initial data settings.
 * @param studentId The id of the student.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapInitialDataSettings = (studentId) => {
    return axios.get(Settings.getLearningGapInitialDataSettingsAPI(studentId));
};

/**
 * To post the learning gap initial data settings.
 * @param studentId The id of the student.
 * @returns {AxiosPromise}
 */
export const postLearningGapInitialDataSettings = (studentId) => {
    return axios({
        method: "POST",
        url: Settings.getLearningGapInitialDataSettingsAPI(studentId),
        headers: { "Content-Type": "application/json" }
    });
}

/**
 * To get the learning gap student lesson details.
 * @param studentId The id of the student.
 * @param learningGapAcademicYearScoreModelList THe list of the learning gap academic year score model.
 * @returns {*}
 */
export const postLearningGapCreateOrUpdateGradeScoreManually = (studentId, learningGapAcademicYearScoreModelList) => {

    return axios({
        method: "POST",
        url: Settings.getLearningGapCreateOrUpdateGradeScoreManuallyAPI(studentId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(learningGapAcademicYearScoreModelList)
    });
}

/**
 * To call the recommendations.
 * @param studentId The student's id.
 * @param fromPage The page.
 * @param limiter How many items per pege.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapStudentRecommendations = (studentId, fromPage, limiter) => {
    return axios.get(Settings.getLearningGapRecommendationsAPI(studentId, fromPage, limiter));
};

/**
 * To get the PDF of the learning gap student recommendation.
 * @param recommendationId The id of the recommendation.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapStudentRecommendationPrint = (recommendationId) => {
    return axios.get(Settings.getLearningGapRecommendationPrintAPI(recommendationId));
}

/**
 * To get the learning gap student annotations.
 * @param studentId The student's id.
 * @param fromPage The page.
 * @param limiter The limiter.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapStudentAnnotations = (studentId, fromPage, limiter) => {
    return axios.get(Settings.getLearningGapStudentAnnotationsAPI(studentId, fromPage, limiter));
};

/**
 * To post the learning gap student annotations.
 * @param studentId The id of the student.
 * @param annotation The annotation (text).
 * @returns {Promise<AxiosResponse<any>>}
 */
export const postLearningGapStudentAnnotations = (studentId, annotation) => {
    let data = {
        "StudentId": studentId,
        "Text": annotation
    }
    return axios({
        method: "POST",
        url: Settings.getLearningGapStudentAnnotationPostAPI(studentId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To assign the score of a quiz manually.
 * @param learningGapAssignedLessonId The learning gap assigned lesson id.
 * @param studentId The id of the student.
 * @param quizId The id of the quiz.
 * @param score The score.
 * @returns {*}
 */
export const postLearningGapAssignManualQuizToStudent = (learningGapAssignedLessonId, studentId, quizId, score) => {
    let data = {
        "StudentId": studentId,
        "QuizId": quizId,
        "Score": score,
        "LearningGapAssignedLessonId": learningGapAssignedLessonId
    }
    return axios({
        method: "POST",
        url: Settings.getLearningGapAssignManualQuizToStudentAPI(studentId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(data)
    });
};

/**
 * To get the delete learning gap assign lesson to student.
 * @param assignedLessonId The assigned lesson id.
 * @returns {*}
 */
export const deleteLearningGapAssignLessonToStudent = (assignedLessonId) => {
    return axios({
        method: "DELETE",
        url: Settings.getLearningGapPutAssignLessonToStudentAPI(assignedLessonId),
        headers: { "Content-Type": "application/json" }
    });
}

/**
 * To get the preview details.
 * @param subjectCode The subject's code.
 * @param levelCode The level's code.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapDiagnosticPreview = (subjectCode, levelCode ) => {
    return axios.get(Settings.getLearningGapDiagnosticPreviewAPI(subjectCode, levelCode));
}

/**
 * To get the diagnostic score.
 * @param studentId The id of the student.
 * @param subjectCode The subject code. (optional)
 * @param levelCode The level code. (optional)
 * @returns {Promise<AxiosResponse<any>>}
 */
export const getLearningGapDiagnosticScore = (studentId, subjectCode = null, levelCode = null) => {
    return axios.get(Settings.getLearningGapDiagnosticScoreAPI(studentId, subjectCode, levelCode));
}

/**
 * To post the learning gap diagnostic score.
 * @param studentId The id of the student.
 * @param learningGapAcademicYearScoreModelList The learning gap academic year score.
 * @returns {AxiosPromise}
 */
export const postLearningGapCreateOrUpdateDiagnosticScoreManually = (studentId, learningGapAcademicYearScoreModelList) => {
    return axios({
        method: "POST",
        url: Settings.getPostLearningGapDiagnosticScoreAPI(studentId),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(learningGapAcademicYearScoreModelList)
    });
}

/**
 * To post the diagnostic tests dates.
 * @param studentIds The ids of the students.
 * @param groupsIds The ids of the groups.
 * @param subjectCode The subject code.
 * @param levelCode The level code.
 * @param initialDiagnosticDateLimit The initial diagnostic date limit.
 * @param finalDiagnosticDateLimiter The final diagnostic date limiter.
 * @param initialDiagnosticChanged If the initial diagnostic changed.
 * @param finalDiagnosticChanged If the final diagnostic changed.
 * @returns {Promise<AxiosResponse<any>>}
 */
export const postLearningGapDiagnosticTestDates = (id, studentIds, groupsIds, subjectCode,
                                                   levelCode, initialDiagnosticDateLimit,
                                                   finalDiagnosticDateLimiter, initialDiagnosticChanged,
                                                   finalDiagnosticChanged) => {

    let diagnosticTestDates = {
        "Id": id,
        "StudentIds": studentIds,
        "TeacherGroupIds": groupsIds,
        "SubjectCode": subjectCode,
        "LevelCode": levelCode,
        "InitialDiagnosticDateLimit": initialDiagnosticDateLimit,
        "FinalDiagnosticDateLimit": finalDiagnosticDateLimiter,
        "InitialDiagnosticDateUpdated": initialDiagnosticChanged,
        "FinalDiagnosticDateUpdated": finalDiagnosticChanged
    }

    return axios({
        method: "POST",
        url: Settings.getPostLearningGapDiagnosticTestDatesAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(diagnosticTestDates)
    });
}

export const getLearningGapTeacherNotifications = (studentId, fromPage, limiter) => {
    return axios({
        method: "GET",
        url: Settings.getLearningGapTeacherNotificationsAPI(studentId, fromPage, limiter),
        headers: { "Content-Type": "application/json" },
    });
}

export const getLearningGapTeacherHasNotifications = () => {
    return axios({
        method: "GET",
        url: Settings.getLearningGapTeacherHasNotificationsAPI(),
        headers: { "Content-Type": "application/json" },
    });
}

export const getLearningGapStudentHasNotifications = () => {
    return axios({
        method: "GET",
        url: Settings.getLearningGapStudentHasNotificationsAPI(),
        headers: { "Content-Type": "application/json" },
    });
}

//Get the subjects from Athenas
export const getAthenasSubjects = () => {
    return axios.get(Settings.getAthenasSubjectsAPI());
}

//Get the grades/levels from Athenas
export const getAthenasGrades = () => {
    return axios.get(Settings.getAthenasGradesAPI());
}

//Get the standards from Athenas
export const getAthenasStandards = (fromPage = 0, limiter = 10) => {
    return axios.get(Settings.getAthenasStandardsAPI(fromPage, limiter));
}

//Get the expectations from Athenas
export const getAthenasExpectations = (fromPage = 0, limiter = 10) => {
    return axios.get(Settings.getAthenasExpectationsAPI(fromPage, limiter));
}

// Get the lessons from Athenas (with filters)
export const getAthenasAdvancedLessons = async (data) => {
    try {
        const response = await axios({
            method: "POST",
            url: Settings.getPostAthenasAdvancedLessonsAPI(),
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(data)
        });
        return response;
    } catch (error) {
    console.error('Error fetching lessons', error);
    }
}

// Get the pronunciation lessons
export const getPronunciationLessons = (fromPage, limiter, subjectCode, levelCode) => {
    return axios.get(Settings.getPronunciationLessonsAPI(fromPage, limiter, subjectCode, levelCode));
}

/**
 * To get the events by month and year (for teachers, which includes the lessons and homework provided by them).
 * @param month The month (for filters).
 * @param year The year (for filters).
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const getEventsByMonthAndYearForTeachers = (month, year) => {
    return axios.get(Settings.getEventsByMonthAndYearForTeachersAPI(month, year));
};

/**
 * To get how many groups the teacher has.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const getTeacherGroupsTotal = () => {
    return axios.get(Settings.getStudentsGroupsTotalAPI());
};

/**
 * To get the lessons of the previous and next 7 days.
 * @returns {Promise<axios.AxiosResponse<any>>}
 */
export const callPlanLessonsSummary = () => {
    return axios.get(Settings.getTeachersPlansLessonsCurrentAPI());
};
