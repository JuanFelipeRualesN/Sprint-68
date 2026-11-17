import axios from "./interceptors";
import Settings from "../custom-classes/settings";

/**
 * Returns the languages
 * @returns {Promise.<T>|*}
 */
export const getLanguages = () =>{
    return axios({
        method: "POST",
        url: Settings.getLanguagesAPI(),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * TO submit Genial Skills Web Quiz
 * @param UserResponseRequestModel object that includes all student responses.
 * @returns {Promise<string|any>}
 */
export const submitGenialQuiz = (UserResponseRequestModel) =>{
    return axios({
        method: "POST",
        url: Settings.getSubmitGenialQuizAPI(),
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify(UserResponseRequestModel)
    });
};

/**
 * To show the client their profiles activities
 */
export const getProfilesActivity = () =>{
    return axios.get(Settings.getProfilesActivityAPI());
};

/**
 * For client: To get the overall student average.
 * @param profileId The id of the profile.
 * @param levelCode Optional: The level code.
 * @param dateFrom Optional: The date from.
 * @param dateTo Optional: The date to.
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getStudentOverallAverage = (profileId, levelCode = null, dateFrom = null, dateTo = null) =>{
    return axios.get(Settings.getProfileOverallAverageAPI(profileId, levelCode, dateFrom, dateTo));
}

/**
 * For client: To get the student overall average per lessons.
 * @param profileId The id of the student.
 * @param subjectCode Optional: The subject code.
 * @param levelCode Optional: The level code.
 * @param dateFrom Optional: The date when it was updated.
 * @param dateTo Optional: The date when it was updated.
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getStudentOverallLessonAverage = (profileId, subjectCode = null, levelCode = null, dateFrom = null, dateTo = null) =>{
    return axios.get(Settings.getProfileOverallLessonSubjectAverageAPI(profileId, subjectCode, levelCode, dateFrom, dateTo));
}

/**
 * For client: To get the profile average details.
 * @param profileId The profile id.
 * @param lessonId The lesson id.
 * @returns {Promise<AxiosResponse<never>>}
 */
export const getProfileLessonAverageDetails = (profileId, lessonId) =>{
    return axios.get(Settings.getProfileLessonAverageDetailsAPI(profileId, lessonId));
}


/**
 * Inserts/Updates the mini game as locked or not
 * @param locked true if locked, else false
 * @returns {*|Promise.<T>}
 */
export const insertMiniGamesLockCreate = (locked) =>{
    return axios({
        method: "POST",
        url: Settings.getMiniGamesLockCreateAPI(locked),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * Get if the mini games area is locked or not by the parent
 * @returns {*|Promise.<T>}
 */
export const getMiniGamesLock = (fromParentApp) =>{
    return axios.get(Settings.getMiniGamesBlockReaderAPI(fromParentApp));
};

/**
 * Returns the student average by a specific lesson
 * @param profileId The student Id
 * @param lessonId The lesson Id
 */
export const getStudentLessonAverage = (profileId, lessonId) =>{
    return axios.get(Settings.getStudentLessonAverageApi(profileId, lessonId));
};

/**
 * Returns the student average.
 * @param {*} profileId : The profile id of the student
 * @param {*} subjectCode : The subject code
 * @param {*} levelCode: The level code.
 * @public
 */
export const getStudentAverage = (profileId, subjectCode, levelCode) =>{
    return axios.get(Settings.getStudentAverageAPI(profileId, subjectCode, levelCode));
};



/**
 * Get User subscribed items
 * @returns {*|Promise.<T>}
 */
export const getUserSubscribedItems = () =>{
    return axios.get(Settings.getItemsSubscribedUsersAPI());
};

/**
 * Return the user quizzes tries according to the quiz and profile id
 *
 * @param {string} quizId : The id of the quiz we want to check how many times the uses did tried
 * @param {string} profileId : The id of the profile (student) we want to check
 * @public
 */
export const getQuizzesTries = (quizId, profileId) =>{
    return axios.get(Settings.getUserQuizzesTriesAPI(profileId, quizId));
};

/**
 * Returns the user quiz' details.
 * @param userQuizId User Quiz Id.
 * @param profileId Profile (student user's id)
 * @returns {Promise<AxiosResponse<T>>}
 */
export const getUserQuizDetails = (userQuizId, profileId) =>{
    return axios.get(Settings.getUserQuizDetailsAPI(profileId, userQuizId));
};

/**
 * Returns the lesson details
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
export const getLessonDetailsStudent = (lessonId) =>{
    return axios.get(Settings.getLessonDetailsAPIGame(lessonId));
};

export const getLessonConcept = (lessonId) =>{
    return axios.get(Settings.getLessonConcept(lessonId));
};

export const getLessonDefinitions = (lessonId) =>{
    return axios.get(Settings.getLessonDefinitions(lessonId));
};

export const getLessonExamples = (lessonId) =>{
    return axios.get(Settings.getLessonExamples(lessonId));
};

export const getLessonPractices = (lessonId) =>{
    return axios.get(Settings.getLessonPractices(lessonId));
};

export const getLessonExamsHomeschooling = (lessonId) =>{
    return axios.get(Settings.getLessonExams(lessonId));
};



/**
 * Returns the lesson definitions
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
 export const getLessonDefinitionsStudent = (lessonId) =>{
    return axios.get(Settings.getLessonDefinitions(lessonId));
};
/**
 * Returns the lesson examples
 * @param lessonId The lesson Id
 * @returns {Promise<any>}
 */
 export const getLessonExamplesStudent = (lessonId) =>{
    return axios.get(Settings.getLessonExamples(lessonId));
};
/**
 * Returns the countries
 */
export const getCountries = (countryIso) =>{
    return axios.get(Settings.getCountriesAPI(countryIso));
};


export const getSettingsPowerDe = () => {
    return axios.get(Settings.getSettingsPowerDeAPI());
};

export const modifySettingsPowerDe = (data) => {
    return axios({
        method: "POST",
        url: Settings.getSettingsPowerDeAPI(),
        data: JSON.stringify(data),
        headers: { "Content-Type": "application/json" },
    });
};

/**
 * To get the lessons
 */

export const getLessonById = (id) => {
    return axios.get(Settings.getLessonByIdAPI(id));
};

export const getLessonByIdConcept = (id) => {
    return axios.get(Settings.getLessonByIdConceptAPI(id));
};

export const getLessonByIdDefinitions = (id) => {
    return axios.get(Settings.getLessonByIdDefinitionsAPI(id));
};

export const getLessonByIdExamples = (id) => {
    return  axios.get(Settings.getLessonByIdExamplesAPI(id));
};

export const getLessonByIdPractices = (id) => {
    return  axios.get(Settings.getLessonByIdPracticesAPI(id));
};


